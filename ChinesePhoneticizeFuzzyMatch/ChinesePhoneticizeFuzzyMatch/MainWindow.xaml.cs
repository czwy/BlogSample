using Bogus;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChinesePhoneticizeFuzzyMatch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext  = new MainViewModel() {
                AllUser = new Faker<UserInfo>("zh_CN")
                .RuleFor(s => s.UserName, f => f.Name.LastName() + f.Name.FirstName()).Generate(1000).Distinct().ToList()
            };
        }
    }
}