using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinesePhoneticizeFuzzyMatch
{
    public class MainViewModel:ObservableObject
    {
        public List<UserInfo>? AllUser;

        private ObservableCollection<UserInfo> _candidateUser = new ObservableCollection<UserInfo>();

        public ObservableCollection<UserInfo> CandidateUser
        {
            get { return _candidateUser; }
            set { SetProperty(ref _candidateUser, value); }
        }

        private string? _searchStr;

        public string? SearchStr
        {
            get { return _searchStr; }
            set { SetProperty(ref _searchStr, value); }
        }

        private RelayCommand<UserInfo>? _addCandidateCommand;
        public RelayCommand<UserInfo> AddCandidateCommand
        {
            get
            {
                return _addCandidateCommand = _addCandidateCommand ?? new RelayCommand<UserInfo>((item) =>
                {
                    SearchStr = item?.UserName;
                });
            }
        }

        private RelayCommand<string>? _fuzzyMatchCommand;
        public RelayCommand<string> FuzzyMatchCommand
        {
            get
            {
                return _fuzzyMatchCommand = _fuzzyMatchCommand ?? new RelayCommand<string>((item) =>
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        CandidateUser = new ObservableCollection<UserInfo>(AllUser.Where(r =>
                        {
                            bool ret;
                            if (ret = CommonHelper.fuzzyMatchChar(r.UserName, item, out int start, out int count))
                            {
                                r.HighlightStart = start;
                                r.HighlightCount = count;
                            }
                            return ret;
                        }));
                    }
                    else
                    {
                        CandidateUser.Clear();
                    }
                });
            }
        }
    }
}
