using RegistrationWPF.ApplicationData;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RegistrationWPF.AdminPage
{
    /// <summary>
    /// Логика взаимодействия для AddEditUser.xaml
    /// </summary>
    public partial class AddEditUser : Page
    {
        private User _user = new User();
        private Person _person = new Person();

        public AddEditUser(User user)
        {
            InitializeComponent();
            SetFilterRolesItems();

            if (user != null)
            {
                _user = user;
                user.Phone = user.Phone.Replace("-", "");
                user.Phone = user.Phone.Replace("+", "");
                FindFilterRoleId();
            }
            else
            {
                DeleteBtn.Visibility = Visibility.Hidden;
                AddEditBtn.Content = "Добавить";
            }
            
            DataContext = _user;
        }

        enum Country
        {
            Ru,
            Blr
        }

        static Dictionary<Country, string> Mask = new Dictionary<Country, string>
        {
            [Country.Ru] = "+#-###-###-##-##",
            [Country.Blr] = "+###(##)###-##-##"
        };

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            AppConnect.ModelOdb.User.Remove(_user);
            AppConnect.ModelOdb.SaveChanges();
            NavigationService.GoBack();
        }

        private void SetFilterRolesItems()
        {
            foreach (var role in AppConnect.ModelOdb.Role)
            {
                Roles.Items.Add(role.Name);
            }
        }

        private void FindFilterRoleId()
        {
            var roleUser = AppConnect.ModelOdb.Role.FirstOrDefault(x => x.Id == _user.IdRole);

            Roles.SelectedItem = roleUser.Name;
        }

        private void FindFilterRoleItem()
        {
            var roleUser = AppConnect.ModelOdb.Role.FirstOrDefault(x => x.Name == Roles.SelectedItem.ToString());
            if (roleUser == null)
            {
                MessageBox.Show("Такого элемента нет!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                _user.IdRole = roleUser.Id;
            }
        }

        private void AddEditBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            TextBox[] textBox = { SurnameTxt, NameTxt, SurnameTxt1, MailTxt, PhoneTxt, LoginTxt };

            foreach (TextBox txt in textBox)
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    MessageBox.Show("Не все поля заполнены", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(PasswordTxt.Password))
                errors.AppendLine("Укажите пароль");
            if (!Regex.IsMatch(MailTxt.Text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$") && !string.IsNullOrEmpty(MailTxt.Text))
                errors.AppendLine("Неправильно указана почта");
            if (!Regex.IsMatch(PasswordTxt.Password, @"((?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z])[0-9!@#$%^&*a-zA-Z]{6,})")
                && !string.IsNullOrEmpty(PasswordTxt.Password))
                errors.AppendLine("Введите корректный пароль");
            if (PhoneTxt.Text.Length > 12 || PhoneTxt.Text.Length < 11 || !PhoneTxt.Text.StartsWith("7")
                && !string.IsNullOrEmpty(PhoneTxt.Text))
                errors.AppendLine("Номер телефона начинается с 7 и больше 11 символов");
            if (datePicker.SelectedDate > DateTime.Now.AddYears(-18)
                || datePicker.SelectedDate <= DateTime.Now.AddYears(-99))
                errors.AppendLine("Регистрироваться могут совершеннолетние граждане и люди до 99 лет");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            FindFilterRoleItem();
            try
            {
                Person personObj = new Person()
            {
                Surname = SurnameTxt.Text,
                Name = NameTxt.Text,
                Secondname = SurnameTxt1.Text,
                Date = DateTime.Parse(datePicker.SelectedDate.ToString())
            };

            Entities.GetContext().SaveChanges();
            AppConnect.ModelOdb.SaveChanges();

            if (_user.Id == 0)
                {
                    AppConnect.ModelOdb.Person.Add(personObj);
                    _user.IdPerson = 1 + (from m in Entities.GetContext().Person select m.Id).ToList().Last() ;
                }

            User userObj = new User()
            {
                Login = LoginTxt.Text,
                Password = PasswordTxt.Password.ToString(),
                IdPerson = _user.IdPerson,
                IdRole = _user.IdRole,
                Mail = MailTxt.Text,
                Phone = Convert.ToInt64(PhoneTxt.Text).ToString(Mask[Country.Ru])
            };

            _user.Phone = Convert.ToInt64(PhoneTxt.Text).ToString(Mask[Country.Ru]);

                if (_user.Id == 0)
                {
                    AppConnect.ModelOdb.User.Add(userObj);
                }
            
                Entities.GetContext().SaveChanges();
                AppConnect.ModelOdb.SaveChanges();

                MessageBox.Show("Данные успешно добавлены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                AppFrame.fraimMain.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении данных!" + ex.Message, "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                DblPasswordTxt.Clear();
            }
        }

        private void DblPasswordTxt_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordTxt.Password != DblPasswordTxt.Password)
            {
                AddEditBtn.IsEnabled = false;
                DblPasswordTxt.Background = Brushes.LightCoral;
                DblPasswordTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                AddEditBtn.IsEnabled = true;
                DblPasswordTxt.Background = Brushes.LightGreen;
                DblPasswordTxt.BorderBrush = Brushes.Green;
            }
        }

        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.fraimMain.GoBack();
        }

        private void SurnameTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Zа-яА-Я]"))
            {
                e.Handled = true;
            }
        }

        private void NameTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Zа-яА-Я]"))
            {
                e.Handled = true;
            }
        }

        private void SurnameTxt1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Zа-яА-Я]"))
            {
                e.Handled = true;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
