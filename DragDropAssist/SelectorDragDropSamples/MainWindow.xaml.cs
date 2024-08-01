using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SelectorDragDropSamples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VmWindow _vmwindow;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _vmwindow = new VmWindow();
            if (_vmwindow.ClassInfos != null)
                _vmwindow.ClassInfos.Clear();
            else
                _vmwindow.ClassInfos = new ObservableCollection<ClassInfo>();
            int si = 1;
            for (int i = 1; i <= 3; i++)
            {
                ClassInfo classInfo = new ClassInfo()
                {
                    ClassID = $"c{i}",
                    ClassName = $"班级{i}"
                };
                for (int p = 1; p <= 20; p++)
                {
                    Student student = new Student()
                    {
                        Id = si++,
                    };
                    student.Name = $"学生{student.Id}";
                    classInfo.Students.Add(student);
                    _vmwindow.Students.Add(student);
                }
                _vmwindow.ClassInfos.Add(classInfo);
            }

        }
    }

    public class VmWindow : ObservableObject
    {
        private ObservableCollection<ClassInfo> _classInfo;

        public ObservableCollection<ClassInfo> ClassInfos
        {
            get { return _classInfo; }
            set { Set(ref _classInfo, value); }
        }

        private ObservableCollection<Student> _students=new ObservableCollection<Student>();

        public ObservableCollection<Student> Students
        {
            get { return _students; }
            set { Set(ref _students, value);}
        }
    }

    
}
