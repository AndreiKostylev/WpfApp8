using ClosedXML.Excel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp8
{


public partial class MainWindow : Window
   {
          metalRussEntities db;
          List<orders> allOrders;

        public MainWindow()
        {
            InitializeComponent();
            db = new metalRussEntities();
            allOrders = db.orders.ToList(); // Инициализируем allOrders
            dgOrders.ItemsSource = allOrders;
           
            LoadClientsData();
            LoadMetalsData();
            LoadSuppliersData();
            LoadEmployeesData();

        }
       
        private void LoadClientsData()
        {
            var clientsData = from client in db.clients
                              select new
                              {
                                  id = client.id,
                                  first_name = client.first_name,
                                  last_name = client.last_name,
                                  phone = client.phone,
                                  email = client.email
                              };

            dgClients.ItemsSource = clientsData.ToList();
        }
        private void LoadMetalsData()
        {
            var metalsData = from metal in db.metals
                             select new
                             {
                                 id = metal.id,
                                 name = metal.name,
                                 category = metal.category,
                                 supplier_id = metal.supplier_id
                             };

            dgMetals.ItemsSource = metalsData.ToList();
        }
        private void LoadSuppliersData()
        {
            var suppliersData = from supplier in db.suppliers
                                select new
                                {
                                    id = supplier.id,
                                    name = supplier.name,
                                    contact_person = supplier.contact_person,
                                    phone = supplier.phone,
                                    email = supplier.email
                                };

            dgSuppliers.ItemsSource = suppliersData.ToList();
        }
        private void LoadEmployeesData()
        {
            var employeesData = from employee in db.employees
                                select new
                                {
                                    id = employee.id,
                                    first_name = employee.first_name,
                                    last_name = employee.last_name,
                                    phone = employee.phone,
                                    email = employee.email
                                };

            dgEmployees.ItemsSource = employeesData.ToList();
        }
        private void dgOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgOrders.SelectedItem != null)
            {
                orders selectedOrder = (orders)dgOrders.SelectedItem;
                OrderWindow orderWindow = new OrderWindow(selectedOrder);
                orderWindow.ShowDialog();

                // Обновляем DataGrid после закрытия окна
                dgOrders.ItemsSource = db.orders.ToList();
            }
        }

        private void AddEmployee(object sender, RoutedEventArgs e)
        {

        }

        private void AddClient(object sender, RoutedEventArgs e)
        {

        }

        

        private void AddSupplier(object sender, RoutedEventArgs e)
        {

        }

        

        private void dgOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }



     
        private void AddOrder(object sender, RoutedEventArgs e)
        {
            orders ord = new orders();
            ord.id = Convert.ToInt32(oId.Text);
            ord.order_date = Convert.ToDateTime(oDate.Text);
            ord.status = oStatus.Text;
            ord.client_id = Convert.ToInt32(oClient.Text);
            ord.total_amount = Convert.ToDecimal(oAmount.Text);
            db.orders.Add(ord);
            db.SaveChanges();
            allOrders = db.orders.ToList(); // Обновляем список всех заказов
            dgOrders.ItemsSource = allOrders; // Обновляем источник данных в DataGrid
            oId.Text = string.Empty;
            oDate.Text = string.Empty;
            oStatus.Text = string.Empty;
            oClient.Text = string.Empty;
            oAmount.Text = string.Empty;
        }

 

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedStatus = (statusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string filterText = filterTextBox.Text.ToLower();

            var filteredOrders = allOrders.Where(o =>
                (selectedStatus == "Все" || o.status == selectedStatus) &&
                (string.IsNullOrWhiteSpace(filterText) || o.client_id.ToString().Contains(filterText) ||
                o.total_amount.ToString().Contains(filterText))
            ).ToList();

            dgOrders.ItemsSource = filteredOrders; // Устанавливаем отфильтрованный источник данных
        }

        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            // Сбрасываем фильтры и показываем все заказы
            statusComboBox.SelectedIndex = 0; // "All"
            filterTextBox.Clear();
            dgOrders.ItemsSource = allOrders; // Показываем все заказы
        }

        private void statusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Обработка изменения выбора статуса (если необходимо)
        }

        private void filterTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            // Обработка нажатия клавиш в TextBox (если необходимо)
        }
        private string selectedPhotoPath; // Переменная для хранения пути к выбранному фото

        private void SelectPhoto_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.png;*.jpeg)|*.jpg;*.png;*.jpeg"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                selectedPhotoPath = openFileDialog.FileName; // Сохраняем путь к выбранному фото
                MessageBox.Show($"Выбрано фото: {selectedPhotoPath}");
            }
        }
        private string SavePhoto(string originalPath)
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory; // Путь к каталогу приложения
            string imagesFolder = System.IO.Path.Combine(appDirectory, "Images");

            // Создаем папку Images, если она не существует
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            // Копируем фото в папку Images
            string fileName = System.IO.Path.GetFileName(originalPath);
            string destinationPath = System.IO.Path.Combine(imagesFolder, fileName);

            File.Copy(originalPath, destinationPath, true); // Копируем фото

            // Возвращаем относительный путь для хранения в БД
            return $"Images/{fileName}";
        }
        private void Image_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Получаем объект Image, на который нажал пользователь
            var image = sender as Image;

            if (image != null && image.Source != null)
            {
                // Открываем новое окно с большим изображением
                var fullImageWindow = new FullImageWindow(image.Source.ToString());
                fullImageWindow.ShowDialog();
            }
        }
        private void AddProduct(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(selectedPhotoPath))
            {
                MessageBox.Show("Выберите фото перед добавлением записи!");
                return;
            }

            try
            {
                // Сохраняем фото и получаем путь
                string photoPath = SavePhoto(selectedPhotoPath);

                // Создаем новую запись
                metals newMetal = new metals
                {
                    id = Convert.ToInt32(pId.Text),
                    name = pName.Text,
                    category = pCategory.Text,
                    supplier_id = int.Parse(pSupplierId.Text),
                    photo_path = photoPath // Сохраняем путь к фото
                };

                // Добавляем запись в базу данных
                db.metals.Add(newMetal);
                db.SaveChanges();

                // Обновляем DataGrid
                dgMetals.ItemsSource = db.metals.ToList();
                MessageBox.Show("Металл добавлен успешно!");

                // Очищаем поля ввода
                pName.Clear();
                pCategory.Clear();
                pSupplierId.Clear();
                selectedPhotoPath = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void dgMetals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false; // Блокировка интерфейса во время печати
                PrintDialog pD = new PrintDialog();

                // Запрос для получения заказов со статусом "Pending"
                var ordersData = from o in db.orders
                                 join c in db.clients on o.client_id equals c.id
                                 select new
                                 {
                                     id = o.id,
                                     order_date = o.order_date,
                                     status = o.status,
                                     client_id = c.first_name + " " + c.last_name, // Объединяем имя и фамилию клиента
                                     total_amount = o.total_amount
                                 };

                dgOrders.ItemsSource = ordersData.ToList();

                // Проверяем, есть ли данные для печати
                if (!ordersData.Any())
                {
                    MessageBox.Show("Нет заказов со статусом 'Ожидание' для печати.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Отображаем данные в DataGrid перед печатью
                dgOrders.ItemsSource = ordersData.ToList();

                if (pD.ShowDialog() == true) // Проверка, был ли выбран принтер
                {
                    // Печать содержимого DataGrid
                    pD.PrintVisual(dgOrders, "Отчет по заказам");
                }
            }
            finally
            {
                this.IsEnabled = true; // Включение интерфейса после завершения печати
            }
        }
        private void ExportToCSV(object sender, RoutedEventArgs e)
        {
            dgOrders.SelectAllCells();
            dgOrders.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, dgOrders);
            dgOrders.UnselectAllCells();

            string result = (string)Clipboard.GetData(DataFormats.Text);
            using (StreamWriter sw = new StreamWriter("DataGridExport.xls"))
            {
                sw.WriteLine(result);
            }
            MessageBox.Show("Данные успешно экспортированы в Excel!");
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        
    }
}
