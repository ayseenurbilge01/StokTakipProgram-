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
    public partial class Giris : Form
    {
        public Giris()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=LAPTOP-EEFUFS2L;Initial Catalog=stoktakipprogrami;Integrated Security=True");

        private void Giris_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            Kayit k = new Kayit();
            k.Show();
            this.Hide();

        }

        private void btnGirisYap_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "Yonetici" && textBox2.Text == "Sifre123") 
            {
                Urunler u = new Urunler();
                u.Show();
                this.Hide();
            }
            else
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("Select * from kullanicilar where kullaniciadi=@kullaniciadi and sifre=@sifre", baglanti);
                komut.Parameters.AddWithValue("@kullaniciadi", textBox1.Text);
                komut.Parameters.AddWithValue("@sifre", textBox2.Text);
                SqlDataReader read = komut.ExecuteReader();
                if (read.Read())
                {
                    KullanıcıUrunler ku = new KullanıcıUrunler(textBox1.Text);
                    ku.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Yanlış kullanıcı adı veya şifre");
                }
                baglanti.Close();
            }
           
        }

       
    }
}
