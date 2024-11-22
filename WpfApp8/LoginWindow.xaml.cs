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
using WpfApp8;

namespace WpfApp8
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private metalRussEntities db;
        private int failedAttempts = 0;

        public LoginWindow()
        {
            InitializeComponent();
            db = new metalRussEntities();
            
        }

        // Метод для генерации соли
        private static string GenerateSalt()
        {
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            byte[] saltBytes = new byte[32];
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        // Метод для хеширования пароля
        private static string HashPassword(string password, string salt)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var saltedPassword = password + salt;
                byte[] saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
                byte[] hashBytes = sha256.ComputeHash(saltedPasswordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        // Метод для регистрации пользователя
        private void RegisterUser(string login, string password)
        {
            string salt = GenerateSalt();
            string hash = HashPassword(password, salt);

            Users newUser = new Users
            {
                Login = login,
                Salt = salt,
                PasswordHash = hash
            };

            db.Users.Add(newUser);
            db.SaveChanges();
            MessageBox.Show("Пользователь успешно зарегистрирован!");
        }

        // Метод для авторизации пользователя
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = db.Users.FirstOrDefault(d => d.Login == txtUsername.Text);

                if (user != null)
                {
                    string hashInputPassword = HashPassword(txtPassword.Password, user.Salt);
                    if (hashInputPassword == user.PasswordHash)
                    {
                        failedAttempts = 0; // Сброс счетчика попыток
                        MainWindow main = new MainWindow();
                        main.Show();
                        this.Close();
                    }
                    else
                    {
                        failedAttempts++;
                        MessageBox.Show($"Неверный пароль! Попытка: {failedAttempts}");

                        if (failedAttempts >= 3)
                        {
                            MessageBox.Show("Открытие окна капчи...");

                            var captchaWindow = new CaptchaWindow();
                            bool? result = captchaWindow.ShowDialog();

                            if (result == true)
                            {
                                failedAttempts = 0; // Сброс счетчика при успешном прохождении капчи
                                MessageBox.Show("Капча успешно пройдена.");
                            }
                            else
                            {
                                MessageBox.Show("Проверка капчи не пройдена!");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Пользователь не найден!");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString());
            }
        }

        // Обработка кнопки "Регистрация"
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string login = txtUsername.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Логин и пароль не могут быть пустыми!");
                return;
            }

            // Проверка, существует ли уже пользователь
            if (db.Users.Any(u => u.Login == login))
            {
                MessageBox.Show("Пользователь с таким логином уже существует!");
            }
            else
            {
                RegisterUser(login, password);
            }
        }

        // Обработка кнопки "Отмена"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
