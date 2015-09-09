using System;
using System.Collections.Generic;
using System.Text;

namespace AME.Views.View_Components.Chart {
	
	public class BarLayer : ILayer {

		private double[] data = null;
		private String label = null;

		public BarLayer(double[] data, String label) {
			this.data = data;
			this.label = label;
		}

		public double[] Data {
			get { 
				return data; 
			}
		}

		public String Label {
			get { 
				return label; 
			}
		}
	}
}
