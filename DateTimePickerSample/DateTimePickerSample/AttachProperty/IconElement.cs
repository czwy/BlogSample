using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace DateTimePickerSample
{
    public class IconElement
    {
        public static readonly DependencyProperty GeometryProperty = DependencyProperty.RegisterAttached(
            "Geometry", typeof(Geometry), typeof(IconElement), new PropertyMetadata(default(Geometry)));

        public static void SetGeometry(DependencyObject element, Geometry value)
        { element.SetValue(GeometryProperty, value); }

        public static Geometry GetGeometry(DependencyObject element)
        {
            return (Geometry)element.GetValue(GeometryProperty);
        }

        public static readonly DependencyProperty WidthProperty = DependencyProperty.RegisterAttached(
            "Width", typeof(double), typeof(IconElement), new PropertyMetadata(double.NaN));

        public static void SetWidth(DependencyObject element, double value)
        {
            element.SetValue(WidthProperty, value);
        }

        public static double GetWidth(DependencyObject element)
        {
            return (double)element.GetValue(WidthProperty);
        }

        public static readonly DependencyProperty HeightProperty = DependencyProperty.RegisterAttached(
            "Height", typeof(double), typeof(IconElement), new PropertyMetadata(double.NaN));

        public static void SetHeight(DependencyObject element, double value)
        {
            element.SetValue(HeightProperty, value);
        }

        public static double GetHeight(DependencyObject element)
        {
            return (double)element.GetValue(HeightProperty);
        }


        // Using a DependencyProperty as the backing store for NormalBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NormalBrushProperty =
            DependencyProperty.RegisterAttached("NormalBrush", typeof(Brush), typeof(IconElement), new PropertyMetadata(SystemColors.ControlTextBrush));

        public static Brush GetNormalBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(NormalBrushProperty);
        }

        public static void SetNormalBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(NormalBrushProperty, value);
        }


        // Using a DependencyProperty as the backing store for HoverBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverBrushProperty =
            DependencyProperty.RegisterAttached("HoverBrush", typeof(Brush), typeof(IconElement), new PropertyMetadata(SystemColors.ControlTextBrush));

        public static Brush GetHoverBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(HoverBrushProperty);
        }

        public static void SetHoverBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(HoverBrushProperty, value);
        }






    }
}
