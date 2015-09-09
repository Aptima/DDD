using System;
using System.Collections.Generic;
using System.Text;

namespace Aptima.Asim.DDD.Client.Common.GLCore.PathController
{
    public abstract class BasePathObject
    {
        private long start_timer;

        protected long ElaspedTicks
        {
            get
            {
                return GameFramework.QueryPerformanceTimer() - start_timer;
            }
        }
        protected float ElaspedSeconds
        {
            get
            {
                return ((float)ElaspedTicks / (float)GameFramework.GetTicksPerSecond());
            }
        }

        private float _rate_per_second;
        protected float Rate
        {
            get
            {
                return _rate_per_second;
            }
            set
            {
                _rate_per_second = value;
            }
        }


        protected void ResetTimer()
        {
            start_timer = GameFramework.QueryPerformanceTimer();
        }

        // Distance = Rate * Time;
        protected float CalculateDistance()
        {
            return _rate_per_second * ElaspedSeconds;
        }
        protected float CalculateTTD(float distance)
        {
            return distance / _rate_per_second;
        }

    }
}
