using Bizz.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bizz.DataService.Services
{
    public class UserService(DbService db):BaseService
    {
        public async Task<Response> Create(User user)
        {
            var res = new Response();

            try
            {
                await using var connection = db.GetQueenTouchTechDBConnection();
                await connection.OpenAsync();

                string sql = "INSERT INTO users " +
                         "(Name, Email, Password, OrgId, Role, DefaultStatus, IsActive, Mobile, " +
                         "Address, Code, ReportToId, CreatedAt, UpdatedAt, user_wallet, " +
                         "user_imageurl, dashboard_url, Website) " +
                         "VALUES " +
                         "(@Name, @Email, @Password, @OrgId, @Role, @DefaultStatus, @IsActive, @Mobile, " +
                         "@Address, @Code, @ReportToId, @CreatedAt, @UpdatedAt, @UserWallet, " +
                         "@UserImageUrl, @DashboardUrl, @Website); " +
                         "SELECT SCOPE_IDENTITY();";


                using var command = new SqlCommand(sql, connection);

                // Add parameters to prevent SQL injection
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@OrgId", user.OrgId);
                command.Parameters.AddWithValue("@Role", user.Role);
                command.Parameters.AddWithValue("@DefaultStatus", "Access");
                command.Parameters.AddWithValue("@IsActive", user.IsActive);
                command.Parameters.AddWithValue("@Mobile", user.Mobile);
                command.Parameters.AddWithValue("@Address", user.Address);
                command.Parameters.AddWithValue("@Code", user.Code);
                command.Parameters.AddWithValue("@ReportToId", user.ReportToId);
                command.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString());
                command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now.ToString());
                command.Parameters.AddWithValue("@UserWallet", Convert.ToDecimal(0.00));
                command.Parameters.AddWithValue("@UserImageUrl", user.UserImageUrl);
                command.Parameters.AddWithValue("@DashboardUrl", user.DashboardUrl);
                command.Parameters.AddWithValue("@Website", user.Website);

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

        public async Task<List<User>> ListAllAsync()
        {
            List<User> users = new List<User>();

            await using var connection = db.GetQueenTouchTechDBConnection();
            await connection.OpenAsync();

            string query = "SELECT * FROM Users";

            using var command = new SqlCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                User user = new User
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Name = reader["Name"].ToString() ?? "",
                    Email = reader["Email"].ToString() ?? "",
                    Password = reader["Password"].ToString() ?? "",
                    OrgId = Convert.ToInt32(reader["OrgId"]),
                    Role = reader["Role"].ToString() ?? "",
                    DefaultStatus = reader["DefaultStatus"].ToString() ?? "",
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                    Mobile = reader["Mobile"].ToString() ?? "",
                    Address = reader["Address"].ToString() ?? "",
                    Code = reader["Code"].ToString() ?? "",
                    ReportToId = Convert.ToInt32(reader["ReportToId"]),
                    CreatedAt = reader["CreatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedAt"]) : default(DateTime),
                    UpdatedAt = reader["UpdatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["UpdatedAt"]) : default(DateTime),
                    UserWallet = reader["user_wallet"] != DBNull.Value ? Convert.ToDecimal(reader["user_wallet"]) : default(decimal),
                    UserImageUrl = reader["user_imageurl"].ToString() ?? "",
                    DashboardUrl = reader["dashboard_url"].ToString() ?? "",
                    Website = reader["Website"].ToString() ?? ""
                    // Map other columns as needed
                };

                users.Add(user);
            }

            return users;
        }
    }
}
