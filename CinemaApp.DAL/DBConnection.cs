using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.IO;


namespace CinemaApp.DAL
{
    public class DBConnection
    {
        private string connectionString;
        private OleDbConnection conn;

        public DBConnection()
        {
            // Uygulamanın çalıştığı dizini alıyoruz
            string databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SinemaOtomasyonu1.accdb");
            connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={databasePath}";
            conn = new OleDbConnection(connectionString);
        }

        public OleDbConnection GetConnection()
        {
            return conn;
        }

        public void CloseConnection()
        {
            if (conn != null && conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }

    }
}
