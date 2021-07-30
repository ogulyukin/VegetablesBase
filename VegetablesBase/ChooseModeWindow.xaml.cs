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
    /// Логика взаимодействия для ChooseModeWindow.xaml
    /// </summary>
    public partial class ChooseModeWindow : Window
    {
        public bool ResultForm;
        public ChooseModeWindow()
        {
            InitializeComponent();
            ResultForm = false;
        }
        public ChooseModeWindow(List<InfoClass> types, List<InfoClass> colors)
        {
            InitializeComponent();
            ResultForm = false;
            ListOfColors.ItemsSource = colors;
            ListOfColors.SelectedIndex = 0;
            ListOfTypes.ItemsSource = types;
            ListOfTypes.SelectedIndex = 0;
        }

        private void checkCalory_Checked(object sender, RoutedEventArgs e)
        {
            if(((CheckBox)sender).Name == "checkMinCalory")
            {
                checkMaxCalory.IsChecked = false;
                checkRangeCalory.IsChecked = false;
            }
            if (((CheckBox)sender).Name == "checkMaxCalory")
            {
                checkMinCalory.IsChecked = false;
                checkRangeCalory.IsChecked = false;
            }
            if (((CheckBox)sender).Name == "checkRangeCalory")
            {
                checkMaxCalory.IsChecked = false;
                checkMinCalory.IsChecked = false;
            }
        }

        private void newModeButton_Click(object sender, RoutedEventArgs e)
        {
            ResultForm = true;
            this.Close();
        }

        private void canselButton_Click(object sender, RoutedEventArgs e)
        {
            ResultForm = false;
            this.Close();
        }
    }
}
