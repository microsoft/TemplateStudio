// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Windows.AppNotifications;

class ToastExample
{
    public const int NotificationId = 1;
    public const string ScenarioName = "Local Toast Example";

    public static bool SendToast()
    {
        // The ScenarioIdToken uniquely identify a scenario and is used to route the response received when the user clicks on a toast to the correct scenario.

        var xmlPayload = new string(
            "<toast launch = \"action=ToastClick&amp;NotificationId=" + NotificationId.ToString() + "\">"
        +       "<visual>"
        +           "<binding template = \"ToastGeneric\">"
        +               "<text>Sample Toast Notification</text>"
        +               "<text></text>"
        +               "<image placement = \"appLogoOverride\" hint-crop=\"circle\" src = \"C:\\AppNotifsRef\\AppNotifsRef\\Assets\\Square150x150Logo.png\"/>"
        +           "</binding>"
        +       "</visual>"
        +       "<actions>"
        +           "<action "
        +               "content = \"Go to CGrid\" "        
        +               "arguments = \"action=CGridPage&amp;NotificationId=" + NotificationId.ToString() + "\"/>"
        +       "</actions>"
        +   "</toast>" );

        var toast = new AppNotification(xmlPayload);
        AppNotificationManager.Default.Show(toast);

        return toast.Id != 0; // return true (indicating success) if the toast was sent (if it has an Id)
    }
}
