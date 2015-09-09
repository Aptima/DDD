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
    /// Interaction logic for SingleItemBox.xaml
    /// </summary>
    public partial class SingleItemBox : UserControl
    {
        private CellRange _range;
        public SingleItemBox()
        {
            InitializeComponent();
        }
        public SingleItemBox(CellRange range)
        {
            InitializeComponent();
            _range = range;
            idLabel.Content = _range.CellNumber;
        }

        internal void SetSelected(int p)
        {
            switch (p)
            { 
                case 1:
                    myBorder.Background = new SolidColorBrush(Colors.Yellow);
                    break;
                case 2:
                    myBorder.Background = new SolidColorBrush(Colors.GreenYellow);
                    break;
                default:
                    myBorder.Background = new SolidColorBrush(Color.FromRgb(233, 233, 233));
                    break;
            }
        }
    }
}
