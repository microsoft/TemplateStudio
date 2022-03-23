// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.Core.PostActions.Catalog.SortNamespaces
{
    public class UsingComparer : NamespaceComparer
    {
        public override string Keyword => UsingComparer.UsingKeyword;

        public static string UsingKeyword => @"using";

        protected override string Pattern => Keyword + @"\s(?<Ns>.*?);";

        protected override bool StripTrailingCharacter => true;
    }
}
