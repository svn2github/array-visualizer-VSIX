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

namespace ArrayVisualizerExt
{
  /// <summary>
  /// Interaction logic for LargeArrayHandler.xaml
  /// </summary>
  public partial class LargeArrayHandler : UserControl
  {
    public event EventHandler<RoutedEventArgs> LoadArrayRequest;

    public LargeArrayHandler()
    {
      InitializeComponent();
    }

    public LargeArrayHandler(int itemsCount, int autoMax, int absoluteMax)
      : this()
    {
      string basemassage = "The selected array contains {0:N0} elements.\r\nFor performance reasons, the Array Visualizer will only display small arrays ({1:N0} elements or less) automatically.\r\n";
      if (itemsCount > absoluteMax)
      {
        ButtonLoadArray.Visibility = System.Windows.Visibility.Hidden;
        MessageBlock.Text = string.Format(basemassage + "You can force medium size arrays ({1:N0} to {2:N0} elements) to load manually.\r\n\r\nThis array is too large!", itemsCount, autoMax, absoluteMax);
      }
      else
      {
        ButtonLoadArray.Visibility = System.Windows.Visibility.Visible;
        MessageBlock.Text = string.Format(basemassage + "Click \"Load Array\" to load the array anyway.", itemsCount, autoMax);
      }
    }

    private void LoadArray_Click(object sender, RoutedEventArgs e)
    {
      OnLoadArrayRequest();
    }

    protected void OnLoadArrayRequest()
    {
      if (LoadArrayRequest != null)
        LoadArrayRequest(this, new RoutedEventArgs());
    }
  }
}
