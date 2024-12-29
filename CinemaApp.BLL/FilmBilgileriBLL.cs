using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaApp.DAL;
using CinemaApp.Entity;


namespace CinemaApp.BLL
{
    public class FilmBilgileriBLL
    {
        private FilmBilgileriDAL filmDal;

        public FilmBilgileriBLL()
        {
            filmDal = new FilmBilgileriDAL();
        }

        public void FilmEkle(FilmBilgileri film)
        {
            // Yapım yılı doğrulaması: Yıl formatında mı?
            if (!int.TryParse(film.YapimYili, out int yapimYili) || yapimYili < 1900 || yapimYili > DateTime.Now.Year)
            {
                throw new Exception("Yapım yılı geçerli bir yıl formatında olmalıdır (örn: 2002).");
            }

            // Diğer mantıksal kontrolleri burada ekleyebilirsiniz
            // Örneğin, FilmAdi veya Yonetmen boş geçilemez
            if (string.IsNullOrWhiteSpace(film.FilmAdi))
            {
                throw new Exception("Film adı boş bırakılamaz.");
            }

            if (string.IsNullOrWhiteSpace(film.Yonetmen))
            {
                throw new Exception("Yönetmen adı boş bırakılamaz.");
            }
            // İş mantığı
            filmDal.FilmEkle(film);
        }

        public string GetFilmAfis(string filmAdi)
        {
            // İş mantığı: Film adı boş olamaz
            if (string.IsNullOrWhiteSpace(filmAdi))
            {
                throw new Exception("Film adı boş bırakılamaz.");
            }

            // DAL'dan afiş yolunu al
            string resimYolu = filmDal.GetFilmResim(filmAdi);

            if (resimYolu == null)
            {
                throw new Exception("Seçilen film bulunamadı.");
            }

            return resimYolu;
        }

        public List<string> GetFilmAdlari()
        {
            string query = "SELECT * FROM FilmBilgileri";
            string columnName = "FilmAdi";
            return filmDal.FilmBilgileriGetir(query, columnName);
        }

        public List<string> GetSalonAdlari()
        {
            string query = "SELECT * FROM SalonBilgileri";
            string columnName = "SalonAdi";
            return filmDal.FilmBilgileriGetir(query, columnName);
        }

        // Film bilgilerini almak için DAL metodunu çağıran metod
        public List<FilmBilgileri> FilmBilgileriGetir()
        {
            return filmDal.FilmBilgileriGetir(); // DAL'dan veri al
        }


        

        public bool FilmGuncelle(FilmBilgileri film)
        {
            return filmDal.FilmGuncelle(film); // DAL'daki metodu çağır
        }

        // Film silme işlemi
        public bool FilmSil(string filmAdi)
        {
            return filmDal.FilmSil(filmAdi); // DAL'dan silme işlemi yapılır
        }

        public decimal? GetFilmUcreti(string filmAdi)
        {
            if (string.IsNullOrEmpty(filmAdi))
            {
                throw new ArgumentException("Film adı boş olamaz.");
            }

            // DAL katmanından film ücretini alıyoruz
            return filmDal.GetFilmUcreti(filmAdi);
        }



    }
}
