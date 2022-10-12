using RegistrationWPF.AdminPage;
using RegistrationWPF.ApplicationData;
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

namespace RegistrationWPF.PageMain
{
    /// <summary>
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();

            SetSortItems();

            LbUser.ItemsSource = FindUser();

            Application.Current.MainWindow.Title = "Просмотр пользователей";
        }

        User[] FindUser()
        {
            List<User> users = AppConnect.ModelOdb.User.ToList();

            var userAll = users;

            if (TbFinder.Text != null)
            {
                users = users.
                    Where(x => x.Login.ToLower().Contains(TbFinder.Text.ToLower()) 
                            ).ToList();

                switch (CbSort.SelectedIndex)
                {
                    case 0:
                        users = users.OrderBy(x => x.Person.Surname).ToList();
                        break;
                    case 1:
                        users = users.OrderByDescending(x => x.Person.Surname).ToList();
                        break;
                }
            }

            if (users.Count > 0)
            {
                TbFindeUsers.Text = "Найдено " + users.Count.ToString() + " из " + userAll.Count().ToString();
            }
            else
            {
                TbFindeUsers.Text = "Элементы не найдены";
            }

            return users.ToArray();
        }

        private void BtnChangeToUserTable_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.fraimMain.Navigate(new FurniturePage());
        }

        private void TbFinder_TextChanged(object sender, TextChangedEventArgs e)
        {
            LbUser.ItemsSource = FindUser();
        }

        private void LbProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbUser.SelectedItem != null && SelectedUser.user != null && SelectedUser.user.Role.Id == 1)
            {
                User user = LbUser.SelectedItem as User;
                NavigationService.Navigate(new AddEditUser(user));
            }
        }

        private void SetSortItems()
        {
            CbSort.Items.Add("< Нет >");
            CbSort.Items.Add("Стоимость по возрастанию");
            CbSort.Items.Add("Стоимость по убыванию");

            CbSort.SelectedIndex = 0;
        }

        private void CbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LbUser.ItemsSource = FindUser();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AppConnect.ModelOdb.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            LbUser.ItemsSource = FindUser();
        }

        private void BtnAddFurniture_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.fraimMain.Navigate(new AddEditUser((sender as Button).DataContext as User));
        }
    }
}
