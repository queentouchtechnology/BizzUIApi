using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Bizz.Entities.Models;

namespace Bizz.DataService.Services
{
    public class OrganizationService(DbService db) : BaseService
    {

        public async Task<List<OrganizationModel>> GetOrganizationsAsync()
        {
            List<OrganizationModel> organizations = [];

            await using var connection = db.GetQueenTouchTechDBConnection();
            {
                await connection.OpenAsync();

                string query = "SELECT * FROM organizations order by id desc"; // Adjust your query as needed

                using (SqlCommand command = new(query, connection))
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        OrganizationModel organization = new()
                        {
                            Id = GetValue<int>(reader, "Id"),
                            Name = GetValue<string>(reader, "Name"),
                            ContactName = GetValue<string>(reader, "ContactName"),
                            Email = GetValue<string>(reader, "Email"),
                            Mobile = GetValue<string>(reader, "Mobile"),
                            Address = GetValue<string>(reader, "Address"),
                            Code = GetValue<string>(reader, "Code"),
                            IsActive = GetValue<bool>(reader, "IsActive"),
                            CreatedAt = GetValue<DateTime>(reader, "CreatedAt"),
                            UpdatedAt = GetValue<DateTime>(reader, "UpdatedAt"),
                            OrgWallet = GetValue<Decimal>(reader, "OrgWallet"),
                            OrgImageUrl = GetValue<string>(reader, "OrgImageUrl"),
                            OrgPass = GetValue<string>(reader, "OrgPass"),
                            MstrId = GetValue<int>(reader, "MstrId"),
                            TokenId = GetValue<string>(reader, "TokenId"),
                            OrgWebsite = GetValue<string>(reader, "OrgWebsite"),
                            APIToken = GetValue<string>(reader, "APIToken"),
                            DashboardUrl = GetValue<string>(reader, "DashboardUrl")
                            // Add other properties as needed
                        };

                        organizations.Add(organization);
                    }
                }
            }

            return organizations;
        }

        private static T GetValue<T>(SqlDataReader reader, string columnName)
        {
            int ordinal;
            try
            {
                ordinal = reader.GetOrdinal(columnName);
            }
            catch (IndexOutOfRangeException)
            {
                // Handle missing column gracefully
                return default(T);
            }

            return reader.IsDBNull(ordinal) ? default(T) : reader.GetFieldValue<T>(ordinal);
        }

        // Helper method to extract the column name from the error message
        private string ExtractViolatingColumn(string errorMessage)
        {
            // This is a simplistic example, you may need to parse the error message more robustly based on your actual error format.
            // Adjust this method based on your specific database engine and error message format.
            const string prefix = "Cannot insert duplicate key in object 'YourTableName'.";

            int startIndex = errorMessage.IndexOf(prefix);
            int endIndex = errorMessage.IndexOf("'", startIndex + prefix.Length + 1);

            if (startIndex != -1 && endIndex != -1)
            {
                return errorMessage.Substring(startIndex + prefix.Length, endIndex - (startIndex + prefix.Length));
            }

            // If the parsing fails, return a default column name
            return "column";
        }
        public async Task<Response> SetOrg(OrganizationModel org)        
        {
            Guid UniqueID = Guid.NewGuid();

            var res = new Response();
            try
            {
                await using var connection = db.GetQueenTouchTechDBConnection();
               
                using SqlCommand cmd = new("Set_Org", connection);

                //EXEC  '-1','user2','user_one4@user.com','password',null,null,null,'9876543210',null,'JC1005',null 
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Clear(); // add this line to clear InsertParameters collection before adding parameters
                cmd.Parameters.AddWithValue("@id", "-1");
                cmd.Parameters.AddWithValue("@name", org.Name);
                cmd.Parameters.AddWithValue("@contactName", org.ContactName);
                cmd.Parameters.AddWithValue("@email", org.Email);
                cmd.Parameters.AddWithValue("@mid", "1");
                cmd.Parameters.AddWithValue("@website", org.OrgWebsite);
                cmd.Parameters.AddWithValue("@mobile", org.Mobile);
                cmd.Parameters.AddWithValue("@address", org.Address);
                cmd.Parameters.AddWithValue("@code", "ORG" + Slno("id", "organizations"));
                cmd.Parameters.AddWithValue("@dashboard_url","test");
                cmd.Parameters.AddWithValue("@tokenid", "A101BC1E-79BE-419C-BCEE-7BA4246A7F7E");

                // cmd.Parameters.Add("@webRes", SqlDbType.VarChar, 30);
                //cmd.Parameters["@webRes"].Direction = ParameterDirection.Output;

                await connection.OpenAsync();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        res.Status = true;
                        res.RespText = rd.GetValue(0).ToString() ?? "";
                        // id = rd.GetValue(1).ToString();

                    }
                }
                connection.Close();
            }
            catch (SqlException ex) when (ex.Number == 2627) // Unique key violation error number
            {
                // Handle unique key violation
                // Log the error or provide a user-friendly message

                // Extract the violating column name from the error message
                string violatingColumn = ExtractViolatingColumn(ex.Message);

                // Display a more specific error message based on the violating column
                string errorMessage = $"Same {violatingColumn} value already exists.";

                // Set the response status and message
                res.Status = false;
                res.RespText = errorMessage;
            }

            catch (Exception e)
            {
                res.Status = false;
                res.RespText = $"Error: {e}";
            }
            return res;
        }
    }
}
