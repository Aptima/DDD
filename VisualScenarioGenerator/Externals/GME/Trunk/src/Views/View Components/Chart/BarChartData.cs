using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace AME.Views.View_Components.Chart {
	
	public class BarChartData : IChartData {

		private static readonly ILog logger = LogManager.GetLogger(typeof(BarChartData));

        private String title = String.Empty;
		private double[] data = null;
		private String[] labels = null;
		private int[] colors = null;

		public BarChartData(String title, double[] data, String[] labels, int[] colors) {
            this.title = title;
            this.data = data;
            this.labels = labels;
			this.colors = colors;
        }

        public virtual ChartType ChartType {
            get {
                return ChartType.BAR;
            }
        }

        public virtual String Title {
            get {
                return this.title;
            }
        }

        public virtual double[] Data {
            get {
                return this.data;
            }
        }

        public virtual String[] Labels {
            get {
                return this.labels;
            }
        }

		public int[] Colors {
			get { 
				return colors; 
			}
		}
	}
}
