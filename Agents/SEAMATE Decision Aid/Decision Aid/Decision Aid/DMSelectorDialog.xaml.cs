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
    /// Interaction logic for DMSelectorDialog.xaml
    /// </summary>
    public partial class DMSelectorDialog : Window
    {
        private List<String> _decisionMakerList;
        private String _selectedDM = "";
        public String SelectedDecisionMaker
        {
            get { return _selectedDM; }
        }
        public DMSelectorDialog()
        {
            InitializeComponent();
            _decisionMakerList = new List<string>();
        }
        public DMSelectorDialog(List<String> DMs)
        {
            InitializeComponent();
            _decisionMakerList = new List<string>(DMs);
            listBoxDecisionMakers.Items.Clear();
            foreach (String s in DMs)
            {
                listBoxDecisionMakers.Items.Add(s);
            }
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            int i = listBoxDecisionMakers.SelectedIndex;
            if (i < 0)
                return;

            _selectedDM = _decisionMakerList[i];
            DialogResult = true;
            this.Close();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

    }
}
