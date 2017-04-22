using Aurochs.Desktop.ViewModels.AuthorizationViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Aurochs.Desktop.Views
{
    /// <summary>
    /// AuthorizationWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        private AuthViewModel VM { get; set; }

        public AuthorizationWindow()
        {
            InitializeComponent();
            this.DataContext = VM = new AuthViewModel();
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            VM.Password = passwordBox.Password;
        }
    }
}
