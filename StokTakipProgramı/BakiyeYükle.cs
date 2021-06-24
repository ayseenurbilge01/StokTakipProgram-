using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace StokTakipProgramı
{
    public partial class BakiyeYükle : Form
    {
        public BakiyeYükle()
        {
            InitializeComponent();
        }
         String kullaniciadi;
        public BakiyeYükle(String kullaniciadi)
        {
            this.kullaniciadi = kullaniciadi;
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=LAPTOP-EEFUFS2L;Initial Catalog=stoktakipprogrami;Integrated Security=True");
        

        private void BakiyeYükle_Load(object sender, EventArgs e)
        {
            
            textBox1.Enabled = false;
            baglanti.Open();
            SqlCommand komut1 = new SqlCommand("Select * from kullanicilar where kullaniciadi=@kullaniciadi", baglanti);
            komut1.Parameters.AddWithValue("@kullaniciadi", kullaniciadi);
            SqlDataReader read = komut1.ExecuteReader();
            if (read.Read())
            {
                textBox1.Text = read[3].ToString();
            }
            komut1.Dispose();
            read.Close();
            baglanti.Close();
        }

        private void btnBakiyeYükle_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(textBox2.Text))
                {
                    MessageBox.Show("Lütfen bir bakiye değeri giriniz.");
                }
                else if (Convert.ToInt16(textBox2.Text) <= 0)
                {
                    MessageBox.Show("Lütfen geçerli bir bakiye yükleme değeri giriniz.");
                }
                else
                {
                    baglanti.Open();
                    SqlCommand komut2 = new SqlCommand("UPDATE kullanicilar SET bakiye+=@bakiye WHERE kullaniciadi=@kullaniciadi ", baglanti);
                    komut2.Parameters.AddWithValue("@bakiye", Convert.ToInt16(textBox2.Text));
                    komut2.Parameters.AddWithValue("@kullaniciadi", kullaniciadi);
                    int i =komut2.ExecuteNonQuery();
                    if(i==1)
                    {
                        komut2.Dispose();
                        MessageBox.Show("Bakiye Güncelleme Başarılı");
                        SqlCommand komut1 = new SqlCommand("Select * from kullanicilar where kullaniciadi=@kullaniciadi", baglanti);
                        komut1.Parameters.AddWithValue("@kullaniciadi", kullaniciadi);
                        SqlDataReader read = komut1.ExecuteReader();
                        if (read.Read())
                        {
                            textBox1.Text = read[3].ToString();
                        }
                        komut1.Dispose();
                        read.Close();
                    }
                    baglanti.Close();
                    
                }
            }
            catch(FormatException hata)
            {
                MessageBox.Show("Hatalı türde veri girişi yaptınız."+hata.ToString());
            }
           
        }

        private void BakiyeYükle_Load()
        {
        }

        private void btnGeriDon_Click(object sender, EventArgs e)
        {
            KullanıcıUrunler ku = new KullanıcıUrunler(kullaniciadi);
            ku.Show();
            this.Hide();
        }
    }
}
