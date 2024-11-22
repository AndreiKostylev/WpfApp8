using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfApp8
{
    public partial class CaptchaWindow : Window
    {
        private string captchaText;

        public CaptchaWindow()
        {
            InitializeComponent();
            GenerateCaptcha();
        }

        private void GenerateCaptcha()
        {
            captchaText = CaptchaGenerator.GenerateCaptchaText();
            var imageBytes = CaptchaGenerator.GenerateCaptchaImage(captchaText);

            using (var ms = new MemoryStream(imageBytes))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = ms;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                CaptchaImage.Source = bitmap;
            }
        }

        private void VerifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (CaptchaTextBox.Text == captchaText)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Капча не пройдена, попробуйте еще раз.");
                GenerateCaptcha();
            }
        }
    }
}
