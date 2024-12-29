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
    public partial class frmSatısListele : Form
    {
        public SatisBilgileriBLL satisBilgileriBLL;
        private frmAnaSayfa anasayfa;
        public frmSatısListele(frmAnaSayfa gelenform)
        {
            InitializeComponent();
            satisBilgileriBLL = new SatisBilgileriBLL();
            anasayfa = gelenform;
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

        private void button2_Click(object sender, EventArgs e)//Ana Sayfaya Dön
        {
            anasayfa.Show(); // Ana formu göster
            this.Close(); // Bu formu kapat
        }

        private void button1_Click(object sender, EventArgs e)//Satış Listele
        {
            VerileriOku();
        }
    }
}
