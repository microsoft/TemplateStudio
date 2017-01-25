using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Wizard.PostActions
{
	public class CustomPostActionDefinition
	{
		public string Name { get; set; }
		public PostActionType Type { get; set; }
		public string Description { get; set; }
		public IReadOnlyDictionary<string, string> Parameters { get; set; }
	}
}
