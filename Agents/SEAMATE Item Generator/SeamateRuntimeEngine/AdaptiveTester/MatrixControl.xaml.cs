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
    /// Interaction logic for MatrixControl.xaml
    /// </summary>
    public partial class MatrixControl : UserControl
    {
        private static bool _isModifying = false;
        private Dictionary<int, SingleItemBox> _workaround;
        public MatrixControl()
        {
            InitializeComponent();

            _workaround = new Dictionary<int, SingleItemBox>();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            /*if (_isModifying)
                return;
            _isModifying = true;
            ((MatrixControl)sender).Height = Math.Min(e.NewSize.Width, e.NewSize.Height);
            ((MatrixControl)sender).Width = ((MatrixControl)sender).Height;
            //e.Handled = true;
            _isModifying = false;*/
        }
        internal void SetCurrentAndNextColors(int currentId, int nextId)
        { 
            SingleItemBox p= _workaround[1];
            bool continuing = p != null;
            int x = 1;
            while(_workaround.ContainsKey(x))
            {
                p = _workaround[x];
                p.SetSelected(0);
                
                x++;
            }

            p =  _workaround[currentId];
            p.SetSelected(1);

            p =  _workaround[nextId];
            p.SetSelected(2);


        }
        internal void SetMatrix(AdaptiveSelector.CpeMatrix cpeMatrix)
        {
            labelBlank.Visibility = System.Windows.Visibility.Collapsed;
            int cols = cpeMatrix.GetColumnCount();
            int rows = cpeMatrix.GetRowCount();
            double colWidth = this.Width / (cols - 1);
            double rowHeight = this.Height / (rows - 1);

            ColumnDefinition cd;
            RowDefinition rd;
            for (int x = 0; x < cols; x++)
            { 
                cd = new ColumnDefinition();
                cd.Width = new GridLength(colWidth, GridUnitType.Star);
                mainGrid.ColumnDefinitions.Add(cd);
            }
            for (int x = 0; x < rows; x++)
            {
                rd = new RowDefinition();
                rd.Height = new GridLength(rowHeight, GridUnitType.Star);
                mainGrid.RowDefinitions.Add(rd);
            }

            SingleItemBox box;
            CellRange c;
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    c = cpeMatrix.GetCellByIndex(y, x);
                    Console.WriteLine(String.Format("Making item '{0}' at x={1};y={2};", c.CellNumber,x,y));
                    box = new SingleItemBox(c);
                    box.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                    box.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                    box.Name = "box_" + c.CellNumber;
                    Grid.SetColumn(box, x);
                    Grid.SetRow(box, y);
                    _workaround[c.CellNumber] = box;
                    mainGrid.Children.Add(box);
                }
            }
        }
    }
}
