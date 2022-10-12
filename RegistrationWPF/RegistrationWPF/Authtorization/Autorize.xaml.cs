using RegistrationWPF.ApplicationData;
using RegistrationWPF.PageMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RegistrationWPF.Authtorization
{
	/// <summary>
	/// Логика взаимодействия для Autorize.xaml
	/// </summary>
	public partial class Autorize : Window
	{
		public Autorize()
		{
			InitializeComponent(); 
            AppConnect.ModelOdb = new Entities();
        }

        private void ButtonIN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userObj = AppConnect.ModelOdb.User.FirstOrDefault(x => x.Login == txtLogin.Text && x.Password == txtPassword.Password);

                if (userObj == null)
                {
                    MessageBox.Show("Такого пользователя нет!", "Ошибка при авторизации!",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    Capcha capcha = new Capcha();
                    capcha.ShowDialog();
                }
                else if (userObj.Login == txtLogin.Text && txtPassword.Password == null)
                {
                    MessageBox.Show("Пользователь с таким логином уже есть!", "Ошибка при авторизации!", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    SelectedUser.user = userObj;

                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    Close();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Ошибка " + Ex.Message.ToString() + "Критическая работа приложения", "Уведомление",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ButtonRegist_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            Close();
        }

        private void ShowFurniture_Click(object sender, RoutedEventArgs e)
        {
            SelectedUser.user = null;

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
	}
}
