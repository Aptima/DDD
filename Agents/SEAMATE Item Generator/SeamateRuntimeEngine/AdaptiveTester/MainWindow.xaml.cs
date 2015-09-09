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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AdaptiveSelector;

namespace AdaptiveTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AdaptiveSelector.AdaptiveSelector _selector;
        public MainWindow()
        {
            InitializeComponent();
            _selector = new AdaptiveSelector.AdaptiveSelector(new MaintainMethod(), "");
            matrixControl.SetMatrix(_selector.GetMatrix());

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double d = Math.Min(e.NewSize.Width, e.NewSize.Height - ((Grid)sender).RowDefinitions[0].Height.Value - ((Grid)sender).RowDefinitions[2].Height.Value);
            matrixControl.Height = d;
            matrixControl.Width = d;
        }

        private void buttonProcess_Click(object sender, RoutedEventArgs e)
        {
            double cpe1 = 0;
            double cpe2 = 0;
            if (Double.TryParse(tbCpe1.Text, out cpe1) == false || Double.TryParse(tbCpe2.Text, out cpe2) == false)
            {
                MessageBox.Show("Invalid input");
                return;
            }
            
           
            String method = ((ComboBoxItem)comboBox1.SelectedItem).Content.ToString();
            switch (method)
            { 
                case "Maintain":
                    _selector.SetGoal(new MaintainMethod());
                    break;
                case "Challenge":
                    _selector.SetGoal(new ChallengeMethod());
                    break;
                case "Consolidate":
                    _selector.SetGoal(new ConsolidateMethod());
                    break;
            }
            CellRange cur = _selector.GetCurrentCpeItem(cpe1, cpe2);
            CellRange next = _selector.GetNextItem(cpe1, cpe2);

            matrixControl.SetCurrentAndNextColors(cur.CellNumber, next.CellNumber);
        }
    }
}
