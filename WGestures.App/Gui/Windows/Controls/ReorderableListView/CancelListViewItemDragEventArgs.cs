using System.ComponentModel;
using System.Windows.Forms;

// Dragging items in a ListView control with visual insertion guides
// http://www.cyotek.com/blog/dragging-items-in-a-listview-control-with-visual-insertion-guides

namespace WGestures.App.Gui.Windows.Controls
{
  /// <summary>
  /// Provides data for the <see cref="ReorderableListView.ItemDragging"/> event of the <see cref="ReorderableListView"/> control.
  /// </summary>
  public class CancelListViewItemDragEventArgs : CancelEventArgs
  {
    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CancelListViewItemDragEventArgs"/> class.
    /// </summary>
    /// <param name="item">The source <see cref="ListViewItem"/> the event data relates to.</param>
    public CancelListViewItemDragEventArgs(ListViewItem item)
    {
      this.Item = item;
    }

    #endregion

    #region Protected Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CancelListViewItemDragEventArgs"/> class.
    /// </summary>
    protected CancelListViewItemDragEventArgs()
    { }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the <see cref="ListViewItem"/> that is the source of the drag operation.
    /// </summary>
    /// <value>The <see cref="ListViewItem"/> that initiated the drag operation.</value>
    public ListViewItem Item { get; protected set; }

    #endregion
  }
}
