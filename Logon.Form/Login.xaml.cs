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
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        FileClass file;
        Registration regist;
        List<Info> person;

        public Login()
        {
            InitializeComponent();

            person = new List<Info>();
            file = new FileClass();
            regist = new Registration();

            Update();
        }

        void Update()
        {
            person = file.ReadProfiles();

            if (person.Count >= 3)
            {
                Registration.Visibility = Visibility.Hidden;
            }

            for (int i = 0; i < person.Count; i++)
            {
                Grid personGrid = CreatePerson(person[i].name, person[i].surname, person[i].avatarAddres);

                MyGrid.Children.Add(personGrid);

                Grid.SetRow(personGrid, 1);
                Grid.SetColumn(personGrid, 1 + i * 2);
            }
        }

        Grid CreatePerson(string name, string surname, string avatar)
        {
            Image _avatar = new Image();
            Grid _gr = new Grid();

            RowDefinition rd1 = new RowDefinition();
            RowDefinition rd2 = new RowDefinition();

            rd2.Height = new GridLength(40);

            _gr.RowDefinitions.Add(rd1);
            _gr.RowDefinitions.Add(rd2);

            Color backCol = new Color()
            {
                A = 100,
                R = 50,
                G = 50,
                B = 80
            };

            Label nameLbl = new Label()
            {
                Content = name + "\n" + surname,
                Background = new SolidColorBrush(backCol)
            };

            _avatar.Source = new BitmapImage(new Uri(avatar));

            _gr.Children.Add(nameLbl);
            _gr.Children.Add(_avatar);

            Grid.SetColumn(_avatar, 0);
            Grid.SetRow(_avatar, 0);

            Grid.SetColumn(nameLbl, 0);
            Grid.SetRow(nameLbl, 1);

            return _gr;
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            regist.ShowDialog();
            Visibility = Visibility.Visible;

            Update();
        }
    }
}
