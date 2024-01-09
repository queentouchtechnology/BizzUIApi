using System.Data.SqlClient;

namespace Bizz.DataService.Services;

public class DbService : BaseService
{
    private const string ConnectionString = "Server=wish.grabweb.in,5422;Initial Catalog=qt_BizzUI;Persist Security Info=True;User ID=qt_BizzUI_User;Password=0&948idDc;Encrypt=True;TrustServerCertificate=true;MultipleActiveResultSets=True";

    private const string QueenTouchTechDB = "Server=wish.grabweb.in,4519;Initial Catalog=queentouch_db;Persist Security Info=True;User ID=qtt_auth_user;Password=2F^a2q29k;Encrypt=True;TrustServerCertificate=true;MultipleActiveResultSets=True";
    //2F^a2q29k qtt_auth_user
    public SqlConnection GetConnection()
    {
        Console.WriteLine("GetConnection");
        return new SqlConnection(ConnectionString);
    }

    public SqlConnection GetQueenTouchTechDBConnection()
    {
        Console.WriteLine("GetQueenTouchTechDBConnection");
        return new SqlConnection(QueenTouchTechDB);
    }
}