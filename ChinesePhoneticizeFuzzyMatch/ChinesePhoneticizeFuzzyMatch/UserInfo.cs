using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinesePhoneticizeFuzzyMatch
{
    public class UserInfo:ObservableObject
    {
        public string UserName { get; set; }
        public int Age { get; set; }

        private int _highlightStart;
        public int HighlightStart
        {
            get { return _highlightStart; }
            set { SetProperty(ref _highlightStart, value); }
        }

        private int _highlightCount;
        public int HighlightCount
        {
            get { return _highlightCount; }
            set { SetProperty(ref _highlightCount, value); }
        }
    }
}
