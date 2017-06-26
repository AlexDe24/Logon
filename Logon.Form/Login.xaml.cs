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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Logon.Form
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            /*Grid personGrid = CreatePerson(null,null);

            MyGrid.Children.Add(personGrid);

            Grid.SetRow(personGrid, 1);
            Grid.SetColumn(personGrid, 1);*/
        }
        Grid CreatePerson(string Name, Image Avatar)
        {
            Grid gr = new Grid();

            RowDefinition rd1 = new RowDefinition();
            RowDefinition rd2 = new RowDefinition();

            rd2.Height = new GridLength(40);

            gr.RowDefinitions.Add(rd1);
            gr.RowDefinitions.Add(rd2);

            Color backCol = new Color()
            {
                A = 100,
                R = 50,
                G = 50,
                B = 80
            };

            Label lbl = new Label()
            {
                Content = Name,
                Background = new SolidColorBrush(backCol)
            };

            gr.Children.Add(lbl);
            gr.Children.Add(Avatar);

            Grid.SetColumn(Avatar, 0);
            Grid.SetRow(Avatar, 0);

            Grid.SetColumn(lbl, 0);
            Grid.SetRow(lbl, 1);

            return gr;
        }
    }
}
