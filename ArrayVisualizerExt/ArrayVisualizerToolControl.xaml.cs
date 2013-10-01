using System;
using System.Collections;
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
using Microsoft.VisualStudio.Shell;
using Syncfusion.Windows.Chart;
using Expression = EnvDTE.Expression;

namespace ArrayVisualizerExt
{
  public partial class ArrayVisualizerToolControl
  {
    #region Fields

    private readonly DTE2 dte;
    private List<ExpressionInfo> expressions;
    private DebuggerEvents debugerEvents;
    private ArrayControl arrCtl;
    private Chart chartCtl;
    private Array data;
    private Type undelyingExpressionType;
    private int[] dimensions;

    private IArrayLoader arrayLoader;
    private Parsers parsers;
    private Exception lastLoadException;
    private readonly HashSet<Type> loadedParsers;

    private bool arraysPending;
    private bool toolActive;
    private int lastTabIndex;

    #endregion

    #region Constructor

    public ArrayVisualizerToolControl()
    {
      InitializeComponent();

      dte = Package.GetGlobalService(typeof(DTE)) as DTE2;
      loadedParsers = new HashSet<Type>();

      LoadSettings();

      SetDebugEvents();
      toolActive = true;
      arraysPending = true;
      ShowArrays();
    }

    #endregion

    #region Debugger Events

    private void SetDebugEvents()
    {
      debugerEvents = dte.Events.DebuggerEvents;
      debugerEvents.OnEnterBreakMode += DebuggerEvents_OnEnterBreakMode;
      debugerEvents.OnEnterDesignMode += debugerEvents_OnEnterDesignMode;
      debugerEvents.OnEnterRunMode += debugerEvents_OnEnterRunMode;
    }

    private void debugerEvents_OnEnterRunMode(dbgEventReason reason)
    {
      SaveSettings();
      arraysPending = false;
      ClearVisualizer();
    }

    private void debugerEvents_OnEnterDesignMode(dbgEventReason reason)
    {
      SaveSettings();
      arraysPending = false;
      ClearVisualizer();
    }

    private void DebuggerEvents_OnEnterBreakMode(dbgEventReason reason, ref dbgExecutionAction executionAction)
    {
      arraysPending = true;
      ShowArrays();
    }

    public void ToolActivated()
    {
      toolActive = true;
      ShowArrays();
    }

    public void ToolDeactivated()
    {
      toolActive = false;
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
        bool ok = double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out temp);
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

      int[] dims = arrCtl.Data.GetDimensions();
      switch (arrCtl.Data.Rank)
      {
        case 2:
          Reset((arrCtl.Data.AsEnumerable<object>().ToArray(dims[0], dims[1])).Rotate(angle));
          break;
        case 3:
          Reset((arrCtl.Data.AsEnumerable<object>().ToArray(dims[0], dims[1], dims[2])).Rotate(r, angle));
          break;
        case 4:
          Reset((arrCtl.Data.AsEnumerable<object>().ToArray(dims[0], dims[1], dims[2], dims[3])).Rotate(r, angle));
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
            mainPanel.Children.Add(largeArrayHandler);
            break;
          case LoadResults.NotSupported:
            break;
          case LoadResults.Exception:
            Label errorLabel = new Label
            {
              Content = string.Format("Error rendering array '{0}'\r\n\r\n'{1}'",
              expressionInfo.Name, lastLoadException.Message)
            };
            mainPanel.Children.Add(errorLabel);
            break;
        }
      }
    }

    private void resetButton_Click(object sender, RoutedEventArgs e)
    {
      Reset(data);
    }

    private void supportlabel_MouseUp(object sender, MouseButtonEventArgs e)
    {
      System.Diagnostics.Process.Start("http://bit.ly/155n1RK");
    }

    private void gridLinesType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      SetGridLines();
    }

    private void arrCtl_CellClick(object sender, CellClickEventArgs e)
    {
      Array values = new DefaultParser(arrayLoader).GetValues((Expression)e.Data);
      Color color = ((SolidColorBrush)mainPanel.Background).Color;
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
      if (Equals(e.OriginalSource, VisualizerTab))
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
      if (arrCtl != null)
        Reset(arrCtl.Data);
    }

    private void Parser_CheckedChanged(object sender, RoutedEventArgs e)
    {
      CheckBox chkControl = (CheckBox)sender;

      Type parserType = Type.GetType((string)chkControl.Tag);

      if (parserType != null)
        if (chkControl.IsChecked.HasValue && chkControl.IsChecked.Value)
          loadedParsers.Add(parserType);
        else
          loadedParsers.Remove(parserType);

      arraysPending = true;
      ShowArrays();
    }

    private void GridSplitter_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      double width = MainGrid.ColumnDefinitions[0].Width.Value;
      if (width > 212.01 || width < 11.99)
        MainGrid.ColumnDefinitions[0].Width = new GridLength(212, GridUnitType.Pixel);
      else
        MainGrid.ColumnDefinitions[0].Width = new GridLength(360, GridUnitType.Pixel);
    }

    #endregion

    #region Methods

    private void ClearVisualizer()
    {
      arraysListBox.ItemsSource = null;
      mainPanel.Children.Clear();
      rotateGrid.IsEnabled = false;
    }

    private void LoadScopeArrays()
    {
      ClearVisualizer();
      expressions = new List<ExpressionInfo>();

      if (dte.Debugger.CurrentMode == dbgDebugMode.dbgBreakMode)
      {
        foreach (Expression expression in dte.Debugger.CurrentStackFrame.Locals)
          expressions.AddRange(arrayLoader.GetArrays(string.Empty, expression, parsers, 0));

        arraysListBox.ItemsSource = expressions.OrderBy(A => A.SectionCode).ThenBy(A => A.Name);
        arraysListBox.DisplayMemberPath = "FullName";
      }
    }

    private LoadResults LoadArray(Expression expression, bool ignoreArraySize)
    {
      lastLoadException = null;
      data = null;
      try
      {
        if (expression.Value != "null")
        {
          object[] values = null;

          foreach (ITypeParser parser in parsers.Where(P => P.IsExpressionTypeSupported(expression)))
          {
            dimensions = parser.GetDimensions(expression);
            int count = parser.GetMembersCount(expression);
            if (ignoreArraySize || count <= 2000)
              values = parser.GetValues(expression);
            else
              return LoadResults.LargeArray;

            break;
          }

          switch (dimensions.Length)
          {
            case 1:
              data = values.ToArray(dimensions[0]);
              break;
            case 2:
              data = values.ToArray(dimensions[0], dimensions[1]);
              break;
            case 3:
              data = values.ToArray(dimensions[0], dimensions[1], dimensions[2]);
              break;
            case 4:
              data = values.ToArray(dimensions[0], dimensions[1], dimensions[2], dimensions[3]);
              break;
            default:
              return LoadResults.NotSupported;
          }
        }
      }
      catch (Exception ex)
      {
        lastLoadException = ex;
        return LoadResults.Exception;
      }
      return LoadResults.Success;
    }

    private void SetupArrayControl(ExpressionInfo expressionInfo)
    {
      if (arrCtl != null && arrCtl.Tag == expressionInfo)
        return;

      SetRotationOptions(dimensions.Length);

      switch (dimensions.Length)
      {
        case 1:
          arrCtl = new Array1D();
          break;
        case 2:
          arrCtl = new Array2D();
          break;
        case 3:
          arrCtl = new Array3D();
          break;
        case 4:
          arrCtl = new Array4D();
          break;
        default:
          return;
      }
      arrCtl.Formatter = formatterTextBox.Text;
      arrCtl.CaptionBuilder = GetCaptionBuilder(arrCtl.Formatter);

      arrCtl.CellHeight = GetCellSize(cellHeightTextBox.Text, 40);
      arrCtl.CellWidth = GetCellSize(cellWidthTextBox.Text, 60);

      arrCtl.LeftBracket = arrayLoader.LeftBracket;
      arrCtl.RightBracket = arrayLoader.RightBracket;

      arrCtl.CellClick += arrCtl_CellClick;

      arrCtl.SetControlData(data);

      arrCtl.Padding = new Thickness(8);
      arrCtl.Width += 16;
      arrCtl.Height += 16;

      arrCtl.Tag = expressionInfo;
    }

    private Func<object, string, string> GetCaptionBuilder(string formatter)
    {
      if (!string.IsNullOrEmpty(arrCtl.Formatter))
        switch (arrCtl.Formatter[0])
        {
          case 'D':
          case 'd':
          case 'X':
          case 'x':
            return IntegralCaptionBuilder;
        }

      return DefaultCaptionBuilder;
    }

    private static int GetCellSize(string text, int defaultValue)
    {
      double value;
      if (double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
        return (int)value;

      return defaultValue;
    }

    private void SetupChartControl(ExpressionInfo expressionInfo) 
    {
      IEnumerable<double> chartData = null;

      int dimenstionsCount = dimensions.Length;

      try
      {
        if ((dimenstionsCount == 1 || dimenstionsCount == 2) && data != null)
        {
          chartData = ConvertToDoubles(data);
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

          if (chartCtl == null || chartCtl.Tag != expressionInfo)
          {
            chartCtl = new Chart();
            ChartArea area = new ChartArea();

            if (dimenstionsCount == 1)
              area.Series.Add(GetSeries(GetSelectedChartType(), chartData));
            else   //2
            {
              double[] chartDataFlat = chartData.ToArray();
              for (int i = 0; i < dimensions[0]; i++)
                area.Series.Add(GetSeries(GetSelectedChartType(), chartDataFlat.Skip(i * dimensions[1]).Take(dimensions[1])));
            }
            chartCtl.Areas.Add(area);

            SetChartLineStroke();
            SetChartStackModeOptions();
            SetGridLines();
            chartCtl.Tag = expressionInfo;
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

    private static RangeCalculationMode GetSelectedChartCalculationMode(ChartTypes selectedChartType)
    {
      switch (selectedChartType)
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

    private static IEnumerable<double> ConvertToDoubles(IEnumerable array)
    {
      foreach (object item in array)
      {
        double value;
        if (double.TryParse(item.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out value))
          yield return value;
        else
          yield break;
      }
    }

    private static ChartSeries GetSeries(ChartTypes seriesChartType, IEnumerable<double> chartData)
    {
      ChartSeries chartSeries = new ChartSeries(seriesChartType);
      chartSeries.Data = new ChartData.VisualizerPointsCollection(chartData);
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
          axisComboBox.Visibility = Visibility.Hidden;
          angelComboBox.Visibility = Visibility.Hidden;
          break;
        case 2:
          axisComboBox.Visibility = Visibility.Hidden;
          angelComboBox.Visibility = Visibility.Visible;
          break;
        case 3:
          axisComboBox.Visibility = Visibility.Visible;
          angelComboBox.Visibility = Visibility.Visible;
          break;
        case 4:
          angelComboBox.Items.Add(360);
          angelComboBox.Items.Add(450);
          axisComboBox.Items.Add("A");
          axisComboBox.Visibility = Visibility.Visible;
          angelComboBox.Visibility = Visibility.Visible;
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
      if (dte.Mode == vsIDEMode.vsIDEModeDebug && dte.Debugger.CurrentStackFrame != null)
      {
        if (arraysPending && toolActive)
        {
          arraysPending = false;

          string language = dte.Debugger.CurrentStackFrame.Language;
          switch (language)
          {
            case "C#":
              arrayLoader = new CsArrayLoader();
              break;
            case "F#":
              arrayLoader = new FsArrayLoader();
              break;
            case "Basic":
              arrayLoader = new VbArrayLoader();
              break;
            //case "C++":
            //  arrayLoader = new CppArrayLoader();
            //  break;
            default:
              arrayLoader = GetLanguageLoader();
              if (arrayLoader != null)
                break;

              ClearVisualizer();
              Label msg = new Label
                  {
                    Content = string.Format("Sorry, currently {0} is not supported.", language)
                  };
              mainPanel.Children.Add(msg);
              return;
          }
          parsers = new Parsers(arrayLoader, loadedParsers);
          LoadScopeArrays();
        }
      }
    }

    private static IArrayLoader GetLanguageLoader()
    {
      return null; // Todo, try to load from dlls in bin folder
    }

    private static string DefaultCaptionBuilder(object captionData, string formatter)
    {
      Expression exp = captionData as Expression;
      string text;
      if (exp == null)
        text = (captionData ?? "").ToString();
      else
        text = (exp.Value ?? "");

      double number;
      if (double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out number))
        text = number.ToString(formatter, CultureInfo.InvariantCulture);
      return text;
    }

    private static string IntegralCaptionBuilder(object captionData, string formatter)
    {
      Expression exp = captionData as Expression;
      string text;
      if (exp == null)
        text = (captionData ?? "").ToString();
      else
        text = (exp.Value ?? "");

      long number;
      if (long.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out number))
        text = number.ToString(formatter, CultureInfo.InvariantCulture);
      return text;
    }

    private void SetupControls(ExpressionInfo expressionInfo)
    {
      if (data == null || arraysListBox.SelectedIndex == -1)
        return;

      int tabIndex = VisualizerTab.SelectedIndex;
      if (tabIndex == 2)
        tabIndex = lastTabIndex;

      switch (tabIndex)
      {
        case 0: //defaultData
          SetupArrayControl(expressionInfo);
          SetupChartControl(expressionInfo);
          ShowElement(arrCtl);
          break;
        case 1: //chart
          SetupChartControl(expressionInfo);
          if (VisualizerTab.SelectedIndex == 1)
            ShowElement(chartCtl);
          break;
      }
      lastTabIndex = tabIndex;
    }

    private void ShowElement(Control control)
    {
      if (mainPanel != null)
        mainPanel.Children.Clear();
      if (mainPanel != null && (control != null && !mainPanel.Children.Contains(control)))
        mainPanel.Children.Add(control);
    }

    private void SetChartType()
    {
      if (chartCtl != null && chartCtl.Areas.Any())
      {
        ChartTypes selectedChartType = GetSelectedChartType();
        RangeCalculationMode calculationMode = GetSelectedChartCalculationMode(selectedChartType);

        foreach (ChartArea area in chartCtl.Areas)
        {
          area.PrimaryAxis.RangeCalculationMode = calculationMode;
          foreach (var item in area.Series)
            item.Type = selectedChartType;
        }
      }
    }

    private void SetGridLines()
    {
      bool seconday = gridLinesType.SelectedIndex == 1 || gridLinesType.SelectedIndex == 3;
      bool primary = gridLinesType.SelectedIndex == 2 || gridLinesType.SelectedIndex == 3;
      if (chartCtl != null && chartCtl.Areas.Any())
        foreach (ChartArea area in chartCtl.Areas)
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
      }
      if (chartCtl != null && chartCtl.Areas.Any())
      {
        foreach (ChartArea area in chartCtl.Areas)
          foreach (var item in area.Series)
            item.StrokeThickness = thickness;
      }
    }

    private void Reset(Array defaultData)
    {
      if (arrCtl != null)
      {
        arrCtl.Formatter = formatterTextBox.Text;
        arrCtl.CaptionBuilder = GetCaptionBuilder(arrCtl.Formatter);
        arrCtl.CellHeight = double.Parse(cellHeightTextBox.Text, CultureInfo.InvariantCulture);
        arrCtl.CellWidth = double.Parse(cellWidthTextBox.Text, CultureInfo.InvariantCulture);
        arrCtl.SetControlData(defaultData);
        arrCtl.Padding = new Thickness(8);
        arrCtl.Width += 16;
        arrCtl.Height += 16;
      }
    }

    private void SetChartStackModeOptions()
    {
      if (dimensions == null)
        return;

      bool chart3D = dimensions.Length == 2;
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
      Properties.Settings ds = Properties.Settings.Default;

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
      Properties.Settings ds = Properties.Settings.Default;
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