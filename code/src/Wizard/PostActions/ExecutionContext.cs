using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Wizard.PostActions
{
	public class ExecutionContext
	{
		public string ProjectName { get; set; }

		public string ProjectPath { get; set; }

		public string PagePath { get; set; }

		public string UserName { get; set; }
	}
}
