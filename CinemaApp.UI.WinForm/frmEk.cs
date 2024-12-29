using CinemaApp.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CinemaApp.Entity;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Net.Mail;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CinemaApp.UI.WinForm
{
    public partial class frmEk : Form
    {
        public SatisBilgileriBLL satisBilgileriBLL;
        private frmAnaSayfa anasayfa;
        public frmEk(frmAnaSayfa gelenform)
        {
            InitializeComponent();
            satisBilgileriBLL = new SatisBilgileriBLL();
            anasayfa = gelenform;
        }

        private void FilmlereGoreBiletGrafik()
        {
            try
            {
                // BLL katmanından bilet sayıları verilerini alıyoruz
                List<(string FilmAdi, int BiletSayisi)> biletSayilari = satisBilgileriBLL.GetBiletSayilari();

                // Chart'ı temizle
                chart1.Series.Clear();

                // Yeni bir seri oluştur
                var series = chart1.Series.Add("Bilet Sayısı");
                series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column; // Sütun grafik
                series.IsValueShownAsLabel = true; // Değerleri göster

                // Verileri Chart kontrolüne ekle
                foreach (var bilet in biletSayilari)
                {
                    // Chart'a veri ekle
                    series.Points.AddXY(bilet.FilmAdi, bilet.BiletSayisi);
                }

                // Chart ayarları
                chart1.ChartAreas[0].AxisX.Title = "Filmler";
                chart1.ChartAreas[0].AxisY.Title = "Bilet Sayısı";
                chart1.ChartAreas[0].AxisX.Interval = 1; // X ekseni düzgün görünmesi için

                MessageBox.Show("Grafik başarıyla oluşturuldu!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void VerileriOku()
        {
            try
            {
                // BLL katmanından verileri al
                var satisBilgileri = satisBilgileriBLL.GetSatisBilgileri();

                // DataGridView'e veri yükle
                dataGridView1.DataSource = satisBilgileri;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }


        private void button4_Click(object sender, EventArgs e)//Ana Sayfaya Dön
        {
            anasayfa.Show(); // Ana formu göster
            this.Close(); // Bu formu kapat
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FilmlereGoreBiletGrafik();
        }

        private void button2_Click(object sender, EventArgs e)//PDF oluştur ve Veri Göster
        {
            VerileriOku();
            // PDF oluştur
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Dosyası|*.pdf";
            saveFileDialog.Title = "PDF Kaydet";
            saveFileDialog.FileName = "ÜrünListesi.pdf";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 10f, 10f);
                        iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();

                        PdfPTable table = new PdfPTable(dataGridView1.Columns.Count);
                        table.WidthPercentage = 100;

                        // Sütun başlıklarını ekleme
                        foreach (DataGridViewColumn column in dataGridView1.Columns)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);
                        }

                        // Verileri ekleme
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                table.AddCell(cell.Value?.ToString() ?? "");
                            }
                        }

                        pdfDoc.Add(table);
                        pdfDoc.Close();
                        stream.Close();
                    }

                    MessageBox.Show("PDF başarıyla oluşturuldu!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("PDF oluşturulurken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SendSalesInfoByEmail()
        {
            try
            {
                string recipientEmail = textBox1.Text.Trim();
                if (string.IsNullOrEmpty(recipientEmail))
                {
                    MessageBox.Show("Lütfen bir e-posta adresi girin.");
                    return;
                }

                // Satış bilgilerini al
                List<SatisBilgileri> salesInfoList = satisBilgileriBLL.GetSalesInfo();

                // Mailin içeriği için bir StringBuilder oluştur
                StringBuilder mailBody = new StringBuilder();
                mailBody.AppendLine("Satış Bilgileri:");

                // Verileri mail gövdesine ekle
                foreach (var sale in salesInfoList)
                {
                    mailBody.AppendLine($"Satış ID: {sale.SatısId}");
                    mailBody.AppendLine($"Koltuk No: {sale.KoltukNo}");
                    mailBody.AppendLine($"Salon Adı: {sale.SalonAdi}");
                    mailBody.AppendLine($"Film Adı: {sale.FilmAdi}");
                    mailBody.AppendLine($"Tarih: {sale.Tarih}");
                    mailBody.AppendLine($"Saat: {sale.Saat}");
                    mailBody.AppendLine($"Ad: {sale.Ad}");
                    mailBody.AppendLine($"Soyad: {sale.Soyad}");
                    mailBody.AppendLine($"Ücret: {sale.Ucret}");
                    mailBody.AppendLine($"Satılma Tarihi: {sale.SatilmaTarihi}");
                    mailBody.AppendLine(); // Satır arası boşluk
                }

                // Mail mesajını oluştur
                MailMessage mm = new MailMessage("gorselmail3434@gmail.com", recipientEmail);
                mm.Subject = "Satış Bilgileri";
                mm.Body = mailBody.ToString();

                // SMTP ayarlarını yap
                SmtpClient smtpClient = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Credentials = new System.Net.NetworkCredential("gorselmail3434@gmail.com", "gljq rqxy qhii gjpz"),
                    EnableSsl = true
                };

                // E-postayı gönder
                smtpClient.Send(mm);

                MessageBox.Show("Satış bilgileri başarıyla gönderildi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message + "\n" + ex.StackTrace);
            }
        }


        private void button3_Click(object sender, EventArgs e)//Mail At
        {
            SendSalesInfoByEmail();
        }
    }
}
