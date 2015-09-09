using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SeamateAdapter.DDD;

namespace SeamateRuntimeEngine.ItemGenerators
{
    public class CrossingGenerator : Generator
    {

        public CrossingGenerator(DDDAdapter ddd)
            : base(ddd)
        {
            
        }
        public override void Generate(T_Item currentItem, String dmID)
        {
            //currentItem.Parameters.Crossing //True, False
        }
    }
}
