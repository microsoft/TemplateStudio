Imports Param_ItemNamespace.Models
Imports System.Collections.Generic
Imports System.Collections.ObjectModel

Namespace Services
    ' This class holds sample data used by some generated pages to show how they can be used.
    ' TODO WTS: Delete this file once your app is using real data.
    Public NotInheritable Class SampleDataService
        Private Sub New()
        End Sub
        Private Shared Function AllOrders() As IEnumerable(Of SampleOrder)
            ' The following is order summary data
            Dim data = New ObservableCollection(Of SampleOrder)() From {
                New SampleOrder With {
                    .OrderId = 70,
                    .OrderDate = New DateTime(2017, 5, 24),
                    .Company = "Company F",
                    .ShipTo = "Francisco Pérez-Olaeta",
                    .OrderTotal = 2490.0,
                    .Status = "Closed"
                },
                New SampleOrder With {
                    .OrderId = 71,
                    .OrderDate = New DateTime(2017, 5, 24),
                    .Company = "Company CC",
                    .ShipTo = "Soo Jung Lee",
                    .OrderTotal = 1760.0,
                    .Status = "Closed"
                },
                New SampleOrder With {
                    .OrderId = 72,
                    .OrderDate = New DateTime(2017, 6, 3),
                    .Company = "Company Z",
                    .ShipTo = "Run Liu",
                    .OrderTotal = 2310.0,
                    .Status = "Closed"
                },
                New SampleOrder With {
                    .OrderId = 73,
                    .OrderDate = New DateTime(2017, 6, 5),
                    .Company = "Company Y",
                    .ShipTo = "John Rodman",
                    .OrderTotal = 665.0,
                    .Status = "Closed"
                },
                New SampleOrder With {
                    .OrderId = 74,
                    .OrderDate = New DateTime(2017, 6, 7),
                    .Company = "Company H",
                    .ShipTo = "Elizabeth Andersen",
                    .OrderTotal = 560.0,
                    .Status = "Shipped"
                },
                New SampleOrder With {
                    .OrderId = 75,
                    .OrderDate = New DateTime(2017, 6, 7),
                    .Company = "Company F",
                    .ShipTo = "Francisco Pérez-Olaeta",
                    .OrderTotal = 810.0,
                    .Status = "Shipped"
                },
                New SampleOrder With {
                    .OrderId = 76,
                    .OrderDate = New DateTime(2017, 6, 11),
                    .Company = "Company I",
                    .ShipTo = "Sven Mortensen",
                    .OrderTotal = 196.5,
                    .Status = "Shipped"
                },
                New SampleOrder With {
                    .OrderId = 77,
                    .OrderDate = New DateTime(2017, 6, 14),
                    .Company = "Company BB",
                    .ShipTo = "Amritansh Raghav",
                    .OrderTotal = 270.0,
                    .Status = "New"
                },
                New SampleOrder With {
                    .OrderId = 78,
                    .OrderDate = New DateTime(2017, 6, 14),
                    .Company = "Company A",
                    .ShipTo = "Anna Bedecs",
                    .OrderTotal = 736.0,
                    .Status = "New"
                },
                New SampleOrder With {
                    .OrderId = 79,
                    .OrderDate = New DateTime(2017, 6, 18),
                    .Company = "Company K",
                    .ShipTo = "Peter Krschne",
                    .OrderTotal = 800.0,
                    .Status = "New"
                }
            }

            Return data
        End Function
    End Class
End Namespace
