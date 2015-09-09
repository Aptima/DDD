using System;
using System.Collections.Generic;
using System.Text;
using Aptima.Asim.DDD.CommonComponents.SimulationEventTools;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
namespace Aptima.Asim.DDD.DDDAgentFramework
{
    public class ScenarioData
    {
        private String m_mapFileName;
        private String m_utmZone;
        private Double m_utmEasting;
        private Double m_utmNorthing;
        private Double m_horizontalMetersPerPixel;
        private Double m_verticalMetersPerPixel;

        private String m_scenarioName;
        private String m_scenarioDescription;
        private String m_playerBrief;
        private String m_iconLibrary;

        public String MapFileName
        {
            set { m_mapFileName = value; }
            get { return m_mapFileName; }
        }
        public String UTMZone
        {
            set { m_utmZone = value; }
            get { return m_utmZone; }
        }
        public String ScenarioName
        {
            set { m_scenarioName = value; }
            get { return m_scenarioName; }
        }
        public String ScenarioDescription
        {
            set { m_scenarioDescription = value; }
            get { return m_scenarioDescription; }
        }
        public String PlayerBrief
        {
            set { m_playerBrief = value; }
            get { return m_playerBrief; }
        }
        public String IconLibrary
        {
            set { m_iconLibrary = value; }
            get { return m_iconLibrary; }
        }
        public Double UTMEasting
        {
            set { m_utmEasting = value; }
            get { return m_utmEasting; }
        }
        public Double UTMNorthing
        {
            set { m_utmNorthing = value; }
            get { return m_utmNorthing; }
        }
        public Double HorizontalMetersPerPixel
        {
            set { m_horizontalMetersPerPixel = value; }
            get { return m_horizontalMetersPerPixel; }
        }
        public Double VerticalMetersPerPixel
        {
            set { m_verticalMetersPerPixel = value; }
            get { return m_verticalMetersPerPixel; }
        }

        public ScenarioData(SimulationEvent e)
        {
            MapFileName = ((StringValue)e["MapName"]).value;
            UTMZone = "";
            ScenarioName = ((StringValue)e["ScenarioName"]).value;
            ScenarioDescription = ((StringValue)e["ScenarioDescription"]).value;
            PlayerBrief = ((StringValue)e["PlayerBrief"]).value;
            IconLibrary = ((StringValue)e["IconLibrary"]).value;
            UTMEasting = ((DoubleValue)e["UTMEasting"]).value;
            UTMNorthing = ((DoubleValue)e["UTMNorthing"]).value;
            HorizontalMetersPerPixel = ((DoubleValue)e["HorizontalPixelsPerMeter"]).value;
            VerticalMetersPerPixel = ((DoubleValue)e["VerticalPixelsPerMeter"]).value;
        }
    }
}
