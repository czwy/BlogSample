using DateTimePickerSample.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows;

namespace DateTimePickerSample
{
    /// <summary>
    /// 水印效果的文本输入框
    /// </summary>
    public class WatermarkTextBox : TextBox
    {
        public enum AutoSelectBehaviorEnum
        {
            Never,
            OnFocus,
        }



        public PlacementMode ErrorInfoPlacement
        {
            get { return (PlacementMode)GetValue(ErrorInfoPlacementProperty); }
            set { SetValue(ErrorInfoPlacementProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ErrorInfoPlacement.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorInfoPlacementProperty =
            DependencyProperty.Register("ErrorInfoPlacement", typeof(PlacementMode), typeof(WatermarkTextBox), new PropertyMetadata(PlacementMode.Right));



        /// <summary>
        /// 是否显示清除按钮
        /// </summary>
        public bool ShowClearButton
        {
            get { return (bool)GetValue(ShowClearButtonProperty); }
            set { SetValue(ShowClearButtonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowClearButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowClearButtonProperty =
            DependencyProperty.Register("ShowClearButton", typeof(bool), typeof(WatermarkTextBox), new PropertyMetadata(false));

        public AutoSelectBehaviorEnum AutoSelectBehavior
        {
            get { return (AutoSelectBehaviorEnum)GetValue(AutoSelectBehaviorProperty); }
            set { SetValue(AutoSelectBehaviorProperty, value); }
        }

        public static readonly DependencyProperty AutoSelectBehaviorProperty =
            DependencyProperty.Register("AutoSelectBehavior", typeof(AutoSelectBehaviorEnum), typeof(WatermarkTextBox), new PropertyMetadata(AutoSelectBehaviorEnum.Never, (dd, ee) =>
            {
                WatermarkTextBox textBox = dd as WatermarkTextBox;
                textBox.OnAutoSelectBehaviorChanged((AutoSelectBehaviorEnum)ee.NewValue);
            }));

        static WatermarkTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WatermarkTextBox), new FrameworkPropertyMetadata(typeof(WatermarkTextBox)));
        }

        public WatermarkTextBox()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Clear, (s, e) =>
            {
                SetCurrentValue(TextProperty, "");
            }));
        }

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(string), typeof(WatermarkTextBox), new PropertyMetadata(null));

        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        public DataTemplate WatermarkTemplate
        {
            get { return (DataTemplate)GetValue(WatermarkTemplateProperty); }
            set { SetValue(WatermarkTemplateProperty, value); }
        }

        public static readonly DependencyProperty WatermarkTemplateProperty =
            DependencyProperty.Register("WatermarkTemplate", typeof(DataTemplate), typeof(WatermarkTextBox), new PropertyMetadata(null));

        private void OnAutoSelectBehaviorChanged(AutoSelectBehaviorEnum value)
        {
            LostFocus -= WatermarkTextBox_LostFocus;
            GotFocus -= WatermarkTextBox_GotFocus;

            if (value == AutoSelectBehaviorEnum.OnFocus)
            {
                LostFocus += WatermarkTextBox_LostFocus;
                GotFocus += WatermarkTextBox_GotFocus;
            }
        }

        private void WatermarkTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Text.Length > 0)
                Select(0, 0);
        }

        private void WatermarkTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(delegate
            {
                if (Text.Length > 0)
                    Select(0, 0);
                SelectAll();
            }));
        }

    }
}
