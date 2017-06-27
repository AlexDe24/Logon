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

        public Profile(PersonInfo person)
        {
            InitializeComponent();

            fileWork = new FileClass();
            _person = person;

            Name.Content = _person.name;
            Surname.Content = _person.surname;
            Middlename.Content = _person.middlename;

            BirthDate.Content = _person.birthDateDay + "." + _person.birthDateMonth + "." + _person.birthDateYear;

            Gender.Content = _person.gender;

            ProfileImage.Source = new BitmapImage(new Uri(_person.avatarAddres));

            for (int i = 1; i <= 31; i++)
            {
                birthDay.Items.Add(i);
            }
            for (int i = 1; i <= 12; i++)
            {
                birthMonth.Items.Add(i);
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

            if (_person.gender == "М")
                GenderM.IsChecked = true;
            else
                GenderW.IsChecked = true;
        }

        private void ImageLoad_Click(object sender, RoutedEventArgs e)
        {
            string _startDir = fileWork.homeDir;
            string _addres = null;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            openFileDialog.InitialDirectory = _startDir;
            if (openFileDialog.ShowDialog() == true)
                _addres = openFileDialog.FileName;

            ProfileImage.Source = new BitmapImage(new Uri(_addres));
            _person.avatarAddres = _addres;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Edit.Content = "Сохранить";
            Edit.Click += Save_Click;
            
            ProfilePanel.Visibility = Visibility.Visible;
            ImageLoad.Visibility = Visibility.Visible;
            EditPassword.Visibility = Visibility.Visible;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Edit.Click += Edit_Click;

            _person.name = NameNew.Text;
            _person.surname = SurnameNew.Text;
            _person.middlename = MiddlenameNew.Text;

            _person.birthDateDay = birthDay.Text;
            _person.birthDateMonth = birthMonth.Text;
            _person.birthDateYear = birthYear.Text;

            if (GenderM.IsPressed)
                _person.gender = "М";
            else
                _person.gender = "Ж";

            if (_person.avatarAddres == null)
                _person.avatarAddres = fileWork.homeDir + "default.jpg";

            if (isEditPassword)
                if (PasswordOrig.Password != _person.password)
                {
                    MessageBox.Show("Пароль неверный!", "Ошибка!");
                }
                else
                {
                    if (PasswordOrig.Password != PasswordControl.Password)
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

        private void EditPassword_Click(object sender, RoutedEventArgs e)
        {
            isEditPassword = true;

            PasswordPanel.Visibility = Visibility.Visible;
            PasswordPanelLabel.Visibility = Visibility.Visible;
        }
    }
}
