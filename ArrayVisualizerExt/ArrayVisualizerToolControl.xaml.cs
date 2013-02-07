using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ArrayVisualizerControls;
using ArrayVisualizerExt.ArrayLoaders;
using EnvDTE;
using EnvDTE80;
using LinqLib.Array;
using Microsoft.VisualStudio.Shell;

namespace ArrayVisualizerExt
{
  using System.Globalization;
  using System.Linq;

  using Expression = EnvDTE.Expression;

  /// <summary>
  /// Interaction logic for ArrayVisualizerToolControl.xaml
  /// </summary>
  public partial class ArrayVisualizerToolControl : UserControl
  {
    #region Fields

    private DTE2 dte;
    private Dictionary<string, EnvDTE.Expression> expressions;
    private EnvDTE.DebuggerEvents debugerEvents;
    private ArrayControl arrCtl;

    IArrayLoader ArrayLoader;

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
        LoadArrayControl((string)e.AddedItems[0]);
    }

    private void refreshButton_Click(object sender, RoutedEventArgs e)
    {
      if (arraysListBox.SelectedItems.Count == 1)
        LoadArrayControl((string)arraysListBox.SelectedItem);
    }


    private void supportlabel_MouseUp(object sender, MouseButtonEventArgs e)
    {
      System.Diagnostics.Process.Start("http://www.amirliberman.com/ArrayVisualizer.aspx?v=1.0.0.6");
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
        foreach (Expression expression in this.dte.Debugger.CurrentStackFrame.Locals)
        {
          this.ArrayLoader.ArraysLoader(this.expressions, string.Empty, expression);
        }
      }

      foreach (string item in this.expressions.Keys)
      {
        this.arraysListBox.Items.Add(item);
      }
    }

    private void LoadArrayControl(string arrayName)
    {
      mainPanel.Children.Clear();
      try
      {
        EnvDTE.Expression expression = expressions[arrayName];
        if (expression.Value != "null")
        {
          bool truncate = false;
          int members;
          string[] values;
          Array arr;
          int[] dimenstions;

          switch (expression.Type)
          {
            case "SharpDX.Matrix":
              dimenstions = new[] { 4, 4 };
              values =
                expression.DataMembers.Cast<Expression>()
                          .Where(e => e.Name.StartsWith("M"))
                          .Select(e => e.Value)
                          .ToArray();
              members = values.Length;
              break;
            case "SharpDX.Matrix3x2":
              //SharpDX.Matrix3x2 a; 2 columns
              dimenstions = new[] { 3, 2 };
              values =
                expression.DataMembers.Cast<Expression>()
                          .OrderBy(e => e.Name)
                          .Where(e => e.Name.StartsWith("M"))
                          .Select(e => e.Value)
                          .ToArray();
              members = values.Length;
              break;
            case "SharpDX.Matrix5x4":
              dimenstions = new[] { 5, 4 };
              values =
                expression.DataMembers.Cast<Expression>()
                          .OrderBy(e => e.Name)
                          .Where(e => e.Name.StartsWith("M"))
                          .Select(e => e.Value)
                          .ToArray();
              members = values.Length;
              break;
            case "SharpDX.Vector2":
              dimenstions = new[] { 2, 1 };
              values =
                expression.DataMembers.Cast<Expression>()
                          .Where(e => e.Name.EndsWith("X") || e.Name.EndsWith("Y"))
                          .Select(e => e.Value)
                          .ToArray();
              members = values.Length;
              break;
            case "SharpDX.Vector3":
              dimenstions = new[] { 3, 1 };
              values =
                expression.DataMembers.Cast<Expression>()
                          .Where(e => e.Name.EndsWith("X") || e.Name.EndsWith("Y") || e.Name.EndsWith("Z"))
                          .Select(e => e.Value)
                          .ToArray();
              members = values.Length;
              break;
            case "SharpDX.Vector4":
              dimenstions = new[] { 4, 1 };
              values =
                expression.DataMembers.Cast<Expression>()
                          .Where(
                            e =>
                            e.Name.EndsWith("X") || e.Name.EndsWith("Y") || e.Name.EndsWith("Z") || e.Name.EndsWith("W"))
                          .Select(e => e.Value)
                          .ToArray();
              string tmp = values[0];
              values[0] = values[1];
              values[1] = values[2];
              values[2] = values[3];
              values[3] = tmp;

              members = values.Length;
              break;
            default:
              dimenstions = this.ArrayLoader.DimensionsLoader(expression);
              members = expression.DataMembers.Count;
              truncate = members > 1500;
              if (truncate)
              {
                int dims = dimenstions.Length;
                double r = Math.Pow((double)members / 1500, 1.0 / dims);
                int members2 = 1;
                for (int i = 0; i < dims; i++)
                {
                  dimenstions[i] = (int)(dimenstions[i] / r + .5);
                  members2 = members2 * dimenstions[i];
                }
                members = members2;
              }
              values = new string[members];

              for (int i = 0; i < members; i++)
              {
                values[i] = expression.DataMembers.Item(i + 1).Value;
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
          this.arrCtl.CellHeight = double.Parse(this.cellHeightTextBox.Text, CultureInfo.InvariantCulture);
          this.arrCtl.CellWidth = double.Parse(this.cellWidthTextBox.Text, CultureInfo.InvariantCulture);

          this.arrCtl.SetControlData(arr);

          if (truncate)
          {
            var msg = new Label();
            msg.Content = string.Format("Array is too large, displaying first {0} items only.", members);
            this.mainPanel.Children.Add(msg);
          }
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
  }
}