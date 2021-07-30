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
    /// Логика взаимодействия для AddGood.xaml
    /// </summary>
    public partial class AddGood : Window
    {
        private List<InfoClass> _GoodsView;
        public AddGood()
        {
            InitializeComponent();
        }
        public AddGood(List<InfoClass> types, List<InfoClass> colors, List<Good> goods)
        {
            InitializeComponent();
            _GoodsView = new();
            ListOfColors.ItemsSource = colors;
            ListOfColors.SelectedIndex = 0;
            ListOfTypes.ItemsSource = types;
            ListOfTypes.SelectedIndex = 0;
            ListOfGoods.ItemsSource = _GoodsView;
            foreach(var tempGood in goods)
            {
                string tempGoodStr = tempGood.Name + " Тип: " + findName(types, tempGood.TypeId) + " Цвет: " + findName(colors, tempGood.ColorId) + " Калорийность: " + tempGood.Calory;
                InfoClass goodsItem = new InfoClass(0, tempGoodStr);
                _GoodsView.Add(goodsItem);
            }
        }

        private void canselButton_Click(object sender, RoutedEventArgs e)
        {
            newGoodText.Text = "";
            this.Close();
        }

        private void newGoodButton_Click(object sender, RoutedEventArgs e)
        {
            if (newGoodText.Text == "")
            {
                MessageBox.Show("Не введено название!", "Ошибка");
                return;
            }
            if (newCaloryText.Text == "")
            {
                MessageBox.Show("Не введена калорийность!", "Ошибка");
                return;
            }
            this.Close();
        }
        private string findName(List<InfoClass> list, int id)
        {
            foreach(var temp in list)
            {
                if (temp.ID == id)
                    return temp.Name;
            }
            return "Not found";
        }
    }
}
