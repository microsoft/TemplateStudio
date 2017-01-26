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
		public abstract PostActionResult Execute(ExecutionContext context);

		internal string GetValueFromParameter(string parameterName)
		{
			string parameterValue;
			if (this._parameters != null && !string.IsNullOrEmpty(parameterName) && _parameters.TryGetValue(parameterName, out parameterValue))
			{
				return parameterValue;
			}
			return null;
		}

	}
}
