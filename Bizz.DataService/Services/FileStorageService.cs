using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Hosting;

using Bizz.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bizz.DataService.Services;

public class FileStorageService(DbService db) : BaseService
{
    public async Task<Response> Create(SaveFileStorageReq req)
    {
        var res = new Response();
        try
        {
            await using var connection = db.GetConnection();
            {
                connection.Open();

                const string query = "INSERT INTO [dbo].FileStorage (UserId, FileName) VALUES (@UserId, @FileName)";
                await using var cmd = new SqlCommand(query, connection);
                {
                    cmd.Parameters.AddWithValue("@UserId", req.UserId);
                    cmd.Parameters.AddWithValue("@FileName", req.File.FileName);

                    var rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                    {
                        res.Status = false;
                        res.RespText = "Insert Error: Something went wrong";
                    }
                }
                connection.Close();
                // env.ContentRootPath
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Images", req.UserId.ToString());
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                filepath = Path.Combine(filepath, req.File.FileName);
                await using var stream = new FileStream(filepath, FileMode.Create);
                {
                    await req.File.CopyToAsync(stream);
                }
            }
        }
        catch (Exception e)
        {
            res.Status = false;
            res.RespText = $"Error: {e}";
        }
        return res;
    }

    public async Task<ListResponse<FileStorageRes>> GetList()
    {
        var res = new ListResponse<FileStorageRes>();
        try
        {
            await using var connection = db.GetConnection();
            {
                connection.Open();
                await using var cmd = new SqlCommand("SELECT * FROM FileStorage", connection);
                {
                    var sda = new SqlDataAdapter(cmd);
                    var dt = new DataTable();
                    sda.Fill(dt);

                    // var host = httpContext.Request.Host.Value;
                    // var port = httpContext.Request.Host.Port;
                    // string scheme = httpContext.Request.Scheme;
                    // if (host == "localhost")
                    // {
                    //     host = $@"localhost:{port}";
                    // }
                    var files = new List<FileStorageRes>();
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        var row = dt.Rows[i];
                        var UserId = Convert.ToInt32(row["UserId"]);
                        var FileName = Convert.ToString((string)row["FileName"]);
                        files.Add(
                            new FileStorageRes
                            {
                                Id = Convert.ToInt32(row["Id"]),
                                UserId = UserId,
                                FileName = FileName,
                                FilePath = $@"/Images/{UserId}/{FileName}"
                            });
                    }
                    res.Data = files;
                }
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

}