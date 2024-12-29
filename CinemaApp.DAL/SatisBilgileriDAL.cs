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
    public class SatisBilgileriDAL
    {
        private DBConnection dbConnection;

        public SatisBilgileriDAL()
        {
            dbConnection = new DBConnection();
        }

        public List<string> GetDoluKoltuklar(string filmAdi, string salonAdi, string tarih, string saat)
        {
            List<string> doluKoltuklar = new List<string>();
            OleDbConnection conn = null;

            try
            {
                conn = dbConnection.GetConnection();
                conn.Open();

                string query = "SELECT KoltukNo FROM SatisBilgileri WHERE FilmAdi=@FilmAdi AND SalonAdi=@SalonAdi AND Tarih=@Tarih AND Saat=@Saat";
                using (OleDbCommand command = new OleDbCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@FilmAdi", filmAdi);
                    command.Parameters.AddWithValue("@SalonAdi", salonAdi);
                    command.Parameters.AddWithValue("@Tarih", tarih);
                    command.Parameters.AddWithValue("@Saat", saat);

                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["KoltukNo"] != DBNull.Value)
                            {
                                doluKoltuklar.Add(reader["KoltukNo"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Dolu koltuklar alınırken hata oluştu: " + ex.Message);
            }
            finally
            {
                dbConnection.CloseConnection();
            }

            return doluKoltuklar;
        }


        public void BiletSat(string filmAdi, string salonAdi, string tarih, string saat, string koltukNo, string ad, string soyad, string ucret, string satilmaTarihi)
        {
            OleDbConnection conn = null;
            try
            {
                conn = dbConnection.GetConnection();
                conn.Open();

                string query = "INSERT INTO SatisBilgileri (FilmAdi, SalonAdi, Tarih, Saat, KoltukNo, Ad, Soyad, Ucret, SatilmaTarihi) " +
                               "VALUES (@FilmAdi, @SalonAdi, @Tarih, @Saat, @KoltukNo, @Ad, @Soyad, @Ucret, @SatilmaTarihi)";
                using (OleDbCommand command = new OleDbCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@FilmAdi", filmAdi);
                    command.Parameters.AddWithValue("@SalonAdi", salonAdi);
                    command.Parameters.AddWithValue("@Tarih", tarih);
                    command.Parameters.AddWithValue("@Saat", saat);
                    command.Parameters.AddWithValue("@KoltukNo", koltukNo);
                    command.Parameters.AddWithValue("@Ad", ad);
                    command.Parameters.AddWithValue("@Soyad", soyad);
                    command.Parameters.AddWithValue("@Ucret", ucret);
                    command.Parameters.AddWithValue("@SatilmaTarihi", satilmaTarihi);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Bilet satışında bir hata oluştu: " + ex.Message);
            }
            finally
            {
                dbConnection.CloseConnection();
            }
        }

        public bool BiletIptali(string koltukNo)
        {
            OleDbConnection conn = null;
            try
            {
                conn = dbConnection.GetConnection();
                conn.Open();

                string query = "DELETE FROM SatisBilgileri WHERE KoltukNo = @KoltukNo";
                using (OleDbCommand command = new OleDbCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@KoltukNo", koltukNo);
                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Bilet iptali sırasında bir hata oluştu: " + ex.Message);
            }
            finally
            {
                dbConnection.CloseConnection();
            }
        }

        public List<(string FilmAdi, int BiletSayisi)> GetBiletSayilari()
        {
            var biletSayilari = new List<(string FilmAdi, int BiletSayisi)>();

            string query = "SELECT FilmAdi, COUNT(*) AS BiletSayisi FROM SatisBilgileri GROUP BY FilmAdi";
            OleDbConnection conn = null;
            try
            {
                conn = dbConnection.GetConnection();
                conn.Open();

                using (OleDbCommand command = new OleDbCommand(query, conn))
                {
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string filmAdi = reader["FilmAdi"].ToString();
                            int biletSayisi = Convert.ToInt32(reader["BiletSayisi"]);
                            biletSayilari.Add((filmAdi, biletSayisi));
                        }
                    }
                }
            }
            finally
            {
                dbConnection.CloseConnection();
            }

            return biletSayilari;
        }


        public DataTable GetSatisBilgileri()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM SatisBilgileri";
            OleDbConnection conn = dbConnection.GetConnection();
            try
            {
                conn.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("Veri okunurken hata oluştu: " + ex.Message);
            }
            finally
            {
                dbConnection.CloseConnection();
            }
            return dt;
        }

        // Veritabanından satış bilgilerini alır ve liste olarak döndürür
        public List<SatisBilgileri> GetSalesInfo()
        {
            OleDbConnection conn = dbConnection.GetConnection();
            List<SatisBilgileri> salesList = new List<SatisBilgileri>(); // Listeyi burada oluşturuyoruz.

            try
            {
                string query = "SELECT SatısId, KoltukNo, SalonAdi, FilmAdi, Tarih, Saat, Ad, Soyad, Ucret, SatilmaTarihi FROM SatisBilgileri";
                OleDbCommand command = new OleDbCommand(query, conn);
                conn.Open();

                OleDbDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    // Veriyi listeye ekle
                    salesList.Add(new SatisBilgileri
                    {
                        SatısId = Convert.ToInt32(reader["SatısId"]),
                        KoltukNo = reader["KoltukNo"].ToString(),
                        SalonAdi = reader["SalonAdi"].ToString(),
                        FilmAdi = reader["FilmAdi"].ToString(),
                        Tarih = reader["Tarih"].ToString(),
                        Saat = reader["Saat"].ToString(),
                        Ad = reader["Ad"].ToString(),
                        Soyad = reader["Soyad"].ToString(),
                        Ucret = reader["Ucret"].ToString(),
                        SatilmaTarihi = reader["SatilmaTarihi"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda işlemi loglamak veya göstermek isterseniz buraya ekleyebilirsiniz.
                throw new Exception("Veritabanı işlemi sırasında bir hata oluştu: " + ex.Message);
            }
            finally
            {
                // Veritabanı bağlantısını her durumda kapat
                dbConnection.CloseConnection();
            }

            return salesList; // Listeyi döndürüyoruz
        }







    }
}
