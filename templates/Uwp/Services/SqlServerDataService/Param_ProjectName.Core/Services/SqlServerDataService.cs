using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Param_RootNamespace.Core.Models;

namespace Param_RootNamespace.Core.Services
{
    // This class holds sample data used by some generated pages to show how they can be used.
    // More information on using and configuring this service can be found at https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/features/sql-server-data-service.md
    // TODO WTS: Change your code to use this instead of the SampleDataService.
    public static class SqlServerDataService
    {
        // TODO WTS: Specify the connection string in a config file or below.
        private static string GetConnectionString()
        {
            // Attempt to get the connection string from a config file
            // Learn more about specifying the connection string in a config file at https://docs.microsoft.com/dotnet/api/system.configuration.configurationmanager?view=netframework-4.7.2
            var conStr = ConfigurationManager.ConnectionStrings["MyAppConnectionString"]?.ConnectionString;

            if (!string.IsNullOrWhiteSpace(conStr))
            {
                return conStr;
            }
            else
            {
                // If no connection string is specified in a config file, use this as a fallback.
                return @"Data Source=*server*\*instance*;Initial Catalog=*dbname*;Integrated Security=SSPI";
            }
        }

        public static async Task<IEnumerable<SampleOrder>> AllOrders()
        {
            // List orders from all companies
            var companies = await AllCompanies();
            return companies.SelectMany(c => c.Orders);
        }

        // This method returns data with the same structure as the SampleDataService but based on the NORTHWIND sample database.
        // Use this as an alternative to the sample data to test using a different datasource without changing any other code.
        // TODO WTS: Remove this when or if it isn't needed.
        public static async Task<IEnumerable<SampleCompany>> AllCompanies()
        {
            // This hard-coded SQL statement is included to make this sample simpler.
            // You can use Stored procedure, ORMs, or whatever is appropriate to access data in your app.
            const string getSampleDataQuery = @"
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
                inner join dbo.Categories on dbo.Categories.CategoryID = dbo.Products.CategoryID";

            var sampleCompanies = new List<SampleCompany>();

            try
            {
                using (var conn = new SqlConnection(GetConnectionString()))
                {
                    await conn.OpenAsync();

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = getSampleDataQuery;

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    // Company Data
                                    var companyID = reader.GetString(0);
                                    var companyName = reader.GetString(1);
                                    var sampleCompany = sampleCompanies.FirstOrDefault(c => c.CompanyID == companyID);
                                    if (sampleCompany == null)
                                    {
                                        var contactName = !reader.IsDBNull(2) ? reader.GetString(2) : string.Empty;
                                        var contactTitle = !reader.IsDBNull(3) ? reader.GetString(3) : string.Empty;
                                        var address = !reader.IsDBNull(4) ? reader.GetString(4) : string.Empty;
                                        var city = !reader.IsDBNull(5) ? reader.GetString(5) : string.Empty;
                                        var postalCode = !reader.IsDBNull(6) ? reader.GetString(6) : string.Empty;
                                        var country = !reader.IsDBNull(7) ? reader.GetString(7) : string.Empty;
                                        var phone = !reader.IsDBNull(8) ? reader.GetString(8) : string.Empty;
                                        var fax = !reader.IsDBNull(9) ? reader.GetString(9) : string.Empty;

                                        sampleCompany = new SampleCompany()
                                        {
                                            CompanyID = companyID,
                                            CompanyName = companyName,
                                            ContactName = contactName,
                                            ContactTitle = contactTitle,
                                            Address = address,
                                            City = city,
                                            PostalCode = postalCode,
                                            Country = country,
                                            Phone = phone,
                                            Fax = fax,
                                            Orders = new List<SampleOrder>()
                                        };
                                        sampleCompanies.Add(sampleCompany);
                                    }

                                    // Order Data
                                    var orderID = reader.GetInt32(10);
                                    var sampleOrder = sampleCompany.Orders.FirstOrDefault(o => o.OrderID == orderID);
                                    if (sampleOrder == null)
                                    {
                                        var orderDate = !reader.IsDBNull(11) ? reader.GetDateTime(11) : default(DateTime);
                                        var requiredDate = !reader.IsDBNull(12) ? reader.GetDateTime(12) : default(DateTime);
                                        var shippedDate = !reader.IsDBNull(13) ? reader.GetDateTime(13) : default(DateTime);
                                        var freight = !reader.IsDBNull(14) ? double.Parse(reader.GetDecimal(14).ToString()) : 0;
                                        var shipperName = !reader.IsDBNull(15) ? reader.GetString(15) : string.Empty;
                                        var shipperPhone = !reader.IsDBNull(16) ? reader.GetString(16) : string.Empty;
                                        var shipTo = !reader.IsDBNull(17) ? reader.GetString(17) : string.Empty;
                                        var status = !reader.IsDBNull(18) ? reader.GetString(18) : string.Empty;
                                        var symbolCode = !reader.IsDBNull(19) ? reader.GetInt32(19) : 0;
                                        var orderTotal = !reader.IsDBNull(29) ? reader.GetDouble(20) : 0.0;

                                        sampleOrder = new SampleOrder()
                                        {
                                            OrderID = orderID,
                                            OrderDate = orderDate,
                                            RequiredDate = requiredDate,
                                            ShippedDate = shippedDate,
                                            ShipperName = shipperName,
                                            ShipperPhone = shipperPhone,
                                            Freight = freight,
                                            Company = companyName,
                                            ShipTo = shipTo,
                                            Status = status,
                                            SymbolCode = symbolCode,
                                            OrderTotal = orderTotal,
                                            Details = new List<SampleOrderDetail>()
                                        };
                                        sampleCompany.Orders.Add(sampleOrder);
                                    }

                                    // Product Data
                                    var productID = reader.GetInt32(21);
                                    var productName = reader.GetString(22);
                                    var quantity = reader.GetInt16(23);
                                    var discount = reader.GetFloat(24);
                                    var unitPrice = double.Parse(reader.GetDecimal(25).ToString());
                                    var quantityPerUnit = !reader.IsDBNull(26) ? reader.GetString(26) : string.Empty;
                                    var categoryName = !reader.IsDBNull(27) ? reader.GetString(27) : string.Empty;
                                    var categoryDescription = !reader.IsDBNull(28) ? reader.GetString(28) : string.Empty;
                                    var productTotal = !reader.IsDBNull(29) ? reader.GetFloat(29) : 0.0;

                                    sampleOrder.Details.Add(new SampleOrderDetail()
                                    {
                                        ProductID = productID,
                                        ProductName = productName,
                                        Quantity = quantity,
                                        Discount = discount,
                                        QuantityPerUnit = quantityPerUnit,
                                        UnitPrice = unitPrice,
                                        CategoryName = categoryName,
                                        CategoryDescription = categoryDescription,
                                        Total = productTotal
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }

            return sampleCompanies;
        }
    }
}
