// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;

namespace Localization.Extensions
{
    public static class ResourcesExtensions
    {
        public static Dictionary<string, string> GetResourcesByFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new Dictionary<string, string>();
            }

            using (var reader = new ResXResourceReader(filePath))
            {
                return reader
                        .Cast<DictionaryEntry>()
                        .ToDictionary(k => k.Key.ToString(), v => v.Value.ToString());
            }
        }

        public static Dictionary<string, string> GetResourceValues(string filePath, IEnumerable<string> keys)
        {
            using (ResXResourceSet resx = new ResXResourceSet(filePath))
            {
                return resx
                        .Cast<DictionaryEntry>()
                        .Where(item => keys.Contains(item.Key))
                        .ToDictionary(k => k.Key.ToString(), v => v.Value.ToString());
            }
        }

        public static FileInfo CreateResxFile(string path, Dictionary<string, string> dictionary)
        {
            if (dictionary is null)
            {
                return null;
            }

            using (var writer = new ResXResourceWriter(path))
            {
                foreach (var entry in dictionary)
                {
                    writer.AddResource(entry.Key, entry.Value);
                }

                writer.Generate();
            }

            return new FileInfo(path);
        }

        public static void MergeResxFiles(FileInfo sourceFile, FileInfo destFile)
        {
            var sourceEntries = GetResourcesByFile(sourceFile.FullName);

            using (var reader = new ResXResourceReader(destFile.FullName))
            using (var writer = new ResXResourceWriter(destFile.FullName))
            {
                reader.UseResXDataNodes = true;

                foreach (DictionaryEntry item in reader)
                {
                    var node = (ResXDataNode)item.Value;
                    var key = item.Key.ToString();

                    if (sourceEntries.Keys.Contains(key))
                    {
                        var newNode = new ResXDataNode(key, sourceEntries[key]);
                        newNode.Comment = node.Comment;
                        writer.AddResource(newNode);
                    }
                    else
                    {
                        writer.AddResource(node);
                    }
                }

                writer.Generate();
            }
        }
    }
}
