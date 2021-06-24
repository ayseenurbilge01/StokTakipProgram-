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
    public partial class KullanıcıUrunler : Form
    {
        public KullanıcıUrunler()
        {
            InitializeComponent();
        }
        String kullaniciadi;
        public KullanıcıUrunler(String kullaniciadi)
        {
            InitializeComponent();
            this.kullaniciadi = kullaniciadi;
        }

        SqlConnection baglanti = new SqlConnection("Data Source=LAPTOP-EEFUFS2L;Initial Catalog=stoktakipprogrami;Integrated Security=True");
        
        private void KullanıcıUrunler_Load(object sender, EventArgs e)
        {
            kayitgetir();
        }
        
        private void kayitgetir()
        {
            baglanti.Open();
            string urunler = "Select * from urunler";
            SqlCommand komut1 = new SqlCommand(urunler, baglanti);
            DataTable tablo1 = new DataTable();
            SqlDataAdapter veri1 = new SqlDataAdapter(komut1);
            veri1.Fill(tablo1);
            dataGridView1.DataSource = tablo1;
            veri1.Dispose();
            tablo1.Dispose();
            komut1.Dispose();
            baglanti.Close();
        }

        private void btnGoster_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty( textBox1.Text))
            {
                MessageBox.Show("Lütfen bir değer girin.");
            }
            else
            {
                baglanti.Open();
                SqlCommand komut1 = new SqlCommand("Select * from urunler where urunadi=@urunadi", baglanti);
                komut1.Parameters.AddWithValue("@urunadi", textBox1.Text.ToUpper());
                SqlDataReader read = komut1.ExecuteReader();
                if (read.Read())
                {
                    read.Close();
                    DataTable tablo1 = new DataTable();
                    SqlDataAdapter veri1 = new SqlDataAdapter(komut1);
                    veri1.Fill(tablo1);
                    dataGridView1.DataSource = tablo1;
                }
                else
                {
                    MessageBox.Show("Aradığınız ürün bulunamadı.");
                }
                baglanti.Close();
            }
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            pictureBox1.ImageLocation = dataGridView1.CurrentRow.Cells[5].Value.ToString();
        }

        private void btnBakiyeSorgula_Click(object sender, EventArgs e)
        {
            BakiyeYükle by = new BakiyeYükle(kullaniciadi);
            by.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            kayitgetir();
        }

        private void btnSepetim_Click(object sender, EventArgs e)
        {
            Sepetim s = new Sepetim(kullaniciadi);
            s.Show();
            this.Hide();
        }
        int kullaniciid;
        private void kullaniciidsorgula()
        {
            baglanti.Open();
            SqlCommand komut1 = new SqlCommand("Select * from kullanicilar where kullaniciadi=@kullaniciadi", baglanti);
            komut1.Parameters.AddWithValue("@kullaniciadi", kullaniciadi);
            SqlDataReader read = komut1.ExecuteReader();
            if (read.Read())
            {
                kullaniciid = Convert.ToInt16(read[0]);
            }
            baglanti.Close();
        }

        private void btnSepeteEkle_Click(object sender, EventArgs e)
        {
            Sepetim s = new Sepetim(kullaniciadi);
            s.SepetToplam();
            int secilenurununadedi = Convert.ToInt16(dataGridView1.CurrentRow.Cells[2].Value);
            if(secilenurununadedi ==0)
            {
                MessageBox.Show("Maalesef, ürün stokta kalmadı.");

            }
            else if(secilenurununadedi<numericUpDown1.Value)
            {
                MessageBox.Show("Stokta istediğiniz kadar ürün bulunmamakta.");
            }
            else if(numericUpDown1.Value==0)
            {
                MessageBox.Show("Lütfen geçerli bir adet miktarı giriniz.");
            }
            else
            {
                kullaniciidsorgula();
                baglanti.Open();
                SqlCommand komut1 = new SqlCommand("Select * from sepetim where kullaniciid=@kullaniciid and urunkodu=@urunkodu", baglanti);
                komut1.Parameters.AddWithValue("@kullaniciid", kullaniciid);
                komut1.Parameters.AddWithValue("@urunkodu", dataGridView1.CurrentRow.Cells[0].Value);
                SqlDataReader read = komut1.ExecuteReader();
                if(read.Read())
                {
                    read.Close();
                    SqlCommand komut3 = new SqlCommand("UPDATE sepetim SET urunadedi+=@adet WHERE kullaniciid=@kullaniciid and urunkodu=@urunkodu ", baglanti);
                    komut3.Parameters.AddWithValue("@kullaniciid", kullaniciid);
                    komut3.Parameters.AddWithValue("@urunkodu", dataGridView1.CurrentRow.Cells[0].Value);
                    komut3.Parameters.AddWithValue("@adet",numericUpDown1.Value);
                    int i =komut3.ExecuteNonQuery();
                    if (i == 1)
                    {
                        MessageBox.Show("Ürün sepete eklendi.");
                    }
                    komut3.Dispose();
                }
                else
                {
                    read.Close();
                    komut1.Dispose();
                    SqlCommand komut = new SqlCommand("insert into sepetim(kullaniciid,urunkodu,tarih,saat,urunadedi) values (@kullaniciid,@urunkodu,@tarih,@saat,@urunadedi)", baglanti);
                    komut.Parameters.AddWithValue("@kullaniciid", kullaniciid);
                    komut.Parameters.AddWithValue("@urunkodu", dataGridView1.CurrentRow.Cells[0].Value);
                    komut.Parameters.AddWithValue("@tarih", DateTime.Now);
                    komut.Parameters.AddWithValue("@saat", DateTime.Now.TimeOfDay);
                    komut.Parameters.AddWithValue("@urunadedi", numericUpDown1.Value);
                    int i = komut.ExecuteNonQuery();
                    if (i == 1)
                    {
                        komut.Dispose();
                        SqlCommand komut2 = new SqlCommand("UPDATE urunler SET urunadedi-=1 WHERE urunkodu=@urunkodu ", baglanti);//güncellenecek
                        komut2.Parameters.AddWithValue("@urunkodu", dataGridView1.CurrentRow.Cells[0].Value);
                        komut2.ExecuteNonQuery();
                        komut2.Dispose();
                        MessageBox.Show("Ürün sepete eklendi.");
                    }
                    
                    
                }
                baglanti.Close();
                kayitgetir();
            }
            
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
