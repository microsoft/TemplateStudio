﻿'{[{
Imports Microsoft.AppCenter
Imports Microsoft.AppCenter.Analytics
Imports Microsoft.AppCenter.Crashes
'}]}

NotInheritable Partial Class App
    Inherits Application

    Public Sub New()
        InitializeComponent()
        '{[{

        ' TODO: Add your app in the app center and set your secret here. More at https://docs.microsoft.com/appcenter/sdk/getting-started/uwp
        AppCenter.Start("<Your App Secret>", GetType(Analytics), GetType(Crashes))
        '}]}
    End Sub
End Class
