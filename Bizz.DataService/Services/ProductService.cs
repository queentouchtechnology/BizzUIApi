// using System;
// using System.Collections.Generic;

using System.Data;
using System.Data.SqlClient;
using Bizz.Entities.Models;

namespace Bizz.DataService.Services;

public class ProductService(DbService db) : IDisposable
{


    public async Task<DataTable> GetDataTable()
	{
		DataTable dataTable = new();

		try
		{
			using var connection = db.GetConnection();
			connection.Open();

			using var command = new SqlCommand("SELECT * FROM customers", connection);
			command.CommandType = CommandType.Text;

			using SqlDataAdapter da = new(command);
			da.Fill(dataTable);



            Console.WriteLine($"Number of rows retrieved: {dataTable.Rows.Count}");



			foreach (DataRow row in dataTable.Rows)
			{
				foreach (DataColumn column in dataTable.Columns)
				{
					Console.Write($"{column.ColumnName}: {row[column]} | ");
				}
				Console.WriteLine();
			}



			Console.WriteLine(dataTable.ToString());
		}
		catch (Exception ex)
		{
			// Handle the exception (log, rethrow, etc.)
			Console.WriteLine($"Error retrieving data: {ex.Message}");
		}

		Console.WriteLine($"------->{dataTable}");

		return dataTable;

	}

	public async Task<List<T>> MapDataTableToModel<T>(DataTable dataTable) where T : new()
	{
		List<T> result = [];

		foreach (DataRow row in dataTable.Rows)
		{
			T item = new();

			foreach (DataColumn column in dataTable.Columns)
			{
				// Assuming that the column names in DataTable match the property names in the model.
				var property = typeof(T).GetProperty(column.ColumnName);

				if (property != null && row[column] != DBNull.Value)
				{
					property.SetValue(item, row[column]);
				}
			}

			result.Add(item);
		}

		return result;
	}
	





	public async Task<Response> Create(Product req)
    {
        var res = new Response();
        try
        {
            await using var connection = db.GetConnection();
            {
                connection.Open();

                const string query = "INSERT INTO Products (Name, Price) VALUES (@Name, @Price)";
                await using var cmd = new SqlCommand(query, connection);
                {
                    cmd.Parameters.AddWithValue("@Name", req.Name);
                    cmd.Parameters.AddWithValue("@Price", req.Price);

                    var rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                    {
                        res.Status = false;
                        res.RespText = "Insert Error: Something went wrong";
                    }
                }
                connection.Close();
            }
        }
        catch (Exception e)
        {
            res.Status = false;
            res.RespText = $"Error: {e.ToString()}";
        }

        return res;
    }

    public async Task<Response> UpdateById(int Id, Product req)
    {
        var res = new Response();
        try
        {
            await using var connection = db.GetConnection();
            {
                connection.Open();

                const string query = "UPDATE Products SET Name=@Name, Price=@Price Where Id=@Id";
                await using var cmd = new SqlCommand(query, connection);
                {
                    cmd.Parameters.AddWithValue("@Name", req.Name);
                    cmd.Parameters.AddWithValue("@Price", req.Price);
                    cmd.Parameters.AddWithValue("@Id", Id);

                    var rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                    {
                        res.Status = false;
                        res.RespText = "UpdateError: Invalid id";
                    }
                }
                connection.Close();
            }
        }
        catch (Exception e)
        {
            res.Status = false;
            res.RespText = $"Error: {e.ToString()}";
        }

        return res;
    }

    public async Task<GetResponse<Product>> GetById(int Id)
    {
        var res = new GetResponse<Product>();
        try
        {
            await using var connection = db.GetConnection();
            {
                connection.Open();

                const string query = "SELECT * FROM Products WHERE Id=@Id";
                await using var cmd = new SqlCommand(query, connection);
                {
                    cmd.Parameters.AddWithValue("@Id", Id);

                    await using var reader = await cmd.ExecuteReaderAsync();
                    {
                        if (reader.Read())
                        {
                            var product = new Product
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Price = reader.GetDecimal(2)
                            };
                            res.Data = product;
                        }
                        else
                        {
                            res.Status = false;
                            res.RespText = "Product NotFound";
                        }
                    }
                }
                connection.Close();
            }
        }
        catch (Exception e)
        {
            res.Status = false;
            res.RespText = $"Error: {e.ToString()}";
        }

        return res;
    }

    public async Task<ListResponse<Product>> GetList()
    {
        var res = new ListResponse<Product>();
        try
        {
            await using var connection = db.GetConnection();
            {
                connection.Open();
                await using var cmd = new SqlCommand("SELECT * FROM Products", connection);
                {
                    var sda = new SqlDataAdapter(cmd);
                    var dt = new DataTable();
                    sda.Fill(dt);
                    var products = new List<Product>();
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        var row = dt.Rows[i];
                        products.Add(
                            new Product
                            {
                                Id = Convert.ToInt32(row["Id"]),
                                Name = Convert.ToString((string)row["Name"]),
                                Price = Convert.ToDecimal(row["Price"])
                            });
                    }
                    res.Data = products;
                }
                connection.Close();
            }
        }
        catch (Exception e)
        {
            res.Status = false;
            res.RespText = $"Error: {e.ToString()}";
        }
        return res;
    }


	


	public async Task<Response> Delete(int Id)
    {
        var res = new Response();
        try
        {
            await using var connection = db.GetConnection();
            {
                connection.Open();

                const string query = "DELETE FROM Products WHERE Id = @Id";
                await using var cmd = new SqlCommand(query, connection);
                {
                    cmd.Parameters.AddWithValue("@Id", Id);

                    var rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                    {
                        res.Status = false;
                        res.RespText = "Insert Error: Something went wrong";
                    }
                }
            }
        }
        catch (Exception e)
        {
            res.Status = false;
            res.RespText = $"Error: {e.ToString()}";
        }

        return res;
    }

    public Response InsertDummyData()
    {
        var rspn = new Response();
        try
        {
            using var connection = db.GetConnection();
            connection.Open();

            // Example SQL command to insert dummy data
            const string insertQuery = "INSERT INTO Products (Name, Price) VALUES (@Name, @Price)";

            using var command = new SqlCommand(insertQuery, connection);
            // Add parameters to prevent SQL injection
            command.Parameters.AddWithValue("@Name", "DummyProduct1");
            command.Parameters.AddWithValue("@Price", 19.99);

            // You can add more data and execute the command as needed

            // Execute the query
            var rowsAffected = command.ExecuteNonQuery();

            // Optionally, check the number of rows affected or handle errors
            Console.WriteLine(rowsAffected > 0 ? "Dummy data inserted successfully." : "Failed to insert dummy data.");
            connection.Close();
        }
        catch (Exception e)
        {
            rspn.Status = false;
            rspn.RespText = $"Error: {e.ToString()}";
        }

        return rspn;
    }

    public ListResponse<Product> GetProducts()
    {
        var rspn = new ListResponse<Product>();
        try
        {
            using var connection = db.GetConnection();
            connection.Open();
            using var command = new SqlCommand("SELECT * FROM Products", connection);
            using var reader = command.ExecuteReader();

            var products = new List<Product>();
            while (reader.Read())
            {
                products.Add(new Product
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = Convert.ToString((string)reader["Name"]),
                    Price = Convert.ToDecimal(reader["Price"])
                });
            }

            connection.Close();
            rspn.Data = products;
        }
        catch (Exception e)
        {
            rspn.Status = false;
            rspn.RespText = $"Error: {e.ToString()}";
        }

        return rspn;
    }

    public bool DeleteProductById(int id)
    {
        using (var connection = db.GetConnection())
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("DELETE FROM Products WHERE Id = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
    }

    public Product GetProductById(int id)
    {
        // SqlConnection connection = new SqlConnection(db.GetConnection().ToString())


        using (var connection = db.GetConnection())
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("SELECT * FROM Products WHERE Id = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var product = new Product
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Price = reader.GetDecimal(2)
                        };
                        return product;
                    }
                    else
                    {
                        throw new InvalidOperationException($"Product with ID {id} not found.");
                    }
                }
            }
        }
    }

    public async Task<ApiResponse<List<Product>>> AllProducts()
    {
        Console.WriteLine("Get Local Products");
        var rspn = new ApiResponse<List<Product>>();
        try
        {
            await using var connection = db.GetConnection();
            connection.Open();
            await using var command = new SqlCommand("SELECT * FROM Products", connection);
            await using var reader = await command.ExecuteReaderAsync();

            var products = new List<Product>();
            while (reader.Read())
            {
                products.Add(new Product
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = Convert.ToString((string)reader["Name"]),
                    Price = Convert.ToDecimal(reader["Price"])
                });
            }

            connection.Close();
            rspn.RespText = "ok";
            rspn.Status = true;
            rspn.Data = products;
        }
        catch (Exception e)
        {
            rspn.Status = false;
            rspn.RespText = $"Error: {e.ToString()}";
        }

        return rspn;
    }

    public async Task<CommonResponse> AddProduct(Product pdt)
    {
        var rspn = new CommonResponse();
        try
        {
            await using var connection = db.GetConnection();
            connection.Open();

            // Example SQL command to insert dummy data
            const string insertQuery = "INSERT INTO Products (Name, Price) VALUES (@Name, @Price)";

            await using var command = new SqlCommand(insertQuery, connection);
            // Add parameters to prevent SQL injection
            command.Parameters.AddWithValue("@Name", pdt.Name);
            command.Parameters.AddWithValue("@Price", pdt.Price);

            // You can add more data and execute the command as needed

            // Execute the query
            var rowsAffected = command.ExecuteNonQuery();

            // Optionally, check the number of rows affected or handle errors
            Console.WriteLine(rowsAffected > 0
                ? "Dummy data inserted successfully."
                : "Failed to insert dummy data.");
            connection.Close();
            rspn.RespText = "ok";
            rspn.Status = true;
        }
        catch (Exception e)
        {
            rspn.RespText = e.ToString();
            rspn.Status = false;
        }

        return rspn;
    }

    public void Dispose()
    {
        Console.WriteLine("ProductService Dispose");
        GC.SuppressFinalize(this);
    }

    // Implement other CRUD operations (Create, Update, Delete) as needed.
}