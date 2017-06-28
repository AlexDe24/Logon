using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Logon.Logic;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;

namespace Logon.Form
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        List<PersonInfo> _allPersons;
        FileClass _fileWork;
        public PersonInfo newPerson;

        /// <summary>
        /// Класс управления регистрацией
        /// </summary>
        /// <param name="allPersons">список всех пользователей для проверки уникальности логина</param>
        public Registration(List<PersonInfo> allPersons)
        {
            InitializeComponent();

            _allPersons = allPersons; //список всех пользователей для проверки логина
            _fileWork = new FileClass(); //класс работы с файлами
            newPerson = new PersonInfo(); //класс данных о пользователе

            for (int i = 1; i <= 31; i++)
            {
                birthdayDay.Items.Add(i);
            }
            for (int i = 1; i <= 12; i++)
            {
                birthdayMonth.Items.Add(i);
            }
            for (int i = 0; i < 120; i++)
            {
                birthdayYear.Items.Add(i+1900);
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
            openFileDialog.InitialDirectory = _fileWork.homeDirImage;
            if (openFileDialog.ShowDialog() == true)
                _addres = openFileDialog.FileName;

            if (_addres != null)
            {
                ProfileImage.Source = new BitmapImage(new Uri(_addres));
                newPerson.avatarAddres = _addres;
            }
        }

        /// <summary>
        /// При нажатии кнопки "Сохранить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            newPerson.name = Name.Text;
            newPerson.surname = Surname.Text;
            newPerson.middlename = Middlename.Text;

            newPerson.birthDateDay = birthdayDay.Text;
            newPerson.birthDateMonth = birthdayMonth.Text;
            newPerson.birthDateYear = birthdayYear.Text;

            if (GenderM.IsPressed)
                newPerson.gender = "М";
            else
                newPerson.gender = "Ж";

            if (newPerson.avatarAddres == null)
                newPerson.avatarAddres = "pack://siteoforigin:,,,/Resources/default.jpg";


            if (Login.Text == "")
                MessageBox.Show("Введите логин!", "Ошибка!");
            else
            {
                if (_allPersons.Any(x => x.login == Login.Text))
                    MessageBox.Show("Логин уже занят!", "Ошибка!");
                else
                {
                    newPerson.login = Login.Text;
                    if (PasswordOrig.Password != PasswordControl.Password)
                    {
                        MessageBox.Show("Пароли не совпадают!", "Ошибка!");
                    }
                    else
                    {
                        newPerson.password = PasswordOrig.Password;
                        _fileWork.WriteProfile(newPerson);

                        Close();
                    }
                }
            }
        }
    }
}
