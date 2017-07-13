using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Localization
{
    internal delegate void OnCommand(ToolCommandInfo commandInfo);

    internal class ToolCommandHandler
    {
        private const string splitPattern = @"(""[a-zA-Z0-9\s_@:.;\-^!#$%&+={}\(\)\[\]\\\/]*?"")|([a-zA-Z0-9]+)";
        private Dictionary<string, List<OnCommand>> handlers;

        internal void Listen()
        {
            ToolCommandInfo commandInfo;
            string commandLine;
            string[] commandParts;
            while (1 == 1)
            {
                Console.Write(">> ");
                commandLine = Console.ReadLine().Trim();
                MatchCollection matches = Regex.Matches(commandLine, splitPattern);
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
                        Array.ConstrainedCopy(commandParts, 1, arguments, 0, arguments.Length);
                    commandInfo = new ToolCommandInfo(commandParts[0].Trim().ToLower(), arguments);
                    if (commandInfo.Command == "exit")
                        break;
                    if (this.handlers != null && this.handlers.ContainsKey(commandInfo.Command) && this.handlers[commandInfo.Command].Count > 0)
                    {
                        foreach (OnCommand handler in this.handlers[commandInfo.Command])
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
            if (this.handlers == null)
                this.handlers = new Dictionary<string, List<OnCommand>>();
            if (!this.handlers.ContainsKey(command))
                this.handlers.Add(command, new List<OnCommand>());
            this.handlers[command].Add(handler);
        }
    }
}
