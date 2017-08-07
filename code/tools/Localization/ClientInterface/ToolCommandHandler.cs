// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Localization
{
    internal delegate void OnCommand(ToolCommandInfo commandInfo);

    internal class ToolCommandHandler
    {
        private const string SplitPattern = @"(""[a-zA-Z0-9\s_@:.;\-^!#$%&+={}\(\)\[\]\\\/]*?"")|([a-zA-Z0-9]+)";
        private Dictionary<string, List<OnCommand>> _handlers;

        internal void Listen()
        {
            ToolCommandInfo commandInfo;
            string commandLine;
            string[] commandParts;

            while (true)
            {
                Console.Write(">> ");
                commandLine = Console.ReadLine().Trim();
                MatchCollection matches = Regex.Matches(commandLine, SplitPattern);
                commandParts = new string[matches.Count];
                int i = 0;

                foreach (Match match in matches)
                {
                    commandParts[i++] = match.Value.Trim("\"".ToCharArray());
                }

                if (commandParts.Length > 0)
                {
                    string[] arguments = commandParts.Length > 1 ? new string[commandParts.Length - 1] : new string[] { };

                    if (commandParts.Length > 1)
                    {
                        Array.ConstrainedCopy(commandParts, 1, arguments, 0, arguments.Length);
                    }

                    commandInfo = new ToolCommandInfo(commandParts[0].Trim().ToLower(), arguments);

                    if (commandInfo.Command == "exit")
                    {
                        break;
                    }

                    if (_handlers != null && _handlers.ContainsKey(commandInfo.Command) && _handlers[commandInfo.Command].Count > 0)
                    {
                        foreach (OnCommand handler in _handlers[commandInfo.Command])
                        {
                            try
                            {
                                handler.Invoke(commandInfo);
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Error executing command {commandInfo.Command.ToUpper()}:");
                                Console.Error.WriteLine(ex.ToString());
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Command unknown. Type HELP for help.");
                    }
                }
            }
        }

        internal void SubscribeOnCommand(string command, OnCommand handler)
        {
            command = command.ToLower();

            if (_handlers == null)
            {
                _handlers = new Dictionary<string, List<OnCommand>>();
            }

            if (!_handlers.ContainsKey(command))
            {
                _handlers.Add(command, new List<OnCommand>());
            }

            _handlers[command].Add(handler);
        }
    }
}
