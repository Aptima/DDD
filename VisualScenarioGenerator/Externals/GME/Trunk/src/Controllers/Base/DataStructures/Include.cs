using System;
using System.Collections.Generic;
using System.Text;

namespace AME.Controllers.Base.DataStructures
{
    public class Include
    {
        private string componentType, linkType;
        private bool isDynamic, includeParameters;
        uint? levelDown = null;

        public Boolean IncludeParameters
        {
            get { return includeParameters; }
            set { includeParameters = value; }
        }

        public uint? LevelDown
        {
            get { return levelDown; }
            set { levelDown = value; }
        }

        public string ComponentType
        {
            get { return componentType; }
            set { componentType = value; }
        }

        public string LinkType
        {
            get { return linkType; }
            set { linkType = value; }
        }

        public bool IsDynamic
        {
            get { return isDynamic; }
            set { isDynamic = value; }
        }


        public Include(String componentType, String linkType, bool isDynamic, uint? levelDown, bool includeParameters)
        {
            this.componentType = componentType;
            this.linkType = linkType;
            this.isDynamic = isDynamic;
            this.levelDown = levelDown;
            this.includeParameters = includeParameters;
        }
    }
}
