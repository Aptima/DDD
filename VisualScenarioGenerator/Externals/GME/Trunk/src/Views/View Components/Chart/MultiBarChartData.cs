using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace AME.Views.View_Components.Chart {
	
	public class MultiBarChartData : IChartData {

		private static readonly ILog logger = LogManager.GetLogger(typeof(MultiBarChartData));

        private String title = String.Empty;
		private BarLayer[] barLayers = null;
		private String[] xLabels = null;

		public MultiBarChartData(String title, BarLayer[] barLayers, String[] xLabels) {
            this.title = title;
			this.barLayers = barLayers;
            this.xLabels = xLabels;
        }

        public virtual ChartType ChartType {
            get {
                return ChartType.MULTIBAR;
            }
        }

        public virtual String Title {
            get {
                return this.title;
            }
        }

		public virtual BarLayer[] BarLayers {
            get {
                return this.barLayers;
            }
        }

        public virtual String[] XLabels {
            get {
                return this.xLabels;
            }
        }
	}
}
