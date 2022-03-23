// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Templates;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Templates.Core.PostActions.Catalog.AddJsonDictionaryItem
{
    public class AddJsonDictionaryItemPostAction : TemplateDefinedPostAction
    {
        public const string Id = "CB387AC0-16D0-4E07-B41A-F1EA616A7CA9";

        private readonly Dictionary<string, string> _parameters;

        private readonly string _destinationPath;

        public override Guid ActionId { get => new Guid(Id); }

        public AddJsonDictionaryItemPostAction(string relatedTemplate, IPostAction templatePostAction, Dictionary<string, string> parameters, string destinationPath)
            : base(relatedTemplate, templatePostAction)
        {
            _parameters = parameters;
            _destinationPath = destinationPath;
        }

        internal override void ExecuteInternal()
        {
            var parameterReplacements = new FileRenameParameterReplacements(_parameters);

            var jsonPath = Path.Combine(_destinationPath, parameterReplacements.ReplaceInPath(Args["jsonPath"]));

            var keyToDict = Args["key"];

            JObject json = JObject.Parse(File.ReadAllText(jsonPath));

            var dictContent = json[keyToDict].ToObject<Dictionary<string, string>>();
            var contentToAdd = JsonConvert.DeserializeObject<Dictionary<string, string>>(Args["dict"]);

            var newContent = dictContent.Merge(contentToAdd);

            json[keyToDict] = JObject.FromObject(newContent);

            var originalEncoding = GetEncoding(jsonPath);

            File.WriteAllText(jsonPath, json.ToString(Formatting.Indented), originalEncoding);
        }

        private Encoding GetEncoding(string originalFilePath)
        {
            // Will read the file, and look at the BOM to check the encoding.
            using (var reader = new StreamReader(File.OpenRead(originalFilePath), true))
            {
                var bytes = File.ReadAllBytes(originalFilePath);
                var encoding = reader.CurrentEncoding;

                // The preamble is the first couple of bytes that may be appended to define an encoding.
                var preamble = encoding.GetPreamble();

                // We preserve the read encoding unless there is no BOM, if it is UTF-8 we return the non BOM encoding.
                if (preamble == null || preamble.Length == 0 || preamble.Where((p, i) => p != bytes[i]).Any())
                {
                    if (encoding.EncodingName == Encoding.UTF8.EncodingName)
                    {
                        return new UTF8Encoding(false);
                    }
                }

                return encoding;
            }
        }
    }
}
