using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Logon.Logic;
using Microsoft.Win32;

namespace Logon.Form
{
    /// <summary>
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        PersonInfo _person;
        FileClass fileWork;
        bool isEditPassword;

        /// <summary>
        /// Класс управления профилем
        /// </summary>
        /// <param name="personNow">выбранный пользователь</param>
        /// <param name="personProfile">пользователь в сети</param>
        public Profile(PersonInfo personNow, PersonInfo personProfile)
        {
            InitializeComponent();

            fileWork = new FileClass(); //класс работы с файлами
            _person = personNow; //класс данных о пользователе

            if (personNow.login != personProfile.login && personProfile.login != "Admin")
                ControlPanel.Visibility = Visibility.Hidden;

            Name.Content += _person.name;
            Surname.Content += _person.surname;
            Middlename.Content += _person.middlename;

            BirthDate.Content += _person.birthDateDay + "." + _person.birthDateMonth + "." + _person.birthDateYear;

            Gender.Content += _person.gender;

            ProfileImage.Source = new BitmapImage(new Uri(_person.avatarAddres));

            for (int i = 1; i <= 31; i++)
            {
                birthDay.Items.Add(i-1);
            }
            for (int i = 1; i <= 12; i++)
            {
                birthMonth.Items.Add(i-1);
            }
            for (int i = 0; i < 120; i++)
            {
                birthYear.Items.Add(i + 1900);
            }

            NameNew.Text = _person.name;
            SurnameNew.Text = _person.surname;
            MiddlenameNew.Text = _person.middlename;

            try
            {
                birthDay.SelectedIndex = Convert.ToInt32(_person.birthDateDay);
                birthMonth.SelectedIndex = Convert.ToInt32(_person.birthDateMonth);
                birthYear.SelectedIndex = Convert.ToInt32(_person.birthDateYear) - 1900;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Загрузка изображения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageLoad_Click(object sender, RoutedEventArgs e)
        {
            string _addres = null;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            openFileDialog.InitialDirectory = fileWork.homeDirImage;
            if (openFileDialog.ShowDialog() == true)
                _addres = openFileDialog.FileName;

            if (_addres != null)
            {
                ProfileImage.Source = new BitmapImage(new Uri(_addres));
                _person.avatarAddres = _addres;
            }
        }

        /// <summary>
        /// Срабатывает при нажатии кнопки "Редактировать"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Edit.Content = "Сохранить";

            Edit.Click -= Edit_Click;
            Edit.Click += Save_Click;
            
            ProfilePanel.Visibility = Visibility.Visible;
            ImageLoad.Visibility = Visibility.Visible;
            EditPassword.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Срабатывает при нажатии кнопки "Сохранить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            fileWork.DelProfile(_person);

            _person.name = NameNew.Text;
            _person.surname = SurnameNew.Text;
            _person.middlename = MiddlenameNew.Text;

            _person.birthDateDay = birthDay.Text;
            _person.birthDateMonth = birthMonth.Text;
            _person.birthDateYear = birthYear.Text;

            if (GenderM.IsChecked == true)
                _person.gender = "М";
            else
                _person.gender = "Ж";

            if (_person.avatarAddres == null)
                _person.avatarAddres = fileWork.homeDirImage + "default.jpg";

            if (isEditPassword)
                if (PasswordOld.Password != _person.password)
                {
                    MessageBox.Show("Пароль неверный!", "Ошибка!");
                }
                else
                {
                    if (PasswordControl.Password != PasswordOrig.Password)
                    {
                        MessageBox.Show("Пароли не совпадают!", "Ошибка!");
                    }
                    else
                    {
                        _person.password = PasswordOrig.Password;
                        fileWork.WriteProfile(_person);

                        Close();
                    }
                }
            else
            {
                fileWork.WriteProfile(_person);

                Close();
            }
        }

        /// <summary>
        /// Срабатывает при нажатии кнопки "Сменить пароль"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditPassword_Click(object sender, RoutedEventArgs e)
        {
            if (!isEditPassword)
            {
                isEditPassword = true;

                PasswordPanel.Visibility = Visibility.Visible;
                PasswordPanelLabel.Visibility = Visibility.Visible;
            }
            else
            {
                isEditPassword = false;

                PasswordPanel.Visibility = Visibility.Hidden;
                PasswordPanelLabel.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Происходит при нажатии кнопки "Удалить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelPerson_Click(object sender, RoutedEventArgs e)
        {
            fileWork.DelProfile(_person);
            Close();
        }
    }
}
