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

namespace Microsoft.ArrayVisualizerExt
{
  /// <summary>
  /// Interaction logic for ArrayVisualizerToolControl.xaml
  /// </summary>
  public partial class ArrayVisualizerToolControl : UserControl
  {
    DTE2 dte;
    private Dictionary<string, EnvDTE.Expression> expressions;

    public ArrayVisualizerToolControl()
    {
      InitializeComponent();
      dte = Package.GetGlobalService(typeof(DTE)) as DTE2;
      dte.Events.DebuggerEvents.OnEnterBreakMode += new _dispDebuggerEvents_OnEnterBreakModeEventHandler(DebuggerEvents_OnEnterBreakMode);
    }

    void DebuggerEvents_OnEnterBreakMode(dbgEventReason Reason, ref dbgExecutionAction ExecutionAction)
    {
      LoadScopeArrays();
    }

    private void rotateButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void refreshButton_Click(object sender, RoutedEventArgs e)
    {
      LoadScopeArrays();
    }

    private void LoadScopeArrays()
    {
      arraysListBox.Items.Clear();
      expressions = new Dictionary<string, EnvDTE.Expression>();

      if (dte.Debugger.CurrentMode == dbgDebugMode.dbgBreakMode)
        foreach (EnvDTE.Expression expression in dte.Debugger.CurrentStackFrame.Locals)
        {
          string expType = expression.Type;
          if (expType.EndsWith("[,]") || expType.EndsWith("[,,]") || expType.EndsWith("[,,,]"))
          {
            string item = expression.Name + " - " + expression.Value;
            arraysListBox.Items.Add(item);
            expressions.Add(item, expression);
          }
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

          ArrayControl arrCtl;
          switch (dimenstions.Length)
          {
            case 2:
              arr = values.ToArray(dimenstions[0], dimenstions[1]);
              arrCtl = new Array2D();                             
              break;
            case 3:
              arr = values.ToArray(dimenstions[0], dimenstions[1], dimenstions[2]);
              arrCtl = new Array3D();                             
              break;
            case 4:
              arr = values.ToArray(dimenstions[0], dimenstions[1], dimenstions[2], dimenstions[3]);
              arrCtl = new Array4D();                             
              break;
            default:
              return;
          }

          arrCtl.Data = arr;
          mainPanel.Children.Add (arrCtl);

        }
      }
    }
  }
}