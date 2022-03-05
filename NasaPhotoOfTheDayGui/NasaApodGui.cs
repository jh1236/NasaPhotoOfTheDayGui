/*
 * Nasa APOD Background Changer GUI
 * 
 * Jared Healy, All rights reserved (c) 2022
 * 
 * The GUI Component of the Nasa APOD background changer.  
 * Also a first attempt at using Windows Forms to program!
 * 
 * 10/02/2022; Commented through all code
 *             Minimised the window to tray
 *             
 * 11/02/2022; Fixed Time Not Resetting upon pressing Refresh
 * 
 * 05/03/2022; Refactored the way that the time elapsed is detected
 */



using System.ComponentModel;

namespace Nasa
{
    public partial class NasaApodGui : Form
    {
        private bool shouldClose = false;
        private long lastRun = 0;
        public NasaApodGui()
        {
            InitializeComponent();

            string directory = Directory.GetCurrentDirectory();
            string apiKey;
            try
            {
                apiKey = File.ReadAllText(directory + "/key.txt");
            }
            catch
            {
                apiKey = "";
            }
            PicDownloader.apiKey = apiKey;
            textBox1.Text = apiKey;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            System.Windows.Forms.Timer updateTimer = new()
            {
                Interval = (1000) // 1 sec
            };

            updateTimer.Tick += new EventHandler(UpdateScreen);
            updateTimer.Start();
            Resize += Form1_Resize1;
            FormClosing += CloseHandler;
        }

        private static long GetCurrentTimeMs()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void Form1_Resize1(object? sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
                Opacity = 0;
                Hide();
            }
        }

        private async void UpdateScreen(object? sender, EventArgs? e)
        {
            int timeLeft = 21600000 - (int)(GetCurrentTimeMs() - lastRun);
            if (timeLeft <= 0)
            {
                lastRun = GetCurrentTimeMs();
                await PicDownloader.SetTodaysPhoto();
            }
            textBox2.Text = (timeLeft/1000).ToString() + " seconds";
        }

        private async void Button1ClickAsync(object sender, EventArgs e)
        {
            lastRun = GetCurrentTimeMs();
            await PicDownloader.SetTodaysPhoto();
        }

        private void TextBox1TextChanged(object sender, EventArgs e)
        {
            PicDownloader.apiKey = textBox1.Text;
        }

        private void Form1Load(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.BalloonTipText = "Nasa Background setter successfully loaded!";
            notifyIcon1.BalloonTipTitle = "Nasa APOD Bacground Setter";
            notifyIcon1.ShowBalloonTip(2000);
        }

        private void OpenMenuToolStripMenuItemClick(object sender, EventArgs e)
        {
            this.Focus();
            this.Show();
            this.Opacity = 1;
        }

        private void CloseHandler(object? sender, CancelEventArgs e)
        {
            e.Cancel = !shouldClose;
            if (!shouldClose)
            {
                WindowState = FormWindowState.Normal;
                Opacity = 0;
                Hide();
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string directory = Directory.GetCurrentDirectory();
            File.WriteAllText(directory + "/key.txt", PicDownloader.apiKey);
            shouldClose = true;
            Close();
        }
    }
}