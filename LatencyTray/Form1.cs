using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LatencyTray
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            notifyIcon.ContextMenuStrip = contextMenuStrip1;
            startLatencyTray();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(this.Hide));
        }
        public Icon createIconFromText(string text)
        {
            var image = new Bitmap(16, 16);
            var drawing = Graphics.FromImage(image);
            drawing.DrawString(text, new Font(FontFamily.GenericSerif, 11), Brushes.Red, -5, 0);
            drawing.Save();
            return Icon.FromHandle(image.GetHicon());
        }
        public long getLatency(string address)
        {
            try
            {
                var response = new Ping().Send(address);
                var time = response.RoundtripTime > 0 ? response.RoundtripTime : (long)999.0;
                return time;
            }
            catch (Exception ex)
            {
                return 999;
            }
        }
        public async void startLatencyTray()
        {
            while (true)
            {
                await Task.Delay(1000);
                //notifyIcon.Icon = createIconFromText(getLatency("google.com"));
                //notifyIcon.Icon = createIconFromText("25");
                switch (getLatency("google.com"))
                {
                    case long n when (n <= 100):
                        notifyIcon.Icon = createIconFromSolidColor(Color.Green);
                        break;
                    case long n when (n <= 200):
                        notifyIcon.Icon = createIconFromSolidColor(Color.Yellow);
                        break;
                    default:
                        notifyIcon.Icon = createIconFromSolidColor(Color.Red);
                        break;
                }
            }
        }
        public Icon createIconFromSolidColor(Color color)
        {
            var image = new Bitmap(16, 16);
            Graphics.FromImage(image).Clear(color);
            return Icon.FromHandle(image.GetHicon());
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
