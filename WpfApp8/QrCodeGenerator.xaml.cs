using System;
using System.Windows;
using System.Windows.Media;
using QRCoder;
using QRCoder.Xaml;

namespace WpfApp8
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class QrCodeGenerator : Window
    {
        public QrCodeGenerator()
        {
            InitializeComponent();
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            // Создаем данные QR-кода из введенного текста
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(inputBox.Text, QRCodeGenerator.ECCLevel.H);
            XamlQRCode qrCode = new XamlQRCode(qrCodeData);

            // Генерируем случайные шестнадцатеричные цвета для QR-кода и фона
            string qrColorHex = $"#{new Random().Next(0x1000000):X6}";
            string backgroundColorHex = $"#{new Random().Next(0x1000000):X6}";

            // Проверяем контраст между цветом QR-кода и фоном
            while (!IsContrastEnough(qrColorHex, backgroundColorHex))
            {
                // Если контраста недостаточно, выбираем новые случайные цвета
                qrColorHex = $"#{new Random().Next(0x1000000):X6}";
                backgroundColorHex = $"#{new Random().Next(0x1000000):X6}";
            }

            // Генерируем QR-код с контрастирующими цветами
            DrawingImage qrCodeAsXaml = qrCode.GetGraphic(20, qrColorHex, backgroundColorHex);
            qrImage.Source = qrCodeAsXaml;
        }

        // Функция проверки контраста между двумя цветами
        private bool IsContrastEnough(string colorHex1, string colorHex2)
        {
            Color color1 = (Color)ColorConverter.ConvertFromString(colorHex1);
            Color color2 = (Color)ColorConverter.ConvertFromString(colorHex2);

            // Расчет яркости цветов по формуле Y = 0.299 * R + 0.587 * G + 0.114 * B
            double brightness1 = 0.299 * color1.R + 0.587 * color1.G + 0.114 * color1.B;
            double brightness2 = 0.299 * color2.R + 0.587 * color2.G + 0.114 * color2.B;

            // Возвращаем true, если разница в яркости достаточна (например, больше 125)
            return Math.Abs(brightness1 - brightness2) > 125;
        }
    }
}
