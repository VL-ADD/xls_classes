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
using System.Xml.Linq;
using System.IO.Compression;
using System.IO;
using System.Xml;
using System.Collections.ObjectModel;

namespace uchot_tovara
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public class SKLAD
    {
        public string NAME { get; set; } // название товара
        public double QUANTITY { get; set; } // количество товара
        public double PRICE { get; set; } // цена за единицу товара
        public string UNIT { get; set; } // единыцы измерения 
    }

    public class MENU_LIST // 
    {
        public double coast { get; set; }// цена блюда
        public string menu_name { get; set; }// название блюда
        public List<string> INGRES = new List<string>(); // список названий ингридиентов 
        public List<double> INGRES_QUANTITY = new List<double>();// список количества ингридиентов, соответствует по разрядности списку ингридиентов и соответствует его элементам
        
        public MENU_LIST(string[] ingr, double[] quant)
        {
            int i = 1;
            menu_name = ingr[0];
            coast = quant[0];
            for(i = 1; i< ingr.Length; i++)
            {
                if(ingr[i] != null)
                {
                    
                    INGRES.Add(ingr[i].Trim());
                }
                
            }
            for (i = 1; i < quant.Length; i++)
            {
                if(quant[i] != 0)
                {
                    INGRES_QUANTITY.Add(quant[i]);
                }
                
            } 
        }
    }

    public class MENU_LIST_VIEW
    {
        public double coast { get; set; } //цена
        public string name { get; set; }// название
        public double coast_prise { get; set; }// сибестоимость
        public MENU_LIST_VIEW(MENU_LIST menu)
        {
            coast = menu.coast;
            name = menu.menu_name;
            coast_prise = coast_prise_made(menu);
           
        }
        public MENU_LIST_VIEW()
        {

        }

        public double coast_prise_made(MENU_LIST menu)
        {
            double COAST = 0;
            for (int i = 0; i < menu.INGRES.Count; i++)
            {
                for(int j =0; j< MainWindow.sklad.Count; j++)
                {
                    if(menu.INGRES[i] == MainWindow.sklad[j].NAME)
                    {
                        double buff = menu.INGRES_QUANTITY[i] * MainWindow.sklad[j].PRICE;
                        COAST += buff;
                        break;
                    }
                }
            }
            return COAST;
        }

        public void create_items_view(ObservableCollection<MENU_LIST_VIEW> d, List<MENU_LIST> menu)
        {
            for(int i =0; i < menu.Count; i++)
            {
                d.Add(new MENU_LIST_VIEW(menu[i]));
            }
           
        }
       
    }


    public class check
    {
        //public string filename { get; set; }


        public void createcheck_like_a_text(TextBox a, List<string> ss)
        {
            string buff = "";
            string[] buff_mass = a.Text.Split(new char[] { ' ', ',', '.', '/', '\t' });
            foreach (string f in buff_mass)
            {
                buff = f.Trim();
                ss.Add(buff);
            }
        }

        public void create_file_check(string file_name, List<string> ss)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(file_name, false))
                {
                    foreach (string s in ss)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("format exception");
            }
        }

        public void write_add_file_check(string file_name, List<string> ss)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(file_name, true))
                {
                    foreach (string s in ss)
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("format exception");
            }
        }

        public void write_txt(string textname, string path)
        {
            string[] u = DateTime.Today.ToString("d").Split('.');
            string txt = @"C:\Users\Admin\Desktop\dir\" + textname +" " +u[1] + "_" + u[2]+".txt";
            double pr = 0, selfrp = 0;
            path = path.Trim();
            string[] buff = path.Split(',', '/', '.', ';');
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i] = buff[i].Trim();
            }
            List<string> str = new List<string>();
            str = create_text_for_chek(buff, pr, selfrp);
            

            using (FileStream file = new FileStream(txt, FileMode.Append, FileAccess.Write))
            {

                using (StreamWriter sw = new StreamWriter(file))
                {


                    sw.Write("_____________________________++++++++++_______________________________\n");
                    sw.Write(DateTime.Now.ToString() + '\n');
                    sw.Write("\n");

                    for(int l1 =0; l1< str.Count -2; l1++)
                    {
                        sw.Write(str[l1] + "\n");
                    }


                    sw.WriteLine();
                    int l = str.Count;
                    sw.Write("всего за заказ: " + str[l-2] + "   общая сибестоимость:  " + str[l-1] + "\n" );
                    sw.Write("_____________________________++++++++++_______________________________\n");

                    //sw.WriteLine(path);

                    sw.Close();
                }

            }
        }

        public List<string> create_text_for_chek( string[] path, double all_prise, double all_selfprice)
        {
            List<string> ret = new List<string>();
            int ind = 0;
            
            foreach(string e in path)
            {
                string bufer = ind.ToString() + " |  ";
                foreach(MENU_LIST menu in MainWindow.MENU)
                {
                    if(e == menu.menu_name)
                    {
                        MENU_LIST_VIEW cc = new MENU_LIST_VIEW();
                        bufer += menu.menu_name + "  " + "цена: " + menu.coast + " (руб) " + "сибестоимость: " + Convert.ToString( cc.coast_prise_made(menu) ) + " (руб)";
                        all_prise += menu.coast;
                        all_selfprice += cc.coast_prise_made(menu);
                        ret.Add(bufer);
                        break;
                    }
                   
                   

                }
                ind++;
                
            }
            ret.Add(Convert.ToString(all_prise));
            ret.Add(Convert.ToString(all_selfprice));
            foreach (string ee in path)
            {
                try
                {
                    MainWindow r = new MainWindow();
                    r.SPISANIE(ee);
                }
                catch (Exception)
                {

                }
            }



            return ret;
        }



    }

   

    public partial class MainWindow : Window
    {
        static public List<SKLAD> sklad = new List<SKLAD>();// coolection for sklad
        static public List<MENU_LIST> MENU = new List<MENU_LIST>();// collection for menu
        public ObservableCollection<MENU_LIST_VIEW> MENU_LIST_VIEW_ITEMS = new ObservableCollection<MENU_LIST_VIEW>();// collection of items for menulist
        //public ObservableCollection<string> TEXT_OF_CHEK = new ObservableCollection<string>();// коллекция нля формирования одного заказа в строке text box
        static public List<MENU_LIST_VIEW> this_zakaz = new List<MENU_LIST_VIEW>();//коллекция с чеком на 1 заказ 
        static public List<string> check_name = new List<string>();// name of collektion for txt file for check
        static public string name_of_chek_file = "check_name";// txt file for check name

        string zipname = @"C:\Users\Admin\Desktop\sklad.xls";  //ДОКУМЕНТ EXEL С СКЛАДОМ
        string zipdir = @"C:\Users\Admin\Desktop\dir";        //ОБЩАЯ ПАПКА ДЛЯ ФАЙЛОВ 
        string zipextract = @"C:\Users\Admin\Desktop\dir\sklad"; //ПАПКА С СОДЕРЖИМЫМ ФАЙЛА EXEL склад

        string menufile = @"C:\Users\Admin\Desktop\menu.xls"; //ФАЙЛ АРХИВ С МЕНЮ
        string MENUEXTRAKT = @"C:\Users\Admin\Desktop\dir\menu"; //ПАПКА С СОДЕРЖИМЫМ ФАЙЛА EXEL МЕНЮ


        MENU_LIST_VIEW menulistviewobject = new MENU_LIST_VIEW();

         public MainWindow()
        {
            
            InitializeComponent();
            createdirectory();
            MENU.Clear();
            sklad.Clear();
            try
            {
                work_exel_SKLAD();// заполняем коллекцию элементов на складе
                exel_menu_load(); // заполняем коллекцию элементов в меню
            }
            catch(DirectoryNotFoundException)
            {
                refreshfile();
                work_exel_SKLAD();// заполняем коллекцию элементов на складе
                exel_menu_load(); // заполняем коллекцию элементов в меню
            }
           
            
            menulistviewobject.create_items_view(MENU_LIST_VIEW_ITEMS, MENU); // заполняем коллекцию элементов для списка :ListViev  в форме (список меню)
            menulist.ItemsSource = MENU_LIST_VIEW_ITEMS;


            
        }
      
        private void ewr(object sender, EventArgs e)
        {
            int i = -1;
            try
            {
                 i = menulist.SelectedIndex;

            }
            catch (Exception)
            {

            }

            if(i >= 0)
            {
                string nam = MENU_LIST_VIEW_ITEMS[i].name;
                string buff = "";
                foreach (MENU_LIST it in MENU)
                {
                    if (it.menu_name == nam)
                    {
                        for (int j = 0; j < it.INGRES.Count; j++)
                        {
                            buff = buff + it.INGRES[j] + "(" + " " + it.INGRES_QUANTITY[j] + ")  ";
                        }
                    }
                }

                MessageBox.Show("состав :  " + "" + buff);
            }      
        }

        private void TO_SKLAD(object sender, EventArgs e)
        {

            new uchot_tovara.Properties.Window1().Show();
            this.Close();
        }


        private void leftmouse(object sender, EventArgs e)
        {
            int i = -1;
            try
            {
                 i = menulist.SelectedIndex;

            }
            catch(Exception)
            {
                
            }
         
            if(i>=0)
            {
                string nam = " " + "," + MENU_LIST_VIEW_ITEMS[i].name;
                nazvanie_bluda.Text += nam;
            }
          
        }

        private void EXIT(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SPISANIE(string zakaz_name)
        {
           
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(@"C:\Users\Admin\Desktop\dir\sklad\xl\worksheets\sheet1.xml");
            XmlElement xRoot = xDoc.DocumentElement;

            XmlDocument xString = new XmlDocument();
            xString.Load(@"C:\Users\Admin\Desktop\dir\sklad\xl\sharedStrings.xml");
            XmlElement strings = xString.DocumentElement;

            XmlNodeList a = strings.ChildNodes;

            int number = 0; 
            for(int i = 0; i< MENU.Count; i++)
            {
                if(MENU[i].menu_name == zakaz_name)
                {
                    number = i;
                    break;
                }
            }
            
            foreach (XmlNode data in xRoot.ChildNodes)
            {

                if (data.Name == "sheetData")
                {
                    foreach (XmlNode rows in data)
                    {
                       
                        if( MENU[number].INGRES.Contains(  a[Convert.ToInt32(rows.FirstChild.InnerText)].InnerText ) ) //проверяем есть ли в меню блюда  текущий ингридеент
                        {

                            int index = MENU[number].INGRES.FindIndex(x => x == a[Convert.ToInt32(rows.FirstChild.InnerText)].InnerText); // выбираем индекс текущего ингридиента в массиве ингридиентов
                            double buff = Convert.ToDouble(rows.ChildNodes[1].InnerText); // заносим соответствующее ингридиенту количество на складе в буферную переменную
                            buff -= MENU[number].INGRES_QUANTITY[index]; // вычитаем из количества на складе количество в меню
                            rows.ChildNodes[1].InnerText = Convert.ToString(buff); // изменяем значение количества на складе на новое

                            //  int index1 = MENU[number].INGRES.FindIndex(x => x == a[Convert.ToInt32(rows.FirstChild.InnerText)].InnerText);


                            //for (int i = 0; i < sklad.Count; i++)
                            //{
                            //    if (sklad[i].NAME == a[Convert.ToInt32(rows.FirstChild.InnerText)].InnerText)
                            //    {
                            //        sklad[i].QUANTITY = buff;
                            //        break;
                            //    }
                            //}
                            break;///
                        }   
                    }
                    break;
                }
            }
            xDoc.Save(@"C:\Users\Admin\Desktop\dir\sklad\xl\worksheets\sheet1.xml");
        }
        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////
       
        private void CR_CHEK(object sender, EventArgs e)
        {
            string textname = "chek";
            string path = nazvanie_bluda.Text;
            check c = new check();

            c.write_txt(textname, nazvanie_bluda.Text);


            MessageBox.Show("Чек заполнен");
            nazvanie_bluda.Text = "";


        }
        /// <summary>
        /// //////////////////////////////////////////////////////////////////
        private void option(object sender, EventArgs e)
        {
            menuskladoption.Text = menufile;
            skladfileopt.Text = zipname;
            opt.Visibility = Visibility.Visible;

        }


        private void Save_opt(object sender, EventArgs e)
        {
            menufile = menuskladoption.Text;
            zipname = skladfileopt.Text;
            opt.Visibility = Visibility.Hidden;

        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void ZACHISLENIE(string name, double q, double pr, string u)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(@"C:\Users\Admin\Desktop\dir\sklad\xl\worksheets\sheet1.xml");
            XmlElement xRoot = xDoc.DocumentElement;

            XmlDocument xString = new XmlDocument();
            xString.Load(@"C:\Users\Admin\Desktop\dir\sklad\xl\sharedStrings.xml");
            XmlElement strings = xString.DocumentElement;

            XmlNodeList a = strings.ChildNodes;
            string atr_r = "";

            int number = 0;
            for (int i = 0; i < sklad.Count; i++) // проверяем есть ли на складе такое наименивание
            {
                if (sklad[i].NAME == name)
                {
                    number ++;// индекс наименования
                    break;
                }
            }
            if(number != 0)
                {

                foreach (XmlNode data in xRoot.ChildNodes)
                {

                    if (data.Name == "sheetData")
                    {
                        foreach (XmlNode rows in data)
                        {

                            if (name ==  a[Convert.ToInt32(rows.FirstChild.InnerText)].InnerText) //находим соответствующею позицию в файле склада
                            {

                                //int index = MENU[number].INGRES.FindIndex(x => x == a[Convert.ToInt32(rows.FirstChild.InnerText)].InnerText); // выбираем индекс текущего ингридиента в массиве ингридиентов

                                double buff = Convert.ToDouble(rows.ChildNodes[1].InnerText); // заносим соответствующее ингридиенту количество на складе в буферную переменную
                                buff += q; // добавляем к количеству на складе кол во поступившего
                                rows.ChildNodes[1].InnerText = Convert.ToString(buff); // изменяем значение количества на складе на новое

                                for (int i = 0; i < sklad.Count; i++)
                                {
                                    if (sklad[i].NAME == name)
                                    {
                                        sklad[i].QUANTITY = buff;
                                        break;
                                    }
                                }

                            }
                            break;
                        }
                    }
                }
            }
            else
            {

                foreach (XmlNode data in xRoot.ChildNodes) // находим номер последней строки
                {

                    if (data.Name == "sheetData")
                    {
                        var atr = data.LastChild.Attributes.GetNamedItem("r");
                        atr_r = atr.Value;// номер последней строки
                    }
                }
                int ss = a.Count/* + 1*/;                        // создаём новый элемент для файла со строками
                XmlElement si = xString.CreateElement("si");
                XmlElement t = xString.CreateElement("t");
                XmlText t_text = xString.CreateTextNode(name);
                t.AppendChild(t_text);
                si.AppendChild(t);
                strings.AppendChild(si);


                XmlElement c1 = xDoc.CreateElement("c");// создаём новый элемент для строки
                XmlAttribute r_c1 = xDoc.CreateAttribute("r");
                XmlAttribute t_c1 = xDoc.CreateAttribute("t");
                XmlElement v_c1 = xDoc.CreateElement("v");
                XmlText r_c1_text = xDoc.CreateTextNode("A"+ atr_r);
                XmlText t_c1_text = xDoc.CreateTextNode("s");
                XmlText v_c1_text = xDoc.CreateTextNode(Convert.ToString(ss));
                r_c1.AppendChild(r_c1_text);
                t_c1.AppendChild(t_c1_text);
                v_c1.AppendChild(v_c1_text);
                c1.Attributes.Append(r_c1);
                c1.Attributes.Append(t_c1);
                c1.AppendChild(v_c1);

                XmlElement c2 = xDoc.CreateElement("c");
                XmlAttribute r_c2 = xDoc.CreateAttribute("r");
                XmlElement v_c2 = xDoc.CreateElement("v");
                XmlText r_c2_text = xDoc.CreateTextNode("B" + atr_r);
                XmlText v_c2_text = xDoc.CreateTextNode(Convert.ToString(q));
                r_c2.AppendChild(r_c2_text);
                v_c2.AppendChild(v_c2_text);
                c2.Attributes.Append(r_c2);
                c2.AppendChild(v_c2);

                XmlElement c3 = xDoc.CreateElement("c");
                XmlAttribute r_c3 = xDoc.CreateAttribute("r");
                XmlElement v_c3 = xDoc.CreateElement("v");
                XmlText r_c3_text = xDoc.CreateTextNode("C" + atr_r);
                XmlText v_c3_text = xDoc.CreateTextNode(Convert.ToString(pr));
                r_c3.AppendChild(r_c3_text);
                v_c3.AppendChild(v_c3_text);
                c3.Attributes.Append(r_c3);
                c3.AppendChild(v_c3);

                XmlElement c4 = xDoc.CreateElement("c");
                XmlAttribute r_c4 = xDoc.CreateAttribute("r");
                XmlAttribute t_c4 = xDoc.CreateAttribute("t");
                XmlElement v_c4 = xDoc.CreateElement("v");
                XmlText r_c4_text = xDoc.CreateTextNode("D" + atr_r);
                XmlText t_c4_text = xDoc.CreateTextNode("s");
                r_c4.AppendChild(r_c4_text);
                t_c4.AppendChild(t_c4_text);
                //v_c1.AppendChild(v_c1_text);
                c4.Attributes.Append(r_c4);
                c4.Attributes.Append(t_c4);
                //c1.AppendChild(v_c1);

                int flag = 0;
                int ind = 0;
                for(int i = 0; i<a.Count;i++)
                {
                    if(a[i].InnerText == u)
                    {
                        flag ++;
                        ind = i;
                        break;///????///
                    }
                }
                if(flag == 0)//////????????
                {
                    XmlText v_c4_text = xDoc.CreateTextNode(Convert.ToString(a.Count /*+ 1*//*   2   */)); // если такое наименование  еденицы измерения не существует , то мы заносим его новый номер в тег <v> 
                    v_c4.AppendChild(v_c4_text);
                    c4.AppendChild(v_c4);

                    XmlElement si1 = xString.CreateElement("si");// заносим в файл строк новое значение
                    XmlElement t1 = xString.CreateElement("t");
                    XmlText t_text1 = xString.CreateTextNode(u);
                    t1.AppendChild(t_text1);
                    si1.AppendChild(t1);
                    strings.AppendChild(si1);
                }
                else
                {
                    XmlText v_c4_1_text = xDoc.CreateTextNode(Convert.ToString(ind));// если такое наименование  еденицы измерения существует , то мы заносим его номер в тег <v> 
                    v_c4.AppendChild(v_c4_1_text);
                    c4.AppendChild(v_c4);
                }

                XmlElement row = xDoc.CreateElement("row"); // создаём строку для файла лист
                XmlAttribute r = xDoc.CreateAttribute("r");
                XmlAttribute spans = xDoc.CreateAttribute("spans");
                XmlText r_text = xDoc.CreateTextNode(atr_r);
                XmlText spans_text = xDoc.CreateTextNode("1:4");
                r.AppendChild(r_text);
                spans.AppendChild(spans_text);
                row.Attributes.Append(r);
                row.Attributes.Append(spans);
                row.AppendChild(c1);
                row.AppendChild(c2);
                row.AppendChild(c3);
                row.AppendChild(c4);

                foreach (XmlNode data in xRoot.ChildNodes)
                {

                    if (data.Name == "sheetData")
                    {
                        data.AppendChild(row);
                        break;
                    }
                    
                }

           

            }
            xDoc.Save(@"C:\Users\Admin\Desktop\dir\sklad\xl\worksheets\sheet1.xml");
            xString.Save(@"C:\Users\Admin\Desktop\dir\sklad\xl\sharedStrings.xml");
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void eeec(object sender, EventArgs e)
        {
            
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void work_exel_SKLAD() // загружает список фигни на складе в коллекцию 
        {
           
           
            string[] row = new string[4]; // буферный массив в котором будет находится строка файла  SKLAD

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(@"C:\Users\Admin\Desktop\dir\sklad\xl\worksheets\sheet1.xml");
            XmlElement xRoot = xDoc.DocumentElement;

            XmlDocument xString = new XmlDocument();
            xDoc.Load(@"C:\Users\Admin\Desktop\dir\sklad\xl\sharedStrings.xml");
            XmlElement strings = xDoc.DocumentElement;
           

            // обходим все дочерние узлы элемента
            foreach (XmlNode data in xRoot.ChildNodes)
                {
                  
                   if (data.Name == "sheetData")
                   {
                      foreach(XmlNode rows in data)
                      {
                        int i = 0;
                        foreach(XmlNode cc in rows)
                        {
                          XmlNode attr = cc.Attributes.GetNamedItem("t");
                            if (attr != null)
                            {
                                if (attr.Value == "s")
                                {
                                    int s_i = Convert.ToInt32(cc.FirstChild.InnerText);
                                    XmlNodeList a = strings.ChildNodes;
                                    row[i] = a[s_i].InnerText.Trim();/////////////////////////////
                                }
                               
                            }
                            else

                            {
                                row[i] = Convert.ToString(cc.FirstChild.InnerText);
                            }
                            i++;
                        }
                        SKLAD s1 = new SKLAD { NAME = row[0], PRICE = Convert.ToDouble( row[2]), QUANTITY = Convert.ToDouble(row[1]), UNIT = row[3] };////////
                        sklad.Add(s1);
                        i = 0;
                      }

                    break;
                   }
            }             
        }
       ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void exel_menu_load(/*object sender, EventArgs e*/)// загружает меню из файла в коллекцию
        {


            string[] row = new string[100]; // буферный массив в котором будет находится строка файла  SKLAD
            double[] quantity = new double[100];// буферный массив  в котором будет строка количества ингр
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(@"C:\Users\Admin\Desktop\dir\menu\xl\worksheets\sheet1.xml");
            XmlElement xRoot = xDoc.DocumentElement;

            XmlDocument xString = new XmlDocument();
            xDoc.Load(@"C:\Users\Admin\Desktop\dir\menu\xl\sharedStrings.xml");
            XmlElement strings = xDoc.DocumentElement;


            // обходим все дочерние узлы элемента
            foreach (XmlNode data in xRoot.ChildNodes)
            {

                if (data.Name == "sheetData")
                {
                    foreach (XmlNode rows in data)
                    {
                        int i = 0;
                        int j = 0;
                        foreach (XmlNode cc in rows)
                        {
                            XmlNode attr = cc.Attributes.GetNamedItem("t");
                            if (attr != null)
                            {
                                if (attr.Value == "s")
                                {
                                    int s_i = Convert.ToInt32(cc.FirstChild.InnerText);
                                    XmlNodeList a = strings.ChildNodes;
                                   
                                        row[i] = a[s_i].InnerText;
                                    
                                    
                                    i++;
                                }

                            }
                            else

                            {
                                
                                string r = cc.FirstChild.InnerText;
                                string[] buff = r.Split('.');
                                if(buff.Length == 1)
                                {
                                    quantity[j] = Convert.ToDouble(r); //  число целое ( в строке нет запятой)
                                }
                                else
                                {
                                    r = buff[0] + "," + buff[1];
                                    quantity[j] = Convert.ToDouble(r);
                                }
                               
                                j++;
                            }
                           
                        }
                        
                        MENU.Add(new MENU_LIST(row, quantity));
                        i = 0;
                        j = 0;
                    }

                    break;
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        void Buton1_Click(object sender, RoutedEventArgs e)
        {
            
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        void win_unloaded(object sender, EventArgs e)
        {
            
        }

        private void delitdir()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(@"C:\Users\Admin\Desktop\dir\sklad"); // удаляем папку меню с рабочими файлами перед выходом из программы
            if (dirInfo.Exists)
            {
                dirInfo.Delete(true);
            }

            DirectoryInfo dirInfo1 = new DirectoryInfo(@"C:\Users\Admin\Desktop\dir\menu"); // удаляем папку с рабочими файлами перед выходом из программы
            if (dirInfo1.Exists)
            {
                dirInfo1.Delete(true);
            }
        }

        private void refreshfile()
        {
            delitdir();
            createdirectory();

            MENU.Clear();
            sklad.Clear();
            work_exel_SKLAD();// заполняем коллекцию элементов на складе
            exel_menu_load(); // заполняем коллекцию элементов в меню

            MENU_LIST_VIEW_ITEMS.Clear();
            menulistviewobject.create_items_view(MENU_LIST_VIEW_ITEMS, MENU); // заполняем коллекцию элементов для списка :ListViev  в форме (список меню)
            menulist.ItemsSource = MENU_LIST_VIEW_ITEMS;

        }

        private void befreshbutton(object sender, EventArgs e)
        {
            refreshfile();
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        void START(object sender, EventArgs e)
        {
           
        }

        private void createdirectory()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(zipdir);      //создаём папку для файлов
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

          
            if(!File.Exists(zipextract))
            {
                dirInfo.CreateSubdirectory("sklad");               //создаём папку для рабочих файлов архива exel sklad

                if(!File.Exists(@"C:\Users\Admin\Desktop\dir\sklad\xl\worksheets\sheet1.xml"))
                {
                    ZipFile.ExtractToDirectory(zipname, zipextract);    //ИЗВЛЕКАЕМ АРХИВ EXEL В ПАПКУ
                }
               
            }

            if (!File.Exists(MENUEXTRAKT))
            {
                dirInfo.CreateSubdirectory("menu");               //создаём папку для рабочих файлов архива exel menu
                if (!File.Exists(@"C:\Users\Admin\Desktop\dir\menu\xl\worksheets\sheet1.xml"))
                {
                    ZipFile.ExtractToDirectory(menufile, MENUEXTRAKT);    //ИЗВЛЕКАЕМ АРХИВ EXEL В ПАПКУ
                }
               
            }
               
        }

        private void TO_SKLAD(object sender, RoutedEventArgs e)
        {

        }
    }
}
