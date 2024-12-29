using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CinemaApp.BLL;
using CinemaApp.Entity;

namespace CinemaApp.UI.WinForm
{
    public partial class frmSeans : Form
    {
        public SeansBilgileriBLL seansBilgileriBLL;
        public FilmBilgileriBLL filmBLL;
        private frmAnaSayfa anasayfa;
        public frmSeans(frmAnaSayfa gelenform)
        {
            InitializeComponent();
            seansBilgileriBLL = new SeansBilgileriBLL();
            filmBLL=new FilmBilgileriBLL();
            anasayfa = gelenform;
        }

        private string RadioButtonSeciliyse()
        {
            if (radioButton1.Checked) return radioButton1.Text;
            if (radioButton2.Checked) return radioButton2.Text;
            if (radioButton3.Checked) return radioButton3.Text;
            if (radioButton4.Checked) return radioButton4.Text;
            if (radioButton5.Checked) return radioButton5.Text;
            if (radioButton6.Checked) return radioButton6.Text;
            if (radioButton7.Checked) return radioButton7.Text;
            if (radioButton8.Checked) return radioButton8.Text;
            if (radioButton9.Checked) return radioButton9.Text;
            if (radioButton10.Checked) return radioButton10.Text;
            if (radioButton11.Checked) return radioButton11.Text;
            if (radioButton12.Checked) return radioButton12.Text;

            return ""; // Hiçbir radio button seçilmediyse boş döner.
        }

        private void button2_Click(object sender, EventArgs e)//Ana Sayfaya Dön
        {
            anasayfa.Show(); // Ana formu göster
            this.Close(); // Bu formu kapat
        }

        

        private void SeansEkleHandler()
        {
            try
            {
                string filmAdi = comboBox1.Text;
                string salonAdi = comboBox2.Text;
                string tarih = dateTimePicker1.Text;
                string seans = RadioButtonSeciliyse();

                if (string.IsNullOrEmpty(seans))
                {
                    MessageBox.Show("Lütfen bir seans seçin!");
                    return;
                }

                seansBilgileriBLL.SeansEkle(filmAdi, salonAdi, tarih, seans);

                MessageBox.Show("Seans başarıyla eklendi!");

                // Temizleme işlemleri
                comboBox1.Text = "";
                comboBox2.Text = "";
                dateTimePicker1.Text = DateTime.Now.ToShortDateString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }


        private void frmSeans_Load(object sender, EventArgs e)
        {
            try
            {
                // Film Adlarını Combobox1'e Getir
                List<string> filmAdlari = filmBLL.GetFilmAdlari();
                comboBox1.Items.AddRange(filmAdlari.ToArray());

                // Salon Adlarını Combobox2'ye Getir
                List<string> salonAdlari = filmBLL.GetSalonAdlari();
                comboBox2.Items.AddRange(salonAdlari.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Veri Yükleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)//Seans Ekle
        {
            SeansEkleHandler();
        }
    }
}
