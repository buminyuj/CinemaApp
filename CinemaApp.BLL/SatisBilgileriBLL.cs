using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaApp.DAL;
using CinemaApp.Entity;

namespace CinemaApp.BLL
{
    public class SatisBilgileriBLL
    {
        private SatisBilgileriDAL satisBilgileriDAL;

        public SatisBilgileriBLL()
        {
            satisBilgileriDAL = new SatisBilgileriDAL();
        }


        public List<string> GetDoluKoltuklar(string filmAdi, string salonAdi, string tarih, string saat)
        {
            if (string.IsNullOrEmpty(filmAdi) || string.IsNullOrEmpty(salonAdi) || string.IsNullOrEmpty(tarih) || string.IsNullOrEmpty(saat))
            {
                throw new ArgumentException("Film adı, salon adı, tarih veya saat boş olamaz.");
            }

            return satisBilgileriDAL.GetDoluKoltuklar(filmAdi, salonAdi, tarih, saat);
        }


        public void BiletSat(string filmAdi, string salonAdi, string tarih, string saat, string koltukNo, string ad, string soyad, string ucret)
        {
            // Veri doğrulama
            if (string.IsNullOrEmpty(filmAdi) ||
                string.IsNullOrEmpty(salonAdi) ||
                string.IsNullOrEmpty(tarih) ||
                string.IsNullOrEmpty(saat) ||
                string.IsNullOrEmpty(koltukNo) ||
                string.IsNullOrEmpty(ad) ||
                string.IsNullOrEmpty(soyad) ||
                string.IsNullOrEmpty(ucret))
            {
                throw new ArgumentException("Lütfen tüm alanları doldurun!");
            }

            // DAL katmanına yönlendirme
            string satilmaTarihi = DateTime.Now.ToString("dd/MM/yyyy");
            satisBilgileriDAL.BiletSat(filmAdi, salonAdi, tarih, saat, koltukNo, ad, soyad, ucret, satilmaTarihi);
        }

        public void BiletIptali(string koltukNo)
        {
            // Veri doğrulama
            if (string.IsNullOrEmpty(koltukNo))
            {
                throw new ArgumentException("Lütfen iptal edilecek koltuğu seçin!");
            }

            // DAL katmanına yönlendirme
            bool success = satisBilgileriDAL.BiletIptali(koltukNo);

            if (!success)
            {
                throw new Exception("Bilet bulunamadı veya başka bir hata oluştu.");
            }
        }

        public List<(string FilmAdi, int BiletSayisi)> GetBiletSayilari()
        {
            // BLL katmanında, veritabanından verileri çekmeden önce herhangi bir iş mantığı ekleyebilirsiniz.
            return satisBilgileriDAL.GetBiletSayilari();
        }

        // Veritabanından SatisBilgileri verilerini alır
        public DataTable GetSatisBilgileri()
        {
            return satisBilgileriDAL.GetSatisBilgileri();
        }

        public List<SatisBilgileri> GetSalesInfo()
        {
            return satisBilgileriDAL.GetSalesInfo(); // Veritabanından alınan listeyi döndürüyoruz.
        }



    }
}
