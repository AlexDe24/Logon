using Microsoft.Win32;
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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        FileClass filework;
        Info person;

        public Registration()
        {
            InitializeComponent();

            filework = new FileClass();
            person = new Info();

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
            string _startDir = @"C:\Logon";
            string _addres = null;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            openFileDialog.InitialDirectory = _startDir;
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
            
            person.birthday = birthdayDay.Text + "." + birthdayMonth.Text + "." + birthdayYear.Text;

            if (GenderM.IsPressed)
                person.gender = "М";
            else
                person.gender = "Ж";

            if (person.avatarAddres == null)
                person.avatarAddres = @"C:\Logon\default.jpg";

            if (PasswordOrig.Password != PasswordControl.Password)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошбка!");
            }
            else
            {
                person.password = PasswordOrig.Password;
                filework.WriteProfile(person);
            }
            Close();

        }
    }
}
