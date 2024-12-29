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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CinemaApp.UI.WinForm
{
    public partial class frmAnaSayfa : Form
    {
        public FilmBilgileriBLL filmBLL;
        public frmAnaSayfa()
        {
            InitializeComponent();
            filmBLL = new FilmBilgileriBLL();
        }
        int sayac = 0;

        private void FilmAfisGoster()
        {
            try
            {
                // Seçilen film adını al
                string filmAdi = comboBox1.Text;

                // BLL katmanından afiş yolunu al
                string afisYolu = filmBLL.GetFilmAfis(filmAdi);

                // Afişi göster
                pictureBox1.ImageLocation = afisYolu;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya mesaj göster
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Film_Saati_Getir()
        {
            // ComboBox'ları temizliyoruz
            comboBox4.Items.Clear();

            // BLL katmanından gerekli veriyi alıyoruz
            SeansBilgileriBLL seansBilgileriBLL = new SeansBilgileriBLL();
            List<TimeSpan> seansSaatleri = seansBilgileriBLL.GetSeansSaatleri(comboBox1.Text, comboBox2.Text);

            // Alınan seans saatlerini ComboBox'a ekliyoruz
            foreach (TimeSpan seansSaati in seansSaatleri)
            {
                if (!comboBox4.Items.Contains(seansSaati.ToString()))
                {
                    comboBox4.Items.Add(seansSaati.ToString());
                }
            }
        }



        private void Bos_Koltuklar()
        {
            sayac = 1;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    System.Windows.Forms.Button btn = new System.Windows.Forms.Button();
                    btn.Size = new Size(30, 30);
                    btn.BackColor = Color.White;
                    btn.Location = new Point(j * 30 + 20, i * 30 + 30);
                    btn.Name = sayac.ToString();
                    btn.Text = sayac.ToString();
                    if (j == 4)
                    {
                        continue;
                    }
                    sayac++;
                    this.panel1.Controls.Add(btn);
                    btn.Click += btn_Click;

                }
            }
        }

        private void btn_Click(object sender, EventArgs e)//textbox1 enabled false olacak butona basınca koltuk no getiriyor 
        {
            System.Windows.Forms.Button b = (System.Windows.Forms.Button)sender;
            if (b.BackColor == Color.White)
            {
                textBox1.Text = b.Text;
            }
        }

        private void Combo_Dolu_Koltuklar()
        {
            comboBox5.Items.Clear();
            comboBox5.Text = "";
            foreach (Control item in panel1.Controls)
            {
                if (item is System.Windows.Forms.Button)
                {
                    if (item.BackColor == Color.Red)
                    {
                        comboBox5.Items.Add(item.Text);//bilet iptal koltuk no combobox'u
                    }
                }
            }

        }

        private void YenidenRenklendir()
        {
            foreach (Control item in panel1.Controls)
            {
                if (item is System.Windows.Forms.Button)
                {
                    item.BackColor = Color.White;
                }
            }
        }


        private void Film_Ucreti_Getir()
        {
            try
            {
                // BLL katmanını kullanıyoruz
                FilmBilgileriBLL filmBilgileriBLL = new FilmBilgileriBLL();

                // ComboBox'dan seçilen film adını alıyoruz
                string filmAdi = comboBox1.Text;

                // Film ücretini alıyoruz
                decimal? filmUcreti = filmBilgileriBLL.GetFilmUcreti(filmAdi);

                // Eğer ücret varsa TextBox'a yazıyoruz, yoksa boş bırakıyoruz
                textBox4.Text = filmUcreti.HasValue ? filmUcreti.Value.ToString("C") : "Ücret Bulunamadı";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }



        private void Veritabani_Dolu_Koltuklar()
        {
            try
            {
                // BLL katmanını çağırıyoruz
                SatisBilgileriBLL satisBilgileriBLL = new SatisBilgileriBLL();

                // ComboBox'lardan seçili değerleri alıyoruz
                string filmAdi = comboBox1.Text;
                string salonAdi = comboBox2.Text;
                string tarih = comboBox3.Text;
                string saat = comboBox4.Text;

                // Dolu koltukları alıyoruz
                List<string> doluKoltuklar = satisBilgileriBLL.GetDoluKoltuklar(filmAdi, salonAdi, tarih, saat);

                // Paneldeki butonları renklendiriyoruz
                foreach (Control item in panel1.Controls)
                {
                    if (item is System.Windows.Forms.Button button && doluKoltuklar.Contains(button.Text))
                    {
                        button.BackColor = Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

       

        private void BiletSat()
        {
            try
            {
                // BLL sınıfını çağırıyoruz
                SatisBilgileriBLL satisBilgileriBLL = new SatisBilgileriBLL();

                // Kullanıcıdan alınan değerleri okuyoruz
                string filmAdi = comboBox1.Text;
                string salonAdi = comboBox2.Text;
                string tarih = comboBox3.Text;
                string saat = comboBox4.Text;
                string koltukNo = textBox1.Text;
                string ad = textBox2.Text;
                string soyad = textBox3.Text;
                string ucret = textBox4.Text;

                // BLL katmanına veri gönderiyoruz
                satisBilgileriBLL.BiletSat(filmAdi, salonAdi, tarih, saat, koltukNo, ad, soyad, ucret);

                // Başarılı mesajı
                MessageBox.Show("Kayıt başarıyla eklendi!");
                
            }
            catch (ArgumentException ex)
            {
                // Doğrulama hatası
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                // Genel hata
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void BiletIptali()
        {
            try
            {
                // BLL sınıfını çağırıyoruz
                SatisBilgileriBLL satisBilgileriBLL = new SatisBilgileriBLL();

                // Kullanıcıdan alınan koltuk numarası
                string koltukNo = comboBox5.SelectedItem?.ToString();

                // BLL katmanına gönderiyoruz
                satisBilgileriBLL.BiletIptali(koltukNo);

                // Başarılı mesajı
                MessageBox.Show("Bilet başarıyla iptal edildi.");
            }
            catch (ArgumentException ex)
            {
                // Doğrulama hatası
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                // Genel hata
                MessageBox.Show("Hata: " + ex.Message);
            }
        }









        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)//film adı değişince
        {
            FilmAfisGoster();
            YenidenRenklendir();
            Combo_Dolu_Koltuklar();
            Film_Ucreti_Getir();
        }

        private void frmAnaSayfa_Load(object sender, EventArgs e)
        {

            Bos_Koltuklar();
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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)//salon adı değişince
        {
            // İş Mantığı Katmanından tarihleri getiriyoruz
            SeansBilgileriBLL seansBilgileriBLL = new SeansBilgileriBLL();

            try
            {
                string filmAdi = comboBox1.Text;
                string salonAdi = comboBox2.Text;

                // Tarihleri al ve ComboBox'a ekle
                List<string> tarihListesi = seansBilgileriBLL.GetValidSeansTarihleri(filmAdi, salonAdi);

                comboBox3.Items.Clear();
                comboBox3.Items.AddRange(tarihListesi.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }

            Film_Saati_Getir();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)//seans comboboxı değişince
        {
            YenidenRenklendir();
            Veritabani_Dolu_Koltuklar();
            Combo_Dolu_Koltuklar();
            
        }

        private void button7_Click(object sender, EventArgs e)//Bilet Sat
        {
            BiletSat();
            YenidenRenklendir();
            Veritabani_Dolu_Koltuklar();
            Combo_Dolu_Koltuklar();
        }

        private void button8_Click(object sender, EventArgs e)//Bilet iptal
        {
            BiletIptali();
            YenidenRenklendir();
            Veritabani_Dolu_Koltuklar();
            Combo_Dolu_Koltuklar();
        }

        private void button2_Click(object sender, EventArgs e)//Film işlemlerine gider
        {
            Form1 ekle = new Form1(this);
            ekle.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)//Ek işlemlere gider
        {
            frmEk ekle= new frmEk(this);
            ekle.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmSeans ekle = new frmSeans(this);
            ekle.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmSatısListele ekle=new frmSatısListele(this);
            ekle.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmSalonEkle ekle=new frmSalonEkle(this);
            ekle.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)//Çıkış
        {
            Application.Exit();
        }
    }
}
