using Microsoft.Win32;
using RegistrationWPF.ApplicationData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace RegistrationWPF.Admin
{
    /// <summary>
    /// Логика взаимодействия для AddFurniture.xaml
    /// </summary>
    public partial class AddFurniture : Page
    {
		private Furniture _furniture = new Furniture();

        public AddFurniture(Furniture furniture)
        {
            InitializeComponent();
			Application.Current.MainWindow.Title = "Добавление/изменение мебели";

			//this.furniture = furniture;
			SetFilterTypeItems();
            SetFilterSupplierItems();
			SetFilterProducerItems();
			if (furniture != null)
			{
				_furniture = furniture;
				
				FindFilterTypeId();
				FindFilterProducerId();
				FindFilterSupllierId();
			}
			else
			{
				FilterProducer.SelectedIndex = 0;
				FilterSupplier.SelectedIndex = 0;
				FilterType.SelectedIndex = 0;
				DeleteData.Visibility = Visibility.Hidden;
				AddData.Content = "Добавить мебель";
			}

			DataContext = _furniture;
        }

		private void SetFilterProducerItems()
		{
			foreach (var producers in AppConnect.ModelOdb.Producer)
			{
				FilterProducer.Items.Add(producers.Name);
			}
		}

		private void SetFilterSupplierItems()
		{
			foreach (var suppliers in AppConnect.ModelOdb.Supplier)
			{
				FilterSupplier.Items.Add(suppliers.Name);
			}
		}

		private void SetFilterTypeItems()
		{
			foreach (var types in AppConnect.ModelOdb.Type)
			{
                FilterType.Items.Add(types.Name);
			}
		}

		private void FindFilterTypeId()
        {
			var typeFurniture = 
				AppConnect.ModelOdb.Type.FirstOrDefault(x => x.Id == _furniture.IdType);

			FilterType.SelectedItem=typeFurniture.Name;
        }

		private void FindFilterProducerId()
		{
			var producerFurniture = AppConnect.ModelOdb.Producer.FirstOrDefault(x => x.Id == _furniture.IdProducer);

			FilterProducer.SelectedItem = producerFurniture.Name;
		}

		private void FindFilterSupllierId()
		{
			var supplierFurniture = AppConnect.ModelOdb.Supplier.FirstOrDefault(x => x.Id == _furniture.IdSupplier);

			FilterSupplier.SelectedItem = supplierFurniture.Name;
		}                                                                                                                                     

		private void FindFilterTypeItem()
        {
			var typeFirniture = AppConnect.ModelOdb.Type.FirstOrDefault(x => x.Name == FilterType.SelectedItem.ToString());
			if (typeFirniture == null)
			{
				MessageBox.Show("Такого элемента нет!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				_furniture.IdType = typeFirniture.Id;
			}
        }

		private void FindFilterProducerItem()
		{
			var producerFirniture = AppConnect.ModelOdb.Producer.FirstOrDefault(x => x.Name == FilterProducer.SelectedItem.ToString());
			if (producerFirniture == null)
			{
				MessageBox.Show("Такого элемента нет!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				_furniture.IdProducer = producerFirniture.Id;
			}
		}

		private void FindFilterSupplierItem()
		{
			var supplierFirniture = AppConnect.ModelOdb.Supplier.FirstOrDefault(x => x.Name == FilterSupplier.SelectedItem.ToString());
			if (supplierFirniture == null)
			{
				MessageBox.Show("Такого элемента нет!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				_furniture.IdSupplier = supplierFirniture.Id;
			}
		}

		private void AddData_Click(object sender, RoutedEventArgs e)
		{
			TextBox[] textBox = { NameFurniture, Cost, InstallationPrice, Existance, Description };

			foreach (TextBox txt in textBox)
			{
				if (string.IsNullOrWhiteSpace(txt.Text))
				{
					MessageBox.Show("Не все поля заполнены", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
					return;
				}
			}

            try
            {
                FindFilterTypeItem();
				FindFilterProducerItem();
				FindFilterSupplierItem();

                if (_furniture.Id == 0)
                {
					//_furniture.Cost = Math.Round(_furniture.Cost,2);
					//_furniture.InstallationPrice = Math.Round(_furniture.InstallationPrice, 2);
					Entities.GetContext().Furniture.Add(_furniture);
                }
				//Cost.Text = Math.Round(Convert.ToDouble(Cost.Text), 2).ToString();

				Entities.GetContext().SaveChanges();
                
				AppConnect.ModelOdb.SaveChanges();
				MessageBox.Show("Сохранил!");

				AppFrame.fraimMain.GoBack();
			}
            catch (Exception Ex)
            {
                MessageBox.Show("Ошибка " + Ex.Message.ToString() + "Критическая работа приложения", "Уведомление",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
            }
		}                                                            

		private void AddImage_Click(object sender, RoutedEventArgs e)                   
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Multiselect= true;
			dialog.Filter = "img files (*.jpg,.*png,*.jpeg)|*.jpg;.*png;*.jpeg";
			dialog.ShowDialog();

			string selectedFileName = dialog.FileName;

			if (dialog.FileName.ToString() == "")
            {
				return;
            }

			var uri = new Uri(dialog.FileName);
			var bitmap = new BitmapImage(uri);
			

            if (bitmap.Height > 300 || bitmap.Width > 200)
            {
				MessageBox.Show("Фото должно быть по габаритам 300*200", "Уведомление", MessageBoxButton.OK);	
				image.Source = null;
				return;
			}

			string directory;
			directory = dialog.FileName.Substring(dialog.FileName.LastIndexOf('\\'),
				dialog.FileName.Length - dialog.FileName.Substring(0, dialog.FileName.LastIndexOf('\\')).Length);


            string fullpath = System.IO.Path.GetFullPath("Resourses");
            fullpath = fullpath.Replace(@"\bin\Debug", "");
            BitmapImage bmp = new BitmapImage();

            fullpath += @"\Images" + directory;

            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
            }
			File.Copy(dialog.FileName, fullpath);

			_furniture.Image = dialog.SafeFileName;
			AppConnect.ModelOdb.SaveChanges();
			DataContext = null;
			DataContext = _furniture;

			image.Source = bitmap;
		}

		private void NameFurniture_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Zа-яА-Я]"))
            {
                e.Handled = true;
            }
        }

        private void TextBoxCosts_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"^[0-9,]"))
			{
				e.Handled = true;
			}
		}

		private void TextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			TextBox txtBox = sender as TextBox;
			double d;
			if (double.TryParse(txtBox.Text, out d))
			{
				txtBox.Text = d.ToString("f2");
			}
		}

        private void DeleteData_Click(object sender, RoutedEventArgs e)
        {
			AppConnect.ModelOdb.Furniture.Remove(_furniture);
			AppConnect.ModelOdb.SaveChanges();
			NavigationService.GoBack();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
			AppFrame.fraimMain.GoBack();
        }

    }
}
