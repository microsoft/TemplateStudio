using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public static class StringExtensions
    {
        public static int GetLeadingTrivia(this string statement)
        {
            return statement.TakeWhile(Char.IsWhiteSpace).Count();
        }

        public static string WithLeadingTrivia(this string statement, int triviaCount)
        {
            if (triviaCount < 1)
            {
                return statement;
            }
            else
            {
                return string.Concat(new string(' ', triviaCount), statement);
            }
        }
    }
}
