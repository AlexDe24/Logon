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
using Logon.Logic;

namespace Logon.Form
{
    /// <summary>
    /// Логика взаимодействия для Access.xaml
    /// </summary>
    public partial class Access : Window
    {
        List<PersonInfo> _persons;
        FileClass _fileWork;
        Registration _regist;
        Login _mainForm;
        List<PersonInfo> _findPerson;

        public Access()
        {
            InitializeComponent();

            _fileWork = new FileClass(); //класс работы с файлами
            _persons = _fileWork.ReadProfiles(); //класс данных о пользователе
            _findPerson = new List<PersonInfo>(); //лист для поиска по параметру

            try
            {
                _mainForm = new Login(_fileWork.IsLogonRead());

                Visibility = Visibility.Hidden;
                _mainForm.ShowDialog();
                Close();
            }
            catch (Exception)
            { 
            }

            for (int i = 0; i < _persons.Count; i++)
            {
                LoginEnter.Items.Add(_persons[i].login);
            }
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            if (LoginEnter.SelectedIndex >= 0)
            {
                _mainForm = new Login(_persons[LoginEnter.SelectedIndex]);

                if (_persons[LoginEnter.SelectedIndex].password == PasswordEnter.Password)
                {
                    Visibility = Visibility.Hidden;
                    _mainForm.ShowDialog();
                    Close();
                }
                else
                    MessageBox.Show("Неверный пароль", "Ошибка!");

                if (IsLog.IsChecked == true)
                {
                    _fileWork.IsLogonWrite(_persons[LoginEnter.SelectedIndex]);
                }
                else
                    _fileWork.IsLogonFalse();
            }
        }

        private void LoginEnter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LoginEnter.SelectedIndex >= 0)
                ProfileImage.Source = new BitmapImage(new Uri(_persons[LoginEnter.SelectedIndex].avatarAddres));
        }

        private void Regist_Click(object sender, RoutedEventArgs e)
        {
            _regist = new Registration(_persons); //класс регистрации

            int personsCount = _persons.Count;

            Visibility = Visibility.Hidden;
            _regist.ShowDialog();
            Visibility = Visibility.Visible;

            if (personsCount < _fileWork.ReadProfiles().Count)
            {
                _mainForm = new Login(_regist.newPerson);

                Visibility = Visibility.Hidden;
                _mainForm.ShowDialog();
                Close();
            }
            
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
