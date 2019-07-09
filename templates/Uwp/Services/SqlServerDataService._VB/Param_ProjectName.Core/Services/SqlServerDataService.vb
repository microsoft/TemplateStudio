Imports System.Configuration
Imports System.Data.SqlClient
Imports Param_RootNamespace.Core.Models

Namespace Services

    ' This class holds sample data used by some generated pages to show how they can be used.
    ' More information on using and configuring this service can be found at https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/features/sql-server-data-service.md
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

        ' List orders from all companies
        Public Async Function AllOrders() As Task(Of IEnumerable(Of SampleOrder))
            Dim companies = Await AllCompanies()
            Dim orders = companies.SelectMany(Function(c) c.Orders)
            Return orders
        End Function

        ' This method returns data with the same structure as the SampleDataService but based on the NORTHWIND sample database.
        ' Use this as an alternative to the sample data to test using a different datasource without changing any other code.
        ' TODO WTS: Remove this when or if it isn't needed.
        Public Async Function AllCompanies() As Task(Of IEnumerable(Of SampleCompany))
            ' This hard-coded SQL statement is included to make this sample simpler.
            ' You can use Stored procedure, ORMs, or whatever is appropriate to access data in your app.
            Const getSampleDataQuery As String = "
            SELECT dbo.Customers.CustomerID,
                dbo.Customers.CompanyName,
                dbo.Customers.ContactName,
                dbo.Customers.ContactTitle,
                dbo.Customers.Address,
                dbo.Customers.City,
                dbo.Customers.PostalCode,
                dbo.Customers.Country,
                dbo.Customers.Phone,
                dbo.Customers.Fax,
                dbo.Orders.OrderID,
                dbo.Orders.OrderDate,
                dbo.Orders.RequiredDate,
                dbo.Orders.ShippedDate,
                dbo.Orders.Freight,
                dbo.Shippers.CompanyName,
                dbo.Shippers.Phone,
                CONCAT(dbo.Customers.Address, ' ', dbo.Customers.City, ' ', dbo.Customers.PostalCode, ' ', dbo.Customers.Country) as ShipTo,
                ISNULL(CHOOSE(CAST(RAND(CHECKSUM(NEWID())) * 3 as INT), 'Shipped', 'Closed'), 'New') as Status,
                CAST(RAND(CHECKSUM(NEWID())) * 200 as INT) + 57600 as SymbolCode,
                SUM(dbo.[Order Details].UnitPrice * dbo.[Order Details].Quantity * (1 + dbo.[Order Details].Discount)) OVER(PARTITION BY Orders.OrderID) AS OrderTotal,
                dbo.Products.ProductID,
                dbo.Products.ProductName,
                dbo.[Order Details].Quantity,
                dbo.[Order Details].Discount,
                dbo.[Order Details].UnitPrice,
                dbo.Products.QuantityPerUnit,
                dbo.Categories.CategoryName,
                dbo.Categories.Description,
                dbo.[Order Details].UnitPrice * dbo.[Order Details].Quantity * (1 + dbo.[Order Details].Discount) as ProductTotal
            FROM dbo.Customers
                inner join dbo.Orders on dbo.Customers.CustomerID = dbo.Orders.CustomerID
                inner join dbo.[Order Details] on dbo.[Order Details].OrderID = dbo.Orders.OrderID
                inner join dbo.Shippers on dbo.Orders.ShipVia = dbo.Shippers.ShipperID
                inner join dbo.Products on dbo.Products.ProductID = dbo.[Order Details].ProductID
                inner join dbo.Categories on dbo.Categories.CategoryID = dbo.Products.CategoryID"

            Dim sampleCompanies = New List(Of SampleCompany)()

            Try

                Using conn = New SqlConnection(GetConnectionString())
                    Await conn.OpenAsync()

                    If conn.State = System.Data.ConnectionState.Open Then

                        Using cmd = conn.CreateCommand()
                            cmd.CommandText = getSampleDataQuery

                            Using reader = Await cmd.ExecuteReaderAsync()

                                While Await reader.ReadAsync()
                                    ' Company Data
                                    Dim companyID = reader.GetString(0)
                                    Dim companyName = reader.GetString(1)
                                    Dim sampleCompany = sampleCompanies.FirstOrDefault(Function(c) c.CompanyID = companyID)
                                    If sampleCompany Is Nothing Then
                                        Dim contactName As String = If(Not reader.IsDBNull(2), reader.GetString(2), String.Empty)
                                        Dim contactTitle As String = If(Not reader.IsDBNull(3), reader.GetString(3), String.Empty)
                                        Dim address As String = If(Not reader.IsDBNull(4), reader.GetString(4), String.Empty)
                                        Dim city As String = If(Not reader.IsDBNull(5), reader.GetString(5), String.Empty)
                                        Dim postalCode As String = If(Not reader.IsDBNull(6), reader.GetString(6), String.Empty)
                                        Dim country As String = If(Not reader.IsDBNull(7), reader.GetString(7), String.Empty)
                                        Dim phone As String = If(Not reader.IsDBNull(8), reader.GetString(8), String.Empty)
                                        Dim fax As String = If(Not reader.IsDBNull(9), reader.GetString(9), String.Empty)

                                        sampleCompany = New SampleCompany() With {
                                            .CompanyID = companyID,
                                            .CompanyName = companyName,
                                            .ContactName = contactName,
                                            .ContactTitle = contactTitle,
                                            .Address = address,
                                            .City = city,
                                            .PostalCode = postalCode,
                                            .Country = country,
                                            .Phone = phone,
                                            .Fax = fax,
                                            .Orders = New List(Of SampleOrder)
                                        }
                                        sampleCompanies.Add(sampleCompany)
                                    End If

                                    ' Order Data
                                    Dim orderID = reader.GetInt32(10)
                                    Dim sampleOrder = sampleCompany.Orders.FirstOrDefault(Function(o) o.OrderID = orderID)
                                    If sampleOrder Is Nothing Then
                                        Dim orderDate As DateTime = If(Not reader.IsDBNull(11), reader.GetDateTime(11), Nothing)
                                        Dim requiredDate As DateTime = If(Not reader.IsDBNull(12), reader.GetDateTime(12), Nothing)
                                        Dim shippedDate As DateTime = If(Not reader.IsDBNull(13), reader.GetDateTime(13), Nothing)
                                        Dim freight As Double = If(Not reader.IsDBNull(14), Double.Parse(reader.GetDecimal(14).ToString()), 0.0)
                                        Dim shipperName As String = If(Not reader.IsDBNull(15), reader.GetString(15), String.Empty)
                                        Dim shipperPhone As String = If(Not reader.IsDBNull(16), reader.GetString(16), String.Empty)
                                        Dim shipTo As String = If(Not reader.IsDBNull(17), reader.GetString(17), String.Empty)
                                        Dim status As String = If(Not reader.IsDBNull(18), reader.GetString(18), String.Empty)
                                        Dim symbolCode As Integer = If(Not reader.IsDBNull(19), reader.GetInt32(19), 0)
                                        Dim orderTotal As Double = If(Not reader.IsDBNull(20), reader.GetDouble(20), 0.0)

                                        sampleOrder = New SampleOrder() With {
                                            .OrderID = orderID,
                                            .OrderDate = orderDate,
                                            .RequiredDate = requiredDate,
                                            .ShippedDate = shippedDate,
                                            .ShipperName = shipperName,
                                            .ShipperPhone = shipperPhone,
                                            .Freight = freight,
                                            .Company = companyName,
                                            .ShipTo = shipTo,
                                            .Status = status,
                                            .SymbolCode = symbolCode,
                                            .OrderTotal = orderTotal,
                                            .Details = New List(Of SampleOrderDetail)
                                        }
                                        sampleCompany.Orders.Add(sampleOrder)
                                    End If

                                    ' Product Data
                                    Dim productID = reader.GetInt32(21)
                                    Dim productName = reader.GetString(22)
                                    Dim quantity = reader.GetInt16(23)
                                    Dim discount = reader.GetFloat(24)
                                    Dim unitPrice = Double.Parse(reader.GetDecimal(25).ToString())
                                    Dim quantityPerUnit As String = If(Not reader.IsDBNull(26), reader.GetString(26), String.Empty)
                                    Dim categoryName As String = If(Not reader.IsDBNull(27), reader.GetString(27), String.Empty)
                                    Dim categoryDescription As String = If(Not reader.IsDBNull(28), reader.GetString(28), String.Empty)
                                    Dim productTotal As Double = If(Not reader.IsDBNull(29), reader.GetFloat(29), 0.0)
                                    sampleOrder.Details.Add(New SampleOrderDetail() With {
                                        .ProductID = productID,
                                        .ProductName = productName,
                                        .Quantity = quantity,
                                        .Discount = discount,
                                        .QuantityPerUnit = quantityPerUnit,
                                        .UnitPrice = unitPrice,
                                        .CategoryName = categoryName,
                                        .CategoryDescription = categoryDescription,
                                        .Total = productTotal
                                    })
                                End While
                            End Using
                        End Using
                    End If
                End Using

            Catch eSql As Exception
                ' Your code may benefit from more robust error handling or logging.
                ' This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}")
            End Try

            Return sampleCompanies
        End Function
    End Module
End Namespace