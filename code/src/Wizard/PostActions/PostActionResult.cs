using System;

namespace Microsoft.Templates.Wizard.PostActions
{
	public class PostActionResult
	{
		public ResultCode ResultCode { get; set; }

		public string Message { get; set; }
		
		public Exception Exception { get; set; }
	}
}