using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Entity
{
    public class SatisBilgileri
    {
        public int SatısId { get; set; }
        public string KoltukNo { get; set; }
        public string SalonAdi { get; set; }
        public string FilmAdi { get; set; }
        public string Tarih { get; set; }
        public string Saat { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Ucret { get; set; }
        public string SatilmaTarihi { get; set; }
    }
}
