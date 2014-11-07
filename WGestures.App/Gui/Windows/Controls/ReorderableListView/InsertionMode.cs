// Dragging items in a ListView control with visual insertion guides
// http://www.cyotek.com/blog/dragging-items-in-a-listview-control-with-visual-insertion-guides

namespace WGestures.App.Gui.Windows.Controls
{
  /// <summary>
  /// Determines the insertion mode of a drag operation
  /// </summary>
  public enum InsertionMode
  {
    /// <summary>
    /// The source item will be inserted before the destination item
    /// </summary>
    Before,

    /// <summary>
    /// The source item will be inserted after the destination item
    /// </summary>
    After
  }
}
