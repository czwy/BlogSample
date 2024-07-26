using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;
using System.Globalization;

namespace DateTimePickerSample
{
    public class BorderElement
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
            "CornerRadius", typeof(CornerRadius), typeof(BorderElement), new FrameworkPropertyMetadata(default(CornerRadius), FrameworkPropertyMetadataOptions.Inherits));

        public static void SetCornerRadius(DependencyObject element, CornerRadius value)
        {
            element.SetValue(CornerRadiusProperty, value);
        }

        public static CornerRadius GetCornerRadius(DependencyObject element)
        {
            return (CornerRadius)element.GetValue(CornerRadiusProperty);
        }

        public static readonly DependencyProperty CircularProperty = DependencyProperty.RegisterAttached(
            "Circular", typeof(bool), typeof(BorderElement), new PropertyMetadata(false, OnCircularChanged));

        private static void OnCircularChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Border border = d as Border;
            if (d != null)
            {
                if ((bool)e.NewValue)
                {
                    var binding = new MultiBinding
                    {
                        Converter = new BorderCircularConverter()
                    };
                    binding.Bindings.Add(new Binding(FrameworkElement.ActualWidthProperty.Name) { Source = border });
                    binding.Bindings.Add(new Binding(FrameworkElement.ActualHeightProperty.Name) { Source = border });
                    border.SetBinding(Border.CornerRadiusProperty, binding);
                }
                else
                {
                    BindingOperations.ClearBinding(border, FrameworkElement.ActualWidthProperty);
                    BindingOperations.ClearBinding(border, FrameworkElement.ActualHeightProperty);
                    BindingOperations.ClearBinding(border, Border.CornerRadiusProperty);
                }
            }
        }

        public static void SetCircular(DependencyObject element, bool value)
        {
            element.SetValue(CircularProperty, value);
        }

        public static bool GetCircular(DependencyObject element)
        {
            return (bool)element.GetValue(CircularProperty);
        }

        // Using a DependencyProperty as the backing store for BorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.RegisterAttached("BorderBrush", typeof(Brush), typeof(BorderElement), new PropertyMetadata(Border.BorderBrushProperty.DefaultMetadata.DefaultValue));

        public static Brush GetBorderBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(BorderBrushProperty);
        }

        public static void SetBorderBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(BorderBrushProperty, value);
        }

        // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.RegisterAttached("Background", typeof(Brush), typeof(BorderElement), new PropertyMetadata(Panel.BackgroundProperty.DefaultMetadata.DefaultValue));

        public static Brush GetBackground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(BackgroundProperty);
        }

        public static void SetBackground(DependencyObject obj, Brush value)
        {
            obj.SetValue(BackgroundProperty, value);
        }




    }

    public class BorderCircularConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2) return DependencyProperty.UnsetValue;
            if (!(values[0] is double && values[1] is double)) return DependencyProperty.UnsetValue;
            double width = (double)values[0];
            double height = (double)values[1];

            if (width < double.Epsilon || height < double.Epsilon)
            {
                return new CornerRadius();
            }

            var min = Math.Min(width, height);
            return new CornerRadius(min / 2);

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
