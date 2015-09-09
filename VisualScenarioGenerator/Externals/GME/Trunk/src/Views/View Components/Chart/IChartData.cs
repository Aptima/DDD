using System;
using System.Collections.Generic;
using System.Text;

namespace AME.Views.View_Components.Chart {

    public interface IChartData {
        ChartType ChartType {
            get;
        }

        String Title {
            get;
        }
    }

    public enum ChartType {
        LINEAR = 0,
		PIE = 1,
		BAR = 2,
		MULTIBAR = 3
    }
}
