using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;
using MetroFramework.Controls;

namespace BeatMaker
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        static bool listen = false;
        Dictionary<string, string> sounds = new Dictionary<string, string>()
        {
            {"ahh", @"C:\Users\Glumi\Desktop\Snap Pack by Ihaksi\Snap 01.wav" },
            {"cough", @"C:\Users\Glumi\Desktop\Snap Pack by Ihaksi\Snap 02.wav" }
        };

        List<string> soundList = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Core.Init(this);
            foreach (MetroButton button in this.Controls.OfType<MetroButton>().OrderBy(c => c.Name))
            {
                if (button.Text == "♬")
                {
                    soundList.Add("");
                }
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            listen = true;
            while (listen)
            {
                Application.DoEvents();
                foreach (MetroPanel full in trackPanel.Controls.OfType<MetroPanel>().OrderBy(c => c.TabIndex))
                {
                    Application.DoEvents();
                    foreach (MetroPanel quarter in full.Controls.OfType<MetroPanel>().OrderBy(c => c.TabIndex))
                    {
                        quarter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                        Application.DoEvents();
                        foreach (MetroToggle toggle in quarter.Controls.OfType<MetroToggle>().Where(c => c.Checked).OrderBy(c => c.TabIndex))
                        {
                            Application.DoEvents();
                            Thread thread = new Thread(delegate () { Play(soundList[int.Parse(toggle.Tag.ToString())]); });
                            thread.Start();
                        }
                        Thread.Sleep((60000 / metroTrackBar1.Value) / 4);
                        quarter.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    }
                }
            }
        }
        void Play(string audioPath)
        {
            try
            {
                MediaPlayer myPlayer = new MediaPlayer();
                myPlayer.Open(new System.Uri(audioPath));
                myPlayer.Play();
            }
            catch { }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            listen = false;
        }

        private void metroTrackBar1_Scroll(object sender, ScrollEventArgs e)
        {
            Label_BPM.Text = "BPM:" + metroTrackBar1.Value;
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            ChooseFileForSound(0);
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            ChooseFileForSound(1);
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            ChooseFileForSound(2);
        }

        private void metroButton6_Click(object sender, EventArgs e)
        {
            ChooseFileForSound(3);
        }

        public void ChooseFileForSound(int elm)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "wav files (*.wav)|*.wav";
                ofd.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    soundList[elm] = ofd.FileName;
                }
            };
        }

        private void metroButton7_Click(object sender, EventArgs e)
        {
            var allFullPanels = trackPanel.Controls.OfType<MetroPanel>().OrderBy(c => c.TabIndex);
            MetroPanel lastFullPanel = allFullPanels.Last();
            MetroPanel newFullPanel = new MetroPanel();
            newFullPanel.Location = new Point(lastFullPanel.Location.X + lastFullPanel.Size.Width + 40, lastFullPanel.Location.Y);
            newFullPanel.Size = lastFullPanel.Size;
            newFullPanel.TabIndex = lastFullPanel.TabIndex + 1;
            newFullPanel.Visible = true;

            for (int i = 0; i < 4; i++)
            {
                MetroPanel newQuarterPanel = new MetroPanel();
                newQuarterPanel.Location = new Point(i * 50, 5);
                newQuarterPanel.TabIndex = i;
                newQuarterPanel.Size = new Size(40, 90);
                newQuarterPanel.Visible = true;

                for (int ii = 0; ii < 4; ii++)
                {
                    MetroToggle newToggleButton = new MetroToggle();
                    newToggleButton.AutoSize = false;
                    newToggleButton.Size = new Size(30, 17);
                    newToggleButton.Location = new Point(5, (ii * 20) + 5);
                    newToggleButton.TabIndex = ii;
                    newToggleButton.Tag = ii;
                    newToggleButton.Visible = true;
                    newToggleButton.DisplayStatus = false;
                    newQuarterPanel.Controls.Add(newToggleButton);
                }

                newFullPanel.Controls.Add(newQuarterPanel);
            }

            trackPanel.Controls.Add(newFullPanel);
        }
    }
}
