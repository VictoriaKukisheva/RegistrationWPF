using RegistrationWPF.ApplicationData;
using RegistrationWPF.Authtorization;
using RegistrationWPF.PageMain;
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

namespace RegistrationWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            AppConnect.ModelOdb = new Entities();
            AppFrame.fraimMain = FrmMain;

            AppFrame.fraimMain.Navigate(new FurniturePage());

            if (SelectedUser.user == null)
            {
                UserName.Text += "Гость";
            }
            else
            {
                UserName.Text += SelectedUser.user.Login.ToString();
            }
        }

        private void Qiut_Btn_Click(object sender, RoutedEventArgs e)
        {
            SelectedUser.user = null;

            Autorize autorize = new Autorize();
            autorize.Show();
            this.Close();
        }
    }
}
