using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Logon.Logic;

namespace Logon.Form
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        FileClass _fileWork;
        Registration _regist;
        Profile _personProfile;
        List<PersonInfo> _persons;
        List<Grid> _personBox;
        List<PersonInfo> _findPerson;
        PersonInfo _logPerson;
        Access _LogForm;

        public Login(PersonInfo logPerson, Access LogForm)
        {
            InitializeComponent();

            _LogForm = LogForm;

            _logPerson = logPerson; //пользватель, под которым сделан вход
            _persons = new List<PersonInfo>(); //класс данных о пользователе
            _fileWork = new FileClass(); //класс работы с файлами
            _personBox = new List<Grid>(); //GroupBox для отображения пользователей
            _findPerson = new List<PersonInfo>(); //лист для поиска по параметру

            Update();
        }

        /// <summary>
        /// Функция обновления данных на форме
        /// </summary>
        void Update()
        {
            _logPerson = _fileWork.ReadProfiles().Where(x => x.login == _logPerson.login).First();
            ProfileImage.Source = new BitmapImage(new Uri(_logPerson.avatarAddres));

            LogPerson.Content = "Вы вошли как " + _logPerson.login + "\n" + _logPerson.name + " " + _logPerson.surname;

            ProfilePanel.Children.Clear();
            _personBox.Clear();

            _findPerson = _fileWork.ReadProfiles();
            _persons.Clear();

            for (int i = 0; i < _findPerson.Count; i++)
            {
                if (_findPerson[i].login.ToUpper().StartsWith(FindTextBox.Text.ToUpper()))
                    _persons.Add(_findPerson[i]);
            }            

            int k = 0;

            for (int i = 0; i < _persons.Count; i++)
            {
                if (_persons.Count > i)
                {
                    _personBox.Add(CreatePerson(_persons[i]));

                    ProfilePanel.Children.Add(_personBox[k]);

                    k++;
                }
            }
        }

        /// <summary>
        /// Создание GropBox для отображение пользователя
        /// </summary>
        /// <param name="_person">класс пользователя</param>
        /// <returns></returns>
        Grid CreatePerson(PersonInfo _person)
        {
            Image avatar = new Image() //элемент для отображение картинки профиля
            {
                Source = new BitmapImage(new Uri(_person.avatarAddres))
            };

            RowDefinition rd1 = new RowDefinition();
            RowDefinition rd2 = new RowDefinition();

            rd2.Height = new GridLength(30);

            Color backCol = new Color()
            {
                A = 100,
                R = 50,
                G = 50,
                B = 80
            };

            Label nameLbl = new Label() //элемент для отображение имени пользователя
            {
                Content = _person.login,
                Background = new SolidColorBrush(backCol),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };

            Grid gr = new Grid(); //сетка для закрепления имени и картинки

            gr.Margin = new Thickness(0,5,0,0);

            gr.RowDefinitions.Add(rd1);
            gr.RowDefinitions.Add(rd2);
            gr.Children.Add(nameLbl);
            gr.Children.Add(avatar);

            Grid.SetColumn(avatar, 0);
            Grid.SetRow(avatar, 0);

            Grid.SetColumn(nameLbl, 0);
            Grid.SetRow(nameLbl, 1);

            gr.Name = "ID" + _person.login;
            gr.MouseDown += Profile_Click;
            
            return gr;
        }

        /// <summary>
        /// Вход в профиль
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            PersonInfo person = _persons.Where(x => "ID" + x.login == (sender as Grid).Name).First();

            int personsCount = _persons.Count;

            _personProfile = new Profile(person, _logPerson); //класс профиля

            Visibility = Visibility.Hidden;
            _personProfile.ShowDialog();
            Visibility = Visibility.Visible;

            if (personsCount > _fileWork.ReadProfiles().Count && person.login == _logPerson.login)
            {
                LogOut_Click(null, null);
            }

            Update();
        }

        /// <summary>
        /// При нажатии кнопки "Регистрация"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            _regist = new Registration(_persons); //класс регистрации

            int personsCount = _persons.Count;

            Visibility = Visibility.Hidden;
            _regist.ShowDialog();
            Visibility = Visibility.Visible;

            if (personsCount < _fileWork.ReadProfiles().Count)
            {
                _logPerson = _regist.newPerson;
            }

            Update();
        }

        /// <summary>
        /// При нажатии кнопки "Помощь"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Random rand = new Random();
            MessageBox.Show("Вы встали в очередь на помощь. \nВаше место в очереди - " + rand.Next(2,10000) + ".\nПожалуйста, подождите.");
        }

        /// <summary>
        /// При вводе текса в поиск
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FindTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            Update();
        }

        /// <summary>
        /// При нажатии кнопки "Выйти"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            _LogForm.Close();
            Close();
        }

        /// <summary>
        /// При нажатии кнопки "Сменить пользователя"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            _fileWork.IsLogonFalse("Online");
            _LogForm.Visibility = Visibility.Visible;
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                _LogForm.Close();
                Close();
            }
        }
    }
}
