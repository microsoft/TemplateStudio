Imports System.Collections.ObjectModel
Imports System.Configuration
Imports System.Data.SqlClient
Imports Param_RootNamespace.Core.Models

Namespace Services

    ' This class holds sample data used by some generated pages to show how they can be used.
    ' TODO WTS: Change your code to use this instead of the SampleDataService.
    Public Module SqlServerDataService

        ' TODO WTS: Specify the connection string in a config file or below.
        Private Function GetConnectionString() As String
            ' Attempt to get the connection string from a config file
            ' Learn more about specifying the connection string in a config file at https://docs.microsoft.com/en-us/dotnet/api/system.configuration.configurationmanager?view=netframework-4.7.2
            Dim conStr = ConfigurationManager.ConnectionStrings("MyAppConnectionString")?.ConnectionString

            If Not String.IsNullOrWhiteSpace(conStr) Then
                Return conStr
            Else
                ' If no connection string is specified in a config file, use this as a fallback.
                Return "Data Source=*server*\*instance*;Initial Catalog=*dbname*;Integrated Security=SSPI"
            End If
        End Function

        ' This method returns data with the same structure as the SampleDataService but based on the NORTHWIND database.
        ' Use this as an alternative to the sample data to test using a different datasource without changing the page code.
        ' Alternatively, use this as a base for your ow data retrieval methods.
        ' TODO WTS: Remove this when or if it isn't needed.
        Public Async Function AllOrders() As Task(Of ObservableCollection(Of SampleOrder))
            Const getSampleOrdersQuery As String = "
SELECT Orders.OrderID,
       Orders.OrderDate,
       Customers.CompanyName,
       Orders.ShipName,
       SUM([Order Details].UnitPrice * [Order Details].Quantity) as OrderTotal,
       ISNULL(CHOOSE(CAST(RAND(CHECKSUM(NEWID())) * 3 as INT), 'Shipped', 'Closed'), 'New') as Status,
       CAST(RAND(CHECKSUM(NEWID())) * 200 as INT) + 57600 as Symbol
FROM dbo.Orders
     inner join dbo.[Order Details] on Orders.OrderID = [Order Details].OrderID
     inner join dbo.Customers ON Orders.CustomerID = Customers.CustomerID
Group by Orders.OrderID, Orders.OrderDate, Customers.CompanyName, Orders.ShipName, Orders.CustomerID
Order BY Orders.OrderID"
            Dim sampleOrders = New ObservableCollection(Of SampleOrder)()

            Try

                Using conn = New SqlConnection(GetConnectionString())
                    Await conn.OpenAsync()

                    If conn.State = System.Data.ConnectionState.Open Then

                        Using cmd = conn.CreateCommand()
                            cmd.CommandText = getSampleOrdersQuery

                            Using reader = Await cmd.ExecuteReaderAsync()

                                While Await reader.ReadAsync()
                                    Dim order = New SampleOrder With {
                                        .OrderId = reader.GetInt32(0),
                                        .OrderDate = reader.GetDateTime(1),
                                        .Company = reader.GetString(2),
                                        .ShipTo = reader.GetString(3),
                                        .OrderTotal = Double.Parse(reader.GetDecimal(4).ToString()),
                                        .Status = reader.GetString(5),
                                        .Symbol = ChrW(reader.GetInt32(6))
                                    }
                                    sampleOrders.Add(order)
                                End While
                            End Using
                        End Using
                    End If
                End Using

            Catch eSql As Exception
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}")
            End Try

            Return sampleOrders
        End Function
    End Module
End Namespace

