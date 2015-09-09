using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace AME.Views.View_Components.Chart
{
    public partial class TablePopUp : Form
    {
        public TablePopUp(String time, String popupLabel, FinanceChartData.ChartDetailDataClass data)
        {
            InitializeComponent();
            this.Text = popupLabel;
            try
            {
                double xLabelTime = (Double.Parse(time) / ChartExplorer.TimeIncrementsPerHour); // pop-ups expect hours, but time is in time increments, convert
                this.timelabel.Text = ChartExplorer.BeginDate.AddHours(xLabelTime).ToString("MM/dd/yyyy HH:mm");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message); // parse could fail or something
            }

            if (data != null)
            {
                for (int i = 0; i < data.Name.Count; i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = data.Name[i];
                }
            }
        }

        //private String formatTimeLabel(String time)
        //{
        //    StringBuilder s2 = new StringBuilder();
        //    int hours = Convert.ToInt32(time);
        //    int days = hours / 24;

        //    hours -= (days * 24);

        //    s2.Append("Time: Day: ");
        //    s2.Append(days);
        //    if (hours > 9)
        //    {
        //        s2.Append(" Time: ");
        //        s2.Append(hours);
        //        s2.Append("00");
        //    }
        //    else
        //    {
        //        s2.Append(" Time: 0");
        //        s2.Append(hours);
        //        s2.Append("00");
        //    }
        //    return s2.ToString();
        //}

    }
}