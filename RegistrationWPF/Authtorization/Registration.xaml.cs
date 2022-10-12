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
using System.Windows.Shapes;

namespace RegistrationWPF.Authtorization
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
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

        private void Regist_Btn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            TextBox[] textBox = { SurnameTxt, NameTxt, SurnameTxt1, MailTxt, PhoneTxt, LoginTxt };
            PasswordBox[] passwordBox = { PasswordTxt, DblPasswordTxt };

            foreach (TextBox txt in textBox)
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    MessageBox.Show("Не все поля заполнены");
                    break;
                }
                txt.Text = txt.Text.Replace(" ", "");
            }

            foreach (PasswordBox pass in passwordBox)
            {
                pass.Password = pass.Password.Replace(" ","");
            }

            if (string.IsNullOrWhiteSpace(PasswordTxt.Password))
                errors.AppendLine("Укажите пароль");
            if (AppConnect.ModelOdb.User.Count(x => x.Login == LoginTxt.Text) > 0
                || AppConnect.ModelOdb.User.Count(x => x.Mail == MailTxt.Text) > 0)
                errors.AppendLine("Пользователь с таким логином уже есть");
            if (!Regex.IsMatch(MailTxt.Text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$") && !string.IsNullOrEmpty(MailTxt.Text))
                errors.AppendLine("Неправильно указана почта");
            if (!Regex.IsMatch(PasswordTxt.Password, @"((?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z])[0-9!@#$%^&*a-zA-Z]{6,})") 
                && !string.IsNullOrEmpty(PasswordTxt.Password))
                errors.AppendLine("Введите корректный пароль");
            if ((PhoneTxt.Text.Length != 11 || !PhoneTxt.Text.StartsWith("7"))
                && !string.IsNullOrEmpty(PhoneTxt.Text))
                errors.AppendLine("Номер телефона начинается с 7 и не больше 11 символов");
            if (calendar.SelectedDate > DateTime.Now.AddYears(-18) 
                || calendar.SelectedDate <= DateTime.Now.AddYears(-99))
                errors.AppendLine("Регистрироваться могут совершеннолетние граждане и люди до 99 лет");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                Person personObj = new Person()
                {
                    Surname = SurnameTxt.Text,
                    Name = NameTxt.Text,
                    Secondname = SurnameTxt1.Text,
                    Date = DateTime.Parse(calendar.SelectedDate.ToString())
                };

                AppConnect.ModelOdb.Person.Add(personObj);
                AppConnect.ModelOdb.SaveChanges();
               
                int ert =  (from m in AppConnect.ModelOdb.Person select m.Id).ToList().Last();

                User userObj = new User()
                {
                    Login = LoginTxt.Text,
                    Password = PasswordTxt.Password.ToString(),
                    IdPerson = ert,
                    IdRole = 3,
                    Mail = MailTxt.Text,
                    Phone = Convert.ToInt64(PhoneTxt.Text).ToString(Mask[Country.Ru])
                };

                AppConnect.ModelOdb.User.Add(userObj);
                AppConnect.ModelOdb.SaveChanges();

                MessageBox.Show("Данные успешно добавлены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);


                SelectedUser.user = userObj;

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();

                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении данных! "+ex.Message, "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                DblPasswordTxt.Clear();
            }
        }

        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            Autorize autorize = new Autorize();
            autorize.Show();
            Close();
        }

        private void DblPasswordTxt_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordTxt.Password != DblPasswordTxt.Password)
            {
                Regist_Btn.IsEnabled = false;
                DblPasswordTxt.Background = Brushes.LightCoral;
                DblPasswordTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                Regist_Btn.IsEnabled = true;
                DblPasswordTxt.Background = Brushes.LightGreen;
                DblPasswordTxt.BorderBrush = Brushes.Green;
            }
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
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-ZА-яа-я]"))
            {
                e.Handled = true;
            }
        }

        private void PhoneTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[0-9]"))
            {
                e.Handled = true;
            }
        }
    }
}
