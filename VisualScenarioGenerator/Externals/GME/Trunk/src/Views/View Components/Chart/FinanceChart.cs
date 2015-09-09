using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ChartDirector;

namespace AME.Views.View_Components.Chart
{
    public partial class FinanceChart : UserControl, Chart.IChartExplorer
    {
        private FinanceChartData.ChartDataClass chartData = null;
        private string chartName;
        private OutputType chartOutputType;
        private String chartYAxis;
        private String chartOutputSubType;

        public FinanceChart(FinanceChartData.ChartDataClass data, String title, OutputType outputType, String outputSubType, String yAxis)
        {
            chartData = data;
            chartName = title;
            chartOutputType = outputType;
            chartYAxis = yAxis;
            chartOutputSubType = outputSubType;
            InitializeComponent();
        }

        #region IChartExplorer Members

        public string getName() { return chartName; }

        public int getNoOfCharts() { return 1; }

        public void createChart(WinChartViewer viewer, DateTime beginDate, Double dateRange)
        {
            //dateRange = dateRange / 3600.0;
            DateTime viewPortStartDate = beginDate.AddSeconds(Math.Round(viewer.ViewPortLeft * dateRange));
            DateTime viewPortEndDate = viewPortStartDate.AddSeconds(Math.Round(viewer.ViewPortWidth * dateRange));
            Double vpStart = Math.Round(viewer.ViewPortLeft * dateRange);
            Double vpEnd = vpStart + Math.Round(viewer.ViewPortWidth * dateRange);
            //TimeSpan hoursCalc = viewPortEndDate.Subtract(viewPortStartDate);
            //int hours = (hoursCalc.Days * 24) + hoursCalc.Hours;
            //viewPortEndDate = viewPortEndDate.AddMinutes(12 * hours); // hack to show hour labels
            //Double axisLowerLimit = 0 + viewer.ViewPortTop * rowRange;
            //Double axisUpperLimit = axisLowerLimit + viewer.ViewPortHeight * (rowRange);

            XYChart c = new XYChart(viewer.Width - 5, viewer.Height - 5);
            // Add a title to the chart
            c.addTitle(chartName);

             // Set the plotarea at (50, 20) and of size 200 x 5200 pixels
            c.setPlotArea(50, 20, viewer.Width - 70, viewer.Height - 75);
            c.setClipping();

            // Add a bar chart layer using the given data
            if (chartOutputType.Equals(OutputType.INFO_ELEMENT))
            {
                c.addBarLayer(chartData.HighData.ToArray(), -1, chartOutputSubType);
            }
            else
            {
                c.addScatterLayer(new double[0],
                new ArrayMath(chartData.HighData.ToArray()).selectNEZ(chartData.IconType.ToArray(), ChartDirector.Chart.NoValue).result(),
                     chartOutputSubType, ChartDirector.Chart.CircleShape, 6, ChartDirector.Chart.CColor(Color.DarkOrange));

                LineLayer ll = c.addStepLineLayer();
                ll.addDataSet(chartData.HighData.ToArray(), ChartDirector.Chart.CColor(Color.DarkGray), "");
                ll.setLineWidth(3);
            }

            c.yAxis().setTitle(chartYAxis);
            c.yAxis().setLinearScale(0, 1.25, 0.25);
            ChartDirector.Mark mark = c.yAxis().addMark(1.0, 0x008000, "Max");
            mark.setLineWidth(2);
            mark.setDrawOnTop(false);

            // Set the labels on the x axis.
            //c.xAxis().setDateScale(viewPortStartDate, viewPortEndDate);
            c.xAxis().setLinearScale(vpStart, vpEnd, 1.0);
            c.xAxis().setMargin(10, 10);

            if (chartOutputType.Equals(OutputType.INFO_ELEMENT))
            {
                ChartDirector.BarLayer bl = c.addBarLayer();
                List<Double> barData = new List<Double>();
                int whiteColor = ChartDirector.Chart.CColor(Color.White);
                int redColor = ChartDirector.Chart.CColor(Color.Red);
                // add marks to the bar chart so we can see if a value should be there (even if it's zero)
                for (int i = 0; i < chartData.RealDataAtThisPoint.Count; i++)
                {
                    if (chartData.RealDataAtThisPoint[i])
                    {
                        barData.Add(0.05);
                        //c.xAxis().addMark(i, redColor);
                    }
                    else
                    {
                        barData.Add(0.0);
                    }
                        
                }
                bl.setBorderColor(redColor);
                bl.setBarWidth(0);
                bl.addDataSet(barData.ToArray(), whiteColor);
            }
                              
            // begin time in chart director format
            String beginCD = ""+ChartDirector.Chart.CTime(beginDate);
            // chart director uses seconds, convert our timeincrement to seconds and add them to the start time
            String partialTimeString = "{=("+beginCD+"+{value}*"+ChartExplorer.TimeIncrementInSecondsString+")|";
            c.xAxis().setMultiFormat(ChartDirector.Chart.StartOfDayFilter(), "<*font=bold*>"+partialTimeString+"m/d hhnn}", // show the day text once for each day
                                     ChartDirector.Chart.AllPassFilter(), partialTimeString+"hhnn}"); // military time for all others
            
            //c.xAxis().setLabels(chartData.TimeStamps.ToArray());
            c.xAxis().setTitle("Time Period: Hours");
            //c.xAxis().setLabelStep(4);
            //c.yAxis().setMultiFormat(ChartDirector.Chart.StartOfHourFilter(), "<*font=bold*>{value|w hhnn}", ChartDirector.Chart.AllPassFilter(), "{value|w hhnn}");


            // output the chart
            try
            {
                viewer.Image = c.makeImage();
                String query = "x={x}&xLabel={xLabel}&dataSet={dataSet}&dataSetName={dataSetName}&value={value}";
                viewer.ImageMap = c.getHTMLImageMap(query, query);
            }
            catch (Exception) // occasionally c.makeImage(); crashes - some sort of drawing exception?
                              // if we catch, the next redraw/resize should be fine...
            {

            }
        }
        #endregion
    }
}

