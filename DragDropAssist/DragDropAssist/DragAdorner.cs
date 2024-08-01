using System;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;

namespace DragDropAssist
{
    /// <summary>
    /// Renders a visual which can follow the mouse cursor, 
    /// such as during a drag-and-drop operation.
    /// </summary>
    public class DragAdorner : Adorner
    {
        #region Data

        private Rectangle child = null;
        private double offsetLeft = 0;
        private double offsetTop = 0;
        /// <summary>
        /// ¹ÊÊÂ°æ
        /// </summary>
        private Storyboard m_Sb;
        private UIElement itemToDrag;
        private double oldItemToDragOpacity;

        private static readonly Brush storkeBrush = GetStorkeBrush();
        private static readonly DropShadowEffect storkeEffect = GetStorkeEffect();
        private const double itemToDragOpacity = 0.5d;


        private static LinearGradientBrush GetStorkeBrush()
        {
            LinearGradientBrush ret = new LinearGradientBrush();
            ret.GradientStops.Add(new GradientStop { Color = Colors.Red, Offset = 0d });
            ret.GradientStops.Add(new GradientStop { Color = Colors.Orange, Offset = 1.0d / 6.0d });
            ret.GradientStops.Add(new GradientStop { Color = Colors.Yellow, Offset = 2.0d / 6.0d });
            ret.GradientStops.Add(new GradientStop { Color = Colors.Green, Offset = 3.0d / 6.0d });
            ret.GradientStops.Add(new GradientStop { Color = Colors.Blue, Offset = 4.0d / 6.0d });
            ret.GradientStops.Add(new GradientStop { Color = Colors.Indigo, Offset = 5.0d / 6.0d });
            ret.GradientStops.Add(new GradientStop { Color = Colors.Purple, Offset = 1.0d });
            ret.StartPoint = new Point(0, 0);
            ret.EndPoint = new Point(1d, 1d);
            if (ret.CanFreeze)
                ret.Freeze();
            return ret;
        }

        private static DropShadowEffect GetStorkeEffect()
        {
            DropShadowEffect ret = new DropShadowEffect
            {
                Color = (Color)ColorConverter.ConvertFromString("#c3c3c3"),
                BlurRadius = 6d,
                ShadowDepth = 0d,
                Opacity = 0.8d,
                RenderingBias = RenderingBias.Performance
            };
            if (ret.CanFreeze)
                ret.Freeze();
            return ret;
        }
        #endregion // Data

        #region Constructor

        /// <summary>
        /// Initializes a new instance of DragVisualAdorner.
        /// </summary>
        /// <param name="adornedElement">The element being adorned.</param>
        /// <param name="size">The size of the adorner.</param>
        /// <param name="brush">A brush to with which to paint the adorner.</param>
        public DragAdorner(UIElement adornedElement, Size size, Brush brush, UIElement itemToDrag)
            : base(adornedElement)
        {
            this.itemToDrag = itemToDrag;
            this.oldItemToDragOpacity = this.itemToDrag.Opacity;
            Rectangle rect = new Rectangle();
            rect.StrokeThickness = 2;
            rect.RadiusY = rect.RadiusX = 3d;

            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext context = drawingVisual.RenderOpen())
            {
                context.DrawRectangle(brush, null, new Rect(size));
                context.Close();
            }

            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromHwnd(IntPtr.Zero))
            {
                PresentationSource source = PresentationSource.FromVisual(itemToDrag);
                var dpix = graphics.DpiX * source.CompositionTarget.TransformToDevice.M11;
                var dpiy = graphics.DpiY * source.CompositionTarget.TransformToDevice.M22;
                var bitmap = new System.Windows.Media.Imaging.RenderTargetBitmap((int)size.Width, (int)size.Height, dpix, dpiy, PixelFormats.Pbgra32);
                bitmap.Render(drawingVisual);
                rect.Fill = new ImageBrush(bitmap) { Stretch = Stretch.None };
            };

            rect.Width = size.Width;
            rect.Height = size.Height;
            rect.IsHitTestVisible = false;
            this.child = rect;

            IsVisibleChanged -= DragAdorner_IsVisibleChanged;
            IsVisibleChanged += DragAdorner_IsVisibleChanged;
        }

        private void DragAdorner_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (((bool)e.NewValue) == false)
            {
                if (child != null)
                {
                    m_Sb.Remove(child);
                    m_Sb = null;
                    child.Effect = null;
                }
                if (this.itemToDrag != null)
                    this.itemToDrag.Opacity = oldItemToDragOpacity;
            }
            else if (child != null && m_Sb == null)
            {
                if (this.itemToDrag != null)
                    this.itemToDrag.Opacity = itemToDragOpacity;

                child.StrokeDashArray = new DoubleCollection(new double[] { (child.Width + child.Height) * 2 + 10, (child.Width + child.Height) * 2 });
                child.Stroke = storkeBrush;
                child.Effect = storkeEffect;

                m_Sb = new Storyboard();
                DoubleAnimation da = new DoubleAnimation();
                da.From = (child.Width + child.Height) * 2 + 10;
                da.To = 0;
                da.Duration = new Duration(TimeSpan.FromSeconds(1.5d));
                da.FillBehavior = FillBehavior.HoldEnd;
                Storyboard.SetTarget(da, child);
                Storyboard.SetTargetProperty(da, new PropertyPath(Rectangle.StrokeDashOffsetProperty));
                m_Sb.Children.Add(da);
                m_Sb.RepeatBehavior = RepeatBehavior.Forever;
                m_Sb.Begin(child, true);
            }
        }

        #endregion // Constructor

        #region Public Interface

        #region GetDesiredTransform

        /// <summary>
        /// Override.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            GeneralTransformGroup result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(this.offsetLeft, this.offsetTop));
            return result;
        }

        #endregion // GetDesiredTransform

        #region OffsetLeft

        /// <summary>
        /// Gets/sets the horizontal offset of the adorner.
        /// </summary>
        public double OffsetLeft
        {
            get { return this.offsetLeft; }
            set
            {
                this.offsetLeft = value;
                UpdateLocation();
            }
        }

        #endregion // OffsetLeft

        #region SetOffsets

        /// <summary>
        /// Updates the location of the adorner in one atomic operation.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        public void SetOffsets(double left, double top)
        {
            this.offsetLeft = left;
            this.offsetTop = top;
            this.UpdateLocation();
        }

        #endregion // SetOffsets

        #region OffsetTop

        /// <summary>
        /// Gets/sets the vertical offset of the adorner.
        /// </summary>
        public double OffsetTop
        {
            get { return this.offsetTop; }
            set
            {
                this.offsetTop = value;
                UpdateLocation();
            }
        }

        #endregion // OffsetTop

        #endregion // Public Interface

        #region Protected Overrides

        /// <summary>
        /// Override.
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size constraint)
        {
            this.child.Measure(constraint);
            return this.child.DesiredSize;
        }

        /// <summary>
        /// Override.
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            this.child.Arrange(new Rect(finalSize));
            return finalSize;
        }

        /// <summary>
        /// Override.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override Visual GetVisualChild(int index)
        {
            return this.child;
        }

        /// <summary>
        /// Override.  Always returns 1.
        /// </summary>
        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        #endregion // Protected Overrides

        #region Private Helpers

        private void UpdateLocation()
        {
            AdornerLayer adornerLayer = this.Parent as AdornerLayer;
            if (adornerLayer != null)
                adornerLayer.Update(this.AdornedElement);
        }

        #endregion // Private Helpers
    }
}
