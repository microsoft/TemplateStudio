//{[{
using System.Collections.Generic;
using System.Linq;
//}]}

namespace Param_RootNamespace
{
    internal static class PageTokens
    {
//^^
//{[{

        internal static IEnumerable<string> GetAll() => typeof(PageTokens)
                                                            .GetFields()
                                                            .Select(p => p.GetValue(p) as string);
//}]}
    }
}
