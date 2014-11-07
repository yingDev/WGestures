using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

// Dragging items in a ListView control with visual insertion guides
// http://www.cyotek.com/blog/dragging-items-in-a-listview-control-with-visual-insertion-guides

namespace WGestures.App.Gui.Windows.Controls
{
    public class ReorderableListView : System.Windows.Forms.ListView
    {
        #region Constants

        private const int WM_PAINT = 0xF;

        #endregion

        #region Instance Fields

        private bool _allowItemDrag;

        private Color _insertionLineColor;

        private DateTime _interval;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReorderableListView"/> class.
        /// </summary>
        public ReorderableListView()
        {
            this.DoubleBuffered = true;
            this.InsertionLineColor = Color.Red;
            this.InsertionIndex = -1;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the AllowItemDrag property value changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler AllowItemDragChanged;

        /// <summary>
        /// Occurs when the InsertionLineColor property value changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler InsertionLineColorChanged;

        /// <summary>
        /// Occurs when a drag-and-drop operation for an item is completed.
        /// </summary>
        [Category("Drag Drop")]
        public event EventHandler<ListViewItemDragEventArgs> ItemDragDrop;

        /// <summary>
        /// Occurs when the user begins dragging an item.
        /// </summary>
        [Category("Drag Drop")]
        public event EventHandler<CancelListViewItemDragEventArgs> ItemDragging;

        #endregion

        #region Overridden Methods

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.DragDrop" /> event.
        /// </summary>
        /// <param name="drgevent">A <see cref="T:System.Windows.Forms.DragEventArgs" /> that contains the event data.</param>
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            if (this.IsRowDragInProgress)
            {
                try
                {
                    ListViewItem dropItem;

                    dropItem = this.InsertionIndex != -1 ? this.Items[this.InsertionIndex] : null;

                    if (dropItem != null)
                    {
                        ListViewItem dragItem;
                        int dropIndex;

                        dragItem = (ListViewItem)drgevent.Data.GetData(typeof(ListViewItem));
                        if (dragItem == null) return;

                        dropIndex = dropItem.Index;

                        if (dragItem.Index < dropIndex)
                        {
                            dropIndex--;
                        }
                        if (this.InsertionMode == InsertionMode.After && dragItem.Index < this.Items.Count - 1)
                        {
                            dropIndex++;
                        }

                        if (dropIndex != dragItem.Index)
                        {
                            ListViewItemDragEventArgs args;
                            Point clientPoint;

                            clientPoint = this.PointToClient(new Point(drgevent.X, drgevent.Y));
                            args = new ListViewItemDragEventArgs(dragItem, dropItem, dropIndex, this.InsertionMode, clientPoint.X, clientPoint.Y);

                            this.OnItemDragDrop(args);

                            if (!args.Cancel)
                            {
                                BeginUpdate();
                                this.Items.Remove(dragItem);
                                this.Items.Insert(dropIndex, dragItem);
                                if (this.SelectedItem != dragItem) SelectedItem = dragItem;
                                EndUpdate();
                            }
                        }
                    }
                }
                finally
                {
                    this.InsertionIndex = -1;
                    this.IsRowDragInProgress = false;
                    this.Invalidate();
                }
            }

            base.OnDragDrop(drgevent);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.DragLeave" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnDragLeave(EventArgs e)
        {
            this.InsertionIndex = -1;
            this.Invalidate();

            base.OnDragLeave(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.DragOver" /> event.
        /// </summary>
        /// <param name="drgevent">A <see cref="T:System.Windows.Forms.DragEventArgs" /> that contains the event data.</param>
        protected override void OnDragOver(DragEventArgs drgevent)
        {
            if (this.IsRowDragInProgress)
            {
                int insertionIndex;
                InsertionMode insertionMode;
                ListViewItem dropItem;
                Point clientPoint;

                clientPoint = this.PointToClient(new Point(drgevent.X, drgevent.Y));
                dropItem = this.GetItemAt(0, Math.Min(clientPoint.Y, this.Items[this.Items.Count - 1].GetBounds(ItemBoundsPortion.Entire).Bottom - 1));

                if (dropItem != null)
                {
                    Rectangle bounds;

                    bounds = dropItem.GetBounds(ItemBoundsPortion.Entire);
                    insertionIndex = dropItem.Index;
                    insertionMode = clientPoint.Y < bounds.Top + (bounds.Height / 2) ? InsertionMode.Before : InsertionMode.After;

                    drgevent.Effect = DragDropEffects.Move;

                    //到达边界时滚动
                    var now = DateTime.UtcNow;
                    if (now - _interval > TimeSpan.FromMilliseconds(150))
                    {
                        EnsureVisible(dropItem.Index);
                        _interval = now;
                    }
                }
                else
                {
                    insertionIndex = -1;
                    insertionMode = this.InsertionMode;

                    drgevent.Effect = DragDropEffects.None;
                }

                if (insertionIndex != this.InsertionIndex || insertionMode != this.InsertionMode)
                {
                    this.InsertionMode = insertionMode;
                    this.InsertionIndex = insertionIndex;
                    this.Invalidate();
                }
            }

            base.OnDragOver(drgevent);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ListView.ItemDrag" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.Windows.Forms.ItemDragEventArgs" /> that contains the event data.</param>
        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            if (this.AllowItemDrag && this.Items.Count > 1)
            {
                CancelListViewItemDragEventArgs args;

                args = new CancelListViewItemDragEventArgs((ListViewItem)e.Item);

                this.OnItemDragging(args);

                if (!args.Cancel)
                {
                    this.IsRowDragInProgress = true;
                    this.DoDragDrop(e.Item, DragDropEffects.Move);
                }
            }

            base.OnItemDrag(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data. </param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        /// <summary>
        /// Overrides <see cref="M:System.Windows.Forms.Control.WndProc(System.Windows.Forms.Message@)" />.
        /// </summary>
        /// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
        [DebuggerStepThrough]
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            switch (m.Msg)
            {
                case WM_PAINT:
                    this.OnWmPaint(ref m);
                    break;
            }
        }

        #endregion

        #region Public Properties

        [Category("Behavior")]
        [DefaultValue(false)]
        public virtual bool AllowItemDrag
        {
            get { return _allowItemDrag; }
            set
            {
                if (this.AllowItemDrag != value)
                {
                    _allowItemDrag = value;

                    this.OnAllowItemDragChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the color of the insertion line drawn when dragging items within the control.
        /// </summary>
        /// <value>The color of the insertion line.</value>
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Red")]
        public virtual Color InsertionLineColor
        {
            get { return _insertionLineColor; }
            set
            {
                if (this.InsertionLineColor != value)
                {
                    _insertionLineColor = value;

                    this.OnInsertionLineColorChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected <see cref="ListViewItem"/>.
        /// </summary>
        /// <value>The selected <see cref="ListViewItem"/>.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ListViewItem SelectedItem
        {
            get { return this.SelectedItems.Count != 0 ? this.SelectedItems[0] : null; }
            set
            {
                this.SelectedItems.Clear();
                if (value != null)
                {
                    value.Selected = true;
                }
                this.FocusedItem = value;
            }
        }

        #endregion

        #region Protected Properties

        protected int InsertionIndex { get; set; }

        protected InsertionMode InsertionMode { get; set; }

        protected bool IsRowDragInProgress { get; set; }

        #endregion

        #region Protected Members

        /// <summary>
        /// Raises the <see cref="AllowItemDragChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnAllowItemDragChanged(EventArgs e)
        {
            EventHandler handler;

            handler = this.AllowItemDragChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="InsertionLineColorChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnInsertionLineColorChanged(EventArgs e)
        {
            EventHandler handler;

            handler = this.InsertionLineColorChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ItemDragDrop" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ListViewItemDragEventArgs" /> instance containing the event data.</param>
        protected virtual void OnItemDragDrop(ListViewItemDragEventArgs e)
        {
            EventHandler<ListViewItemDragEventArgs> handler;

            handler = this.ItemDragDrop;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ItemDragging" /> event.
        /// </summary>
        /// <param name="e">The <see cref="CancelListViewItemDragEventArgs" /> instance containing the event data.</param>
        protected virtual void OnItemDragging(CancelListViewItemDragEventArgs e)
        {
            EventHandler<CancelListViewItemDragEventArgs> handler;

            handler = this.ItemDragging;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnWmPaint(ref Message m)
        {
            this.DrawInsertionLine();
        }

        #endregion

        #region Private Members

        private void DrawInsertionLine()
        {
            if (this.InsertionIndex != -1)
            {
                int index;

                index = this.InsertionIndex;

                if (index >= 0 && index < this.Items.Count)
                {
                    Rectangle bounds;
                    int x;
                    int y;
                    int width;

                    bounds = this.Items[index].GetBounds(ItemBoundsPortion.Entire);
                    x = 0; // aways fit the line to the client area, regardless of how the user is scrolling
                    y = this.InsertionMode == InsertionMode.Before ? bounds.Top : bounds.Bottom;
                    width = Math.Min(bounds.Width - bounds.Left, this.ClientSize.Width); // again, make sure the full width fits in the client area

                    this.DrawInsertionLine(x, y, width);
                }
            }
        }

        private void DrawInsertionLine(int x1, int y, int width)
        {
            using (Graphics g = this.CreateGraphics())
            {
                Point[] leftArrowHead;
                Point[] rightArrowHead;
                int arrowHeadSize;
                int x2;

                x2 = x1 + width;
                arrowHeadSize = 7;
                leftArrowHead = new[]
                        {
                          new Point(x1, y - (arrowHeadSize / 2)), new Point(x1 + arrowHeadSize, y), new Point(x1, y + (arrowHeadSize / 2))
                        };
                rightArrowHead = new[]
                         {
                           new Point(x2, y - (arrowHeadSize / 2)), new Point(x2 - arrowHeadSize, y), new Point(x2, y + (arrowHeadSize / 2))
                         };

                using (Pen pen = new Pen(this.InsertionLineColor))
                {
                    g.DrawLine(pen, x1, y, x2 - 1, y);
                }

                using (Brush brush = new SolidBrush(this.InsertionLineColor))
                {
                    g.FillPolygon(brush, leftArrowHead);
                    g.FillPolygon(brush, rightArrowHead);
                }
            }
        }

        #endregion
    }
}
