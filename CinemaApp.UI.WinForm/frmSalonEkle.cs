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
    public partial class frmSalonEkle : Form
    {
        public SalonBilgileriBLL salonBLL;
        private frmAnaSayfa anasayfa;
        public frmSalonEkle(frmAnaSayfa gelenform)
        {
            InitializeComponent();
            salonBLL=new SalonBilgileriBLL();
            anasayfa = gelenform;
        }

        private void AddSalonToDatabase()
        {
            try
            {
                // TextBox'tan salon adını alıyoruz
                string salonAdi = textBox1.Text.Trim();

                // BLL katmanına gönderiyoruz
                salonBLL.AddSalon(salonAdi);

                // Kullanıcıya başarılı mesajı gösteriliyor
                MessageBox.Show("Salon başarıyla eklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // TextBox'ı temizleme
                textBox1.Clear();
            }
            catch (Exception ex)
            {
                // Kullanıcıya hata mesajı gösteriliyor
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)//Ana Sayfaya Dön
        {
            anasayfa.Show(); // Ana formu göster
            this.Close(); // Bu formu kapat
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddSalonToDatabase();
        }
    }
}
