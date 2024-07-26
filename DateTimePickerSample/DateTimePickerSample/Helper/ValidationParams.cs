using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DateTimePickerSample.Helper
{
    public class ValidationParams : DependencyObject
    {
        public object Param1
        {
            get { return (object)GetValue(Param1Property); }
            set { SetValue(Param1Property, value); }
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Param1Property =
            DependencyProperty.Register("Param1", typeof(object), typeof(ValidationParams), new PropertyMetadata(null));



        public object Param2
        {
            get { return (object)GetValue(Param2Property); }
            set { SetValue(Param2Property, value); }
        }

        // Using a DependencyProperty as the backing store for Param2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Param2Property =
            DependencyProperty.Register("Param2", typeof(object), typeof(ValidationParams), new PropertyMetadata(null));



        public object Param3
        {
            get { return (object)GetValue(Param3Property); }
            set { SetValue(Param3Property, value); }
        }

        // Using a DependencyProperty as the backing store for Param3.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Param3Property =
            DependencyProperty.Register("Param3", typeof(object), typeof(ValidationParams), new PropertyMetadata(null));



        public object Param4
        {
            get { return (object)GetValue(Param4Property); }
            set { SetValue(Param4Property, value); }
        }

        // Using a DependencyProperty as the backing store for Param4.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Param4Property =
            DependencyProperty.Register("Param4", typeof(object), typeof(ValidationParams), new PropertyMetadata(null));




    }

    public class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }


        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new PropertyMetadata(null));


    }
}
