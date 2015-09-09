using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Decision_Aid
{
    /// <summary>
    /// Interaction logic for SelectObjectDialog.xaml
    /// </summary>
    public partial class SelectObjectDialog : Window
    {
        private List<String> _ids;
        private List<String> _names;
        private String _selectedID = "-1";
        public String SelectedID
        {
            get { return _selectedID; }
        }
        public SelectObjectDialog()
        {
            InitializeComponent();
           // this.DialogResult = false;
            _ids = new List<string>();
            _names = new List<string>();
        }
        public void SetList(List<String> ids, List<String> names, String selectedObjectID)
        {
            
            _names.AddRange(names);
            comboBoxObjects.Items.Clear();

            //only works when IDs are ints
            List<Int32> i = new List<int>();
            List<String> si = new List<string>();
            foreach (String s in ids)
            {
                try
                {
                    i.Add(Int32.Parse(s));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            i.Sort();
            //foreach (String s in _names)
            int c = 0;
            int ind = 0;
            foreach(Int32 s in i) //AD made this change 8/5 on Courtney's suggestion
            {
                si.Add(s.ToString());
                comboBoxObjects.Items.Add(s.ToString());
                if (s.ToString() == selectedObjectID)
                {
                    ind = c;
                }
                c++;
            }
            comboBoxObjects.SelectedIndex = ind;
            _ids.AddRange(si);
        }
        

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            int i = comboBoxObjects.SelectedIndex;
            if (i < 0 || i >= _ids.Count)
                return;

            _selectedID = _ids[i];

            this.DialogResult = true;
            this.Close();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
