using System.Collections.Generic;

namespace Microsoft.Templates.Wizard.PostActions
{
	public class ExecutionContext
	{
		public string ProjectName { get; set; }

		public string SolutionPath { get; set; }

		public string ProjectPath { get; set; }

		public string PagePath { get; set; }

		public IDictionary<string, string> GenParams { get; set; }
	}
}
