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

    private IArrayLoader ArrayLoader;

    #endregion

    #region Constructor

    public ArrayVisualizerToolControl()
    {
      InitializeComponent();

      dte = Package.GetGlobalService(typeof(DTE)) as DTE2;
      SetDebugEvents();
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
      ClearVisualizer();
    }

    private void debugerEvents_OnEnterDesignMode(dbgEventReason Reason)
    {
      ClearVisualizer();
    }

    private void DebuggerEvents_OnEnterBreakMode(dbgEventReason Reason, ref dbgExecutionAction ExecutionAction)
    {
      ShowArrays();
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

      switch (arrCtl.Data.Rank)
      {
        case 2:
          arrCtl.SetControlData(((string[,])arrCtl.Data).Rotate(angle));
          break;
        case 3:
          arrCtl.SetControlData(((string[, ,])arrCtl.Data).Rotate(r, angle));
          break;
        case 4:
          arrCtl.SetControlData(((string[, , ,])arrCtl.Data).Rotate(r, angle));
          break;
      }

      arrCtl.Padding = new Thickness(8);
      arrCtl.Width += 16;
      arrCtl.Height += 16;
    }

    private void arraysListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.AddedItems.Count == 1)
        LoadArrayControl((string)e.AddedItems[0], false);
    }

    private void refreshButton_Click(object sender, RoutedEventArgs e)
    {
      if (arraysListBox.SelectedItems.Count == 1)
        LoadArrayControl((string)arraysListBox.SelectedItem, false);
    }


    private void supportlabel_MouseUp(object sender, MouseButtonEventArgs e)
    {
      System.Diagnostics.Process.Start("http://www.amirliberman.com/ArrayVisualizer.aspx?v=1.0.0.11");
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
        foreach (Expression expression in this.dte.Debugger.CurrentStackFrame.Locals)
          this.ArrayLoader.ArraysLoader(this.expressions, string.Empty, expression);

      foreach (string item in this.expressions.Keys)
        this.arraysListBox.Items.Add(item);
    }

    private void LoadArrayControl(string arrayName, bool ignoreArraySize)
    {
      mainPanel.Children.Clear();
      try
      {
        EnvDTE.Expression expression = expressions[arrayName];
        if (expression.Value != "null")
        {
          object[] values;
          int[] dimenstions;
          Array arr;

          switch (expression.Type)
          {
            case "SharpDX.Matrix":
              dimenstions = new[] { 4, 4 };
              values = GetValues(expression, e => 'M' == e.Name[0]);
              break;
            case "SharpDX.Matrix3x2":
              dimenstions = new[] { 3, 2 };
              values = GetValues(expression, e => 'M' == e.Name[0]);
              break;
            case "SharpDX.Matrix5x4":
              dimenstions = new[] { 5, 4 };
              values = GetValues(expression, e => 'M' == e.Name[0]);
              break;
            case "SharpDX.Vector2":
              dimenstions = new[] { 2, 1 };
              values = GetValues(expression, e => "XY".Contains(e.Name.Last()));
              break;
            case "SharpDX.Vector3":
              dimenstions = new[] { 3, 1 };
              values = GetValues(expression, e => "XYZ".Contains(e.Name.Last()));
              break;
            case "SharpDX.Vector4":
              dimenstions = new[] { 4, 1 };
              values = GetValues(expression, e => "XYZW".Contains(e.Name.Last())).RotateLeft(1).ToArray();
              break;
            default:
              dimenstions = this.ArrayLoader.DimensionsLoader(expression);
              int count = expression.DataMembers.Count;
              if (ignoreArraySize || count <= 2000)
                values = GetValues(expression);
              else
              {
                LargeArrayHandler largeArrayHandler = new LargeArrayHandler(count, 2000, 40000);
                largeArrayHandler.LoadArrayRequest += largeArrayHandler_LoadArrayRequest;
                this.mainPanel.Children.Add(largeArrayHandler);
                return;
              }
              break;
          }

          this.SetRotationOptions(dimenstions.Length);

          switch (dimenstions.Length)
          {
            case 1:
              arr = values.ToArray(dimenstions[0]);
              this.arrCtl = new Array1D();
              break;
            case 2:
              arr = values.ToArray(dimenstions[0], dimenstions[1]);
              this.arrCtl = new Array2D();
              break;
            case 3:
              arr = values.ToArray(dimenstions[0], dimenstions[1], dimenstions[2]);
              this.arrCtl = new Array3D();
              break;
            case 4:
              arr = values.ToArray(dimenstions[0], dimenstions[1], dimenstions[2], dimenstions[3]);
              this.arrCtl = new Array4D();
              break;
            default:
              return;
          }
          this.arrCtl.Formatter = this.formatterTextBox.Text;
          this.arrCtl.CaptionBuilder = this.CaptionBuilder;
          this.arrCtl.CellHeight = double.Parse(this.cellHeightTextBox.Text, CultureInfo.InvariantCulture);
          this.arrCtl.CellWidth = double.Parse(this.cellWidthTextBox.Text, CultureInfo.InvariantCulture);

          this.arrCtl.CaptionBuilder = CaptionBuilder;
          this.arrCtl.CellClick += new EventHandler<CellClickEventArgs>(arrCtl_CellClick);

          this.arrCtl.SetControlData(arr);

          this.arrCtl.Padding = new Thickness(8);
          this.arrCtl.Width += 16;
          this.arrCtl.Height += 16;

          this.mainPanel.Children.Add(this.arrCtl);
        }
      }
      catch (Exception ex)
      {
        Label errorLabel = new Label();
        errorLabel.Content = string.Format("Error rendering array '{0}'\r\n\r\n'{1}'", arrayName, ex.Message);
        mainPanel.Children.Add(errorLabel);
      }
    }

    void arrCtl_CellClick(object sender, CellClickEventArgs e)
    {
      Array values = GetValues((Expression)e.Data);
      Color color = ((SolidColorBrush)this.mainPanel.Background).Color;
      ((ArrayControl)sender).ShowArrayPopup((UIElement)e.Source, values, e.ToolTipPrefix, color);
    }

    void largeArrayHandler_LoadArrayRequest(object sender, RoutedEventArgs e)
    {
      if (arraysListBox.SelectedItems.Count == 1)
        LoadArrayControl((string)arraysListBox.SelectedItem, true);
    }

    private static object[] GetValues(EnvDTE.Expression expression, Predicate<Expression> p)
    {
      object[] values;
      if (expression.DataMembers.Item(1).Type.Contains("["))
        values = expression.DataMembers.Cast<Expression>().Where(e => p(e)).ToArray();
      else
        values = expression.DataMembers.Cast<Expression>().Where(e => p(e)).Select(e => e.Value).ToArray();
      return values;
    }

    private static object[] GetValues(EnvDTE.Expression expression)
    {
      object[] values;
      if (expression.DataMembers.Item(1).Type.Contains("["))
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

    #endregion

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
  }
}