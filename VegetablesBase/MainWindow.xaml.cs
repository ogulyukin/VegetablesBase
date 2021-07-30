using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace VegetablesBase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string _connection = "Data Source=base.sqlite;Mode=ReadWrite;";
        private ObservableCollection<InfoClass> _InfoRows;
        private List<Good> _goodsList;
        private List<InfoClass> _colors;
        private List<InfoClass> _types;
        private int _modeV;
        public MainWindow()
        {
            InitializeComponent();
            _InfoRows = new();
            _goodsList = new();
            _colors = new();
            _types = new();
            InformationView.ItemsSource = _InfoRows;
            AddColor.IsEnabled = false;
            AddGood.IsEnabled = false;
            AddType.IsEnabled = false;
            AllGoogs.IsEnabled = false;
            NamesGoods.IsEnabled = false;
            ChooseMode.IsEnabled = false;
        }

        private void LoadData_Click(object sender, RoutedEventArgs e)
        {
            AddColor.IsEnabled = true;
            AddGood.IsEnabled = true;
            AddType.IsEnabled = true;
            AllGoogs.IsEnabled = true;
            NamesGoods.IsEnabled = true;
            ChooseMode.IsEnabled = true;
            _goodsList = DbWorks.getGood(_connection);
            _types = DbWorks.getGoodTypes(_connection);
            _colors = DbWorks.getColors(_connection);
            _modeV = 0;
            RefreshView();
            SummaryText.Text = $"Итого выбрано элементов {_InfoRows.Count}";
            if (_goodsList.Count > 0)
                SetCaloryText();
        }

        private void AllGoogs_Click(object sender, RoutedEventArgs e)
        {
            _modeV = 0;
            RefreshView();
        }
        private void AddData_Click(object sender, RoutedEventArgs e)
        {
            if(((Button)sender).Name == "AddType")
            {
                AddTypeWindow dialog = new AddTypeWindow(_types);
                dialog.ShowDialog();
                if (dialog.newTypeText.Text == "")
                    return;
                int result = DbWorks.AddType(dialog.newTypeText.Text, _connection);
                if (result == 0)
                {
                    MessageBox.Show($"Не удалось добавить тип {dialog.newTypeText.Text}", "Ошибка");
                    return;
                }
                InfoClass newType = new InfoClass(result, dialog.newTypeText.Text);
                _types.Add(newType);
            }
            if (((Button)sender).Name == "AddColor")
            {
                AddColor dialog = new AddColor(_colors);
                dialog.ShowDialog();
                if (dialog.newColorText.Text == "")
                    return;
                int result = DbWorks.AddColor(dialog.newColorText.Text, _connection);
                if (result == 0)
                {
                    MessageBox.Show($"Не удалось добавить цвет {dialog.newColorText.Text}", "Ошибка");
                    return;
                }
                InfoClass newColor = new InfoClass(result, dialog.newColorText.Text);
                _colors.Add(newColor);
            }
            if (((Button)sender).Name == "AddGood")
            {
                if(_types.Count == 0 || _colors.Count == 0)
                {
                    MessageBox.Show("Нет доступных типов или цветов", "Ошибка");
                    return;
                }
                AddGood dialog = new AddGood(_types, _colors, _goodsList);
                dialog.ShowDialog();
                if (dialog.newGoodText.Text == "" || dialog.newCaloryText.Text == "")
                    return;
                
                bool succes = Int32.TryParse(dialog.newCaloryText.Text, out int tempCalory);
                int tempType = ((InfoClass)dialog.ListOfTypes.SelectedItem).ID;
                int tempColor = ((InfoClass)dialog.ListOfColors.SelectedItem).ID;
                                
                if (!succes || tempCalory == 0 || tempColor == 0 || tempType == 0)
                {
                    MessageBox.Show($"Не заполнены все поля для товара {dialog.newGoodText.Text}", "Ошибка");
                    return;
                }
                    int result = DbWorks.AddGood(dialog.newGoodText.Text, tempType, tempColor, tempCalory, _connection);
                if (result == 0)
                {
                    MessageBox.Show($"Не удалось добавить товар {dialog.newGoodText.Text}", "Ошибка");
                    return;
                }
                Good newGood = new Good();
                newGood.ID = result;
                newGood.Name = dialog.newGoodText.Text;
                newGood.TypeId = tempType;
                newGood.ColorId = tempColor;
                newGood.Calory = tempCalory;
                _goodsList.Add(newGood);
                RefreshView();
                if (_goodsList.Count > 0)
                {
                    SetCaloryText();
                    SummaryText.Text = $"Итого выбрано элементов {_InfoRows.Count}";
                }
            }
        }

        private string findName(List<InfoClass> list, int id)
        {
            foreach (var temp in list)
            {
                if (temp.ID == id)
                    return temp.Name;
            }
            return "Not found";
        }

        private void NamesGoods_Click(object sender, RoutedEventArgs e)
        {
            _modeV = 1;
            RefreshView();

        }

        private void ChoseMode_Click(object sender, RoutedEventArgs e)
        {
            ChooseModeWindow dialog = new ChooseModeWindow(_types, _colors);
            dialog.ShowDialog();
            if (!dialog.ResultForm)
                return;
            RefreshView();
            if (dialog.checkType.IsChecked == true )
            {
                List<InfoClass> TempInfo = new();
                int tempType = ((InfoClass)dialog.ListOfTypes.SelectedItem).ID;
                foreach (var temp in _InfoRows.ToList())
                {
                    if (getTypeById(temp.ID) == tempType)
                    {
                        TempInfo.Add(temp);
                    }
                    _InfoRows.Clear();
                    CopyInfoList(TempInfo);
                }
            }
            if(dialog.checkColor.IsChecked == true)
            {
                List<InfoClass> TempInfo = new();
                int tempColor = ((InfoClass)dialog.ListOfColors.SelectedItem).ID;
                foreach (var temp in _InfoRows.ToList())
                {
                    if (getColorById(temp.ID) == tempColor)
                    {
                        TempInfo.Add(temp);
                    }
                    _InfoRows.Clear();
                    CopyInfoList(TempInfo);
                }
            }
            if(dialog.checkMinCalory.IsChecked == true)
            {
                bool succes = Int32.TryParse(dialog.minCalory.Text, out int tempCalory);
                if (!succes || tempCalory == 0)
                    return;
                List<InfoClass> TempInfo = new();
                foreach (var temp in _InfoRows.ToList())
                {
                    if (getCaloryById(temp.ID) < tempCalory)
                    {
                        TempInfo.Add(temp);
                    }
                }
                _InfoRows.Clear();
                CopyInfoList(TempInfo);
            }
            if (dialog.checkMaxCalory.IsChecked == true)
            {
                bool succes = Int32.TryParse(dialog.maxCalory.Text, out int tempCalory);
                if (!succes || tempCalory == 0)
                    return;
                List<InfoClass> TempInfo = new();
                foreach (var temp in _InfoRows.ToList())
                {
                    if (getCaloryById(temp.ID) > tempCalory)
                    {
                        TempInfo.Add(temp);
                    }
                }
                _InfoRows.Clear();
                CopyInfoList(TempInfo);
            }
            if (dialog.checkRangeCalory.IsChecked == true)
            {
                bool succes01 = Int32.TryParse(dialog.startCalory.Text, out int tempCalory01);
                bool succes02 = Int32.TryParse(dialog.endCalory.Text, out int tempCalory02);
                if (!succes01 || !succes02)
                    return;
                List<InfoClass> TempInfo = new();
                foreach (var temp in _InfoRows.ToList())
                {
                    if (getCaloryById(temp.ID) < tempCalory02 && getCaloryById(temp.ID) > tempCalory01)
                    {
                        TempInfo.Add(temp);
                    }
                }
                _InfoRows.Clear();
                CopyInfoList(TempInfo);
            }
            SummaryText.Text = $"Итого выбрано элементов {_InfoRows.Count}";
        }

        private void SetCaloryText()
        {
            int minCalory = _goodsList.ElementAt(0).Calory, maxCalory = _goodsList.ElementAt(0).Calory, summCalory = 0;
            foreach(var tempGood in _goodsList)
            {
                summCalory += tempGood.Calory;
                if (tempGood.Calory < minCalory)
                    minCalory = tempGood.Calory;
                if (tempGood.Calory > maxCalory)
                    maxCalory = tempGood.Calory;
            }
            AverageCaloryText.Text = $"Средняя: {summCalory / _goodsList.Count}";
            MaxCaloryText.Text = $"Максимальная: {maxCalory}";
            MinCaloryText.Text = $"Минимальная: {minCalory}";
        }
        private void RefreshView()
        {
            _InfoRows.Clear();
            if (_modeV == 0)
            {
                foreach (var tempGood in _goodsList)
                {
                    string tempGoodStr = "ID: " + tempGood.ID + ". " + tempGood.Name + " Тип: " + findName(_types, tempGood.TypeId) + " Цвет: " + findName(_colors, tempGood.ColorId) + " Калорийность: " + tempGood.Calory;
                    InfoClass goodsItem = new InfoClass(tempGood.ID, tempGoodStr);
                    _InfoRows.Add(goodsItem);
                }
            }
            if (_modeV == 1)
            {
                foreach (var tempGood in _goodsList)
                {
                    string tempGoodStr = "ID: " + tempGood.ID + ". " + tempGood.Name;
                    InfoClass goodsItem = new InfoClass(tempGood.ID, tempGoodStr);
                    _InfoRows.Add(goodsItem);
                }
            }
            SummaryText.Text = $"Итого выбрано элементов {_InfoRows.Count}";
        }
        
        private int getColorById(int myid)
        {
           return (_goodsList.Find(x => x.ID == myid)).ColorId;
        }
        
        private int getTypeById(int myid)
        {
            return (_goodsList.Find(x => x.ID == myid)).TypeId;
        }
        
        private int getCaloryById(int myid)
        {
            return (_goodsList.Find(x => x.ID == myid)).Calory;
        }

        private void CopyInfoList(List<InfoClass> list)
        {
            foreach (var temp in list)
            {
                _InfoRows.Add(temp);
            }
        }

        private void ColorSummary_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<int, int> colorCount = new();
            foreach(var i in _colors)
            {
                colorCount.Add(i.ID, 0);
            }
            foreach(var i in _goodsList)
            {
                colorCount[i.ColorId]++;
            }
            _InfoRows.Clear();
            foreach(var i in colorCount.Keys)
            {
                InfoClass tempInfo = new InfoClass(i, (_colors.Find(x => x.ID == i)).Name + ": "  + colorCount[i].ToString() + " единиц");
                _InfoRows.Add(tempInfo);
            }
            SummaryText.Text = $"Итого выбрано элементов {_InfoRows.Count}";
        }
    }
}
