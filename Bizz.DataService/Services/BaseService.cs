using Microsoft.JSInterop;
using System.Data.SqlClient;
using System.Net;

namespace Bizz.DataService.Services;

public class BaseService : IDisposable
{
    private readonly IJSRuntime jsRuntime;
    public void Dispose()
    {
        var name = this.ToString();
        var typeName = this.GetType().Name;
        Console.WriteLine($"{typeName} Disposed");
        GC.SuppressFinalize(this);
    }

    public string Slno(string columnname, string tablename)
    {

        DbService dbService = new DbService();
        using var con = dbService.GetQueenTouchTechDBConnection();

        string cmdText = "select  " + columnname + "  from " + tablename + " where " + columnname + " =(select max(" + columnname + ") from " + tablename + ")";
        SqlCommand command = new(cmdText, con);
        try
        {
            con.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                int id = int.Parse(reader[0].ToString()) + 1;
                return id.ToString("0000");
            }
            else
                return "0001";
        }
        catch (SqlException ee)
        {
            return "0";
        }
        finally
        {
            con.Close();
        }
    }
    public static string GetPublicIP()
    {

        String direction = "";
        WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
        using (WebResponse response = request.GetResponse())
        using (StreamReader stream = new StreamReader(response.GetResponseStream()))
        {
            direction = stream.ReadToEnd();
        }

        //Search for the ip in the html
        int first = direction.IndexOf("Address: ") + 9;
        int last = direction.LastIndexOf("</body>");
        direction = direction.Substring(first, last - first);
        return direction;

    }

    private async Task<string> GetFromCookie(string cookiename)
    {
        return await jsRuntime.InvokeAsync<string>("blazorGetCookie", cookiename);
    }

    private void SaveToCookie(string cookiename,string cookievalue)
    {
        jsRuntime.InvokeVoidAsync("blazorSetCookie", cookiename, cookievalue);
    }

    private void RemoveFromCookie(string cookiename)
    {
        jsRuntime.InvokeVoidAsync("blazorRemoveCookie", cookiename);
    }

}