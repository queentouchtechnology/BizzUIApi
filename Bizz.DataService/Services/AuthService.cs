using Bizz.Entities.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace Bizz.DataService.Services
{
        public class AuthService(DbService db) : BaseService
        {

            private readonly IJSRuntime jsRuntime;
           private readonly IServiceProvider serviceProvider;

        public async Task<ClaimsPrincipal> AuthenticateAsync(string username, string password)
            {
          

                await using var connection = db.GetQueenTouchTechDBConnection();
                {
                    await connection.OpenAsync();

                    var query = "Set_master_Login";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@emailORcode", username);
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@loginIp", GetPublicIP());                    

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var userId = reader.GetInt32(2);
                                var userName = reader.GetString(3);
                                var identity = new ClaimsIdentity(new[]
                                {
                                new Claim(ClaimTypes.Name, userName),
                               // new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                            }, CookieAuthenticationDefaults.AuthenticationScheme);
                               return new ClaimsPrincipal(identity);
                            }
                        }
                    }
                }

                return null;
            }

            private void SaveUsernameToCookie(string username)
            {
                jsRuntime.InvokeVoidAsync("blazorSetCookie", "username", username);
            }

            public async Task<SetMasterLoginModel> CheckUser(string username,string password)
            {
                Console.WriteLine($"Login.....");
                var res = new SetMasterLoginModel();
                try
                {
                    await using var connection = db.GetQueenTouchTechDBConnection();
                    await connection.OpenAsync();

                    await using var command = new SqlCommand("Set_master_Login", connection);

                    command.CommandType = CommandType.StoredProcedure;               
                    command.Parameters.AddWithValue("@emailORcode", username);              
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@loginIp", GetPublicIP());

                

                    await using var reader = await command.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                       res.Response = reader.GetValue(0).ToString()??"";
                       res.Token   = reader.GetValue(1).ToString() ?? "";
                       res.UserId  = Convert.ToInt32(reader.GetValue(2));
                       res.Name = reader.GetValue(3).ToString() ?? "";
                       res.Email = reader.GetValue(4).ToString() ?? "";
                       res.Code = reader.GetValue(5).ToString() ?? "";
                       res.UserType = reader.GetValue(6).ToString() ?? "";

                        Console.WriteLine($"Name---> {res.Name} Found");
                        Console.WriteLine($"Token---> {res.Token} ");
                        Console.WriteLine($"UserId---> {res.UserId} ");
                        Console.WriteLine($"Response---> {res.Response} ");
                        Console.WriteLine($"Email---> {res.Email}");
                        Console.WriteLine($"Code---> {res.Code}");
                        Console.WriteLine($"UserType---> {res.UserType}");

                    }
                    else
                    {
                        res.Response = "User Not Found";
                        res.Token = "";
                        res.UserId =0;
                        res.Name = "";
                        res.Email = "";
                        res.Code = "";
                        res.UserType = "";
                        Console.WriteLine($"User Not Found");
                    }


                    connection.Close();
                    return res;

                }
                catch (Exception e)
                {
                    res.Response = $"Error: {e.Message}";
                    res.Token = "";
                    res.UserId = 0;
                    res.Name = "";
                    res.Email = "";
                    res.UserType = "";

                    Console.WriteLine($"Error: {e.Message}");
                    return res;
                }

           
            }
      
        }

}
