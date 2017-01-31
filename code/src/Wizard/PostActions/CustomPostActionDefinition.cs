using System.Collections.Generic;

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
