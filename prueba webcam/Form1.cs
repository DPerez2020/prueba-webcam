using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using AForge.Video;
using System.Drawing.Imaging;
using System.IO;

namespace prueba_webcam
{
    public partial class Form1 : Form
    {
        private string path = Directory.GetCurrentDirectory().Replace(@"\bin\Debug","") + @"\images\";
        private bool hay_dispositivos;
        private FilterInfoCollection misdispositivos;
        private VideoCaptureDevice miwebcam;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (hay_dispositivos)
            {
                carga_dispositivos();
            }
        }
        public void carga_dispositivos()
        {
            misdispositivos = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (misdispositivos.Count>0)
            {
                hay_dispositivos = true;
                for (int i = 0; i < misdispositivos.Count; i++)
                {
                    comboBox1.Items.Add(misdispositivos[i].Name.ToString());
                    comboBox1.Text = misdispositivos[0].Name.ToString();
                }
            }
            else
            {
                hay_dispositivos = false;
            }
        }
        public void cerrar_camera()
        {
            if (miwebcam!=null && miwebcam.IsRunning)
            {
                miwebcam.SignalToStop();
                miwebcam = null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cerrar_camera();
            int i = comboBox1.SelectedIndex;
            string nombre = misdispositivos[i].MonikerString;
            miwebcam = new VideoCaptureDevice(nombre);
            miwebcam.NewFrame += new NewFrameEventHandler (capturando);
            miwebcam.Start();
        }
        private void capturando(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap imagen = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = imagen;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            cerrar_camera();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (miwebcam!=null && miwebcam.IsRunning)
            {
                pictureBox2.Image = pictureBox1.Image;
                pictureBox2.Image.Save(path +"holaquehace.jpg",ImageFormat.Jpeg);
            }
        }
    }
}
