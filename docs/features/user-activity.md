# User Activities

The custom User Activity feature builds upon the [DeepLinking](deep-linking.md).

Allows your app to include new entries to Windows Timeline, a rich task view that takes advantage of User Activities to show a chronological view of what you’ve been working on. View more information about UserActivities on [Microsoft Docs](https://docs.microsoft.com/windows/uwp/launch-resume/useractivities).

## Files in project
 - **UserActivityData.cs** Contains all the information to show in the Windows Timeline card.
 - **UserActivityService.cs** Static service that allows to create and save User Activities.
 - **UserActivityService.Sample.cs** adds the sample implementation to add a UserActivity to the Windows timeline from ActivationService StartupAsync.


## AdaptiveCards

The UserActivity content is fill using a AdaptiveCards, a new way for developers to exchange card content in a common and consistent way. AdaptiveCards SDK is added using a Nuget Reference. View more information about Adaptive Cards on [Microsoft Docs](https://docs.microsoft.com/adaptive-cards/get-started/windows) and [adaptivecards.io](http://adaptivecards.io/).