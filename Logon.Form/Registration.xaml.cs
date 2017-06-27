using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Logon.Logic;

namespace Logon.Form
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        FileClass fileWork;
        PersonInfo person;

        public Registration()
        {
            InitializeComponent();

            fileWork = new FileClass();
            person = new PersonInfo();

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

        private void ImageLoad_Click(object sender, RoutedEventArgs e)
        {
            string _addres = null;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            openFileDialog.InitialDirectory = fileWork.homeDir;
            if (openFileDialog.ShowDialog() == true)
                _addres = openFileDialog.FileName;

            ProfileImage.Source = new BitmapImage(new Uri(_addres));
            person.avatarAddres = _addres;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            person.name = Name.Text;
            person.surname = Surname.Text;
            person.middlename = Middlename.Text;

            person.birthDateDay = birthdayDay.Text;
            person.birthDateMonth = birthdayMonth.Text;
            person.birthDateYear = birthdayYear.Text;

            if (GenderM.IsPressed)
                person.gender = "М";
            else
                person.gender = "Ж";

            if (person.avatarAddres == null)
                person.avatarAddres = fileWork.homeDir + "default.jpg";

            if (PasswordOrig.Password != PasswordControl.Password)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка!");
            }
            else
            {
                person.password = PasswordOrig.Password;
                fileWork.WriteProfile(person);
            }
            
            Close();

        }
    }
}
