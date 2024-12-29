using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaApp.Entity;



namespace CinemaApp.DAL
{
    public class FilmBilgileriDAL
    {
        private DBConnection dbConnection;
        //FilmBilgileri film=new FilmBilgileri();

        public FilmBilgileriDAL()
        {
            dbConnection = new DBConnection();
        }

        public bool FilmAdiVarMi(string filmAdi)
        {
            using (OleDbConnection conn = dbConnection.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM FilmBilgileri WHERE FilmAdi = @FilmAdi";
                OleDbCommand command = new OleDbCommand(query, conn);
                command.Parameters.AddWithValue("@FilmAdi", filmAdi);
                conn.Open();
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        public void FilmEkle(FilmBilgileri film)
        {
            try
            {
                // Bağlantıyı alıyoruz
                OleDbConnection conn = dbConnection.GetConnection();
                string query = "INSERT INTO FilmBilgileri (FilmAdi, Yonetmen, FilmTuru, FilmSuresi, Tarih, YapimYili, Resim, FilmUcreti) VALUES (@FilmAdi, @Yonetmen, @FilmTuru, @FilmSuresi, @Tarih, @YapimYili, @Resim, @FilmUcreti)";

                OleDbCommand command = new OleDbCommand(query, conn);

                // Parametreleri ekliyoruz
                command.Parameters.AddWithValue("@FilmAdi", film.FilmAdi);
                command.Parameters.AddWithValue("@Yonetmen", film.Yonetmen);
                command.Parameters.AddWithValue("@FilmTuru", film.FilmTuru);
                command.Parameters.AddWithValue("@FilmSuresi", film.FilmSuresi);
                command.Parameters.AddWithValue("@Tarih", film.Tarih);
                command.Parameters.AddWithValue("@YapimYili", film.YapimYili);
                command.Parameters.AddWithValue("@Resim", string.IsNullOrEmpty(film.Resim) ? DBNull.Value : (object)film.Resim);
                command.Parameters.AddWithValue("@FilmUcreti", film.FilmUcreti);

                // Bağlantıyı açıyoruz ve komutu çalıştırıyoruz
                conn.Open();
                command.ExecuteNonQuery(); // Veriyi ekliyoruz
            }
            catch (Exception ex)
            {
                // Hata durumunda özel mesaj
                throw new Exception("Film eklenirken hata oluştu: " + ex.Message);
            }
            finally
            {
                // Bağlantıyı kapatıyoruz
                dbConnection.CloseConnection();
            }
        }

        public string GetFilmResim(string filmAdi)
        {
            OleDbConnection conn = null;
            try
            {
                // Bağlantıyı alıyoruz
                conn = dbConnection.GetConnection();
                string query = "SELECT Resim FROM FilmBilgileri WHERE FilmAdi=@FilmAdi";

                using (OleDbCommand command = new OleDbCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@FilmAdi", filmAdi);

                    conn.Open(); // Bağlantıyı açıyoruz

                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader["Resim"]?.ToString(); // Resim yolu döndürülüyor
                        }
                        else
                        {
                            return null; // Film bulunamadı
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata mesajını fırlatıyoruz
                throw new Exception("Film resmini alırken bir hata oluştu: " + ex.Message);
            }
            finally
            {
                // Bağlantıyı kapatıyoruz
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    dbConnection.CloseConnection();
                }
            }
        }

        public List<string> FilmBilgileriGetir(string query, string columnName)
        {
            List<string> items = new List<string>();
            OleDbConnection conn = null;

            try
            {
                // Bağlantıyı al
                conn = dbConnection.GetConnection();

                // Bağlantıyı aç
                conn.Open();

                // Sorguyu çalıştır
                OleDbCommand command = new OleDbCommand(query, conn);
                OleDbDataReader read = command.ExecuteReader();

                // Sonuçları okuyarak listeye ekle
                while (read.Read())
                {
                    items.Add(read[columnName].ToString());
                }

                // Okuyucuyu kapat
                read.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Veritabanından veri getirilirken hata oluştu: " + ex.Message);
            }
            finally
            {
                
                    dbConnection.CloseConnection();
                
            }

            return items;
        }


        public decimal? GetFilmUcreti(string filmAdi)
        {
            decimal? filmUcreti = null;
            OleDbConnection conn = null;

            try
            {
                conn = dbConnection.GetConnection();
                conn.Open();

                string query = "SELECT FilmUcreti FROM FilmBilgileri WHERE FilmAdi = @FilmAdi";
                using (OleDbCommand komut = new OleDbCommand(query, conn))
                {
                    komut.Parameters.AddWithValue("@FilmAdi", filmAdi);

                    using (OleDbDataReader reader = komut.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            filmUcreti = reader["FilmUcreti"] != DBNull.Value ? Convert.ToDecimal(reader["FilmUcreti"]) : (decimal?)null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Film ücreti alınırken hata oluştu: " + ex.Message);
            }
            finally
            {
                
                    dbConnection.CloseConnection();
                
            }

            return filmUcreti;
        }











        // Veritabanından film bilgilerini alacak metod
        public List<FilmBilgileri> FilmBilgileriGetir()
        {
            List<FilmBilgileri> filmListesi = new List<FilmBilgileri>();
            try
            {
                // Bağlantıyı alıyoruz
                OleDbConnection conn = dbConnection.GetConnection();

                // Bağlantıyı açıyoruz
                conn.Open();

                string query = "SELECT * FROM FilmBilgileri";
                OleDbCommand command = new OleDbCommand(query, conn);
                OleDbDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    FilmBilgileri film = new FilmBilgileri
                    {
                        FilmId = Convert.ToInt32(reader["FilmId"]),
                        FilmAdi = reader["FilmAdi"].ToString(),
                        Yonetmen = reader["Yonetmen"].ToString(),
                        FilmTuru = reader["FilmTuru"].ToString(),
                        FilmSuresi = reader["FilmSuresi"].ToString(),
                        Tarih = reader["Tarih"].ToString(),
                        YapimYili = reader["YapimYili"].ToString(),
                        Resim = reader["Resim"].ToString(),
                        FilmUcreti = reader["FilmUcreti"].ToString()
                    };

                    filmListesi.Add(film);
                }

                // Bağlantıyı kapatıyoruz
                dbConnection.CloseConnection();
            }
            catch (Exception ex)
            {
                throw new Exception("Veri okuma hatası: " + ex.Message);
            }

            return filmListesi;
        }


        public bool FilmGuncelle(FilmBilgileri film)
        {
            OleDbConnection conn = null;
            try
            {
                // Bağlantıyı alıyoruz
                conn = dbConnection.GetConnection();
                string query = "UPDATE FilmBilgileri SET Yonetmen=@Yonetmen, FilmTuru=@FilmTuru, FilmSuresi=@FilmSuresi, Tarih=@Tarih, YapimYili=@YapimYili, Resim=@Resim, FilmUcreti=@FilmUcreti WHERE FilmAdi=@FilmAdi";
                OleDbCommand command = new OleDbCommand(query, conn);

                // Parametreleri ekliyoruz
                command.Parameters.AddWithValue("@Yonetmen", film.Yonetmen);
                command.Parameters.AddWithValue("@FilmTuru", film.FilmTuru);
                command.Parameters.AddWithValue("@FilmSuresi", film.FilmSuresi);
                command.Parameters.AddWithValue("@Tarih", film.Tarih);
                command.Parameters.AddWithValue("@YapimYili", film.YapimYili);
                command.Parameters.AddWithValue("@Resim", string.IsNullOrEmpty(film.Resim) ? DBNull.Value : (object)film.Resim);
                command.Parameters.AddWithValue("@FilmUcreti", film.FilmUcreti);
                command.Parameters.AddWithValue("@FilmAdi", film.FilmAdi);

                // Bağlantıyı açıyoruz ve komutu çalıştırıyoruz
                conn.Open();
                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0; // İşlem başarılı mı?
            }
            catch (Exception ex)
            {
                // Hata durumunda özel mesaj
                throw new Exception("Film güncellenirken bir hata oluştu: " + ex.Message);
            }
            finally
            {
                // Bağlantıyı kapatıyoruz
                dbConnection.CloseConnection();
            }
        }


        public bool FilmSil(string filmAdi)
        {
            OleDbConnection conn = null;
            try
            {
                // Bağlantıyı alıyoruz
                conn = dbConnection.GetConnection();
                string query = "DELETE FROM FilmBilgileri WHERE FilmAdi=@FilmAdi";
                OleDbCommand command = new OleDbCommand(query, conn);

                // Parametreyi ekliyoruz
                command.Parameters.AddWithValue("@FilmAdi", filmAdi);

                // Bağlantıyı açıyoruz ve komutu çalıştırıyoruz
                conn.Open();
                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0; // İşlem başarılı mı?
            }
            catch (Exception ex)
            {
                // Hata durumunda özel mesaj
                throw new Exception("Film silinirken bir hata oluştu: " + ex.Message);
            }
            finally
            {
                // Bağlantıyı kapatıyoruz
                dbConnection.CloseConnection();
            }
        }


    }
}
