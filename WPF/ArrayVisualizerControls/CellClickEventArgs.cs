using System.Windows;

namespace ArrayVisualizerControls
{
  public class CellClickEventArgs : RoutedEventArgs
  {
    public CellClickEventArgs()
      : base() { }

    public CellClickEventArgs(RoutedEvent routedEvent)
      : base(routedEvent) { }

    public CellClickEventArgs(RoutedEvent routedEvent, object source)
      : base(routedEvent, source) { }

    public CellClickEventArgs(object data, string toolTipPrefix, RoutedEvent routedEvent, object source)
      : base(routedEvent, source)
    {
      this.Data = data;
      this.ToolTipPrefix = toolTipPrefix;
    }

    public object Data { get; set; }
    public string ToolTipPrefix { get; set; }
  }
}
