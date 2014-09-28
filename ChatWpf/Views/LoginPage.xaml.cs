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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatWpf.Views {
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page {
        public LoginPage() {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrWhiteSpace(txtUserName.Text)) {
                lblLoginResult.Text = "Please enter a user name.";
            } else if (string.IsNullOrWhiteSpace(txtPassword.Password)) {
                lblLoginResult.Text = "Please enter a password.";
            } else {
                lblLoginResult.Text = "Login executed";
                await Task.Delay(2000);
                lblLoginResult.Text = "";
            }
        }
    }
}
