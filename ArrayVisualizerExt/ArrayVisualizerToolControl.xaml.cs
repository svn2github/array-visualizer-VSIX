using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ArrayVisualizerControls;
using ArrayVisualizerExt.ArrayLoaders;
using ArrayVisualizerExt.TypeParsers;
using EnvDTE;
using EnvDTE80;
using LinqLib.Array;
using LinqLib.Sequence;
using Microsoft.VisualStudio.Shell;
using Syncfusion.Windows.Chart;
using Expression = EnvDTE.Expression;

namespace ArrayVisualizerExt
{
  public partial class ArrayVisualizerToolControl : UserControl
  {
    #region Fields

    private DTE2 dte;
    private List<ExpressionInfo> expressions;
    private EnvDTE.DebuggerEvents debugerEvents;
    private ArrayControl arrCtl;
    private Chart chartCtl;
    private Array data;
    private int[] dimenstions;

    private IArrayLoader arrayLoader;
    private Parsers parsers;
    private Exception lastLoadException;
    private HashSet<Type> loadedParsers;

    private bool arraysPending;
    private bool toolActive;
    private int lastTabIndex;

    #endregion

    #region Constructor

    public ArrayVisualizerToolControl()
    {
      InitializeComponent();

      this.dte = Package.GetGlobalService(typeof(DTE)) as DTE2;
      loadedParsers = new HashSet<Type>();

      LoadSettings();

      SetDebugEvents();
      this.toolActive = true;
      this.arraysPending = true;
      ShowArrays();
    }

    #endregion

    #region Debugger Events

    private void SetDebugEvents()
    {
      this.debugerEvents = this.dte.Events.DebuggerEvents;
      this.debugerEvents.OnEnterBreakMode += new _dispDebuggerEvents_OnEnterBreakModeEventHandler(DebuggerEvents_OnEnterBreakMode);
      this.debugerEvents.OnEnterDesignMode += new _dispDebuggerEvents_OnEnterDesignModeEventHandler(debugerEvents_OnEnterDesignMode);
      this.debugerEvents.OnEnterRunMode += new _dispDebuggerEvents_OnEnterRunModeEventHandler(debugerEvents_OnEnterRunMode);
    }

    private void debugerEvents_OnEnterRunMode(dbgEventReason Reason)
    {
      SaveSettings();
      this.arraysPending = false;
      ClearVisualizer();
    }

    private void debugerEvents_OnEnterDesignMode(dbgEventReason Reason)
    {
      SaveSettings();
      this.arraysPending = false;
      ClearVisualizer();
    }

    private void DebuggerEvents_OnEnterBreakMode(dbgEventReason Reason, ref dbgExecutionAction ExecutionAction)
    {
      this.arraysPending = true;
      ShowArrays();
    }

    public void ToolActivated()
    {
      this.toolActive = true;
      ShowArrays();
    }

    public void ToolDeactivated()
    {
      this.toolActive = false;
    }

    #endregion

    #region Other Events

    private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      TextBox textBox = sender as TextBox;
      if (textBox != null)
      {
        string text = textBox.Text + e.Text;
        double temp;
        bool ok = double.TryParse(text, out temp);
        e.Handled = !ok;
      }
    }

    private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Space)
        e.Handled = true;
    }

    private void rotateButton_Click(object sender, RoutedEventArgs e)
    {
      RotateAxis r = RotateAxis.RotateNone;
      int angle = (int)angelComboBox.SelectedItem;

      switch ((string)axisComboBox.SelectedItem)
      {
        case "X":
          r = RotateAxis.RotateX;
          break;
        case "Y":
          r = RotateAxis.RotateY;
          break;
        case "Z":
          r = RotateAxis.RotateZ;
          break;
        case "A":
          r = RotateAxis.RotateA;
          break;
      }

      int[] dims = this.arrCtl.Data.GetDimensions();
      switch (this.arrCtl.Data.Rank)
      {
        case 2:
          Reset((this.arrCtl.Data.AsEnumerable<object>().ToArray(dims[0], dims[1])).Rotate(angle));
          break;
        case 3:
          Reset((this.arrCtl.Data.AsEnumerable<object>().ToArray(dims[0], dims[1], dims[2])).Rotate(r, angle));
          break;
        case 4:
          Reset((this.arrCtl.Data.AsEnumerable<object>().ToArray(dims[0], dims[1], dims[2], dims[3])).Rotate(r, angle));
          break;
      }
    }

    private void arraysListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.AddedItems.Count == 1)
      {
        ExpressionInfo expressionInfo = (ExpressionInfo)e.AddedItems[0];
        LoadResults result = LoadArray(expressionInfo.Expression, false);
        SetupControls(expressionInfo);

        switch (result)
        {
          case LoadResults.LargeArray:
            LargeArrayHandler largeArrayHandler = new LargeArrayHandler(expressionInfo.Expression.DataMembers.Count, 2000, 40000);
            largeArrayHandler.LoadArrayRequest += largeArrayHandler_LoadArrayRequest;
            this.mainPanel.Children.Add(largeArrayHandler);
            break;
          case LoadResults.NotSupported:
            break;
          case LoadResults.Exception:
            Label errorLabel = new Label();
            errorLabel.Content = string.Format("Error rendering array '{0}'\r\n\r\n'{1}'", expressionInfo.Name, this.lastLoadException.Message);
            mainPanel.Children.Add(errorLabel);
            break;
          case LoadResults.Success:
          default:
            break;
        }
      }
    }

    private void resetButton_Click(object sender, RoutedEventArgs e)
    {
      Reset(this.data);
    }

    private void supportlabel_MouseUp(object sender, MouseButtonEventArgs e)
    {
      System.Diagnostics.Process.Start("http://bit.ly/JjoD5P");
    }

    private void gridLinesType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      SetGridLines();
    }

    private void arrCtl_CellClick(object sender, CellClickEventArgs e)
    {
      Array values = new DefaultParser(this.arrayLoader).GetValues((Expression)e.Data);
      Color color = ((SolidColorBrush)this.mainPanel.Background).Color;
      ((ArrayControl)sender).ShowArrayPopup((UIElement)e.Source, values, e.ToolTipPrefix, color);
    }

    private void largeArrayHandler_LoadArrayRequest(object sender, RoutedEventArgs e)
    {
      if (arraysListBox.SelectedItems.Count == 1)
      {
        ExpressionInfo expressionInfo = (ExpressionInfo)arraysListBox.SelectedItem;
        LoadArray(expressionInfo.Expression, true);
        SetupControls(expressionInfo);
      }
    }

    private void VisualizerTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.OriginalSource == VisualizerTab)
      {
        ExpressionInfo expressionInfo = (ExpressionInfo)arraysListBox.SelectedItem;
        SetupControls(expressionInfo);
      }
    }

    private void chartType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      SetChartType();
    }

    private void lineType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      SetChartLineStroke();
    }

    private void syncFusionLabel_MouseUp(object sender, MouseButtonEventArgs e)
    {
      System.Diagnostics.Process.Start("http://bit.ly/Wq50dg");
    }

    private void applyButton_Click(object sender, RoutedEventArgs e)
    {
      if (this.arrCtl != null)
        Reset(this.arrCtl.Data);
    }

    private void Parser_CheckedChanged(object sender, RoutedEventArgs e)
    {
      CheckBox chkControl = (CheckBox)sender;

      Type parserType = Type.GetType((string)chkControl.Tag);

      if (chkControl.IsChecked.Value)
        loadedParsers.Add(parserType);
      else
        loadedParsers.Remove(parserType);

      this.arraysPending = true;
      ShowArrays();
    }

    private void GridSplitter_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (MainGrid.ColumnDefinitions[0].Width.Value != 212)
        MainGrid.ColumnDefinitions[0].Width = new GridLength(212, GridUnitType.Pixel);
      else
        MainGrid.ColumnDefinitions[0].Width = new GridLength(360, GridUnitType.Pixel);
    }

    #endregion

    #region Methods

    private void ClearVisualizer()
    {
      this.arraysListBox.ItemsSource = null;
      mainPanel.Children.Clear();
      rotateGrid.IsEnabled = false;
    }

    private void LoadScopeArrays()
    {
      ClearVisualizer();
      this.expressions = new List<ExpressionInfo>();

      using (NetAssist.Diagnostics.QuickStopwatch q = new NetAssist.Diagnostics.QuickStopwatch())
        if (this.dte.Debugger.CurrentMode == dbgDebugMode.dbgBreakMode)
        {
          foreach (Expression expression in this.dte.Debugger.CurrentStackFrame.Locals)
            this.expressions.AddRange(this.arrayLoader.GetArrays(string.Empty, expression, this.parsers, 0));

          this.arraysListBox.ItemsSource = this.expressions.OrderBy(A => A.SectionCode).ThenBy(A => A.Name);
          this.arraysListBox.DisplayMemberPath = "FullName";
        }
    }

    private LoadResults LoadArray(Expression expression, bool ignoreArraySize)
    {
      this.lastLoadException = null;
      this.data = null;
      try
      {
        if (expression.Value != "null")
        {
          object[] values = null;

          foreach (ITypeParser parser in this.parsers)
            if (parser.IsExpressionTypeSupported(expression))
            {
              this.dimenstions = parser.GetDimensions(expression);
              int count = parser.GetMembersCount(expression);
              if (ignoreArraySize || count <= 2000)
                values = parser.GetValues(expression);
              else
                return LoadResults.LargeArray;

              break;
            }

          switch (this.dimenstions.Length)
          {
            case 1:
              this.data = values.ToArray(this.dimenstions[0]);
              break;
            case 2:
              this.data = values.ToArray(this.dimenstions[0], this.dimenstions[1]);
              break;
            case 3:
              this.data = values.ToArray(this.dimenstions[0], this.dimenstions[1], this.dimenstions[2]);
              break;
            case 4:
              this.data = values.ToArray(this.dimenstions[0], this.dimenstions[1], this.dimenstions[2], this.dimenstions[3]);
              break;
            default:
              return LoadResults.NotSupported;
          }
        }
      }
      catch (Exception ex)
      {
        this.lastLoadException = ex;
        return LoadResults.Exception;
      }
      return LoadResults.Success;
    }

    private void SetupArrayControl(ExpressionInfo expressionInfo)
    {
      if (this.arrCtl != null && this.arrCtl.Tag == expressionInfo)
        return;

      this.SetRotationOptions(this.dimenstions.Length);

      switch (this.dimenstions.Length)
      {
        case 1:
          this.arrCtl = new Array1D();
          break;
        case 2:
          this.arrCtl = new Array2D();
          break;
        case 3:
          this.arrCtl = new Array3D();
          break;
        case 4:
          this.arrCtl = new Array4D();
          break;
        default:
          return;
      }
      this.arrCtl.Formatter = this.formatterTextBox.Text;
      this.arrCtl.CaptionBuilder = this.CaptionBuilder;

      this.arrCtl.CellHeight = GetCellSize(this.cellHeightTextBox.Text, 40);
      this.arrCtl.CellWidth = GetCellSize(this.cellWidthTextBox.Text, 60);

      this.arrCtl.LeftBracket = this.arrayLoader.LeftBracket;
      this.arrCtl.RightBracket = this.arrayLoader.RightBracket;

      this.arrCtl.CaptionBuilder = CaptionBuilder;
      this.arrCtl.CellClick += new EventHandler<CellClickEventArgs>(arrCtl_CellClick);

      this.arrCtl.SetControlData(this.data);

      this.arrCtl.Padding = new Thickness(8);
      this.arrCtl.Width += 16;
      this.arrCtl.Height += 16;

      this.arrCtl.Tag = expressionInfo;
    }

    private int GetCellSize(string text, int defaultValue)
    {
      double value;
      if (double.TryParse(text, out value))
        return defaultValue;
      else
        return 40;
    }

    private void SetupChartControl(ExpressionInfo expressionInfo, bool loadChart)
    {
      IEnumerable<double> chartData = null;

      int dimenstionsCount = this.dimenstions.Length;

      try
      {
        if ((dimenstionsCount == 1 || dimenstionsCount == 2) && this.data != null)
        {
          chartData = ConvertToDoubles(this.data);
          chartTab.IsEnabled = chartData.Any();
        }
        else
          chartTab.IsEnabled = false;
      }
      finally
      {
        if (!chartTab.IsEnabled)
        {
          if (VisualizerTab.SelectedIndex != 2)
            dataTab.IsSelected = true;
        }
        else
        {
          chartTab.IsEnabled = true;

          if (this.chartCtl == null || this.chartCtl.Tag != expressionInfo)
          {
            this.chartCtl = new Chart();
            ChartArea area = new ChartArea();

            if (dimenstionsCount == 1)
              area.Series.Add(GetSeries(GetSelectedChartType(), chartData));
            else   //2
            {
              double[] chartDataFlat = chartData.ToArray();
              for (int i = 0; i < this.dimenstions[0]; i++)
                area.Series.Add(GetSeries(GetSelectedChartType(), chartDataFlat.Skip(i * this.dimenstions[1]).Take(this.dimenstions[1])));
            }
            this.chartCtl.Areas.Add(area);

            SetChartLineStroke();
            SetChartStackModeOptions();
            SetGridLines();
            this.chartCtl.Tag = expressionInfo;
          }
        }
      }
    }

    private ChartTypes GetSelectedChartType()
    {
      switch ((string)((ComboBoxItem)chartType.SelectedItem).Content)
      {
        case "Line":
          return ChartTypes.Line;
        case "Bar":
          return ChartTypes.Bar;
        case "Stack Bar":
          return ChartTypes.StackingBar;
        case "Stack Bar 100":
          return ChartTypes.StackingBar100;
        case "Column":
          return ChartTypes.Column;
        case "Stack Column":
          return ChartTypes.StackingColumn;
        case "Stack Column 100":
          return ChartTypes.StackingColumn100;
        case "Area":
          return ChartTypes.Area;
        case "Stack Area":
          return ChartTypes.StackingArea;
        case "Stack Area 100":
          return ChartTypes.StackingArea100;
        case "Spline":
          return ChartTypes.Spline;
        case "Spline Area":
          return ChartTypes.SplineArea;
        default:
          throw new NotImplementedException();
      }
    }

    private RangeCalculationMode GetSelectedChartCalculationMode(ChartTypes chartType)
    {
      switch (chartType)
      {
        case ChartTypes.Line:
        case ChartTypes.Area:
        case ChartTypes.Spline:
        case ChartTypes.SplineArea:
        case ChartTypes.StackingArea:
        case ChartTypes.StackingArea100:
          return RangeCalculationMode.ConsistentAcrossChartTypes;
        case ChartTypes.Bar:
        case ChartTypes.Column:
        case ChartTypes.StackingBar:
        case ChartTypes.StackingBar100:
        case ChartTypes.StackingColumn:
        case ChartTypes.StackingColumn100:
          return RangeCalculationMode.AdjustAcrossChartTypes;
        default:
          throw new NotImplementedException();
      }
    }

    private IEnumerable<double> ConvertToDoubles(Array array)
    {
      foreach (object item in array)
      {
        double value;
        if (double.TryParse(item.ToString(), out value))
          yield return value;
        else
          yield break;
      }
    }

    private ChartSeries GetSeries(ChartTypes chartType, IEnumerable<double> chartData)
    {
      ChartSeries chartSeries = new ChartSeries(chartType);
      chartSeries.Data = new ArrayVisualizerExt.ChartData.VisualizerPointsCollection(chartData);
      return chartSeries;
    }

    private void SetRotationOptions(int dimensions)
    {
      angelComboBox.Items.Clear();
      angelComboBox.Items.Add(90);
      angelComboBox.Items.Add(180);
      angelComboBox.Items.Add(270);

      axisComboBox.Items.Clear();
      axisComboBox.Items.Add("X");
      axisComboBox.Items.Add("Y");
      axisComboBox.Items.Add("Z");

      switch (dimensions)
      {
        case 1:
          axisComboBox.Visibility = System.Windows.Visibility.Hidden;
          angelComboBox.Visibility = System.Windows.Visibility.Hidden;
          break;
        case 2:
          axisComboBox.Visibility = System.Windows.Visibility.Hidden;
          angelComboBox.Visibility = System.Windows.Visibility.Visible;
          break;
        case 3:
          axisComboBox.Visibility = System.Windows.Visibility.Visible;
          angelComboBox.Visibility = System.Windows.Visibility.Visible;
          break;
        case 4:
          angelComboBox.Items.Add(360);
          angelComboBox.Items.Add(450);
          axisComboBox.Items.Add("A");
          axisComboBox.Visibility = System.Windows.Visibility.Visible;
          angelComboBox.Visibility = System.Windows.Visibility.Visible;
          break;
        default:
          return;
      }

      angelComboBox.SelectedItem = 90;
      axisComboBox.SelectedItem = "Z";

      rotateGrid.IsEnabled = dimensions != 1;
    }

    private void ShowArrays()
    {
      if (this.dte.Mode == vsIDEMode.vsIDEModeDebug && this.dte.Debugger.CurrentStackFrame != null)
      {
        if (this.arraysPending && this.toolActive)
        {
          this.arraysPending = false;

          string language = this.dte.Debugger.CurrentStackFrame.Language;
          switch (language)
          {
            case "C#":
              this.arrayLoader = new CsArrayLoader();
              break;
            case "F#":
              this.arrayLoader = new FsArrayLoader();
              break;
            case "Basic":
              this.arrayLoader = new VbArrayLoader();
              break;
            default:
              this.arrayLoader = GetLanguageLoader();
              if (this.arrayLoader != null)
                break;

              ClearVisualizer();
              Label msg = new Label();
              msg.Content = string.Format("Sorry, currently {0} is not supported.", language);
              mainPanel.Children.Add(msg);
              return;
          }
          parsers = new Parsers(this.arrayLoader, loadedParsers);
          LoadScopeArrays();
        }
      }
    }

    private IArrayLoader GetLanguageLoader()
    {
      return null; //Todo, try to load from dlls in bin folder
    }

    private string CaptionBuilder(object data, string formatter)
    {
      Expression exp = data as Expression;
      string text;
      if (exp == null)
        text = (data ?? "").ToString();
      else
        text = (exp.Value ?? "").ToString();

      double number;
      if (double.TryParse(text, out number))
        text = number.ToString(formatter, System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat);
      return text;
    }

    private void SetupControls(ExpressionInfo expressionInfo)
    {
      if (this.data == null || arraysListBox.SelectedIndex == -1)
        return;

      int tabIndex = VisualizerTab.SelectedIndex;
      if (tabIndex == 2)
        tabIndex = lastTabIndex;

      switch (tabIndex)
      {
        case 0: //data
          SetupArrayControl(expressionInfo);
          SetupChartControl(expressionInfo, false);
          ShowElement(this.arrCtl);
          break;
        case 1: //chart
          SetupChartControl(expressionInfo, true);
          if (VisualizerTab.SelectedIndex == 1)
            ShowElement(this.chartCtl);
          break;
      }
      lastTabIndex = tabIndex;
    }

    private void ShowElement(Control control)
    {
      if (this.mainPanel != null)
        this.mainPanel.Children.Clear();
      if (control != null && !this.mainPanel.Children.Contains(control))
        this.mainPanel.Children.Add(control);
    }

    private void SetChartType()
    {
      if (this.chartCtl != null && this.chartCtl.Areas.Any())
      {
        ChartTypes chartType = GetSelectedChartType();
        RangeCalculationMode calculationMode = GetSelectedChartCalculationMode(chartType);

        foreach (ChartArea area in this.chartCtl.Areas)
        {
          area.PrimaryAxis.RangeCalculationMode = calculationMode;
          foreach (var item in area.Series)
            item.Type = chartType;
        }
      }
    }

    private void SetGridLines()
    {
      bool seconday = gridLinesType.SelectedIndex == 1 || gridLinesType.SelectedIndex == 3;
      bool primary = gridLinesType.SelectedIndex == 2 || gridLinesType.SelectedIndex == 3;
      if (this.chartCtl != null && this.chartCtl.Areas.Any())
        foreach (ChartArea area in this.chartCtl.Areas)
        {
          ChartArea.SetShowGridLines(area.PrimaryAxis, primary);
          ChartArea.SetShowGridLines(area.SecondaryAxis, seconday);
        }
    }

    private void SetChartLineStroke()
    {
      float thickness = 1;
      switch (lineThickness.SelectedIndex)
      {
        case 0:
          thickness = 1;
          break;
        case 1:
          thickness = 1.5f;
          break;
        case 2:
          thickness = 2f;
          break;
        case 3:
          thickness = 3f;
          break;
        default:
          break;
      }
      if (this.chartCtl != null && this.chartCtl.Areas.Any())
      {
        ChartTypes chartType = GetSelectedChartType();
        foreach (ChartArea area in this.chartCtl.Areas)
          foreach (var item in area.Series)
            item.StrokeThickness = thickness;
      }
    }

    private void Reset(Array data)
    {
      if (this.arrCtl != null)
      {
        this.arrCtl.Formatter = this.formatterTextBox.Text;
        this.arrCtl.CaptionBuilder = this.CaptionBuilder;
        this.arrCtl.CellHeight = double.Parse(this.cellHeightTextBox.Text, CultureInfo.InvariantCulture);
        this.arrCtl.CellWidth = double.Parse(this.cellWidthTextBox.Text, CultureInfo.InvariantCulture);
        this.arrCtl.SetControlData(data);
        this.arrCtl.Padding = new Thickness(8);
        this.arrCtl.Width += 16;
        this.arrCtl.Height += 16;
      }
    }

    private void SetChartStackModeOptions()
    {
      if (this.dimenstions == null)
        return;

      bool chart3D = this.dimenstions.Length == 2;
      ((ComboBoxItem)chartType.Items[2]).IsEnabled = chart3D;
      ((ComboBoxItem)chartType.Items[3]).IsEnabled = chart3D;
      ((ComboBoxItem)chartType.Items[5]).IsEnabled = chart3D;
      ((ComboBoxItem)chartType.Items[6]).IsEnabled = chart3D;
      ((ComboBoxItem)chartType.Items[8]).IsEnabled = chart3D;
      ((ComboBoxItem)chartType.Items[9]).IsEnabled = chart3D;

      if (!((ComboBoxItem)chartType.SelectedItem).IsEnabled)
        chartType.SelectedItem = chartType.Items[0];
    }

    private void SaveSettings()
    {
      ArrayVisualizerExt.Properties.Settings ds = ArrayVisualizerExt.Properties.Settings.Default;

      ds.CellWidth = cellWidthTextBox.Text;
      ds.CellHeight = cellHeightTextBox.Text;
      ds.CellFormatter = formatterTextBox.Text;

      ds.LoadSharpDX = SharpDXParserCheckBox.IsChecked.GetValueOrDefault();
      ds.LoadFSharpMatrix = FSharpParserCheckBox.IsChecked.GetValueOrDefault();

      ds.SplitterPosition = MainGrid.ColumnDefinitions[0].Width.Value;

      ds.Save();
    }

    private void LoadSettings()
    {
      ArrayVisualizerExt.Properties.Settings ds = ArrayVisualizerExt.Properties.Settings.Default;
      cellWidthTextBox.Text = ds.CellWidth;
      cellHeightTextBox.Text = ds.CellHeight;
      formatterTextBox.Text = ds.CellFormatter;

      SharpDXParserCheckBox.IsChecked = ds.LoadSharpDX;
      FSharpParserCheckBox.IsChecked = ds.LoadFSharpMatrix;

      MainGrid.ColumnDefinitions[0].Width = new GridLength(ds.SplitterPosition, GridUnitType.Pixel);
    }

    #endregion

    private enum LoadResults
    {
      Success,
      LargeArray,
      NotSupported,
      Exception
    }
  }
}