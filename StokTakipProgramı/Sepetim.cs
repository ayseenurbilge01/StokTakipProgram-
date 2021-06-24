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
    public partial class Sepetim : Form
    {
        public Sepetim()
        {
            InitializeComponent();
        }

        String kullaniciadi;
        String kullaniciid;
        public Sepetim(String kullaniciadi)
        {
            InitializeComponent();
            this.kullaniciadi = kullaniciadi;
        }
        SqlConnection baglanti = new SqlConnection("Data Source=LAPTOP-EEFUFS2L;Initial Catalog=stoktakipprogrami;Integrated Security=True");

        private void btnSatınAl_Click(object sender, EventArgs e)
        {
            if (bakiye >= SepetToplamı)
            {
                kullaniciidsorgula();
                MessageBox.Show("Satın alma başarılı");
                dataGridView1.Columns.Clear();
                baglanti.Open();
                SqlCommand komut1 = new SqlCommand("Update kullanicilar set bakiye-=@bakiye where kullaniciid=@kullaniciid", baglanti);
                komut1.Parameters.AddWithValue("@bakiye", SepetToplamı);
                komut1.Parameters.AddWithValue("@kullaniciid", kullaniciid);
                komut1.ExecuteNonQuery();
                komut1.Dispose();
                SqlCommand komut = new SqlCommand("Delete from sepetim where kullaniciid=@kullaniciid",baglanti);
                komut.Parameters.AddWithValue("@kullaniciid",kullaniciid);
                komut.ExecuteNonQuery();
                komut.Dispose();
                
                baglanti.Close();

                KullanıcıUrunler ku = new KullanıcıUrunler(kullaniciadi);
                ku.Show();
                this.Hide();

            }
            else
                MessageBox.Show("Bakiye yetersiz");
        }
        int SepetToplamı = 0;
        int fiyat = 0;
        int urunadedi = 0;
        int urunkodu = 0;
        public void SepetToplam()
        {
            SepetToplamı = 0;
            kullaniciidsorgula();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("Select s.urunadedi,u.urunkodu,u.fiyat from sepetim s inner join urunler u on u.urunkodu=s.urunkodu where kullaniciid=@kullaniciid ",baglanti);
            komut.Parameters.AddWithValue("@kullaniciid",kullaniciid);
            SqlDataReader read = komut.ExecuteReader();
            while(read.Read())
            {
                urunadedi = Convert.ToInt16(read[0]);
                urunkodu = Convert.ToInt16(read[1]);
                fiyat= Convert.ToInt16(read[2]);
                SepetToplamı += fiyat * urunadedi;
                label1.Text ="Toplam = "+SepetToplamı.ToString();
            }
            read.Close();
            komut.Dispose();
            baglanti.Close();
        }
        private void Sepetim_Load(object sender, EventArgs e)
        {
            SepetToplam();
            kullaniciidsorgula();
            baglanti.Open();
            SqlCommand komut1 = new SqlCommand("Select s.sepetid,s.urunkodu,u.urunadi,u.fiyat,s.tarih,s.saat,s.urunadedi from sepetim s inner join urunler u on s.urunkodu=u.urunkodu where s.kullaniciid=@kullaniciid", baglanti);
            komut1.Parameters.AddWithValue("@kullaniciid", kullaniciid);
            SqlDataReader read = komut1.ExecuteReader();
            if(read.Read())
            {
                read.Close();
                DataTable tablo1 = new DataTable();
                SqlDataAdapter veri1 = new SqlDataAdapter(komut1);
                veri1.Fill(tablo1);
                dataGridView1.DataSource = tablo1;
                veri1.Dispose();
                tablo1.Dispose();
                komut1.Dispose();
                baglanti.Close();
            }
            else
            {
                baglanti.Close();
                MessageBox.Show("Sepetiniz boş :(");
                KullanıcıUrunler ku = new KullanıcıUrunler(kullaniciadi);
                ku.Show();
                this.Close();
            }
            
        }
        int bakiye = 0;
        private void kullaniciidsorgula()
        {
            baglanti.Open();
            SqlCommand komut1 = new SqlCommand("Select * from kullanicilar where kullaniciadi=@kullaniciadi", baglanti);
            komut1.Parameters.AddWithValue("@kullaniciadi", kullaniciadi);
            SqlDataReader read = komut1.ExecuteReader();
            if (read.Read())
            {
                kullaniciid = read[0].ToString();
                bakiye = Convert.ToInt32(read[3]);
            }
            baglanti.Close();
        }
        private void kayitgetir()
        {
            SqlCommand komut1 = new SqlCommand("Select s.sepetid,s.urunkodu,u.urunadi,u.fiyat,s.tarih,s.saat,s.urunadedi from sepetim s inner join urunler u on s.urunkodu=u.urunkodu where s.kullaniciid=@kullaniciid", baglanti);
            komut1.Parameters.AddWithValue("@kullaniciid", kullaniciid);
            DataTable tablo1 = new DataTable();
            SqlDataAdapter veri1 = new SqlDataAdapter(komut1);
            veri1.Fill(tablo1);
            dataGridView1.DataSource = tablo1;
            veri1.Dispose();
            tablo1.Dispose();
            
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btnGüncelle_Click(object sender, EventArgs e)
        {
            int secilensepetid;
            secilensepetid = Convert.ToInt16(dataGridView1.CurrentRow.Cells[0].Value);
            baglanti.Open();
            SqlCommand komut2 = new SqlCommand("UPDATE sepetim SET urunadedi=@adet WHERE sepetid=@sepetid ", baglanti);
            komut2.Parameters.AddWithValue("@sepetid", secilensepetid);
            komut2.Parameters.AddWithValue("@adet", numericUpDown1.Value);
            int i =komut2.ExecuteNonQuery();
            if (i == 1)
            {
                SqlCommand komut1 = new SqlCommand("Select * from sepetim where kullaniciid=@kullaniciid", baglanti);
                komut1.Parameters.AddWithValue("@kullaniciid", kullaniciid);
                DataTable tablo1 = new DataTable();
                SqlDataAdapter veri1 = new SqlDataAdapter(komut1);
                veri1.Fill(tablo1);
                dataGridView1.DataSource = tablo1;
                veri1.Dispose();
                tablo1.Dispose();
                komut2.Dispose();
                SqlDataReader read = komut1.ExecuteReader();
                if (read.Read())
                {
                    komut1.Dispose();
                    if (Convert.ToInt16(read[5]) == 0)
                    {
                        read.Close();
                        SqlCommand komut = new SqlCommand("Delete from sepetim where sepetid=@sepetid", baglanti);
                        komut.Parameters.AddWithValue("@sepetid", dataGridView1.CurrentRow.Cells[0].Value);
                        komut.ExecuteNonQuery();
                        komut.Dispose();
                    }
                    baglanti.Close();
                    kayitgetir();
                }
                else
                {
                    baglanti.Close();
                    kayitgetir();
                }
                    
            }
            else
            {
                baglanti.Close();
                kayitgetir();
            }

            SepetToplam();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("Delete from sepetim where sepetid=@sepetid",baglanti);
            komut.Parameters.AddWithValue("@sepetid",dataGridView1.CurrentRow.Cells[0].Value);
            komut.ExecuteNonQuery();
            komut.Dispose();
            SqlCommand komut1 = new SqlCommand("Update urunler set urunadedi+=@urunadedi where urunkodu=@urunkodu",baglanti);
            komut1.Parameters.AddWithValue("@urunadedi", dataGridView1.CurrentRow.Cells[6].Value);
            komut1.Parameters.AddWithValue("@urunkodu",dataGridView1.CurrentRow.Cells[1].Value);
            komut1.ExecuteNonQuery();
            komut1.Dispose();
            baglanti.Close();
            kayitgetir();
            SepetToplam();
        }

        private void btnGeriDon_Click(object sender, EventArgs e)
        {
            KullanıcıUrunler ku = new KullanıcıUrunler(kullaniciadi);
            ku.Show();
            this.Hide();
        }
    }
}
