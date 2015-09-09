using System;
using System.Collections.Generic;
using System.Text;

namespace Aptima.Asim.DDD.Client
{
    public interface IMapUpdate
    {
        void PositionChange(DDDObjects obj);
    }
}
