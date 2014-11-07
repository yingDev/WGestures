// Dragging items in a ListView control with visual insertion guides
// http://www.cyotek.com/blog/dragging-items-in-a-listview-control-with-visual-insertion-guides
using System.Windows.Forms;

namespace WGestures.App.Gui.Windows.Controls
{
  /// <summary>
  /// Provides data for the <see cref="System.Windows.Forms.ListView.ItemDragDrop"/> event of the <see cref="System.Windows.Forms.ListView"/> control.
  /// </summary>
  public class ListViewItemDragEventArgs : CancelListViewItemDragEventArgs
  {
    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ListViewItemDragEventArgs"/> class.
    /// </summary>
    /// <param name="sourceItem">The <see cref="ListViewItem"/> that initiated the drag operation.</param>
    /// <param name="dropItem">The <see cref="ListViewItem"/> located at the mouse coordinates.</param>
    /// <param name="insertionIndex">The index of the the <see cref="ListViewItem"/> that is the target of the drag operation.</param>
    /// <param name="insertionMode">The relation of the <see cref="InsertionIndex"/>.</param>
    /// <param name="x">The x-coordinate of a mouse click, in pixels.</param>
    /// <param name="y">The y-coordinate of a mouse click, in pixels.</param>
    public ListViewItemDragEventArgs(ListViewItem sourceItem, ListViewItem dropItem, int insertionIndex, InsertionMode insertionMode, int x, int y)
      : this()
    {
      this.Item = sourceItem;
      this.DropItem = dropItem;
      this.X = x;
      this.Y = y;
      this.InsertionIndex = insertionIndex;
      this.InsertionMode = insertionMode;
    }

    #endregion

    #region Protected Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ListViewItemDragEventArgs"/> class.
    /// </summary>
    protected ListViewItemDragEventArgs()
    { }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the <see cref="ListViewItem"/> located at the <see cref="X"/> and <see cref="Y"/> coordinates.
    /// </summary>
    /// <value>The <see cref="ListViewItem"/> located at the mouse coordinates.</value>
    public ListViewItem DropItem { get; protected set; }

    /// <summary>
    /// Gets the insertion index of the drag operation.
    /// </summary>
    /// <value>The index of the the <see cref="System.Windows.Forms.ListViewItem"/> that is the target of the drag operation.</value>
    public int InsertionIndex { get; protected set; }

    /// <summary>
    /// Gets the relation of the <see cref="InsertionIndex"/>
    /// </summary>
    /// <value>The relation of the <see cref="InsertionIndex"/>.</value>
    public InsertionMode InsertionMode { get; protected set; }

    /// <summary>
    /// Gets the x-coordinate of the mouse during the generating event.
    /// </summary>
    /// <value>The x-coordinate of the mouse, in pixels.</value>
    public int X { get; protected set; }

    /// <summary>
    /// Gets the y-coordinate of the mouse during the generating event.
    /// </summary>
    /// <value>The y-coordinate of the mouse, in pixels.</value>
    public int Y { get; protected set; }

    #endregion
  }
}
