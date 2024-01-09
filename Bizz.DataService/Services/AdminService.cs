using Bizz.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bizz.DataService.Services
{
    public class AdminService(DbService db):BaseService
    {
        public async Task<Response> Create(Admin user)
        {
            var res = new Response();

            try
            {
                await using var connection = db.GetQueenTouchTechDBConnection();
                await connection.OpenAsync();

                string sql = @"
                INSERT INTO qttadmin (name, contactName, email, mobile, address, code, isActive, createdAt, updatedAt, pass, wallet, imageurl, website, dashboard_url)
                VALUES (@Name, @ContactName, @Email, @Mobile, @Address, @Code, @IsActive, @CreatedAt, @UpdatedAt, @Password, @Wallet, @ImageUrl, @Website, @DashboardUrl)
            ";


                using var command = new SqlCommand(sql, connection);

                // Add parameters to prevent SQL injection
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@ContactName", user.ContactName);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Mobile", user.Mobile);
                command.Parameters.AddWithValue("@Address", user.Address);
                command.Parameters.AddWithValue("@Code", user.Code);
                command.Parameters.AddWithValue("@IsActive", user.IsActive);
                command.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString());
                command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now.ToString());
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@Wallet", user.Wallet);
                command.Parameters.AddWithValue("@ImageUrl", user.ImageUrl);
                command.Parameters.AddWithValue("@Website", user.Website);
                command.Parameters.AddWithValue("@DashboardUrl", user.DashboardUrl);

                //var newCustomerId = (int)await command.ExecuteScalarAsync();
                var rowsAffected = await command.ExecuteNonQueryAsync();
                if (rowsAffected > 0)
                {
                    res.Status = true;
                    res.RespText = $"OK";
                }
                else
                {
                    res.Status = false;
                    res.RespText = $"Something went wrong";
                }


            }
            catch (Exception e)
            {
                res.Status = false;
                res.RespText = $"Error: {e.Message}";
            }

            return res;
        }

        public async Task<List<Admin>> ListAllAsync()
        {
            List<Admin> users = [];

            await using var connection = db.GetQueenTouchTechDBConnection();
            await connection.OpenAsync();

            string query = "SELECT * FROM qttadmin";

            using var command = new SqlCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Admin user = new()
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Name = reader["Name"].ToString() ?? "",
                    ContactName = reader["ContactName"].ToString() ?? "",
                    Email = reader["Email"].ToString() ?? "",
                    Mobile = reader["Mobile"].ToString() ?? "",
                    Address = reader["Address"].ToString() ?? "",
                    Code = reader["Code"].ToString() ?? "",
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                    CreatedAt = reader["CreatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedAt"]) : default(DateTime),
                    UpdatedAt = reader["UpdatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["UpdatedAt"]) : default(DateTime),
                    Password = reader["pass"].ToString() ?? "",
                    Wallet = Convert.ToDecimal(reader["wallet"]),
                    ImageUrl = reader["imageurl"].ToString() ?? "",
                    Website = reader["website"].ToString() ?? "",
                    DashboardUrl = reader["dashboard_url"].ToString() ?? "",
                    // Map other columns as needed
                };

                users.Add(user);
            }

            return users;
        }
    }
}
