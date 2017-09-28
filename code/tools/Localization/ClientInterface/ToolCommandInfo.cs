// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Localization
{
    internal class ToolCommandInfo
    {
        internal string Command { get; private set; }
        internal string[] Arguments { get; private set; }

        internal ToolCommandInfo(string command, string[] arguments)
        {
            Command = command;
            Arguments = arguments;
        }
    }
}
