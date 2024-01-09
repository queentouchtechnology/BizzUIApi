using Bizz.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Bizz.DataService.Services;

public class CustomersService(DbService db) : IDisposable
{
    public async Task<CustomerResponse<Customer>> GetList(int offset, int pageSize)
    {
        var res = new CustomerResponse<Customer>();


        DataTable dt = new();

        try
        {
            await using var connection = db.GetConnection();
            {
                connection.Open();
                await using (var cmd = new SqlCommand("" +
                                                      @"WITH CustomerCTE AS (
                SELECT
                    *,
                    ROW_NUMBER() OVER (ORDER BY FirstName) AS RowNum,
                    COUNT(*) OVER () AS TotalCount
                FROM
                    customers
            )
            SELECT
                *
            FROM
                CustomerCTE
            WHERE
                RowNum BETWEEN @Offset + 1 AND @Offset + @PageSize;", connection))
                {
                    cmd.Parameters.AddWithValue("@Offset", offset);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    using var sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                }

                var cus = new List<Customer>();

                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    var row = dt.Rows[i];

                    cus.Add(
                        new Customer
                        {
                            // Id = Convert.ToInt32(row["Id"] == DBNull.Value ? 0 : Convert.ToInt32(row["Id"])),
                            Id = Convert.ToInt32(row["Id"] ?? 0),
                            Title = row["title"].ToString() ?? "",
                            Email = row["email"].ToString() ?? "",
                            Mobile = row["mobile"].ToString() ?? "",
                            Address1 = row["address1"].ToString() ?? "",
                            Address2 = row["address2"].ToString() ?? "",
                            City = row["city"].ToString() ?? "",
                            State = row["state"].ToString() ?? "",
                            Country = row["country"].ToString() ?? "",
                            Pincode = row["pincode"].ToString() ?? "",
                            Location = row["location"].ToString() ?? "",
                            AadharNo = row["aadharNo"].ToString() ?? "",
                            CompanyName = row["companyName"].ToString() ?? "",
                            GstNo = row["gstNo"].ToString() ?? "",
                            PanNo = row["panNo"].ToString() ?? "",
                            RegistrationType = row["registrationType"].ToString() ?? "",
                            FirstName = row["firstName"].ToString() ?? "",
                            LastName = row["lastName"].ToString() ?? "",
                            CreatedAt = Convert.ToDateTime(row["createdAt"] ?? DateTimeOffset.MinValue),
                            UpdatedAt = Convert.ToDateTime(row["updatedAt"] ?? DateTimeOffset.MinValue),
                            // UpdatedAt = Convert.ToDateTime(row["updatedAt"]== DBNull.Value ? DateTimeOffset.MinValue : row["updatedAt"]),
                        });
                    res.Count = Convert.ToInt32(row["TotalCount"] ?? 0);
                }

                res.Status = true;
                res.RespText = $"ok";
                res.Data = cus;
                connection.Close();
            }
        }
        catch (Exception e)
        {
            res.Status = false;
            res.RespText = $"Error: {e}";
        }

        return res;
    }


    public async Task<CustomerResponse<Customer>> DeleteCustomer(int customerId)
    {
        var response = new CustomerResponse<Customer>();

        try
        {
            await using var connection = db.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("DELETE FROM customers WHERE Id = @CustomerId", connection);
            command.Parameters.AddWithValue("@CustomerId", customerId);

            var rowsAffected = await command.ExecuteNonQueryAsync();

            if (rowsAffected > 0)
            {
                // Customer deletion was successful
                response.Status = true;
                response.RespText = "Customer deleted successfully.";
            }
            else
            {
                // No rows affected, customer with the given ID not found
                response.RespText = "Customer not found or error deleting customer.";
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions (log, rethrow, etc.)
            response.Status = false;
            response.RespText = $"Error: {ex.Message}";
        }

        return response;
    }

    public async Task<CustomerResponse<bool>> AddCustomer(Customer customer)
    {
        var response = new CustomerResponse<bool>();

        try
        {
            await using var connection = db.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand(
                @"INSERT INTO customers (Title, Email, Mobile, Address1, Address2, City, State, Country, Pincode, Location, AadharNo, CompanyName, GstNo, PanNo, RegistrationType, FirstName, LastName, CreatedAt, UpdatedAt)
                  VALUES (@Title, @Email, @Mobile, @Address1, @Address2, @City, @State, @Country, @Pincode, @Location, @AadharNo, @CompanyName, @GstNo, @PanNo, @RegistrationType, @FirstName, @LastName, @CreatedAt, @UpdatedAt);
                  SELECT CAST(SCOPE_IDENTITY() AS INT);", connection);

            command.Parameters.AddWithValue("@Title", customer.Title);
            command.Parameters.AddWithValue("@Email", customer.Email);
            command.Parameters.AddWithValue("@Mobile", customer.Mobile);
            command.Parameters.AddWithValue("@Address1", customer.Address1);
            command.Parameters.AddWithValue("@Address2", customer.Address2);
            command.Parameters.AddWithValue("@City", customer.City);
            command.Parameters.AddWithValue("@State", customer.State);
            command.Parameters.AddWithValue("@Country", customer.Country);
            command.Parameters.AddWithValue("@Pincode", customer.Pincode);
            command.Parameters.AddWithValue("@Location", customer.Location);
            command.Parameters.AddWithValue("@AadharNo", customer.AadharNo);
            command.Parameters.AddWithValue("@CompanyName", customer.CompanyName);
            command.Parameters.AddWithValue("@GstNo", customer.GstNo);
            command.Parameters.AddWithValue("@PanNo", customer.PanNo);
            command.Parameters.AddWithValue("@RegistrationType", customer.RegistrationType);
            command.Parameters.AddWithValue("@FirstName", customer.FirstName);
            command.Parameters.AddWithValue("@LastName", customer.LastName);
            command.Parameters.AddWithValue("@CreatedAt", customer.CreatedAt);
            command.Parameters.AddWithValue("@UpdatedAt", customer.UpdatedAt);

            var newCustomerId = (int)await command.ExecuteScalarAsync();
            if (newCustomerId > 0)
            {
                response.Status = true;
            }
            else
            {
                response.Status = false;
            }
        }
        catch (Exception e)
        {
            response.Status = false;
            response.RespText = $"Error: {e.Message}";
        }

        return response;
    }


    public async Task<bool> UpdateCustomer(Customer updatedCustomer)
    {
        try
        {
            await using var connection = db.GetConnection();
            await connection.OpenAsync();

            const string query =
                "UPDATE customers SET Title = @Title, Email = @Email, Mobile = @Mobile, Address1 = @Address1, " +
                "Address2 = @Address2, City = @City, State = @State, Country = @Country, Pincode = @Pincode, " +
                "Location = @Location, AadharNo = @AadharNo, CompanyName = @CompanyName, GstNo = @GstNo, PanNo = @PanNo, " +
                "RegistrationType = @RegistrationType, FirstName = @FirstName, LastName = @LastName, UpdatedAt = GETDATE() " +
                "WHERE Id = @Id";
            await using var command = new SqlCommand(query, connection);
            {
                command.Parameters.AddWithValue("@Id", updatedCustomer.Id);
                command.Parameters.AddWithValue("@Title", updatedCustomer.Title);
                command.Parameters.AddWithValue("@Email", updatedCustomer.Email);
                command.Parameters.AddWithValue("@Mobile", updatedCustomer.Mobile);
                command.Parameters.AddWithValue("@Address1", updatedCustomer.Address1);
                command.Parameters.AddWithValue("@Address2", updatedCustomer.Address2);
                command.Parameters.AddWithValue("@City", updatedCustomer.City);
                command.Parameters.AddWithValue("@State", updatedCustomer.State);
                command.Parameters.AddWithValue("@Country", updatedCustomer.Country);
                command.Parameters.AddWithValue("@Pincode", updatedCustomer.Pincode);
                command.Parameters.AddWithValue("@Location", updatedCustomer.Location);
                command.Parameters.AddWithValue("@AadharNo", updatedCustomer.AadharNo);
                command.Parameters.AddWithValue("@CompanyName", updatedCustomer.CompanyName);
                command.Parameters.AddWithValue("@GstNo", updatedCustomer.GstNo);
                command.Parameters.AddWithValue("@PanNo", updatedCustomer.PanNo);
                command.Parameters.AddWithValue("@RegistrationType", updatedCustomer.RegistrationType);
                command.Parameters.AddWithValue("@FirstName", updatedCustomer.FirstName);
                command.Parameters.AddWithValue("@LastName", updatedCustomer.LastName);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0; // Returns true if at least one row was updated
            }
        }
        catch (Exception)
        {
            // Log or handle the exception as needed
            return false; // Update failed
        }
    }


    public async Task<GetResponse<Customer>> GetCustomerById(int customerId)
    {
        Console.WriteLine($"GetCustomerById called with id: {customerId}");
        var res = new GetResponse<Customer>();
        try
        {
            await using var connection = db.GetConnection();
            await connection.OpenAsync();

            await using var command = new SqlCommand("SELECT * FROM customers WHERE Id = @CustomerId", connection);
            command.Parameters.AddWithValue("@CustomerId", customerId);

            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var customer = new Customer
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Title = reader.GetString(reader.GetOrdinal("Title")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Mobile = reader.GetString(reader.GetOrdinal("Mobile")),
                    Address1 = reader.GetString(reader.GetOrdinal("address1")),
                    Address2 = reader.GetString(reader.GetOrdinal("address2")),
                    City = reader.GetString(reader.GetOrdinal("city")),
                    State = reader.GetString(reader.GetOrdinal("state")),
                    Country = reader.GetString(reader.GetOrdinal("country")),
                    Pincode = reader.GetString(reader.GetOrdinal("pincode")),
                    Location = reader.GetString(reader.GetOrdinal("location")),
                    AadharNo = reader.GetString(reader.GetOrdinal("aadharNo")),
                    CompanyName = reader.GetString(reader.GetOrdinal("companyName")),
                    GstNo = reader.GetString(reader.GetOrdinal("gstNo")),
                    PanNo = reader.GetString(reader.GetOrdinal("panNo")),
                    RegistrationType = reader.GetString(reader.GetOrdinal("registrationType")),
                    FirstName = reader.GetString(reader.GetOrdinal("firstName")),
                    LastName = reader.GetString(reader.GetOrdinal("lastName")),
                    CreatedAt = reader.GetDateTime("createdAt"),
                    UpdatedAt = reader.GetDateTime("updatedAt"),
                };
                res.Status = true;
                res.RespText = "ok";
                res.Data = customer;
            }
            else
            {
                res.Status = false;
                res.RespText = "Customer not found";
            }
        }
        catch (Exception e)
        {
            res.Status = false;
            res.RespText = $"Error: {e.Message}";
        }

        return res;
    }

    public void Dispose()
    {
        Console.WriteLine("CustomerService Dispose");
        GC.SuppressFinalize(this);
    }
}