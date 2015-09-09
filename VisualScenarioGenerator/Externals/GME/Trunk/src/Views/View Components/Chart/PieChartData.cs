using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace AME.Views.View_Components.Chart {

	public class PieChartData : IChartData {

        private static readonly ILog logger = LogManager.GetLogger(typeof(PieChartData));

        private String title = String.Empty;
		private double[] data = null;
		private String[] labels = null;
	

        public PieChartData(String title, double[] data, String[] labels) {
            this.title = title;
            this.data = data;
            this.labels = labels;
        }

        public virtual ChartType ChartType {
            get {
                return ChartType.PIE;
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
	}
}
