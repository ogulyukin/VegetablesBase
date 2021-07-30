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

namespace VegetablesBase
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class AddTypeWindow : Window
    {
        public AddTypeWindow() 
        { 
            InitializeComponent(); 
        }
        public AddTypeWindow(List<InfoClass> _InfoRows)
        {
            InitializeComponent();
            TypesList.ItemsSource = _InfoRows;
        }
        private void newTypeButton_Click(object sender, RoutedEventArgs e)
        {
            if (newTypeText.Text == "")
            {
                MessageBox.Show("Не введено название типа!", "Ошибка");
                return;
            }
            this.Close();
        }

        private void canselButton_Click(object sender, RoutedEventArgs e)
        {
            newTypeText.Text = "";
            this.Close();
        }
    }
}
