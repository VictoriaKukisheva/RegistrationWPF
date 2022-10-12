using RegistrationWPF.ApplicationData;
using RegistrationWPF.Admin;
using System;
using System.Collections;
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
    /// Логика взаимодействия для FurniturePage.xaml
    /// </summary>
    public partial class FurniturePage : Page
    {
        public FurniturePage()
        {
            InitializeComponent();

            ShowAdminFunctions();
            SetSortItems();
            SetFilterItems();

            LbFurniture.ItemsSource = FindFurniture();
        }

        Furniture[] FindFurniture()
        {
            List<Furniture> furnitures = AppConnect.ModelOdb.Furniture.ToList();

            var furnituresAll = furnitures;

            if (TbFinder.Text != null)
            {
                furnitures = furnitures.Where(x => x.Name.ToLower().Contains(TbFinder.Text.ToLower())).ToList();


                switch (CbSort.SelectedIndex)
                {
                    case 1:
                        furnitures = furnitures.OrderBy(x => x.Cost).ToList();
                        break;
                    case 2:
                        furnitures = furnitures.OrderByDescending(x => x.Cost).ToList();
                        break;
                }
            }

            if(CbFilter.SelectedIndex > 0)
            {
                furnitures = furnitures.Where(x => x.Producer.Name == CbFilter.SelectedItem.ToString()).ToList();
            }

            if (furnitures.Count > 0)
            {
                TbFindeFurniture.Text =  "Найдено " + furnitures.Count.ToString() + " из " + furnituresAll.Count().ToString();
            }
            else
            {
                TbFindeFurniture.Text = "Элементы не найдены";
            }

            return furnitures.ToArray();
        }

        private void SetSortItems()
        {
            CbSort.Items.Add("< Нет >");
            CbSort.Items.Add("Стоимость по возрастанию");
            CbSort.Items.Add("Стоимость по убыванию");

            CbSort.SelectedIndex = 0;
        }

        private void SetFilterItems()
        {
            CbFilter.Items.Add("< Поставщик >");
            foreach (var producer in AppConnect.ModelOdb.Producer)
            {
                CbFilter.Items.Add(producer.Name);
            }
            CbFilter.SelectedIndex = 0;
        }

        private void ShowAdminFunctions()
        {
            if(SelectedUser.user == null || SelectedUser.user.IdRole != 1)
			{
                BtnAddFurniture.Visibility = Visibility.Hidden;
                BtnChangeToUserTable.Visibility = Visibility.Hidden;    
			}
        }

        private void TbFinder_TextChanged(object sender, TextChangedEventArgs e)
        {
            LbFurniture.ItemsSource = FindFurniture();
        }

        private void CbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LbFurniture.ItemsSource = FindFurniture();
        }

        private void CbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LbFurniture.ItemsSource = FindFurniture();
        }

        private void LbProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbFurniture.SelectedItem != null && SelectedUser.user != null && SelectedUser.user.Role.Id == 1)
            {
                Furniture furniture = LbFurniture.SelectedItem as Furniture;
                NavigationService.Navigate(new Admin.AddFurniture(furniture));
            }
        }

        private void BtnAddFurniture_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddFurniture((sender as Button).DataContext as Furniture));
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            AppConnect.ModelOdb.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            LbFurniture.ItemsSource = FindFurniture();
        }

        private void BtnChangeToUserTable_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.fraimMain.Navigate(new UserPage());
        }
    }
}
