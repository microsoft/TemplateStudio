# Azure Notification Hubs

Azure Notification Hubs provide an easy-to-use, multi-platform, scaled-out push engine. With a single cross-platform API call, you can easily send targeted and personalized push notifications to any mobile platform from any cloud or on-premises backend.

[Learn how to get started with Azure Notification Hubs and UWP UWP.](https://docs.microsoft.com/azure/notification-hubs/notification-hubs-windows-store-dotnet-get-started-wns-push-notification)

[General information about notification hubs.](https://docs.microsoft.com/azure/notification-hubs/notification-hubs-push-notification-overview)

---

The `HubNotificationsService` is in change of configuring the application with the Azure notifications service to allow the application to receive push notifications from a remote service in Azure. The service contains the `InitializeAsync` method that sets up the Hub Notifications. You must specify the hub name and the access signature before start working with Hub Notifications. There is more documentation about how to create and connect an Azure notifications service [here](https://docs.microsoft.com/azure/app-service-mobile/app-service-mobile-windows-store-dotnet-get-started-push).

Toast Notifications sent from Azure notification service should be handled in the same way as locally generated ones. See the [toast notification docs](./toast-notifications.md) for an example of how to do this.

---

[Other useful information about notifications](../notifications.md#other-useful-links-about-notifications)
