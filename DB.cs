using System;
using System.Data;
using System.Data.SqlClient;

namespace Microsoft.AdoNet.DataSetDemo
{
    class NorthwindDataSet
    {
        static void Main()
        {
            string connectionString = GetConnectionString();
            ConnectToData(connectionString);
        }

        private static void ConnectToData(string connectionString)
        {
            //Create a SqlConnection to the Northwind database.
            using (SqlConnection connection =
                       new SqlConnection(connectionString))
            {
                //Create a SqlDataAdapter for the Suppliers table.
                SqlDataAdapter adapter = new SqlDataAdapter();

                // A table mapping names the DataTable.
                adapter.TableMappings.Add("Table", "Suppliers");

                // Open the connection.
                connection.Open();
                Console.WriteLine("The SqlConnection is open.");

                // Create a SqlCommand to retrieve Suppliers data.
                SqlCommand command = new SqlCommand(
                    "SELECT SupplierID, CompanyName FROM dbo.Suppliers;",
                    connection);
                command.CommandType = CommandType.Text;

                // Set the SqlDataAdapter's SelectCommand.
                adapter.SelectCommand = command;

                // Fill the DataSet.
                DataSet dataSet = new DataSet("Suppliers");
                adapter.Fill(dataSet);

                // Create a second Adapter and Command to get
                // the Products table, a child table of Suppliers.
                SqlDataAdapter productsAdapter = new SqlDataAdapter();
                productsAdapter.TableMappings.Add("Table", "Products");

                SqlCommand productsCommand = new SqlCommand(
                    "SELECT ProductID, SupplierID FROM dbo.Products;",
                    connection);
                productsAdapter.SelectCommand = productsCommand;

                // Fill the DataSet.
                productsAdapter.Fill(dataSet);

                //Creating a third Adaptar and cost table
                SqlDataAdapter costAdapter = new SqlDataAdapter();
                costAdapter.TablesMappings.Add("Table","costs");

                SqlCommand costCommand = new SqlCommand(
                    "SELECT ProductCost FROM dbo.costs;",
                    connection);
                costAdapter.SelectCommand = costCommand;

                //Fill the DataSet.
                costAdapter.Fill(dataSet);                
                
                //Creating a 4th adapter:

               SqlDataAdapter checkAvailability = SqlDataAdapter();
               AvailabilityAdapter.TablesMappings.Add("Tables","Availability");

               SqlCommand checkAvailabilityCommand = new SqlCommand(
                   "SELECT Check FROM dbo.Availability",
                    connection);
                checkAvailability.SelectCommand = checkAvailabilityCommand;
            
                //Fill the dataset
                checkAvailability.Fill(dataSet);

                // Close the connection.
                connection.Close();
                Console.WriteLine("The SqlConnection is closed.");

                // Create a DataRelation to link the two tables
                // based on the SupplierID.
                DataColumn parentColumn =
                    dataSet.Tables["Suppliers"].Columns["SupplierID"];
                DataColumn childColumn =
                    dataSet.Tables["Products"].Columns["SupplierID"];
                DataColumn childColumn =
                    dataSet.Tables["costs"].Columns["ProductCost"];
                DataColumn childColumn =
                    dataSet.Tables["Availavilty"].Columns["ProductAvailability"];
                DataRelation relation =
                    new System.Data.DataRelation("SuppliersProducts", "ProductCost", "ProductAvailability",
                    parentColumn, childColumn);
                dataSet.Relations.Add(relation);
                Console.WriteLine(
                    "The {0} DataRelation has been created.",
                    relation.RelationName);
            }
        }

        static private string GetConnectionString()
        {
            // To avoid storing the connection string in your code,
            // you can retrieve it from a configuration file.
            return "Data Source=(local);Initial Catalog=Northwind;"
                + "Integrated Security=SSPI";
        }
    }
}