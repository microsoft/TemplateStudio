using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core;
using System.Collections.Generic;

namespace Microsoft.Templates.Wizard.PostActions
{
    public abstract class PostActionBase
	{
		public string Name { get; set; }

		public string Description { get; set; }

		private readonly IReadOnlyDictionary<string, string> _parameters;

		protected PostActionBase(string name, string description, IReadOnlyDictionary<string, string> parameters)
		{
			Name = name;
			Description = description;
			_parameters = parameters;
		}
		public abstract PostActionResult Execute(string outputPath, GenInfo context, TemplateCreationResult generationResult, GenShell shell);

		internal string GetValueFromParameter(string parameterName)
		{
			string parameterValue;
			if (this._parameters != null && !string.IsNullOrEmpty(parameterName) && _parameters.TryGetValue(parameterName, out parameterValue))
			{
				return parameterValue;
			}
			return null;
		}

		internal string GetValueFromGenParameter(IDictionary<string, string> genParams, string parameterName)
		{
			string parameterValue;
			if (genParams != null && !string.IsNullOrEmpty(parameterName) && genParams.TryGetValue(parameterName, out parameterValue))
			{
				return parameterValue;
			}
			return null;
		}

	}
}
