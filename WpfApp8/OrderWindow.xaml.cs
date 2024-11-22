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

namespace WpfApp8
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private orders _currentOrder;
        private metalRussEntities db;

        public OrderWindow(orders order = null)
        {
            InitializeComponent();
            db = new metalRussEntities();

            if (order != null)
            {
                _currentOrder = order;
                txtId.Text = _currentOrder.id.ToString();
                txtDate.Text = _currentOrder.order_date.ToString();
                txtStatus.Text = _currentOrder.status;
                txtClientId.Text = _currentOrder.client_id.ToString();
                txtTotal.Text = _currentOrder.total_amount.ToString();
            }
            else
            {
                _currentOrder = new orders();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        { // Проверяем, указано ли значение ID
            if (int.TryParse(txtId.Text, out int orderId))
            {
                // Ищем заказ в базе данных
                orders orderToDelete = db.orders.FirstOrDefault(o => o.id == orderId);

                if (orderToDelete != null)
                {
                    // Удаляем заказ из базы данных
                    db.orders.Remove(orderToDelete);
                    db.SaveChanges();

                    // Закрываем окно
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Заказ с указанным ID не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Введите корректный ID.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            _currentOrder.id = int.Parse(txtId.Text);
            _currentOrder.order_date = DateTime.Parse(txtDate.Text);
            _currentOrder.status = txtStatus.Text;
            _currentOrder.client_id = int.Parse(txtClientId.Text);
            _currentOrder.total_amount = decimal.Parse(txtTotal.Text);

            db.SaveChanges();
            this.Close();
        }
    }
}
