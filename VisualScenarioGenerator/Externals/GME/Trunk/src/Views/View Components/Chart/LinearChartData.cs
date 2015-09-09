using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace AME.Views.View_Components.Chart {

    class LinearChartData : IChartData {

        private static readonly ILog logger = LogManager.GetLogger(typeof(LinearChartData));

        private String title = String.Empty;
        private String xAxisLabel = String.Empty;
        private String yAxisLabel = String.Empty;
        private String layerName = String.Empty;
        private List<LinearLayer> layers = new List<LinearLayer>();

        public LinearChartData(String title, String xAxisLabel, String yAxisLabel, String layerName) {
            this.title = title;
            this.xAxisLabel = xAxisLabel;
            this.yAxisLabel = yAxisLabel;
            this.layerName = layerName;
        }

        public LinearChartData(String title, String xAxisLabel, String yAxisLabel, String layerName, LinearLayer[] layer)
            : this(title, xAxisLabel, yAxisLabel, layerName) {

            this.layers.AddRange(layer);
        }

        public virtual ChartType ChartType {
            get {
                return ChartType.LINEAR;
            }
        }

        public virtual String Title {
            get {
                return this.title;
            }
        }

        public virtual String XAxisLabel {
            get {
                return this.xAxisLabel;
            }
        }

        public virtual String YAxisLabel {
            get {
                return this.yAxisLabel;
            }
        }

        public virtual String LayerName {
            get {
                return this.layerName;
            }
        }

        public virtual LinearLayer[] Layers {
            get {
                return layers.ToArray();
            }
        }

        public virtual void addLayer(LinearLayer layer) {
            this.layers.Add(layer);
        }

        public virtual bool removeLayer(LinearLayer layer) {
            return this.layers.Remove(layer);
        }
    }
}
