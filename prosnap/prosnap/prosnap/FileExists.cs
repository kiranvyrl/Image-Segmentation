using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;

namespace prosnap
{
    public partial class FileExists : Form
    {
        public String newfilename;
        public Boolean newfile = false;
        public FileExists()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                newfilename = textBox2.Text;
                if (File.Exists(ConfigurationSettings.AppSettings["filesreceived"] + newfilename))
                {
                    MessageBox.Show("Give Different name", "File already exists", MessageBoxButtons.OK);
                }
                else
                {
                    newfile = true;
                    newfilename = textBox2.Text;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Enter the file name", "File already exists", MessageBoxButtons.OK);
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            newfile = false;
            this.Close();
        }
    }
}
