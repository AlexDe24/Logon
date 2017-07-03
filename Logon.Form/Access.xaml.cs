using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Logon.Logic;
using System.Threading;
using System.Windows.Media;
using System.Windows.Threading;

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
            _findPerson = new List<PersonInfo>(); //лист для поиска по параметру

            Update();

            try
            {
                LoginEnter.Text = _fileWork.IsLogonRead("Remember").login;
                PasswordEnter.Focus();
            }
            catch (Exception)
            {
            }

            try
            {
                _mainForm = new Login(_fileWork.IsLogonRead("Online"), this);

                Visibility = Visibility.Hidden;
                _mainForm.ShowDialog();
            }
            catch (Exception)
            { 
            }

            Dispatcher.BeginInvoke((Action)delegate
            {
                TextBox textBox = GetChildFromVisualTree(LoginEnter, typeof(TextBox)) as TextBox;
                if (textBox != null)
                {
                    textBox.Focus();
                }
            }, DispatcherPriority.Loaded);
            
        }

        /// <summary>
        /// Страшная магия устанвливающая фокус на комбо бокс
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public DependencyObject GetChildFromVisualTree(DependencyObject parent, Type objectType)
        {
            if (parent == null)
                return null;

            DependencyObject returnObject = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject visualElement = VisualTreeHelper.GetChild(parent, i);
                if (objectType.IsInstanceOfType(visualElement))
                {
                    return visualElement;
                }
                else
                {
                    returnObject = GetChildFromVisualTree(visualElement, objectType);
                    if (returnObject != null)
                    {
                        return returnObject;
                    }
                }
            }
            return null;
        }

        void Update()
        {
            _persons = _fileWork.ReadProfiles(); //класс данных о пользователе

            LoginEnter.Items.Clear();

            for (int i = 0; i < _persons.Count; i++)
            {
                Image chooseImage = new Image();
                LoginEnter.Items.Add(_persons[i].login);
            }
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            if (LoginEnter.SelectedIndex >= 0)
            {
                _mainForm = new Login(_persons[LoginEnter.SelectedIndex], this);

                try
                {
                    if (IsLog.IsChecked == true)
                    {
                        _fileWork.IsLogonWrite(_persons[LoginEnter.SelectedIndex], "Online");
                    }
                    else
                        _fileWork.IsLogonFalse("Online");

                    if (IsRemember.IsChecked == true)
                    {
                        _fileWork.IsLogonWrite(_persons[LoginEnter.SelectedIndex], "Remember");
                    }
                    else
                        _fileWork.IsLogonFalse("Remember");
                }
                catch (Exception)
                {
                }

                if (_persons[LoginEnter.SelectedIndex].password == PasswordEnter.Password)
                {
                    Visibility = Visibility.Hidden;
                    _mainForm.ShowDialog();
                    Update();
                }
                else
                    MessageBox.Show("Неверный пароль.", "Предупреждение!");

            }
            else
                MessageBox.Show("Пользователь не найден.", "Предупреждение!");
            PasswordEnter.Clear();
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
                _mainForm = new Login(_regist.newPerson, this);

                Visibility = Visibility.Hidden;
                _mainForm.ShowDialog();
                //Close();
            }
            
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void LoginEnter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                PasswordEnter.Focus();
        }

        private void PasswordEnter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Enter_Click(null, null);
        }
    }
}
