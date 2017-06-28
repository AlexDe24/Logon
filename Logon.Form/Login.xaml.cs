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
        List<GroupBox> _personBox;
        List<PersonInfo> _findPerson;
        PersonInfo _logPerson;

        int _startI;
        int _endI;

        public Login()
        {
            InitializeComponent();

            _startI = 0;
            _endI = 3;

            _logPerson = null; //пользватель, под которым сделан вход
            _persons = new List<PersonInfo>(); //класс данных о пользователе
            _fileWork = new FileClass(); //класс работы с файлами
            _personBox = new List<GroupBox>(); //GroupBox для отображения пользователей
            _findPerson = new List<PersonInfo>(); //лист для поиска по параметру

            Find.Content = "Поиск Выкл.";

            Update();
        }

        /// <summary>
        /// Функция обновления данных на форме
        /// </summary>
        void Update()
        {
            if (_logPerson != null)
            {
                LogOutBox.Height = 25;
                LogOutBox.Visibility = Visibility.Visible;
                LogPerson.Content = "Вы вошли как " + _logPerson.surname + " " + _logPerson.name + ".";
            }
            else
            {
                LogOutBox.Height = 0;
                LogOutBox.Visibility = Visibility.Hidden;
                LogPerson.Content = "";
            }

            PersonGrid.Children.Clear();
            _personBox.Clear();

            _findPerson = _fileWork.ReadProfiles();
            _persons.Clear();

            for (int i = 0; i < _findPerson.Count; i++)
            {
                if (_findPerson[i].login.StartsWith(FindTextBox.Text))
                    _persons.Add(_findPerson[i]);
            }            

            int k = 0;

            for (int i = _startI; i < _endI; i++)
            {
                if (_persons.Count > i)
                {
                    _personBox.Add(CreatePerson(_persons[i]));

                    PersonGrid.Children.Add(_personBox[k]);

                    Grid.SetRow(_personBox[k], 1);
                    Grid.SetColumn(_personBox[k], 1 + k * 2);

                    k++;
                }
            }

            LoginPanel.Visibility = Visibility.Hidden;
            PasswordEnter.Clear();
        }

        /// <summary>
        /// Создание GropBox для отображение пользователя
        /// </summary>
        /// <param name="_person">класс пользователя</param>
        /// <returns></returns>
        GroupBox CreatePerson(PersonInfo _person)
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

            gr.RowDefinitions.Add(rd1);
            gr.RowDefinitions.Add(rd2);
            gr.Children.Add(nameLbl);
            gr.Children.Add(avatar);

            Grid.SetColumn(avatar, 0);
            Grid.SetRow(avatar, 0);

            Grid.SetColumn(nameLbl, 0);
            Grid.SetRow(nameLbl, 1);

            GroupBox gBox = new GroupBox(); //контейнер для хранения сетки

            gBox.Name = "ID" + _person.login;
            gBox.Content = gr;
            gBox.MouseDown += ChoosePerson;

            return gBox;
        }

        /// <summary>
        /// Изменение GroypBox при его выборе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChoosePerson(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < _personBox.Count; i++)
            {
                _personBox[i].Background = new SolidColorBrush();
                _personBox[i].Opacity = 100;
            }

            Color backCol = new Color()
            {
                A = 100,
                R = 50,
                G = 50,
                B = 100
            };

            PasswordEnter.Clear();
            (sender as GroupBox).Opacity = 50;
            (sender as GroupBox).Background = new SolidColorBrush(backCol);

            LoginPanel.Visibility = Visibility.Visible;
            PasswordEnter.Focus();
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
                LogOut_Click(null, null);

                _logPerson = _regist.newPerson;

                PasswordPanel.Visibility = Visibility.Hidden;
                Enter.Content = "Просмотреть профиль";
                Enter.Click -= Enter_Click;
                Enter.Click += Profile_Click;
                Enter.Width = 250;
            }

            Update();

        }

        /// <summary>
        /// Вход в профиль
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            PersonInfo person = _persons.Where(x => "ID" + x.login == _personBox.Where(y => y.Opacity < 100).First().Name).First();

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
        /// Вход в учётную запись
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            PersonInfo person = _persons.Where(x => "ID" + x.login == _personBox.Where(y => y.Opacity == 50).First().Name).First();

            if (PasswordEnter.Password == person.password)
            {
                PasswordPanel.Visibility = Visibility.Hidden;
                Enter.Content = "Просмотреть профиль";
                Enter.Click -= Enter_Click;
                Enter.Click += Profile_Click;
                Enter.Width = 250;
                _logPerson = person;
                Update();
            }
            else
                MessageBox.Show("Неверный пароль!", "Ошибка!");
        }


        /// <summary>
        /// При нажатии кнопки "Закрыть"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// При нажатии кнопки "Выход"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            PasswordPanel.Visibility = Visibility.Visible;
            Enter.Content = "Вход";
            Enter.Click += Enter_Click;
            Enter.Click -= Profile_Click;
            Enter.Width = 150;

            _logPerson = null;
            Update();
        }

        /// <summary>
        /// При нажатии кнопки "Поиск"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Find_Click(object sender, RoutedEventArgs e)
        {
            FindTextBox.Clear();

            if (Find.Content == "Поиск Выкл.")
            {
                Find.Content = "Поиск Вкл.";
                FindPanel.Visibility = Visibility.Visible;
            }
            else
            {
                Find.Content = "Поиск Выкл.";
                FindPanel.Visibility = Visibility.Hidden;
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
        /// ППри вводе текса в поиск
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FindTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            Update();
        }

        /// <summary>
        /// Функция для прокрутки пользователей
        /// </summary>
        /// <param name="IsLeft"></param>
        void Roll(bool IsLeft)
        {
            if (IsLeft)
            {
                if (_startI != 0)
                {
                    _startI--;
                    _endI--;
                }
            }
            if (!IsLeft)
            {
                if (_endI <= _persons.Count - 1)
                {
                    _startI++;
                    _endI++;
                }
            }

            Update();
        }

        /// <summary>
        /// Смещение панели при прокручивании колёсика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyGrid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
                Roll(true);
            else
                Roll(false);
        }

        /// <summary>
        /// Смещение панели при нажатии кнопок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Roll_Click(object sender, RoutedEventArgs e)
        {

            if ((sender as Button).Name == "LeftRoll")
            {
                Roll(true);
            }
            else
            {
                Roll(false);
            }
            
        }
    }
}
