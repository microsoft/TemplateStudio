using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core
{
    public abstract class Validator
    {
        public abstract ValidationResult Validate(string suggestedName);
    }

    public abstract class Validator<TConfig> : Validator
    {
        protected readonly TConfig _config;

        public Validator(TConfig config)
        {
            _config = config;
        }
    }
}
