using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaApp.DAL;
using CinemaApp.Entity;

namespace CinemaApp.BLL
{
    public class SalonBilgileriBLL
    {
        private SalonBilgileriDAL salonDAL;

        public SalonBilgileriBLL() 
        {
            salonDAL=new SalonBilgileriDAL();
        }

        public void AddSalon(string salonAdi)
        {
            // İş mantığı: Boş veya geçersiz giriş kontrolü
            if (string.IsNullOrWhiteSpace(salonAdi))
            {
                throw new Exception("Salon adı boş olamaz! Lütfen geçerli bir salon adı giriniz.");
            }

            try
            {
                // DAL'deki metodu çağırarak veritabanına ekleme işlemi
                salonDAL.AddSalon(salonAdi);
            }
            catch (Exception ex)
            {
                // Hata oluşursa UI'ye anlamlı bir hata mesajı gönderiyoruz
                throw new Exception("Salon ekleme işlemi sırasında bir hata oluştu: " + ex.Message);
            }
        }
    }
}
