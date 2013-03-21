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
    private Dictionary<string, EnvDTE.Expression> expressions;
    private EnvDTE.DebuggerEvents debugerEvents;
    private ArrayControl arrCtl;
    private Chart chartCtl;
    private Array data;
    private int[] dimenstions;

    private IArrayLoader ArrayLoader;
    private Exception lastLoadException;

    private bool arraysPending;
    private bool toolActive;

    #endregion

    #region Constructor

    public ArrayVisualizerToolControl()
    {
      InitializeComponent();

      dte = Package.GetGlobalService(typeof(DTE)) as DTE2;
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
      debugerEvents.OnEnterBreakMode += new _dispDebuggerEvents_OnEnterBreakModeEventHandler(DebuggerEvents_OnEnterBreakMode);
      debugerEvents.OnEnterDesignMode += new _dispDebuggerEvents_OnEnterDesignModeEventHandler(debugerEvents_OnEnterDesignMode);
      debugerEvents.OnEnterRunMode += new _dispDebuggerEvents_OnEnterRunModeEventHandler(debugerEvents_OnEnterRunMode);
    }

    private void debugerEvents_OnEnterRunMode(dbgEventReason Reason)
    {
      arraysPending = false;
      ClearVisualizer();
    }

    private void debugerEvents_OnEnterDesignMode(dbgEventReason Reason)
    {
      arraysPending = false;
      ClearVisualizer();
    }

    private void DebuggerEvents_OnEnterBreakMode(dbgEventReason Reason, ref dbgExecutionAction ExecutionAction)
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
      switch (arrCtl.Data.Rank)
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
        string arrayName = (string)e.AddedItems[0];
        LoadResults result = LoadArray(arrayName, false);
        SetupArrayControl();
        SetupChartControl();
        SetControlsVisibility();

        switch (result)
        {
          case LoadResults.LargeArray:
            LargeArrayHandler largeArrayHandler = new LargeArrayHandler(expressions[arrayName].DataMembers.Count, 2000, 40000);
            largeArrayHandler.LoadArrayRequest += largeArrayHandler_LoadArrayRequest;
            this.mainPanel.Children.Add(largeArrayHandler);
            break;
          case LoadResults.NotSupported:
            break;
          case LoadResults.Exception:
            Label errorLabel = new Label();
            errorLabel.Content = string.Format("Error rendering array '{0}'\r\n\r\n'{1}'", arrayName, lastLoadException.Message);
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
      Array values = GetValues((Expression)e.Data, ArrayLoader.LeftBracket);
      Color color = ((SolidColorBrush)this.mainPanel.Background).Color;
      ((ArrayControl)sender).ShowArrayPopup((UIElement)e.Source, values, e.ToolTipPrefix, color);
    }

    private void largeArrayHandler_LoadArrayRequest(object sender, RoutedEventArgs e)
    {
      if (arraysListBox.SelectedItems.Count == 1)
      {
        LoadArray((string)arraysListBox.SelectedItem, true);
        SetupArrayControl();
        SetupChartControl();
        SetControlsVisibility();
      }
    }

    private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      SetControlsVisibility();
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

    #endregion

    #region Methods

    private void ClearVisualizer()
    {
      arraysListBox.Items.Clear();
      mainPanel.Children.Clear();
      rotateGrid.IsEnabled = false;
    }

    private void LoadScopeArrays()
    {
      ClearVisualizer();
      this.expressions = new Dictionary<string, Expression>();

      if (this.dte.Debugger.CurrentMode == dbgDebugMode.dbgBreakMode)
      {
        
        if (this.ArrayLoader.LoadStaticElements)
      {
        string[] nameParts = this.dte.Debugger.CurrentStackFrame.FunctionName.Split(this.ArrayLoader.NsSeporator);
        string seporator = new string(this.ArrayLoader.NsSeporator, 1);
        for (int i = nameParts.Length - 1; i > 0; i--)
        {
          string name = string.Join(seporator, nameParts, 0, i);
          foreach (Expression expression in this.dte.Debugger.GetExpression(name).DataMembers)
            this.ArrayLoader.ArraysLoader(this.expressions, name + this.ArrayLoader.NsSeporator, expression);
        }
      }

        foreach (Expression expression in this.dte.Debugger.CurrentStackFrame.Locals)
          this.ArrayLoader.ArraysLoader(this.expressions, string.Empty, expression);

        foreach (string item in this.expressions.Keys)
          this.arraysListBox.Items.Add(item);
      }
    }

    private LoadResults LoadArray(string arrayName, bool ignoreArraySize)
    {
      lastLoadException = null;
      data = null;
      try
      {
        EnvDTE.Expression expression = expressions[arrayName];
        if (expression.Value != "null")
        {
          object[] values;

          switch (expression.Type)
          {
            case "SharpDX.Matrix":
              dimenstions = new[] { 4, 4 };
              values = GetValues(expression, e => 'M' == e.Name[0], ArrayLoader.LeftBracket);
              break;
            case "SharpDX.Matrix3x2":
              dimenstions = new[] { 3, 2 };
              values = GetValues(expression, e => 'M' == e.Name[0], ArrayLoader.LeftBracket);
              break;
            case "SharpDX.Matrix5x4":
              dimenstions = new[] { 5, 4 };
              values = GetValues(expression, e => 'M' == e.Name[0], ArrayLoader.LeftBracket);
              break;
            case "SharpDX.Vector2":
              dimenstions = new[] { 2, 1 };
              values = GetValues(expression, e => "XY".Contains(e.Name.Last()), ArrayLoader.LeftBracket);
              break;
            case "SharpDX.Vector3":
              dimenstions = new[] { 3, 1 };
              values = GetValues(expression, e => "XYZ".Contains(e.Name.Last()), ArrayLoader.LeftBracket);
              break;
            case "SharpDX.Vector4":
              dimenstions = new[] { 4, 1 };
              values = GetValues(expression, e => "XYZW".Contains(e.Name.Last()), ArrayLoader.LeftBracket).RotateLeft(1).ToArray();
              break;
            default:
              dimenstions = this.ArrayLoader.DimensionsLoader(expression);
              int count = expression.DataMembers.Count;
              if (ignoreArraySize || count <= 2000)
                values = GetValues(expression, ArrayLoader.LeftBracket);
              else
                return LoadResults.LargeArray;
              break;
          }
          switch (dimenstions.Length)
          {
            case 1:
              data = values.ToArray(dimenstions[0]);
              break;
            case 2:
              data = values.ToArray(dimenstions[0], dimenstions[1]);
              break;
            case 3:
              data = values.ToArray(dimenstions[0], dimenstions[1], dimenstions[2]);
              break;
            case 4:
              data = values.ToArray(dimenstions[0], dimenstions[1], dimenstions[2], dimenstions[3]);
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

    private void SetupArrayControl()
    {
      this.SetRotationOptions(dimenstions.Length);

      switch (dimenstions.Length)
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
      this.arrCtl.CellHeight = double.Parse(this.cellHeightTextBox.Text, CultureInfo.InvariantCulture);
      this.arrCtl.CellWidth = double.Parse(this.cellWidthTextBox.Text, CultureInfo.InvariantCulture);

      arrCtl.LeftBracket = ArrayLoader.LeftBracket;
      arrCtl.RightBracket = ArrayLoader.RightBracket;

      this.arrCtl.CaptionBuilder = CaptionBuilder;
      this.arrCtl.CellClick += new EventHandler<CellClickEventArgs>(arrCtl_CellClick);

      this.arrCtl.SetControlData(data);

      this.arrCtl.Padding = new Thickness(8);
      this.arrCtl.Width += 16;
      this.arrCtl.Height += 16;
    }

    private void SetupChartControl()
    {
      IEnumerable<double> chartData = null;

      int dimenstionsCount = dimenstions.Length;

      try
      {
        if ((dimenstionsCount == 1 || dimenstionsCount == 2) && this.data != null)
        {
          chartData = ConvertToDoubles(this.data);
          chartTab.SetVisible(chartData.Any());
        }
        else
          chartTab.SetVisible(false);
      }
      finally
      {
        if (chartTab.Visibility == System.Windows.Visibility.Hidden)
        {
          dataTab.IsSelected = true;
        }
        else
        {
          chartTab.Visibility = Visibility.Visible;

          this.chartCtl = new Chart();
          ChartArea area = new ChartArea();

          if (dimenstionsCount == 1)
            area.Series.Add(GetSeries(GetSelectedChartType(), chartData));
          else   //2
          {
            double[] chartDataFlat = chartData.ToArray();
            for (int i = 0; i < dimenstions[0]; i++)
              area.Series.Add(GetSeries(GetSelectedChartType(), chartDataFlat.Skip(i * dimenstions[1]).Take(dimenstions[1])));
          }
          chartCtl.Areas.Add(area);

          SetChartLineStroke();
          SetChartStackModeOptions();
          SetGridLines();
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

    private static object[] GetValues(EnvDTE.Expression expression, Predicate<Expression> predicate, char leftBracket)
    {
      object[] values;
      if (expression.DataMembers.Item(1).Type.Contains(leftBracket))
        values = expression.DataMembers.Cast<Expression>().Where(e => predicate(e)).ToArray();
      else
        values = expression.DataMembers.Cast<Expression>().Where(e => predicate(e)).Select(e => e.Value).ToArray();
      return values;
    }

    private static object[] GetValues(EnvDTE.Expression expression, char arrayOpenBarcket)
    {
      object[] values;
      if (expression.DataMembers.Item(1).Type.Contains(arrayOpenBarcket))
        values = expression.DataMembers.Cast<Expression>().ToArray();
      else
        values = expression.DataMembers.Cast<Expression>().Select(e => e.Value).ToArray();
      return values;
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
      if (dte.Mode == vsIDEMode.vsIDEModeDebug && dte.Debugger.CurrentStackFrame != null)
      {
        if (arraysPending && toolActive)
        {
          arraysPending = false;

          //System.Diagnostics.Debug.WriteLine("Loading arrays");

          string language = dte.Debugger.CurrentStackFrame.Language;
          switch (language)
          {
            case "C#":
              ArrayLoader = new CsArrayLoader();
              break;
            case "F#":
              ArrayLoader = new FsArrayLoader();
              break;
            case "Basic":
              ArrayLoader = new VbArrayLoader();
              break;
            default:
              ClearVisualizer();
              Label msg = new Label();
              msg.Content = string.Format("Sorry, currently {0} is not supported.", language);
              mainPanel.Children.Add(msg);
              return;
          }
          LoadScopeArrays();
        }
      }
    }

    private string CaptionBuilder(object data, string formatter)
    {
      EnvDTE.Expression exp = data as EnvDTE.Expression;
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

    private void SetControlsVisibility()
    {
      if (VisualizerTab.SelectedIndex == 0)   // data
        ShowElement(this.arrCtl);
      else                                    // Chart
        ShowElement(this.chartCtl);
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
      if (chartCtl != null && chartCtl.Areas.Any())
      {
        ChartTypes chartType = GetSelectedChartType();
        RangeCalculationMode calculationMode = GetSelectedChartCalculationMode(chartType);

        foreach (ChartArea area in chartCtl.Areas)
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
        default:
          break;
      }
      if (chartCtl != null && chartCtl.Areas.Any())
      {
        ChartTypes chartType = GetSelectedChartType();
        foreach (ChartArea area in chartCtl.Areas)
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
        arrCtl.Padding = new Thickness(8);
        arrCtl.Width += 16;
        arrCtl.Height += 16;
      }
    }

    private void SetChartStackModeOptions()
    {
      if (dimenstions == null)
        return;

      bool chart3D = dimenstions.Length == 2;
      ((ComboBoxItem)chartType.Items[2]).IsEnabled = chart3D;
      ((ComboBoxItem)chartType.Items[3]).IsEnabled = chart3D;
      ((ComboBoxItem)chartType.Items[5]).IsEnabled = chart3D;
      ((ComboBoxItem)chartType.Items[6]).IsEnabled = chart3D;
      ((ComboBoxItem)chartType.Items[8]).IsEnabled = chart3D;
      ((ComboBoxItem)chartType.Items[9]).IsEnabled = chart3D;

      if (!((ComboBoxItem)chartType.SelectedItem).IsEnabled)
        chartType.SelectedItem = chartType.Items[0];
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

  internal static class ext
  {
    public static void SetVisible(this UIElement element, bool value)
    {
      if (value)
        element.Visibility = Visibility.Visible;
      else
        element.Visibility = Visibility.Hidden;
    }
  }
}