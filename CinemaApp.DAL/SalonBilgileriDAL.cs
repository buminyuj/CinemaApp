using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaApp.Entity;

namespace CinemaApp.DAL
{
    public class SalonBilgileriDAL
    {
        private DBConnection dbConnection;

        public SalonBilgileriDAL()
        {
            dbConnection = new DBConnection();
        }

        public void AddSalon(string salonAdi)
        {
            OleDbConnection conn = null;
            try
            {
                conn = dbConnection.GetConnection();
                conn.Open();
                string query = "INSERT INTO SalonBilgileri (SalonAdi) VALUES (@SalonAdi)";
                OleDbCommand command = new OleDbCommand(query, conn);
                command.Parameters.AddWithValue("@SalonAdi", salonAdi);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Salon eklenirken bir hata oluştu: " + ex.Message);
            }
            finally
            {
                dbConnection.CloseConnection();
            }
        }
    }
}
