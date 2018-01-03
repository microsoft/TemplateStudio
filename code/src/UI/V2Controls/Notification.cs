// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.UI.V2Controls
{
    public enum NotificationType
    {
        Information,
        Warning
    }

    public class Notification
    {
        public NotificationType Type { get; private set; }

        public string Message { get; private set; }

        public static Notification Information(string message)
        {
            return new Notification()
            {
                Type = NotificationType.Information,
                Message = message
            };
        }

        public static Notification Warning(string message)
        {
            return new Notification()
            {
                Type = NotificationType.Warning,
                Message = message
            };
        }
    }
}
