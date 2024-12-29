using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaApp.DAL;
using CinemaApp.Entity;

namespace CinemaApp.BLL
{
    public class SeansBilgileriBLL
    {
        private SeansBilgileriDAL seansBilgileriDAL;

        public SeansBilgileriBLL()
        {
            seansBilgileriDAL = new SeansBilgileriDAL();
        }

        public List<string> GetValidSeansTarihleri(string filmAdi, string salonAdi)//bugünün tarihine eşit veya daha sonra olan bir tarihse Seans gün/ay/yıl bilgisi olarak geliyor bugünden önce ise gelmiyor
        {
            List<string> validTarihListesi = new List<string>();
            List<string> tarihListesi = seansBilgileriDAL.GetSeansTarihleri(filmAdi, salonAdi);

            foreach (string tarihStr in tarihListesi)
            {
                DateTime seansTarihi;
                if (DateTime.TryParse(tarihStr, new System.Globalization.CultureInfo("tr-TR"),
                                      System.Globalization.DateTimeStyles.None, out seansTarihi)
                    && seansTarihi >= DateTime.Now.Date)
                {
                    string formattedDate = seansTarihi.ToShortDateString();
                    if (!validTarihListesi.Contains(formattedDate))
                    {
                        validTarihListesi.Add(formattedDate);
                    }
                }
            }

            return validTarihListesi;
        }

        public List<TimeSpan> GetSeansSaatleri(string filmAdi, string salonAdi)
        {
            List<TimeSpan> validSaatListesi = new List<TimeSpan>();

            // Geçerli tarihleri alıyoruz (bugün veya sonrasındaki tarihler)
            List<string> validTarihListesi = GetValidSeansTarihleri(filmAdi, salonAdi);

            // BLL katmanından, tüm tarihleri ve saatleri alıyoruz
            List<TimeSpan> seansSaatleri = seansBilgileriDAL.GetSeansSaatleri(filmAdi, salonAdi);

            // Geçerli tarih ve saatleri kontrol ediyoruz
            foreach (string tarihStr in validTarihListesi)
            {
                DateTime seansTarihi;
                if (DateTime.TryParse(tarihStr, new System.Globalization.CultureInfo("tr-TR"),
                                       System.Globalization.DateTimeStyles.None, out seansTarihi))
                {
                    // Eğer tarih bugüne eşit veya daha sonra bir tarihse
                    if (seansTarihi.Date > DateTime.Now.Date ||
                        (seansTarihi.Date == DateTime.Now.Date && seansTarihi.TimeOfDay > DateTime.Now.TimeOfDay))
                    {
                        // Bu tarihe ait saatleri ekliyoruz
                        foreach (TimeSpan saat in seansSaatleri)
                        {
                            validSaatListesi.Add(saat);
                        }
                    }
                }
            }

            return validSaatListesi;
        }

        public void SeansEkle(string filmAdi, string salonAdi, string tarih, string seans)
        {
            if (string.IsNullOrEmpty(filmAdi) || string.IsNullOrEmpty(salonAdi) || string.IsNullOrEmpty(tarih) || string.IsNullOrEmpty(seans))
            {
                throw new ArgumentException("Film adı, salon adı, tarih ve seans boş olamaz!");
            }

            seansBilgileriDAL.SeansEkle(filmAdi, salonAdi, tarih, seans);
        }





    }
}
