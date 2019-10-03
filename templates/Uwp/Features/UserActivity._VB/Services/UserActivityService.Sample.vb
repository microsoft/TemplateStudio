Imports AdaptiveCards
Imports Param_RootNamespace.Activation
Imports Param_RootNamespace.Views
Imports Windows.UI
Imports Windows.UI.Shell

Namespace Services
    Partial Public Module UserActivityService
        Async Function AddSampleUserActivity() As Task
            Dim activityId = NameOf(SchemeActivationSamplePage)
            Dim displayText = "Sample Activity"
            Dim description = $"Sample UserActivity added from Application '{Package.Current.DisplayName}' at {DateTime.Now.ToShortTimeString()}"
            Dim imageUrl = "http://adaptivecards.io/content/cats/2.png"
            Dim activityData = New UserActivityData(activityId, CreateActivationDataSample(), displayText, Colors.DarkRed)
            Dim adaptiveCard = CreateAdaptiveCardSample(displayText, description, imageUrl)
            Await CreateUserActivityAsync(activityData, adaptiveCard)
        End Function

        Private Function CreateActivationDataSample() As SchemeActivationData
            Dim parameters = New Dictionary(Of String, String)() From {
                {"paramName1", "paramValue1"},
                {"ticks", DateTime.Now.Ticks.ToString()}
            }
            Return New SchemeActivationData(GetType(SchemeActivationSamplePage), parameters)
        End Function

        ' TODO WTS: Change this to configure your own adaptive card
        ' For more info about adaptive cards see http://adaptivecards.io/
        Private Function CreateAdaptiveCardSample(displayText As String, description As String, imageUrl As String) As IAdaptiveCard
            Dim adaptiveCard = New AdaptiveCard("1.0")
            Dim columns = New AdaptiveColumnSet()
            Dim firstColumn = New AdaptiveColumn() With {
                .Width = "auto"
            }
            Dim secondColumn = New AdaptiveColumn() With {
                .Width = "*"
            }
            firstColumn.Items.Add(New AdaptiveImage() With {
                .Url = New Uri(imageUrl),
                .Size = AdaptiveImageSize.Medium
            })
            secondColumn.Items.Add(New AdaptiveTextBlock() With {
                .Text = displayText,
                .Weight = AdaptiveTextWeight.Bolder,
                .Size = AdaptiveTextSize.Large
            })
            secondColumn.Items.Add(New AdaptiveTextBlock() With {
                .Text = description,
                .Size = AdaptiveTextSize.Medium,
                .Weight = AdaptiveTextWeight.Lighter,
                .Wrap = True
            })
            columns.Columns.Add(firstColumn)
            columns.Columns.Add(secondColumn)
            adaptiveCard.Body.Add(columns)
            Return AdaptiveCardBuilder.CreateAdaptiveCardFromJson(adaptiveCard.ToJson())
        End Function
    End Module
End Namespace
