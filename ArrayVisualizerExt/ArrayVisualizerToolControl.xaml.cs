using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EnvDTE;
using EnvDTE80;
using EnvDTE90;
using EnvDTE100;
using Microsoft.VisualStudio.Shell;
using LinqLib.Array;
using ArrayVisualizerControls;

/*  
 * 
 * Refactor code
 * Draw labels in correct angle
 * Tooltips on lables
 * Shorten text if label is too small
 * refresh drawing
 * Icon, Bitmaps, metadata
 */

namespace Microsoft.ArrayVisualizerExt
{
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

    #endregion

    #region Constructor

    public ArrayVisualizerToolControl()
    {
      InitializeComponent();
    
      dte = Package.GetGlobalService(typeof(DTE)) as DTE2;
      SetDebugEvents();
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
      LoadScopeArrays();
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
      else
        throw new NotImplementedException("TextBox_PreviewTextInput Can only Handle TextBoxes");
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
          arrCtl.Data = ((string[,])arrCtl.Data).Rotate(angle);
          break;
        case 3:
          arrCtl.Data = ((string[, ,])arrCtl.Data).Rotate(r, angle);
          break;
        case 4:
          arrCtl.Data = ((string[, , ,])arrCtl.Data).Rotate(r, angle);
          break;
      }
    }

    private void arraysListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.AddedItems.Count == 1)
      {
        mainPanel.Children.Clear();
        string item = (string)e.AddedItems[0];
        EnvDTE.Expression expression = expressions[item];
        if (expression.Value != "null")
        {
          //string type = expression.Type;
          //type = type.Substring(0, type.IndexOf("["));
          string dims = expression.Value;
          dims = dims.Substring(dims.IndexOf("[") + 1);
          dims = dims.Substring(0, dims.IndexOf("]"));

          //Type arrType = Type.GetType(type, false, true);
          int[] dimenstions = dims.Split(',').Select(X => int.Parse(X)).ToArray();
          Array arr;
          int members = expression.DataMembers.Count;
          List<string> values = new List<string>(members);
          for (int i = 1; i <= members; i++)
            values.Add(expression.DataMembers.Item(i).Value);

          angelComboBox.Items.Clear();
          angelComboBox.Items.Add(90);
          angelComboBox.Items.Add(180);
          angelComboBox.Items.Add(270);

          axisComboBox.Items.Clear();
          axisComboBox.Items.Add("X");
          axisComboBox.Items.Add("Y");
          axisComboBox.Items.Add("Z");
          switch (dimenstions.Length)
          {
            case 2:
              arr = values.ToArray(dimenstions[0], dimenstions[1]);
              arrCtl = new Array2D();
              axisComboBox.Visibility = System.Windows.Visibility.Hidden;
              break;
            case 3:
              arr = values.ToArray(dimenstions[0], dimenstions[1], dimenstions[2]);
              arrCtl = new Array3D();
              axisComboBox.Visibility = System.Windows.Visibility.Visible;
              break;
            case 4:
              arr = values.ToArray(dimenstions[0], dimenstions[1], dimenstions[2], dimenstions[3]);
              arrCtl = new Array4D();
              angelComboBox.Items.Add(360);
              angelComboBox.Items.Add(450);
              axisComboBox.Visibility = System.Windows.Visibility.Visible;
              axisComboBox.Items.Add("A");
              break;
            default:
              return;
          }

          arrCtl.Formatter = formatterTextBox.Text;
          arrCtl.CellHeight = double.Parse(cellHeightTextBox.Text);
          arrCtl.CellWidth = double.Parse(cellWidthTextBox.Text);
          angelComboBox.SelectedItem = 90;
          axisComboBox.SelectedItem = "Z";
          rotateGrid.IsEnabled = true;

          arrCtl.Data = arr;

          mainPanel.Children.Add(arrCtl);
        }
      }
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
      expressions = new Dictionary<string, EnvDTE.Expression>();

      if (dte.Debugger.CurrentMode == dbgDebugMode.dbgBreakMode)
        foreach (EnvDTE.Expression expression in dte.Debugger.CurrentStackFrame.Locals)
          LoadArraysInScope("", expression);
    }

    private void LoadArraysInScope(string prefix, EnvDTE.Expression expression)
    {
      string expType = expression.Type.Replace("}", "");
      if (expType.EndsWith("]") || expType.EndsWith("[,]") || expType.EndsWith("[,,]") || expType.EndsWith("[,,,]"))
      {
        string item = prefix + expression.Name + " - " + expression.Value;
        arraysListBox.Items.Add(item);
        expressions.Add(item, expression);
      }
      else if (prefix == "")
      {
        foreach (EnvDTE.Expression subExpression in expression.DataMembers)
          LoadArraysInScope(prefix + expression.Name + ".", subExpression);
      }
    }

    #endregion

  }
}