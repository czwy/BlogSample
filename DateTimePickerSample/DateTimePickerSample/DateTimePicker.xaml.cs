using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DateTimePickerSample
{
    /// <summary>
    /// DateTimePicker.xaml 的交互逻辑
    /// </summary>
    public partial class DateTimePicker : UserControl
    {
        private bool IsBackgroudSelect = false;

        public DateTime HoverStart
        {
            get { return (DateTime)GetValue(HoverStartProperty); }
            set { SetValue(HoverStartProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HoverStart.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverStartProperty =
            DependencyProperty.Register("HoverStart", typeof(DateTime), typeof(DateTimePicker), new PropertyMetadata(DateTime.Today, (d, e) =>
            {
                DateTimePicker dtp = d as DateTimePicker;
                if (e.NewValue == e.OldValue) return;
                dtp.startHours.SelectedItem = DateTime.Now.Hour;
                dtp.startMins.SelectedItem = DateTime.Now.Minute;
                if ((DateTime)e.OldValue == DateTime.Today)
                {
                    dtp.startHours.ItemsSource = Enumerable.Range(0, 24);
                    dtp.startMins.ItemsSource = Enumerable.Range(0, 60);
                }
                if ((DateTime)e.NewValue == DateTime.Today)
                {
                    dtp.startHours.ItemsSource = Enumerable.Range(DateTime.Now.Hour, 24 - DateTime.Now.Hour);
                    dtp.startMins.ItemsSource = Enumerable.Range(DateTime.Now.Minute, 60 - DateTime.Now.Minute);
                }
            }));



        public DateTime HoverEnd
        {
            get { return (DateTime)GetValue(HoverEndProperty); }
            set { SetValue(HoverEndProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HoverEnd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverEndProperty =
            DependencyProperty.Register("HoverEnd", typeof(DateTime), typeof(DateTimePicker), new PropertyMetadata(DateTime.Today.AddDays(1), (d, e) =>
            {
                DateTimePicker dtp = d as DateTimePicker;
                if (e.NewValue == e.OldValue) return;
                dtp.endHours.SelectedItem = DateTime.Now.Hour;
                dtp.endMins.SelectedItem = DateTime.Now.Minute;
                if ((DateTime)e.OldValue == DateTime.Today)
                {
                    dtp.endHours.ItemsSource = Enumerable.Range(0, 24);
                    dtp.endMins.ItemsSource = Enumerable.Range(0, 60);
                }
                if ((DateTime)e.NewValue == DateTime.Today)
                {
                    dtp.endHours.ItemsSource = Enumerable.Range(DateTime.Now.Hour, 24 - DateTime.Now.Hour);
                    dtp.endMins.ItemsSource = Enumerable.Range(DateTime.Now.Minute, 60 - DateTime.Now.Minute);
                }

            }));




        public DateTime DateTimeRangeStart
        {
            get { return (DateTime)GetValue(DateTimeRangeStartProperty); }
            set { SetValue(DateTimeRangeStartProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DateTimeRangeStart.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DateTimeRangeStartProperty =
            DependencyProperty.Register("DateTimeRangeStart", typeof(DateTime), typeof(DateTimePicker), new PropertyMetadata(DateTime.Now, (d, e) =>
            {
                DateTimePicker dtp = d as DateTimePicker;
                if (e.NewValue == null) return;
                DateTime newValue = (DateTime)e.NewValue;
                if (newValue.Date != DateTime.MinValue && newValue.Date != DateTime.MaxValue && newValue.Date >= DateTime.Today)
                {
                    dtp.HoverStart = newValue.Date;
                    dtp.startHours.SelectedItem = newValue.Hour;
                    dtp.startMins.SelectedItem = newValue.Minute;
                    dtp.IsBackgroudSelect = true;
                    if (dtp.DateTimeRangeEnd > newValue)
                    {
                        dtp.startCalendar.SelectedDates.AddRange(newValue, dtp.DateTimeRangeEnd);
                        dtp.endCalendar.SelectedDates.AddRange(newValue, dtp.DateTimeRangeEnd);
                    }
                }
            }));



        public DateTime DateTimeRangeEnd
        {
            get { return (DateTime)GetValue(DateTimeRangeEndProperty); }
            set { SetValue(DateTimeRangeEndProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DateTimeRangeEnd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DateTimeRangeEndProperty =
            DependencyProperty.Register("DateTimeRangeEnd", typeof(DateTime), typeof(DateTimePicker), new PropertyMetadata(DateTime.Now.AddDays(1), (d, e) =>
            {
                DateTimePicker dtp = d as DateTimePicker;
                if (e.NewValue == null) return;
                DateTime newValue = (DateTime)e.NewValue;
                if (newValue.Date != DateTime.MinValue && newValue.Date != DateTime.MaxValue && newValue.Date >= DateTime.Today)
                {
                    dtp.HoverEnd = newValue.Date;
                    dtp.endHours.SelectedItem = newValue.Hour;
                    dtp.endMins.SelectedItem = newValue.Minute;
                    dtp.IsBackgroudSelect = true;
                    if (newValue > dtp.DateTimeRangeStart)
                    {
                        dtp.endCalendar.SelectedDates.AddRange(dtp.DateTimeRangeStart, newValue);
                        dtp.startCalendar.SelectedDates.AddRange(dtp.DateTimeRangeStart, newValue);
                    }
                }
            }));



        public DateTimePicker()
        {
            InitializeComponent();
            endCalendar.DisplayDate = startCalendar.DisplayDate.AddMonths(1);
            startCalendar.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Now.AddDays(-1)));
            endCalendar.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Now.AddDays(-1)));
            startHours.ItemsSource = Enumerable.Range(DateTime.Now.Hour, 24 - DateTime.Now.Hour);
            startMins.ItemsSource = Enumerable.Range(DateTime.Now.Minute, 60 - DateTime.Now.Minute);
            startHours.SelectedItem = DateTime.Now.Hour;
            startMins.SelectedItem = DateTime.Now.Minute;
            endHours.ItemsSource = Enumerable.Range(0, 24);
            endMins.ItemsSource = Enumerable.Range(0, 60);
            endHours.SelectedItem = DateTime.Now.AddDays(1).Hour;
            endMins.SelectedItem = DateTime.Now.AddDays(1).Minute;
            IsBackgroudSelect = true;
            startCalendar.SelectedDates.AddRange(DateTimeRangeStart.Date, DateTimeRangeEnd.Date);
            endCalendar.SelectedDates.AddRange(DateTimeRangeStart.Date, DateTimeRangeEnd.Date);
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsBackgroudSelect) return;
            System.Windows.Controls.Calendar c = sender as System.Windows.Controls.Calendar;
            if (HoverStart != DateTime.MinValue && HoverEnd != DateTime.MinValue)
            {
                HoverStart = HoverEnd = DateTime.MinValue;
                IsBackgroudSelect = true;
                startCalendar.SelectedDates.Clear();
                endCalendar.SelectedDates.Clear();
            }
            if (c != null)
            {
                if (e.AddedItems.Count == 1)
                {//单击时
                    if (e.RemovedItems.Count == 0)
                    {
                        if (HoverStart == DateTime.MinValue)
                            HoverStart = (DateTime)e.AddedItems[0];
                        else
                        {
                            IsBackgroudSelect = true;
                            if ((DateTime)e.AddedItems[0] > HoverStart)
                                HoverEnd = (DateTime)e.AddedItems[0];
                            else
                            {
                                HoverEnd = HoverStart;
                                HoverStart = (DateTime)e.AddedItems[0];
                            }
                            startCalendar.SelectedDates.AddRange(HoverStart, HoverEnd);
                            endCalendar.SelectedDates.AddRange(HoverStart, HoverEnd);
                        }
                    }
                    else if (e.RemovedItems.Count == 1)
                    {
                        if (DateTime.Compare((DateTime)e.RemovedItems[0], (DateTime)e.AddedItems[0]) > 0)
                        {
                            HoverStart = (DateTime)e.AddedItems[0];
                            HoverEnd = (DateTime)e.RemovedItems[0];
                        }
                        else
                        {
                            HoverStart = (DateTime)e.RemovedItems[0];
                            HoverEnd = (DateTime)e.AddedItems[0];
                        }
                        IsBackgroudSelect = true;
                        startCalendar.SelectedDates.AddRange(HoverStart, HoverEnd);
                        endCalendar.SelectedDates.AddRange(HoverStart, HoverEnd);
                    }
                    else
                    {
                        HoverStart = (DateTime)e.AddedItems[0];
                        HoverEnd = DateTime.MinValue;
                    }
                }
                else
                {//拖选操作
                    c.SelectedDates.Clear();
                }
            }
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            IsBackgroudSelect = false;
            if (Mouse.Captured is CalendarItem)
                Mouse.Capture(null);
        }

        private void yesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (HoverStart == default(DateTime) || HoverEnd == default(DateTime)) return;
            DatetimePopup.IsOpen = false;
            DateTimeRangeStart = HoverStart.AddHours((int)startHours.SelectedItem).AddMinutes((int)startMins.SelectedItem);
            DateTimeRangeEnd = HoverEnd.AddHours((int)endHours.SelectedItem).AddMinutes((int)endMins.SelectedItem);

        }

        private void WatermarkTextBox_GotMouseCapture(object sender, MouseEventArgs e)
        {
            DatetimePopup.IsOpen = true;
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            DatetimePopup.IsOpen = true;
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            startCalendar.SelectedDates.Clear();
            startHours.SelectedIndex = 0;
            startMins.SelectedIndex = 0;
            endCalendar.SelectedDates.Clear();
            endHours.SelectedIndex = 0;
            endMins.SelectedIndex = 0;
            HoverStart = HoverEnd = default(DateTime);
        }

        private void startHours_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HoverStart.Equals(DateTime.Today) && ((int)e.AddedItems[0] == DateTime.Now.Hour))
            {
                startMins.ItemsSource = Enumerable.Range(DateTime.Now.Minute, 60 - DateTime.Now.Minute);
            }
            else
            {
                startMins.ItemsSource = Enumerable.Range(0, 60);
            }
        }
    }

    public class SelectedDatesConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType.Name == "CornerRadius")
            {
                if (values.Length < 3) return new CornerRadius(0);
                if (values[0].Equals(values[1])) return new CornerRadius(13, 0, 0, 13);
                else if (values[0].Equals(values[2])) return new CornerRadius(0, 13, 13, 0);
                else return new CornerRadius(0);
            }
            else
            {
                if (values.Length < 3) return Visibility.Collapsed;
                if ((values[0].Equals(values[1]) || values[0].Equals(values[2])) && System.Convert.ToBoolean(values[3]) == false) return Visibility.Visible;
                else return Visibility.Collapsed;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DateTimeAddtionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int monthstep = System.Convert.ToInt32(parameter);
            return ((DateTime)value).AddMonths(monthstep);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime)value).AddMonths(-System.Convert.ToInt32(parameter));
        }
    }
}
