// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Resources;

namespace Localization.Extensions
{
    public static class ResourcesExtensions
    {
        public static Dictionary<string, ResxItem> GetResourcesByFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new Dictionary<string, ResxItem>();
            }

            using (var reader = new ResXResourceReader(filePath))
            {
                reader.UseResXDataNodes = true;
                ITypeResolutionService typeres = null;

                return reader
                        .Cast<DictionaryEntry>()
                        .ToDictionary(k => k.Key.ToString(), v => new ResxItem
                        {
                            Name = v.Key.ToString(),
                            Text = ((ResXDataNode)v.Value).GetValue(typeres).ToString(),
                            Comment = ((ResXDataNode)v.Value).Comment,
                        });
            }
        }

        public static FileInfo CreateResxFile(string path, Dictionary<string, ResxItem> resxItems)
        {
            if (resxItems is null)
            {
                return null;
            }

            using (var writer = new ResXResourceWriter(path))
            {
                foreach (var entry in resxItems.Values)
                {
                    writer.AddResource(entry.ToNode());
                }

                writer.Generate();
            }

            return new FileInfo(path);
        }
    }
}
