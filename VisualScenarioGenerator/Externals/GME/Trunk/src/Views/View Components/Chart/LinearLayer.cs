using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace AME.Views.View_Components.Chart {
    
    class LinearLayer : ILayer {

        private static readonly ILog logger = LogManager.GetLogger(typeof(LinearLayer));

        private String layerLegend;
        private double[] xValues = { };
        private double[] yValues = { };

        public LinearLayer(double[] xValues, double[] yValues, String layerLegend) {
                if (xValues == null || yValues == null) {
                    String message = "XValues or YValues of the LineLayer class cannot be null";
                    logger.Warn(message);
                    throw new Exception(message);
                }
                else if (xValues.Length != yValues.Length) {
                    String message = "Xvalue.Length and YValaue.Length of the LinearLayer class cannot be different";
                    logger.Warn(message);
                    throw new Exception(message);
                }
                else {
                    this.xValues = xValues;
                    this.yValues = yValues;
                    this.layerLegend = layerLegend;
                    logger.Debug("LinearLayer " + this.layerLegend + " is created");
                }
            }

        public double[] XValues {
            get {
                return this.xValues;
            }
        }

        public double[] YValues {
            get {
                return this.yValues;
            }
        }

        public String LayerLegend {
            get {
                return this.layerLegend;
            }
        }
    }
}
