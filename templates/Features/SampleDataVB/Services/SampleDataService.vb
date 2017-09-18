Imports Param_ItemNamespace.Models
Imports System.Collections.Generic
Imports System.Collections.ObjectModel

Namespace Services
    ' This module holds sample data used by some generated pages to show how they can be used.
    ' TODO WTS: Delete this file once your app is using real data.
    Public Module SampleDataService
        Public Function AllOrders() As IEnumerable(Of SampleOrder)
            ' The following is order summary data
            Dim data = New ObservableCollection(Of SampleOrder)() From {
                New SampleOrder() With {
                    Key .OrderId = 70,
                    Key .OrderDate = New DateTime(2017, 5, 24),
                    Key .Company = "Company F",
                    Key .ShipTo = "Francisco Pérez-Olaeta",
                    Key .OrderTotal = 2490.0,
                    Key .Status = "Closed"
                },
                New SampleOrder() With {
                    Key .OrderId = 71,
                    Key .OrderDate = New DateTime(2017, 5, 24),
                    Key .Company = "Company CC",
                    Key .ShipTo = "Soo Jung Lee",
                    Key .OrderTotal = 1760.0,
                    Key .Status = "Closed"
                },
                New SampleOrder() With {
                    Key .OrderId = 72,
                    Key .OrderDate = New DateTime(2017, 6, 3),
                    Key .Company = "Company Z",
                    Key .ShipTo = "Run Liu",
                    Key .OrderTotal = 2310.0,
                    Key .Status = "Closed"
                },
                New SampleOrder() With {
                    Key .OrderId = 73,
                    Key .OrderDate = New DateTime(2017, 6, 5),
                    Key .Company = "Company Y",
                    Key .ShipTo = "John Rodman",
                    Key .OrderTotal = 665.0,
                    Key .Status = "Closed"
                },
                New SampleOrder() With {
                    Key .OrderId = 74,
                    Key .OrderDate = New DateTime(2017, 6, 7),
                    Key .Company = "Company H",
                    Key .ShipTo = "Elizabeth Andersen",
                    Key .OrderTotal = 560.0,
                    Key .Status = "Shipped"
                },
                New SampleOrder() With {
                    Key .OrderId = 75,
                    Key .OrderDate = New DateTime(2017, 6, 7),
                    Key .Company = "Company F",
                    Key .ShipTo = "Francisco Pérez-Olaeta",
                    Key .OrderTotal = 810.0,
                    Key .Status = "Shipped"
                },
                New SampleOrder() With {
                    Key .OrderId = 76,
                    Key .OrderDate = New DateTime(2017, 6, 11),
                    Key .Company = "Company I",
                    Key .ShipTo = "Sven Mortensen",
                    Key .OrderTotal = 196.5,
                    Key .Status = "Shipped"
                },
                New SampleOrder() With {
                    Key .OrderId = 77,
                    Key .OrderDate = New DateTime(2017, 6, 14),
                    Key .Company = "Company BB",
                    Key .ShipTo = "Amritansh Raghav",
                    Key .OrderTotal = 270.0,
                    Key .Status = "New"
                },
                New SampleOrder() With {
                    Key .OrderId = 78,
                    Key .OrderDate = New DateTime(2017, 6, 14),
                    Key .Company = "Company A",
                    Key .ShipTo = "Anna Bedecs",
                    Key .OrderTotal = 736.0,
                    Key .Status = "New"
                },
                New SampleOrder() With {
                    Key .OrderId = 79,
                    Key .OrderDate = New DateTime(2017, 6, 18),
                    Key .Company = "Company K",
                    Key .ShipTo = "Peter Krschne",
                    Key .OrderTotal = 800.0,
                    Key .Status = "New"
                }
            }

            Return data
        End Function
    End Module
End Namespace
