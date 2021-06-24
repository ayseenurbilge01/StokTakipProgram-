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
    public partial class Kayit : Form
    {
        public Kayit()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=LAPTOP-EEFUFS2L;Initial Catalog=stoktakipprogrami;Integrated Security=True");

        private void Kayit_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            Giris g = new Giris();
            g.Show();
            this.Hide();
        }

        private void btnGirisYap_Click(object sender, EventArgs e)
        {
        
            baglanti.Open();
            SqlCommand komut1 = new SqlCommand("Select * from kullanicilar where kullaniciadi=@kullaniciadi",baglanti);
            komut1.Parameters.AddWithValue("@kullaniciadi",textBox1.Text);
            SqlDataReader read = komut1.ExecuteReader();
            if (read.Read())
            {
                MessageBox.Show("Bu kullanıcı adı alınmış.");
            }
            else
            {
                komut1.Dispose();
                read.Close();
                SqlCommand komut = new SqlCommand("insert into kullanicilar(kullaniciadi,sifre) values(@kullaniciadi,@sifre)", baglanti);
                komut.Parameters.Add("@kullaniciadi", textBox1.Text);
                komut.Parameters.Add("@sifre", textBox2.Text);
                komut.ExecuteNonQuery();
                komut.Dispose();
                Giris g = new Giris();
                g.Show();
                this.Hide();
            }
            baglanti.Close();
        }
    }
}
