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
    /// Логика взаимодействия для FullImageWindow.xaml
    /// </summary>
    public partial class FullImageWindow : Window
    {
        public FullImageWindow(string imagePath)
        {
            InitializeComponent();
            // Устанавливаем изображение в окно
            var bitmap = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
            FullImage.Source = bitmap;
        }
    }
}
