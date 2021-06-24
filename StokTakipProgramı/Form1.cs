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
    public partial class Urunler : Form
    {
        public Urunler()
        {
            InitializeComponent();
           
        }
        SqlConnection baglanti = new SqlConnection("Data Source=LAPTOP-EEFUFS2L;Initial Catalog=stoktakipprogrami;Integrated Security=True");
        String imagepath;
        string urunadi;
        byte[] resim;
        string dosyayolu;
        private void btnResimSec_Click(object sender, EventArgs e)
        {
            OpenFileDialog dosya = new OpenFileDialog();
            dosya.Filter = "Resim Dosyası |*.jpg;*.nef;*.png |  Tüm Dosyalar |*.*";
            dosya.ShowDialog();
            dosyayolu = dosya.FileName;
            pictureBox1.ImageLocation = dosyayolu;
        }
       
        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                urunadi = textBox1.Text;
            }
           
            Random rnd = new Random();
            String tarih = DateTime.Now.ToShortDateString();
            String saat = DateTime.Now.ToShortTimeString();
            int urunkodu = rnd.Next(1000);
            if(imagepath!=null)
            {
                FileStream fileStream = new FileStream(imagepath, FileMode.Open, FileAccess.Read);
                BinaryReader binaryReader = new BinaryReader(fileStream);
                resim = binaryReader.ReadBytes((int)fileStream.Length);
                binaryReader.Close();
                fileStream.Close();
            }
            baglanti.Open();
            SqlCommand komut1 = new SqlCommand("Select * from urunler where urunadi=@urunadi and cinsiyet=@cinsiyet",baglanti);
            komut1.Parameters.AddWithValue("@urunadi",urunadi);
            komut1.Parameters.AddWithValue("@cinsiyet",comboBox2.Text);
            SqlDataReader read = komut1.ExecuteReader();
            if(read.Read())
            {
                komut1.Dispose();
                read.Close();
                SqlCommand komut2 = new SqlCommand("UPDATE urunler SET urunadedi+=@adet WHERE urunadi=@urunadi and cinsiyet=@cinsiyet", baglanti);
                komut2.Parameters.AddWithValue("@urunadi", urunadi);
                komut2.Parameters.AddWithValue("@cinsiyet", comboBox2.Text);
                komut2.Parameters.AddWithValue("@adet",numericUpDown1.Value);
                komut2.ExecuteNonQuery();
                komut2.Dispose();
               
            }
            else
            {
                komut1.Dispose();
                read.Close();
                SqlCommand komut = new SqlCommand("insert into urunler(urunkodu,urunadi,urunadedi,tarih,saat,resim,cinsiyet,fiyat) values(@urunkodu,@urunadi,@urunadedi,@tarih,@saat,@images,@cinsiyet,@fiyat)", baglanti);
                komut.Parameters.Add("@urunkodu", urunkodu);
                komut.Parameters.Add("@urunadi", urunadi.ToUpper());
                komut.Parameters.Add("@urunadedi", numericUpDown1.Value.ToString());
                komut.Parameters.Add("@tarih", DateTime.Now);
                komut.Parameters.Add("@saat", DateTime.Now.TimeOfDay);
                if (!String.IsNullOrEmpty(dosyayolu))
                    komut.Parameters.Add("@images", dosyayolu);
                else
                    komut.Parameters.Add("@images",DBNull.Value);
                komut.Parameters.Add("@cinsiyet", comboBox2.Text);
                komut.Parameters.Add("@fiyat",textBox2.Text);
                komut.ExecuteNonQuery();
                komut.Dispose();
         
            }
            baglanti.Close();
            kayitgetir();
            comboBox1_urunYerlestir();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            kayitgetir();
            comboBox1_urunYerlestir();
        }
        private void comboBox1_urunYerlestir()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("Select * from urunler", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if(!comboBox1.Items.Contains(read[1]))
                    comboBox1.Items.Add(read[1]);
            }
            baglanti.Close();
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
            baglanti.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            urunadi = comboBox1.SelectedItem.ToString();
        }

        private void btnCikar_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell drow in dataGridView1.SelectedCells)  
            {
                 int numara = Convert.ToInt32(drow.OwningRow.Cells[0].Value);
                 KayıtSil(numara);
            }
        }

        private void KayıtSil(int numara)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("DELETE FROM urunler WHERE urunkodu=@kod", baglanti);
            komut.Parameters.AddWithValue("@kod", numara);
            komut.ExecuteNonQuery();
            baglanti.Close();
            kayitgetir();
            
        }


        private void btnGuncelle_Click(object sender, EventArgs e)
        {

            String tarih = DateTime.Now.ToShortDateString();
            String saat = DateTime.Now.ToShortTimeString();
            baglanti.Open();
                SqlCommand komut = new SqlCommand("Update urunler Set urunadi=@Ad,urunadedi=@adet,resim=@Resim,fiyat=@fiyat,tarih=@tarih,saat=@saat,cinsiyet=@cinsiyet Where urunkodu=@Id", baglanti);
            if (!String.IsNullOrEmpty(textBox1.Text))
                komut.Parameters.AddWithValue("@Ad", textBox1.Text);
            else
                komut.Parameters.AddWithValue("@Ad", dataGridView1.CurrentRow.Cells[1].Value);
            if (!String.IsNullOrEmpty(numericUpDown1.Value.ToString()))
                komut.Parameters.AddWithValue("@adet", numericUpDown1.Value.ToString());
            else
                komut.Parameters.AddWithValue("@adet", dataGridView1.CurrentRow.Cells[2].Value);
            if (!String.IsNullOrEmpty(dosyayolu))
                komut.Parameters.AddWithValue("@Resim", dosyayolu);
            else
                komut.Parameters.AddWithValue("@Resim", dataGridView1.CurrentRow.Cells[5].Value);
            if (!String.IsNullOrEmpty(comboBox2.Text))
                komut.Parameters.AddWithValue("@cinsiyet", comboBox2.Text);
            else
                komut.Parameters.AddWithValue("@cinsiyet",DBNull.Value);
            if (!String.IsNullOrEmpty(textBox2.Text))
                komut.Parameters.AddWithValue("@fiyat",textBox2.Text);
            else
                komut.Parameters.AddWithValue("@fiyat", DBNull.Value);
            komut.Parameters.AddWithValue("@Id", (dataGridView1.CurrentRow.Cells[0].Value));
            komut.Parameters.AddWithValue("@tarih",tarih);
            komut.Parameters.AddWithValue("@saat",saat);
            komut.ExecuteNonQuery();
            baglanti.Close();
            kayitgetir();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            pictureBox1.ImageLocation = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            comboBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            numericUpDown1.Value = Convert.ToInt16(dataGridView1.CurrentRow.Cells[2].Value);
            comboBox2.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            textBox2.Text =dataGridView1.CurrentRow.Cells[7].Value.ToString();
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            Giris g = new Giris();
            g.Show();
            this.Close();
        }
    }
}
