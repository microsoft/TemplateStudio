// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Templates.Core;

namespace Microsoft.Templates.Test.BuildWithLegacy.Uwp
{
    public sealed class BuildRightClickWithLegacyVBFixture : BuildRightClickWithLegacyFixture, IDisposable
    {

        public override string Platform => Platforms.Uwp;
        public override string Language => ProgrammingLanguages.VisualBasic;


    }
}
