using System;
using System.Collections;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;

namespace DragDropAssist
{
    public class SelectorDragDrop
    {
        #region Data

        bool canInitiateDrag;
        DragAdorner dragAdorner;
        double dragAdornerOpacity;
        int indexToSelect;
        bool isDragInProgress;
        object itemUnderDragCursor;
        Selector selector;
        Point ptMouseDown;
        bool showDragAdorner;

        #endregion // Data

        #region Constructors

        /// <summary>
        /// Initializes a new instance of selectorDragManager.
        /// </summary>
        public SelectorDragDrop()
        {
            this.canInitiateDrag = false;
            this.dragAdornerOpacity = 1d;
            this.indexToSelect = -1;
            this.showDragAdorner = true;
        }

        /// <summary>
        /// Initializes a new instance of selectorDragManager.
        /// </summary>
        /// <param name="selector"></param>
        public SelectorDragDrop(Selector selector)
            : this()
        {
            this.Selector = selector;
        }

        /// <summary>
        /// Initializes a new instance of selectorDragManager.
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="dragAdornerOpacity"></param>
        public SelectorDragDrop(Selector selector, double dragAdornerOpacity)
            : this(selector)
        {
            this.DragAdornerOpacity = dragAdornerOpacity;
        }

        /// <summary>
        /// Initializes a new instance of selectorDragManager.
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="showDragAdorner"></param>
        public SelectorDragDrop(Selector selector, bool showDragAdorner)
            : this(selector)
        {
            this.ShowDragAdorner = showDragAdorner;
        }

        #endregion // Constructors

        #region Public Interface

        #region DragAdornerOpacity

        /// <summary>
        /// Gets/sets the opacity of the drag adorner.  This property has no
        /// effect if ShowDragAdorner is false. The default value is 0.7
        /// </summary>
        public double DragAdornerOpacity
        {
            get { return this.dragAdornerOpacity; }
            set
            {
                if (this.IsDragInProgress)
                    throw new InvalidOperationException("Cannot set the DragAdornerOpacity property during a drag operation.");

                if (value < 0.0 || value > 1.0)
                    throw new ArgumentOutOfRangeException("DragAdornerOpacity", value, "Must be between 0 and 1.");

                this.dragAdornerOpacity = value;
            }
        }

        #endregion // DragAdornerOpacity

        #region IsDragInProgress

        /// <summary>
        /// Returns true if there is currently a drag operation being managed.
        /// </summary>
        public bool IsDragInProgress
        {
            get { return this.isDragInProgress; }
            private set { this.isDragInProgress = value; }
        }

        #endregion // IsDragInProgress

        #region Selector

        /// <summary>
        /// Gets/sets the selector whose dragging is managed.  This property
        /// can be set to null, to prevent drag management from occuring.  If
        /// the selector's AllowDrop property is false, it will be set to true.
        /// </summary>
        public Selector Selector
        {
            get { return selector; }
            set
            {
                if (this.IsDragInProgress)
                    throw new InvalidOperationException("Cannot set the selector property during a drag operation.");

                if (this.selector != null)
                {
                    #region Unhook Events

                    this.selector.PreviewMouseLeftButtonDown -= selector_PreviewMouseLeftButtonDown;
                    this.selector.PreviewMouseMove -= selector_PreviewMouseMove;
                    this.selector.DragOver -= selector_DragOver;
                    this.selector.DragLeave -= selector_DragLeave;
                    this.selector.DragEnter -= selector_DragEnter;
                    this.selector.Drop -= selector_Drop;

                    #endregion // Unhook Events
                }

                this.selector = value;

                if (this.selector != null)
                {
                    if (!this.selector.AllowDrop)
                        this.selector.AllowDrop = true;

                    #region Hook Events

                    this.selector.PreviewMouseLeftButtonDown += selector_PreviewMouseLeftButtonDown;
                    this.selector.PreviewMouseMove += selector_PreviewMouseMove;
                    this.selector.DragOver += selector_DragOver;
                    this.selector.DragLeave += selector_DragLeave;
                    this.selector.DragEnter += selector_DragEnter;
                    this.selector.Drop += selector_Drop;

                    #endregion // Hook Events
                }
            }
        }

        #endregion // selector

        #region ProcessDrop [event]

        /// <summary>
        /// Raised when a drop occurs.  By default the dropped item will be moved
        /// to the target index.  Handle this event if relocating the dropped item
        /// requires custom behavior.  Note, if this event is handled the default
        /// item dropping logic will not occur.
        /// </summary>
        public event EventHandler<ProcessDropEventArgs> ProcessDrop;

        #endregion // ProcessDrop [event]

        #region ShowDragAdorner

        /// <summary>
        /// Gets/sets whether a visual representation of the SelectorItem being dragged
        /// follows the mouse cursor during a drag operation.  The default value is true.
        /// </summary>
        public bool ShowDragAdorner
        {
            get { return this.showDragAdorner; }
            set
            {
                if (this.IsDragInProgress)
                    throw new InvalidOperationException("Cannot set the ShowDragAdorner property during a drag operation.");

                this.showDragAdorner = value;
            }
        }

        #endregion // ShowDragAdorner

        #endregion // Public Interface

        #region Event Handling Methods

        #region selector_PreviewMouseLeftButtonDown

        void selector_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.IsMouseOverScrollbar)
            {
                //Set the flag to false when cursor is over scrollbar.
                this.canInitiateDrag = false;
                return;
            }

            int index = this.IndexUnderDragCursor;
            //Debug.WriteLine(string.Format("this.IndexUnderDragCursor:{0}", index.ToString()));
            this.canInitiateDrag = index > -1;

            if (this.canInitiateDrag)
            {
                // Remember the location and index of the SelectorItem the user clicked on for later.
                this.ptMouseDown = GetMousePosition(this.selector);
                this.indexToSelect = index;
            }
            else
            {
                this.ptMouseDown = new Point(-10000, -10000);
                this.indexToSelect = -1;
            }
        }

        #endregion // selector_PreviewMouseLeftButtonDown

        #region selector_PreviewMouseMove

        void selector_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!this.CanStartDragOperation)
                return;

            // Select the item the user clicked on.
            if (this.selector.SelectedIndex != this.indexToSelect)
                this.selector.SelectedIndex = this.indexToSelect;

            // If the item at the selected index is null, there's nothing
            // we can do, so just return;
            if (this.selector.SelectedItem == null)
                return;

            UIElement itemToDrag = this.GetSelectorItem(this.selector.SelectedIndex);
            if (itemToDrag == null)
                return;

            AdornerLayer adornerLayer = this.ShowDragAdornerResolved ? this.InitializeAdornerLayer(itemToDrag) : null;

            this.InitializeDragOperation(itemToDrag);
            this.PerformDragOperation();
            this.FinishDragOperation(itemToDrag, adornerLayer);
        }

        #endregion // selector_PreviewMouseMove

        #region selector_DragOver

        void selector_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;

            if (this.ShowDragAdornerResolved)
                this.UpdateDragAdornerLocation();

            // Update the item which is known to be currently under the drag cursor.
            int index = this.IndexUnderDragCursor;
            this.ItemUnderDragCursor = index < 0 ? null : this.Selector.Items[index];
        }

        #endregion // selector_DragOver

        #region selector_DragLeave

        void selector_DragLeave(object sender, DragEventArgs e)
        {
            if (!this.IsMouseOver(this.selector))
            {
                if (this.ItemUnderDragCursor != null)
                    this.ItemUnderDragCursor = null;

                if (this.dragAdorner != null)
                {
                    this.dragAdorner.Visibility = Visibility.Collapsed;
                }
            }
        }

        #endregion // selector_DragLeave

        #region selector_DragEnter

        void selector_DragEnter(object sender, DragEventArgs e)
        {
            if (this.dragAdorner != null && this.dragAdorner.Visibility != Visibility.Visible)
            {
                // Update the location of the adorner and then show it.				
                this.UpdateDragAdornerLocation();
                this.dragAdorner.Visibility = Visibility.Visible;
            }
        }

        #endregion // selector_DragEnter

        #region selector_Drop

        void selector_Drop(object sender, DragEventArgs e)
        {
            if (this.ItemUnderDragCursor != null)
                this.ItemUnderDragCursor = null;

            e.Effects = DragDropEffects.None;

            var itemsSource = this.selector.ItemsSource;
            if (itemsSource == null) return;

            int itemsCount = 0;
            Type type = null;
            foreach (object obj in itemsSource)
            {
                type = obj.GetType();
                itemsCount++;
            }

            if (itemsCount < 1) return;
            if (!e.Data.GetDataPresent(type))
                return;

            // Get the data object which was dropped.
            object data = e.Data.GetData(type);
            if (data == null)
                return;

            int oldIndex = -1;
            int index = 0;
            foreach (object obj in itemsSource)
            {
                if (obj == data)
                {
                    oldIndex = index;
                    break;
                }
                index++;
            }
            int newIndex = this.IndexUnderDragCursor;

            if (newIndex < 0)
            {
                // The drag started somewhere else, and our selector is empty
                // so make the new item the first in the list.
                if (itemsCount == 0)
                    newIndex = 0;

                // The drag started somewhere else, but our selector has items
                // so make the new item the last in the list.
                else if (oldIndex < 0)
                    newIndex = itemsCount;

                // The user is trying to drop an item from our selector into
                // our selector, but the mouse is not over an item, so don't
                // let them drop it.
                else
                    return;
            }

            // Dropping an item back onto itself is not considered an actual 'drop'.
            if (oldIndex == newIndex)
                return;

            if (this.ProcessDrop != null)
            {
                // Let the client code process the drop.
                ProcessDropEventArgs args = new ProcessDropEventArgs(itemsSource, data, oldIndex, newIndex, e.AllowedEffects);
                this.ProcessDrop(this, args);
                e.Effects = args.Effects;
            }
            else
            {
                dynamic dItemsSource = itemsSource;
                // Move the dragged data object from it's original index to the
                // new index (according to where the mouse cursor is).  If it was
                // not previously in the ListBox, then insert the item.
                if (oldIndex > -1)
                    dItemsSource.Move(oldIndex, newIndex);
                else
                    dItemsSource.Insert(newIndex, data);

                // Set the Effects property so that the call to DoDragDrop will return 'Move'.
                e.Effects = DragDropEffects.Move;
            }
        }

        #endregion // selector_Drop

        #endregion // Event Handling Methods

        #region Private Helpers

        #region CanStartDragOperation

        bool CanStartDragOperation
        {
            get
            {
                if (Mouse.LeftButton != MouseButtonState.Pressed)
                    return false;

                if (!this.canInitiateDrag)
                    return false;

                if (this.indexToSelect == -1)
                    return false;

                if (!this.HasCursorLeftDragThreshold)
                    return false;

                return true;
            }
        }

        #endregion // CanStartDragOperation

        #region FinishDragOperation

        void FinishDragOperation(UIElement draggedItem, AdornerLayer adornerLayer)
        {
            // Let the SelectorItem know that it is not being dragged anymore.
            SelectorItemDragState.SetIsBeingDragged(draggedItem, false);

            this.IsDragInProgress = false;

            if (this.ItemUnderDragCursor != null)
                this.ItemUnderDragCursor = null;

            // Remove the drag adorner from the adorner layer.
            if (adornerLayer != null)
            {
                adornerLayer.Remove(this.dragAdorner);
                this.dragAdorner = null;
            }
        }

        #endregion // FinishDragOperation

        #region GetSelectorItem

        UIElement GetSelectorItem(int index)
        {
            if (this.selector.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                return null;

            return this.selector.ItemContainerGenerator.ContainerFromIndex(index) as UIElement;
        }

        UIElement GetSelectorItem(object dataItem)
        {
            if (this.selector.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                return null;

            return this.selector.ItemContainerGenerator.ContainerFromItem(dataItem) as UIElement;
        }

        #endregion // GetSelectorItem

        #region HasCursorLeftDragThreshold

        bool HasCursorLeftDragThreshold
        {
            get
            {
                if (this.indexToSelect < 0)
                    return false;

                var item = this.GetSelectorItem(this.indexToSelect);
                if (item == null) return false;
                Rect bounds = VisualTreeHelper.GetDescendantBounds(item);
                Point ptInItem = this.selector.TranslatePoint(this.ptMouseDown, item);

                // In case the cursor is at the very top or bottom of the SelectorItem
                // we want to make the vertical threshold very small so that dragging
                // over an adjacent item does not select it.
                double topOffset = Math.Abs(ptInItem.Y);
                double btmOffset = Math.Abs(bounds.Height - ptInItem.Y);
                double vertOffset = Math.Min(topOffset, btmOffset);

                double width = SystemParameters.MinimumHorizontalDragDistance * 2;
                double height = Math.Min(SystemParameters.MinimumVerticalDragDistance, vertOffset) * 2;
                Size szThreshold = new Size(width, height);

                Rect rect = new Rect(this.ptMouseDown, szThreshold);
                rect.Offset(szThreshold.Width / -2, szThreshold.Height / -2);
                Point ptInselector = GetMousePosition(this.selector);
                return !rect.Contains(ptInselector);
            }
        }

        #endregion // HasCursorLeftDragThreshold

        #region IndexUnderDragCursor

        /// <summary>
        /// Returns the index of the SelectorItem underneath the
        /// drag cursor, or -1 if the cursor is not over an item.
        /// </summary>
        int IndexUnderDragCursor
        {
            get
            {
                //int index = -1;
                //for (int i = 0; i < this.selector.Items.Count; ++i)
                //{
                //    var item = this.GetSelectorItem(i);
                //    if (item != null&& this.IsMouseOver(item))
                //    {
                //        index = i;
                //        break;
                //    }
                //}
                //return index;
                Point ptMouse = GetMousePosition(this.selector);
                HitTestResult res = VisualTreeHelper.HitTest(this.selector, ptMouse);
                if (res == null)
                    return -1;

                DependencyObject depObj = res.VisualHit;
                while (depObj != null)
                {
                    if (depObj is ListBoxItem || depObj is TabItem || depObj is ComboBoxItem)
                    {
                        return this.selector.Items.IndexOf((depObj as FrameworkElement).DataContext);
                    }

                    if (depObj is Visual || depObj is System.Windows.Media.Media3D.Visual3D)
                        depObj = VisualTreeHelper.GetParent(depObj);
                    else
                        depObj = LogicalTreeHelper.GetParent(depObj);
                }

                return -1;
            }
        }

        #endregion // IndexUnderDragCursor

        #region InitializeAdornerLayer

        AdornerLayer InitializeAdornerLayer(UIElement itemToDrag)
        {
            // Create a brush which will paint the SelectorItem onto
            // a visual in the adorner layer.
            VisualBrush brush = new VisualBrush(itemToDrag);

            // Create an element which displays the source item while it is dragged.
            this.dragAdorner = new DragAdorner(this.selector, itemToDrag.RenderSize, brush, itemToDrag);

            // Set the drag adorner's opacity.		
            this.dragAdorner.Opacity = this.DragAdornerOpacity;

            AdornerLayer layer = AdornerLayer.GetAdornerLayer(this.selector);
            layer.Add(dragAdorner);

            // Save the location of the cursor when the left mouse button was pressed.
            this.ptMouseDown = GetMousePosition(this.selector);
            return layer;
        }

        #endregion // InitializeAdornerLayer

        #region InitializeDragOperation

        void InitializeDragOperation(UIElement itemToDrag)
        {
            // Set some flags used during the drag operation.
            this.IsDragInProgress = true;
            this.canInitiateDrag = false;

            // Let the SelectorItem know that it is being dragged.
            SelectorItemDragState.SetIsBeingDragged(itemToDrag, true);
        }

        #endregion // InitializeDragOperation

        #region IsMouseOver

        bool IsMouseOver(Visual target)
        {
            // We need to use WindowNativeMethods to figure out the cursor
            // coordinates because, during a drag-drop operation, the WPF
            // mechanisms for getting the coordinates behave strangely.

            Rect bounds = VisualTreeHelper.GetDescendantBounds(target);
            //Debug.WriteLine(string.Format("Left:{0}--Top:{1}--Width：{2}--Height：{3}", bounds.Left.ToString(), bounds.Top.ToString(), bounds.Width.ToString(), bounds.Height.ToString()));
            Point mousePos = GetMousePosition(target);
            //Debug.WriteLine(string.Format("X:{0}--Y:{1}", mousePos.X.ToString(), mousePos.Y.ToString()));
            return bounds.Contains(mousePos);
        }

        #endregion // IsMouseOver

        #region IsMouseOverScrollbar

        /// <summary>
        /// Returns true if the mouse cursor is over a scrollbar in the selector.
        /// </summary>
        bool IsMouseOverScrollbar
        {
            get
            {
                Point ptMouse = GetMousePosition(this.selector);
                HitTestResult res = VisualTreeHelper.HitTest(this.selector, ptMouse);
                if (res == null)
                    return false;

                DependencyObject depObj = res.VisualHit;
                while (depObj != null)
                {
                    if (depObj is ScrollBar)
                        return true;

                    // VisualTreeHelper works with objects of type Visual or Visual3D.
                    // If the current object is not derived from Visual or Visual3D,
                    // then use the LogicalTreeHelper to find the parent element.
                    if (depObj is Visual || depObj is System.Windows.Media.Media3D.Visual3D)
                        depObj = VisualTreeHelper.GetParent(depObj);
                    else
                        depObj = LogicalTreeHelper.GetParent(depObj);
                }

                return false;
            }
        }

        #endregion // IsMouseOverScrollbar

        #region ItemUnderDragCursor

        object ItemUnderDragCursor
        {
            get { return this.itemUnderDragCursor; }
            set
            {
                if (this.itemUnderDragCursor == value)
                    return;

                // The first pass handles the previous item under the cursor.
                // The second pass handles the new one.
                for (int i = 0; i < 2; ++i)
                {
                    if (i == 1)
                        this.itemUnderDragCursor = value;

                    if (this.itemUnderDragCursor != null)
                    {
                        UIElement selectorItem = this.GetSelectorItem(this.itemUnderDragCursor);
                        if (selectorItem != null)
                            SelectorItemDragState.SetIsUnderDragCursor(selectorItem, i == 1);
                    }
                }
            }
        }

        #endregion // ItemUnderDragCursor

        #region PerformDragOperation

        void PerformDragOperation()
        {
            object selectedItem = this.selector.SelectedItem;
            DragDropEffects allowedEffects = DragDropEffects.Move | DragDropEffects.Move | DragDropEffects.Link;
            if (DragDrop.DoDragDrop(this.selector, selectedItem, allowedEffects) != DragDropEffects.None)
            {
                // The item was dropped into a new location,
                // so make it the new selected item.
                this.selector.SelectedItem = selectedItem;
            }
        }

        #endregion // PerformDragOperation

        #region ShowDragAdornerResolved

        bool ShowDragAdornerResolved
        {
            get { return this.ShowDragAdorner && this.DragAdornerOpacity > 0.0; }
        }

        #endregion // ShowDragAdornerResolved

        #region UpdateDragAdornerLocation

        void UpdateDragAdornerLocation()
        {
            if (this.dragAdorner != null)
            {
                Point ptCursor = GetMousePosition(this.Selector);
                // 1/3/2018 - Made the top and left offset relative to the item being dragged.
                UIElement itemBeingDragged = this.GetSelectorItem(this.indexToSelect);
                Point itemLoc = itemBeingDragged.TranslatePoint(new Point(0, 0), this.Selector);
                double left = itemLoc.X + ptCursor.X - this.ptMouseDown.X;
                double top = itemLoc.Y + ptCursor.Y - this.ptMouseDown.Y;

                this.dragAdorner.SetOffsets(left, top);
            }
        }

        #endregion // UpdateDragAdornerLocation

        /// <summary>
        /// Returns the mouse cursor location.  This method is necessary during 
        /// a drag-drop operation because the WPF mechanisms for retrieving the
        /// cursor coordinates are unreliable.
        /// </summary>
        /// <param name="relativeTo">The Visual to which the mouse coordinates will be relative.</param>
        Point GetMousePosition(Visual relativeTo)
        {
            Win32Point mouse = new Win32Point();
            WindowNativeMethods.GetCursorPos(ref mouse);

            // Using PointFromScreen instead of Dan Crevier's code (commented out below)
            // is a bug fix created by William J. Roberts.  Read his comments about the fix
            // here: http://www.codeproject.com/useritems/ListViewDragDropManager.asp?msg=1911611#xx1911611xx
            Point FromScreen = relativeTo.PointFromScreen(new Point((double)mouse.X, (double)mouse.Y));
            //if(relativeTo is ListBoxItem lsbi)
            //{
            //    Debug.WriteLine(JsonSerializer.Serialize(lsbi.DataContext));
            //}
            Debug.WriteLine(string.Format("navite.X:{0}--navite.Y:{1}--FromScreen.X：{2}--FromScreen.Y：{3}", mouse.X.ToString(), mouse.Y.ToString(), FromScreen.X.ToString(), FromScreen.Y.ToString()));
            return FromScreen;//=relativeTo.PointFromScreen(new Point((double)mouse.X, (double)mouse.Y));
        }

        #endregion // Private Helpers
    }

    #region SelectorItemDragState

    /// <summary>
    /// Exposes attached properties used in conjunction with the selectorDragDropManager class.
    /// Those properties can be used to allow triggers to modify the appearance of SelectorItems
    /// in a selector during a drag-drop operation.
    /// </summary>
    internal static class SelectorItemDragState
    {
        #region IsBeingDragged

        /// <summary>
        /// Identifies the SelectorItemDragState's IsBeingDragged attached property.  
        /// This field is read-only.
        /// </summary>
        internal static readonly DependencyProperty IsBeingDraggedProperty =
            DependencyProperty.RegisterAttached(
                "IsBeingDragged",
                typeof(bool),
                typeof(SelectorItemDragState),
                new UIPropertyMetadata(false));

        /// <summary>
        /// Returns true if the specified SelectorItem is being dragged, else false.
        /// </summary>
        /// <param name="item">The SelectorItem to check.</param>
        internal static bool GetIsBeingDragged(UIElement item)
        {
            return (bool)item.GetValue(IsBeingDraggedProperty);
        }

        /// <summary>
        /// Sets the IsBeingDragged attached property for the specified SelectorItem.
        /// </summary>
        /// <param name="item">The SelectorItem to set the property on.</param>
        /// <param name="value">Pass true if the element is being dragged, else false.</param>
        internal static void SetIsBeingDragged(UIElement item, bool value)
        {
            item.SetValue(IsBeingDraggedProperty, value);
        }

        #endregion // IsBeingDragged

        #region IsUnderDragCursor

        /// <summary>
        /// Identifies the SelectorItemDragState's IsUnderDragCursor attached property.  
        /// This field is read-only.
        /// </summary>
        internal static readonly DependencyProperty IsUnderDragCursorProperty =
            DependencyProperty.RegisterAttached(
                "IsUnderDragCursor",
                typeof(bool),
                typeof(SelectorItemDragState),
                new UIPropertyMetadata(false));

        /// <summary>
        /// Returns true if the specified SelectorItem is currently underneath the cursor 
        /// during a drag-drop operation, else false.
        /// </summary>
        /// <param name="item">The SelectorItem to check.</param>
        internal static bool GetIsUnderDragCursor(UIElement item)
        {
            return (bool)item.GetValue(IsUnderDragCursorProperty);
        }

        /// <summary>
        /// Sets the IsUnderDragCursor attached property for the specified SelectorItem.
        /// </summary>
        /// <param name="item">The SelectorItem to set the property on.</param>
        /// <param name="value">Pass true if the element is underneath the drag cursor, else false.</param>
        internal static void SetIsUnderDragCursor(UIElement item, bool value)
        {
            item.SetValue(IsUnderDragCursorProperty, value);
        }

        #endregion // IsUnderDragCursor
    }

    #endregion // SelectorItemDragState


    #region ProcessDropEventArgs

    /// <summary>
    /// Event arguments used by the selectorDragDropManager.ProcessDrop event.
    /// </summary>
    /// <typeparam name="ItemType">The type of data object being dropped.</typeparam>
    public class ProcessDropEventArgs : EventArgs
    {
        #region Data
        IEnumerable itemsSource;
        object dataItem;
        int oldIndex;
        int newIndex;
        DragDropEffects allowedEffects = DragDropEffects.None;
        DragDropEffects effects = DragDropEffects.None;

        #endregion // Data

        #region Constructor

        internal ProcessDropEventArgs(
            IEnumerable itemsSource,
            object dataItem,
            int oldIndex,
            int newIndex,
            DragDropEffects allowedEffects)
        {
            this.itemsSource = itemsSource;
            this.dataItem = dataItem;
            this.oldIndex = oldIndex;
            this.newIndex = newIndex;
            this.allowedEffects = allowedEffects;
        }

        #endregion // Constructor

        #region Public Properties

        /// <summary>
        /// The items source of the selector where the drop occurred.
        /// </summary>
        public IEnumerable ItemsSource
        {
            get { return this.itemsSource; }
        }

        /// <summary>
        /// The data object which was dropped.
        /// </summary>
        public object DataItem
        {
            get { return this.dataItem; }
        }

        /// <summary>
        /// The current index of the data item being dropped, in the ItemsSource collection.
        /// </summary>
        public int OldIndex
        {
            get { return this.oldIndex; }
        }

        /// <summary>
        /// The target index of the data item being dropped, in the ItemsSource collection.
        /// </summary>
        public int NewIndex
        {
            get { return this.newIndex; }
        }

        /// <summary>
        /// The drag drop effects allowed to be performed.
        /// </summary>
        public DragDropEffects AllowedEffects
        {
            get { return allowedEffects; }
        }

        /// <summary>
        /// The drag drop effect(s) performed on the dropped item.
        /// </summary>
        public DragDropEffects Effects
        {
            get { return effects; }
            set { effects = value; }
        }

        #endregion // Public Properties
    }

    #endregion // ProcessDropEventArgs
}
