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
using System.Collections.ObjectModel;


namespace uchot_tovara.Properties
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    /// 

    public class SKLAD_LIST
    {
        public string NAME { get; set; } // название товара
        public double QUANTITY { get; set; } // количество товара
        public double PRICE { get; set; } // цена за единицу товара
        public string UNIT { get; set; } // единыцы измерения 
        
        public SKLAD_LIST(SKLAD A)
        {
            NAME = A.NAME;
            QUANTITY = A.QUANTITY;
            PRICE = A.PRICE;
            UNIT = A.UNIT;
        }
        public SKLAD_LIST()
        {
            
        }
        public void create_items_sklad(ObservableCollection<SKLAD_LIST> d, List<SKLAD> sklad)
        {
            for (int i = 0; i < sklad.Count; i++)
            {
                d.Add(new SKLAD_LIST(sklad[i]));
            }

        }
    }

    public partial class Window1 : Window
    {
        public ObservableCollection<SKLAD_LIST> SKLAD_LIST_VIEW_ITEMS = new ObservableCollection<SKLAD_LIST>();// collection of items for menulist
        public Window1()
        {
            InitializeComponent();
            SKLAD_LIST s = new SKLAD_LIST();
            s.create_items_sklad(SKLAD_LIST_VIEW_ITEMS, MainWindow.sklad);
            skladlist.ItemsSource = SKLAD_LIST_VIEW_ITEMS;
        }
        private void za(object sender, EventArgs e)
        {
            MainWindow m = new MainWindow();

            string name = nametov.Text.Trim();
            string quant = koltov.Text.Trim();
            string price = pricetow.Text.Trim();
            string unit = unittow.Text.Trim();

            try
            {
                m.ZACHISLENIE(name, Convert.ToDouble(quant), Convert.ToDouble(price), unit);

                MainWindow.sklad.Clear();
                m.work_exel_SKLAD();
                SKLAD_LIST_VIEW_ITEMS.Add(new SKLAD_LIST(MainWindow.sklad.Last()));

                MessageBox.Show("добавлено");
            }
            catch(FormatException)
            {
                MessageBox.Show("неверный формат данных \n повторите попытку");
                nametov.Text = "";
                koltov.Text = "";
                pricetow.Text = "";
                unittow.Text = "";

            }
        
        }
        private void close(object sender, EventArgs e)
        {
            this.Close();
        }
        private void clousee(object sender, EventArgs e)
        {
            new uchot_tovara.MainWindow().Show();
        }
    }
}
