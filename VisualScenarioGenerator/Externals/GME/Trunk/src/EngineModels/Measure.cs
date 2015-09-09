using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using log4net;

namespace AME.EngineModels {

	public abstract class Measure {

		private static readonly ILog logger = LogManager.GetLogger(typeof(Measure));
		private Dictionary<String, Object> parameters = new Dictionary<string, object>();

		protected Measure(IXPathNavigable measureInput) {
			XPathNavigator nav = measureInput.CreateNavigator().SelectSingleNode("Component/Parameters");
			this.populateParametersDictionary(nav);
		}

		private void populateParametersDictionary(XPathNavigator parameters) {
			XPathNodeIterator iterator = parameters.Select("Parameters/Parameter");
			while (iterator.MoveNext()) {
				// TODO: populate the dictionary

			}
		}

		protected Object getParameter(String name) {
			if (name == null || name.Length == 0)
				return null;
			else
				return this.parameters[name];
		}

		protected abstract IXPathNavigable run();
	}
}
