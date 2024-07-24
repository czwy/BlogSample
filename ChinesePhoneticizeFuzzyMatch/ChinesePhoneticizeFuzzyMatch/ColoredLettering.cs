using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace ChinesePhoneticizeFuzzyMatch
{
    public class ColoredLettering
    {
        public static void SetColorStart(TextBlock textElement, int value)
        {
            textElement.SetValue(ColorStartProperty, value);
        }

        public static int GetColorStart(TextBlock textElement)
        {
            return (int)textElement.GetValue(ColorStartProperty);
        }

        // Using a DependencyProperty as the backing store for ColorStart.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorStartProperty =
            DependencyProperty.RegisterAttached("ColorStart", typeof(int), typeof(ColoredLettering), new FrameworkPropertyMetadata(0, OnColorStartChanged));

        private static void OnColorStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBlock textBlock = d as TextBlock;
            if (textBlock != null)
            {
                if (e.NewValue == e.OldValue) return;
                if (e.NewValue is int)
                {
                    int count = GetColorLength(textBlock);
                    Brush brush = GetForeColor(textBlock);
                    if ((int)e.NewValue < 0 || count <= 0 || brush == TextBlock.ForegroundProperty.DefaultMetadata.DefaultValue) return;
                    if (textBlock.TextEffects.Count != 0)
                    {
                        textBlock.TextEffects.Clear();
                    }
                    TextEffect textEffect = new TextEffect()
                    {
                        Foreground = brush,
                        PositionStart = (int)e.NewValue,
                        PositionCount = count
                    };
                    textBlock.TextEffects.Add(textEffect);
                }
            }
        }

        public static void SetColorLength(TextBlock textElement, int value)
        {
            textElement.SetValue(ColorLengthProperty, value);
        }

        public static int GetColorLength(TextBlock textElement)
        {
            return (int)textElement.GetValue(ColorLengthProperty);
        }

        // Using a DependencyProperty as the backing store for ColorStart.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorLengthProperty =
            DependencyProperty.RegisterAttached("ColorLength", typeof(int), typeof(ColoredLettering), new FrameworkPropertyMetadata(0, OnColorLengthChanged));

        private static void OnColorLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBlock textBlock = d as TextBlock;
            if (textBlock != null)
            {
                if (e.NewValue == e.OldValue) return;
                if (e.NewValue is int)
                {
                    int start = GetColorStart(textBlock);
                    Brush brush = GetForeColor(textBlock);
                    if ((int)e.NewValue <= 0 || start < 0 || brush == TextBlock.ForegroundProperty.DefaultMetadata.DefaultValue) return;
                    if (textBlock.TextEffects.Count != 0)
                    {
                        textBlock.TextEffects.Clear();
                    }
                    TextEffect textEffect = new TextEffect()
                    {
                        Foreground = brush,
                        PositionStart = start,
                        PositionCount = (int)e.NewValue
                    };
                    textBlock.TextEffects.Add(textEffect);
                }
            }
        }

        public static void SetForeColor(TextBlock textElement, Brush value)
        {
            textElement.SetValue(ColorStartProperty, value);
        }

        public static Brush GetForeColor(TextBlock textElement)
        {
            return (Brush)textElement.GetValue(ForeColorProperty);
        }

        // Using a DependencyProperty as the backing store for ForeColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ForeColorProperty =
            DependencyProperty.RegisterAttached("ForeColor", typeof(Brush), typeof(ColoredLettering), new PropertyMetadata(TextBlock.ForegroundProperty.DefaultMetadata.DefaultValue, OnForeColorChanged));

        private static void OnForeColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBlock textBlock = d as TextBlock;
            if (textBlock != null)
            {
                if (e.NewValue == e.OldValue) return;
                if (e.NewValue is Brush)
                {
                    int start = GetColorStart(textBlock);
                    int count = GetColorLength(textBlock);
                    if (start < 0 || count <= 0) return;
                    if (textBlock.TextEffects.Count != 0)
                    {
                        textBlock.TextEffects.Clear();
                    }
                    TextEffect textEffect = new TextEffect()
                    {
                        Foreground = (Brush)e.NewValue,
                        PositionStart = start,
                        PositionCount = count
                    };
                    textBlock.TextEffects.Add(textEffect);
                }
            }
        }
    }
}
