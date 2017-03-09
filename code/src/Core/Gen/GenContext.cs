using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Gen
{
    public class GenContext : IDisposable
    {
        private const string DataSlotKey = "GenContext_Ids";
        private static Dictionary<string, GenContext> _genContextTable = new Dictionary<string, GenContext>();

        public static GenToolBox ToolBox { get; private set; }
        public static bool IsInitialized { get; private set; }

        public string ProjectName { get; }
        public string OutputPath { get; }

        public static void Bootstrap(TemplatesRepository repo, GenShell shell)
        {
            ToolBox = new GenToolBox(repo, shell);

            IsInitialized = true;
        }

        private GenContext(string projectName, string outputPath)
        {
            ProjectName = projectName;
            OutputPath = outputPath;
        }

        //public static GenContext CreateNew(string name, string outputPath, string solutionName = null)
        //{
        //    return CreateNew(name, Path.Combine(location, name, name));
        //}

        public static GenContext CreateNew(Dictionary<string, string> replacements)
        {
            var destinationDirectory = new DirectoryInfo(replacements["$destinationdirectory$"]);

            return CreateNew(replacements["$safeprojectname$"], destinationDirectory.FullName);
        }

        public static GenContext CreateNew(string name, string outputPath, string solutionName = null)
        {
            var id = Guid.NewGuid().ToString();

            if (!IsInitialized)
            {
                throw new Exception("Class GenContext is not initialized. Call 'Bootstrap' method first.");
            }

            lock (_genContextTable)
            {
                SetCurrentGenId(id);

                var context = new GenContext(name, outputPath);

                if (!_genContextTable.ContainsKey(id))
                {
                    _genContextTable.Add(id, context);
                }
                else
                {
                    throw new Exception($"The generation '{id}' is currently being processed.");
                }
                return context;
            }
        }

        public static GenContext Current
        {
            get
            {
                lock (_genContextTable)
                {
                    var id = GetCurrentGenId();


                    if (string.IsNullOrEmpty(id) || !_genContextTable.ContainsKey(id))
                    {
                        throw new InvalidOperationException("There is no context for the current gen execution");
                    }
                    return _genContextTable[id];
                }
            }
        }

        public void Dispose()
        {
            lock (_genContextTable)
            {
                var ticket = GetCurrentGenId();
                if (!string.IsNullOrEmpty(ticket))
                {
                    if (_genContextTable.ContainsKey(ticket))
                    {
                        _genContextTable.Remove(ticket);
                    }
                }
                CallContext.FreeNamedDataSlot(DataSlotKey);
            }
        }

        private static void SetCurrentGenId(string id)
        {
            CallContext.LogicalSetData(DataSlotKey, id);
        }

        private static string GetCurrentGenId()
        {
            var id = CallContext.LogicalGetData(DataSlotKey);
            if (id != null)
            {
                return id.ToString();
            }
            return null;
        }
    }
}
