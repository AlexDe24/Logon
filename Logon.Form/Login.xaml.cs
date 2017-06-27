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
        FileClass fileWork;
        Registration regist;
        Profile personProfile;
        List<PersonInfo> persons;
        List<GroupBox> _personBox;

        int _startI;
        int _endI;

        public Login()
        {
            InitializeComponent();

            _startI = 0;
            _endI = 3;

            persons = new List<PersonInfo>();
            fileWork = new FileClass();
            _personBox = new List<GroupBox>();

            Update();
        }

        /// <summary>
        /// Функция обновления данных на форме
        /// </summary>
        void Update()
        {
            PersonGrid.Children.Clear();
            _personBox.Clear();

            persons = fileWork.ReadProfiles();

            int k = 0;

            for (int i = _startI; i < _endI; i++)
            {
                _personBox.Add(CreatePerson(persons[i]));

                PersonGrid.Children.Add(_personBox[k]);

                Grid.SetRow(_personBox[k], 1);
                Grid.SetColumn(_personBox[k], 1 + k * 2);

                k++;
            }

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

            rd2.Height = new GridLength(40);

            Color backCol = new Color()
            {
                A = 100,
                R = 50,
                G = 50,
                B = 80
            };

            Label nameLbl = new Label() //элемент для отображение имени пользователя
            {
                Content = _person.name + "\n" + _person.surname,
                Background = new SolidColorBrush(backCol),
                HorizontalContentAlignment = HorizontalAlignment.Center,
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

            gBox.Name = _person.name + _person.surname + _person.middlename;
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
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            regist = new Registration();

            Visibility = Visibility.Hidden;
            regist.ShowDialog();
            Visibility = Visibility.Visible;

            Update();
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            PersonInfo person = persons.Where(x => x.name + x.surname + x.middlename == _personBox.Where(y => y.Opacity == 50).First().Name).First();
            if (PasswordEnter.Password == person.password)
            {
                personProfile = new Profile(person);

                Visibility = Visibility.Hidden;
                personProfile.ShowDialog();
                Visibility = Visibility.Visible;

                Update();
            }
            else
                MessageBox.Show("Неверный пароль!", "Ошибка!");
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            fileWork.DoCopyIn();
        }

        private void Roll_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Name == "LeftRoll")
            {
                if (_startI != 0)
                {
                    _startI--;
                    _endI--;
                }
            }
            if ((sender as Button).Name == "RightRoll")
            {
                if (_endI <= persons.Count - 1)
                {
                    _startI++;
                    _endI++;
                }
            }

            Update();
        }
    }
}
