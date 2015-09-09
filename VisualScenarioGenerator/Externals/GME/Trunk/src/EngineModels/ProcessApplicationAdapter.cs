using System;
using System.Collections.Generic;
using System.Text;
using AME.Controllers;
using AME.EngineModels;

namespace AME.EngineModels {

	public abstract class ProcessApplicationAdapter {

		public abstract bool applyProcess(int processId);
	}
}
