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
    public partial class Form1 : Form
    {
        public FilmBilgileriBLL filmBLL;
        private frmAnaSayfa anasayfa;
        public Form1(frmAnaSayfa gelenform)
        {
            InitializeComponent();
            filmBLL = new FilmBilgileriBLL();
            anasayfa = gelenform;
        }

        // Veritabanından verileri alıp DataGridView'e yükleyecek metod
        private void VerileriOku()
        {
            try
            {
                // BLL'den film bilgilerini al
                List<FilmBilgileri> filmListesi = filmBLL.FilmBilgileriGetir();

                // DataGridView için veri kaynağı ayarlama
                dataGridView1.DataSource = filmListesi;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }


        // Film güncelleme işlemi
        private void FilmGuncelle()
        {
            try
            {
                // Film Adı boşsa, kullanıcıya hata mesajı ver
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("Lütfen güncellenecek Film Adını giriniz!");
                    return;
                }

                // FilmBilgileri nesnesi oluştur
                FilmBilgileri film = new FilmBilgileri
                {
                    FilmAdi = textBox1.Text,
                    Yonetmen = textBox2.Text,
                    FilmTuru = comboBox1.Text,
                    FilmSuresi = textBox3.Text,
                    Tarih = dateTimePicker1.Text,
                    YapimYili = textBox4.Text,
                    Resim = pictureBox1.ImageLocation,
                    FilmUcreti = textBox5.Text
                };

                // BLL üzerinden güncelleme işlemi yap
                bool isUpdated = filmBLL.FilmGuncelle(film);

                if (isUpdated)
                {
                    MessageBox.Show("Kayıt başarıyla güncellendi!");
                }
                else
                {
                    MessageBox.Show("Film bulunamadı ya da güncellenemedi!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        // Film silme işlemi
        private void FilmSil()
        {
            try
            {
                // Film Adı boşsa, kullanıcıya hata mesajı ver
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("Silmek için Film Adını giriniz!");
                    return;
                }

                // BLL üzerinden film silme işlemi yap
                bool isDeleted = filmBLL.FilmSil(textBox1.Text);

                if (isDeleted)
                {
                    MessageBox.Show("Kayıt başarıyla silindi!");
                }
                else
                {
                    MessageBox.Show("Film bulunamadı ya da silinemedi!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)//Film Görüntüle
        {
            VerileriOku();
        }

        private void button1_Click(object sender, EventArgs e)//Film Ekle
        {
            FilmBilgileri film = new FilmBilgileri
            {
                FilmAdi = textBox1.Text,
                Yonetmen = textBox2.Text,
                FilmTuru = comboBox1.Text,
                FilmSuresi = textBox3.Text,
                Tarih = dateTimePicker1.Text,
                YapimYili = textBox4.Text,
                Resim = pictureBox1.ImageLocation,
                FilmUcreti = textBox5.Text
            };

            //filmBLL.FilmEkle(film);

            try
            {
                filmBLL.FilmEkle(film); 
                MessageBox.Show("Film başarıyla eklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya mesajı gösteriyoruz
                MessageBox.Show("Film eklenirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)//Afiş Seç
        {
            openFileDialog1.ShowDialog();
            pictureBox1.ImageLocation = openFileDialog1.FileName;
        }

        private void button2_Click(object sender, EventArgs e)//Film Güncelle
        {
            FilmGuncelle();
        }

        private void button3_Click(object sender, EventArgs e)//Film Sil
        {
            FilmSil();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Eğer bir satıra tıklandıysa
            if (e.RowIndex >= 0)
            {
                // Seçili satırı al
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // TextBox, ComboBox, PictureBox gibi bileşenlere verileri aktar
                textBox1.Text = row.Cells["FilmAdi"].Value?.ToString() ?? string.Empty;
                textBox2.Text = row.Cells["Yonetmen"].Value?.ToString() ?? string.Empty;
                comboBox1.Text = row.Cells["FilmTuru"].Value?.ToString() ?? string.Empty;
                textBox3.Text = row.Cells["FilmSuresi"].Value?.ToString() ?? string.Empty;
                dateTimePicker1.Text = row.Cells["Tarih"].Value?.ToString() ?? string.Empty;
                textBox4.Text = row.Cells["YapimYili"].Value?.ToString() ?? string.Empty;

                pictureBox1.ImageLocation = row.Cells["Resim"].Value?.ToString() ?? string.Empty;
                textBox5.Text = row.Cells["FilmUcreti"].Value?.ToString() ?? string.Empty;
            }
        }

        private void button6_Click(object sender, EventArgs e)//Ana Sayfaya Dön
        {
            anasayfa.Show(); // Ana formu göster
            this.Close(); // Bu formu kapat
        }
    }
}
