using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Entity
{
    public class FilmBilgileri
    {
        public int FilmId { get; set; }
        public string FilmAdi { get; set; }
        public string Yonetmen { get; set; }
        public string FilmTuru { get; set; }
        public string FilmSuresi { get; set; }
        public string Tarih { get; set; }
        public string YapimYili { get; set; }
        public string Resim { get; set; }
        public string FilmUcreti { get; set; }
    }
}
