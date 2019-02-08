Imports System.Collections.ObjectModel
Imports Param_RootNamespace.Core.Models

Namespace Services
    ' This module holds sample data used by some generated pages to show how they can be used.
    ' TODO WTS: Delete this file once your app is using real data.
    Public Module SampleDataService
        Public Function AllOrders() As IEnumerable(Of SampleOrder)
            ' The following is order summary data
            Dim data = New ObservableCollection(Of SampleOrder)() From {
                New SampleOrder() With {
                    .OrderId = 70,
                    .OrderDate = New DateTime(2017, 5, 24),
                    .Company = "Company F",
                    .ShipTo = "Francisco Pérez-Olaeta",
                    .OrderTotal = 2490.0,
                    .Status = "Closed",
                    .Symbol = ChrW(57643)  ' Symbol.Globe
                },
                New SampleOrder() With {
                    .OrderId = 71,
                    .OrderDate = New DateTime(2017, 5, 24),
                    .Company = "Company CC",
                    .ShipTo = "Soo Jung Lee",
                    .OrderTotal = 1760.0,
                    .Status = "Closed",
                    .Symbol = ChrW(57737)  ' Symbol.Audio
                },
                New SampleOrder() With {
                    .OrderId = 72,
                    .OrderDate = New DateTime(2017, 6, 3),
                    .Company = "Company Z",
                    .ShipTo = "Run Liu",
                    .OrderTotal = 2310.0,
                    .Status = "Closed",
                    .Symbol = ChrW(57699)  ' Symbol.Calendar
                },
                New SampleOrder() With {
                    .OrderId = 73,
                    .OrderDate = New DateTime(2017, 6, 5),
                    .Company = "Company Y",
                    .ShipTo = "John Rodman",
                    .OrderTotal = 665.0,
                    .Status = "Closed",
                    .Symbol = ChrW(57620)  ' Symbol.Camera
                },
                New SampleOrder() With {
                    .OrderId = 74,
                    .OrderDate = New DateTime(2017, 6, 7),
                    .Company = "Company H",
                    .ShipTo = "Elizabeth Andersen",
                    .OrderTotal = 560.0,
                    .Status = "Shipped",
                    .Symbol = ChrW(57633)  ' Symbol.Clock
                },
                New SampleOrder() With {
                    .OrderId = 75,
                    .OrderDate = New DateTime(2017, 6, 7),
                    .Company = "Company F",
                    .ShipTo = "Francisco Pérez-Olaeta",
                    .OrderTotal = 810.0,
                    .Status = "Shipped",
                    .Symbol = ChrW(57661)  ' Symbol.Contact
                },
                New SampleOrder() With {
                    .OrderId = 76,
                    .OrderDate = New DateTime(2017, 6, 11),
                    .Company = "Company I",
                    .ShipTo = "Sven Mortensen",
                    .OrderTotal = 196.5,
                    .Status = "Shipped",
                    .Symbol = ChrW(57619)  ' Symbol.Favorite
                },
                New SampleOrder() With {
                    .OrderId = 77,
                    .OrderDate = New DateTime(2017, 6, 14),
                    .Company = "Company BB",
                    .ShipTo = "Amritansh Raghav",
                    .OrderTotal = 270.0,
                    .Status = "New",
                    .Symbol = ChrW(57615)  ' Symbol.Home
                },
                New SampleOrder() With {
                    .OrderId = 78,
                    .OrderDate = New DateTime(2017, 6, 14),
                    .Company = "Company A",
                    .ShipTo = "Anna Bedecs",
                    .OrderTotal = 736.0,
                    .Status = "New",
                    .Symbol = ChrW(57625)  ' Symbol.Mail
                },
                New SampleOrder() With {
                    .OrderId = 79,
                    .OrderDate = New DateTime(2017, 6, 18),
                    .Company = "Company K",
                    .ShipTo = "Peter Krschne",
                    .OrderTotal = 800.0,
                    .Status = "New",
                    .Symbol = ChrW(57806)  ' Symbol.OutlineStar
                }
            }

            Return data
        End Function
    End Module
End Namespace
