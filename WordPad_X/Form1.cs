using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Timers;



namespace WordPad_X
{
    public partial class Form1 : Form
    {
        string fileDir, fileName;

        public Form1()
        {
            InitializeComponent();

            rchTxtBoxWrdPad.Visible = true;
            rchTxtBoxWrdPad.ReadOnly = true;

            Thread x = new Thread(message);
            x.Start();           
        }

        private void message()
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Save File?", "Reminder", buttons);
            
            if (result == DialogResult.Yes)
            {
                rchTxtBoxWrdPad.Invoke((MethodInvoker)(() => rchTxtBoxWrdPad.ReadOnly = false ));
                Thread FirstThread = new Thread(() => saveToolStripMenuItem.PerformClick());
                FirstThread.SetApartmentState(ApartmentState.STA);
                FirstThread.Start();
            }
            else if (result == DialogResult.No)
            {

                return;

            }
            Thread.Sleep(1000);
        }

            private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Try if a file already exists
            try
            {
                // Save file
                rchTxtBoxWrdPad.SaveFile(fileDir, RichTextBoxStreamType.RichText);

            }
            catch (Exception)
            {
                saveAsToolStripMenuItem.PerformClick();
            }

        }


        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rchTxtBoxWrdPad.Visible == true)
            {
                FileDialog fd = new SaveFileDialog();

                // Save file
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    fileDir = fd.FileName.ToString();
                    fileName = Path.GetFileName(fileDir);

                    if (InvokeRequired)
                    {
                         
                        this.Invoke(new MethodInvoker(delegate { this.Text = fileName + " - WordPad_X"; }));
                        this.Invoke(new MethodInvoker(delegate {
                            rchTxtBoxWrdPad.SaveFile(fileDir, RichTextBoxStreamType.RichText);
                        }));
                        return; 
                    }
                    // Change title text
                    
                    this.Text = fileName + " - WordPad_X";

                    rchTxtBoxWrdPad.SaveFile(fileDir, RichTextBoxStreamType.RichText);
                }
            }
        }



        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileDialog fd = new OpenFileDialog(); // Instantiate file dialog

            // Open file
            if (fd.ShowDialog() == DialogResult.OK)
            {
                fileDir = fd.FileName;
                // Try first if file will open successfully
                try
                {
                    rchTxtBoxWrdPad.LoadFile(fileDir);
                    rchTxtBoxWrdPad.Visible = true;

                    // Change title text
                    fileName = Path.GetFileName(fileDir);
                    this.Text = fileName + " - WordPad_X";
                }
                catch (Exception)
                {
                    MessageBox.Show("Not an RTF File!", "Error");
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Clear rich text box and hide it
            rchTxtBoxWrdPad.Clear();
            rchTxtBoxWrdPad.Visible = false;

            this.Text = "WordPad_X"; // Change title text
        }

        private void rchTxtBoxWrdPad_TextChanged(object sender, EventArgs e)
        {

        }

        private void chkAutoSave_CheckedChanged(object sender, EventArgs e)
        {
            System.Timers.Timer aTimer;
            int x = Int32.Parse(comboBox1.Text);
            x = x * 1000;
            if(chkAutoSave.Checked)
            {
                //seconds to miliseconds
                
                aTimer = new System.Timers.Timer();
                aTimer.Interval = x;
                aTimer.Start();
                aTimer.Elapsed += ATimer_Elapsed;
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
            }
        }

        private void ATimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Thread thread1 = new Thread(AutoSave);
            thread1.Start();

        }
        
        private void AutoSave()
        {
            if (InvokeRequired)
            {

                this.Invoke(new MethodInvoker(delegate { this.Text = fileName + " - WordPad_X"; }));
                this.Invoke(new MethodInvoker(delegate {
                    rchTxtBoxWrdPad.SaveFile(fileDir, RichTextBoxStreamType.RichText);
                }));
                
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close(); // Close form
        }
    }
}
