using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectorDragDropSamples
{
    internal class MockData
    {
    }

    #region 模型
    public class School : ObservableObject
    {
        private bool _isOpen;

        /// <summary>
        /// 获取或设置是否展开
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public bool IsOpen
        {
            get { return _isOpen; }
            set { Set(ref _isOpen, value); }
        }

        private bool _isSelected;

        /// <summary>
        /// 获取或设置是否被选中
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                Set(ref _isSelected, value);
            }
        }
        public string SchoolID { get; set; }

        public string SchoolName { get; set; }

        public List<Grade> listGrade { get; set; } = new List<Grade>() { };
    }

    public class Grade : ObservableObject
    {
        private bool _isOpen;
        /// <summary>
        /// 获取或设置是否展开
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public bool IsOpen { get { return _isOpen; } set { Set(ref _isOpen, value); } }

        private bool _isSelected;
        /// <summary>
        /// 获取或设置是否被选中
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public bool IsSelected { get { return _isSelected; } set { Set(ref _isSelected, value); } }

        public string GradeID { get; set; }

        public string GradeName { get; set; }

        public List<ClassInfo> ListClass { get; set; } = new List<ClassInfo>() { };
    }

    public class ClassInfo : ObservableObject
    {
        private bool _isOpen;

        /// <summary>
        /// 获取或设置是否展开
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public bool IsOpen
        {
            get { return _isOpen; }
            set { Set(ref _isOpen, value); }
        }

        private bool _isSelected;

        /// <summary>
        /// 获取或设置是否被选中
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                Set(ref _isSelected, value);
            }
        }

        public string ClassID { get; set; }

        public string ClassName { get; set; }

        public ObservableCollection<Student> Students { get; set; } = new ObservableCollection<Student>() { };

    }

    public class Student : ObservableObject
    {
        private string _name;
        public string Name //{ get; set; }
        {
            get { return _name; }
            set
            {
                Set(ref _name, value);
            }
        }

        private int _Id;
        public int Id
        {
            get { return _Id; }
            set { _Id = value; RaisePropertyChanged(nameof(Name)); }
        }
    }
    #endregion
}
