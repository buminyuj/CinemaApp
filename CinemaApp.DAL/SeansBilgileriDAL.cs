using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaApp.Entity;

namespace CinemaApp.DAL
{
    public class SeansBilgileriDAL
    {
        private DBConnection dbConnection;

        public SeansBilgileriDAL()
        {
            dbConnection = new DBConnection();
        }


        public List<string> GetSeansTarihleri(string filmAdi, string salonAdi)
        {
            List<string> tarihListesi = new List<string>();
            OleDbConnection conn = null;

            try
            {
                conn = dbConnection.GetConnection();
                conn.Open();

                string query = "SELECT Tarih FROM SeansBilgileri WHERE FilmAdi = @FilmAdi AND SalonAdi = @SalonAdi";

                OleDbCommand komut = new OleDbCommand(query, conn);
                komut.Parameters.AddWithValue("@FilmAdi", filmAdi);
                komut.Parameters.AddWithValue("@SalonAdi", salonAdi);

                OleDbDataReader reader = komut.ExecuteReader();
                while (reader.Read())
                {
                    tarihListesi.Add(reader["Tarih"].ToString());
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Seans bilgileri alınırken hata oluştu: " + ex.Message);
            }
            finally
            {
                
                    dbConnection.CloseConnection();
                
            }

            return tarihListesi;
        }


        public List<TimeSpan> GetSeansSaatleri(string filmAdi, string salonAdi)
        {
            List<TimeSpan> saatListesi = new List<TimeSpan>();
            OleDbConnection conn = null;

            try
            {
                conn = dbConnection.GetConnection();
                conn.Open();

                string query = "SELECT Seans FROM SeansBilgileri WHERE FilmAdi = @FilmAdi AND SalonAdi = @SalonAdi";

                OleDbCommand komut = new OleDbCommand(query, conn);
                komut.Parameters.AddWithValue("@FilmAdi", filmAdi);
                komut.Parameters.AddWithValue("@SalonAdi", salonAdi);

                OleDbDataReader reader = komut.ExecuteReader();
                while (reader.Read())
                {
                    string saatStr = reader["Seans"].ToString();
                    if (TimeSpan.TryParse(saatStr, out TimeSpan seansSaati))
                    {
                        saatListesi.Add(seansSaati);
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Seans bilgileri alınırken hata oluştu: " + ex.Message);
            }
            finally
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    dbConnection.CloseConnection();
                }
            }

            return saatListesi;
        }

        public void SeansEkle(string filmAdi, string salonAdi, string tarih, string seans)
        {
            OleDbConnection conn = null;

            try
            {
                conn = dbConnection.GetConnection();
                conn.Open();

                string query = "INSERT INTO SeansBilgileri (FilmAdi, SalonAdi, Tarih, Seans) VALUES (@FilmAdi, @SalonAdi, @Tarih, @Seans)";
                OleDbCommand command = new OleDbCommand(query, conn);

                command.Parameters.AddWithValue("@FilmAdi", filmAdi);
                command.Parameters.AddWithValue("@SalonAdi", salonAdi);
                command.Parameters.AddWithValue("@Tarih", tarih);
                command.Parameters.AddWithValue("@Seans", seans);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Seans eklenirken hata oluştu: " + ex.Message);
            }
            finally
            {
                dbConnection.CloseConnection();
            }
        }

    }
}
