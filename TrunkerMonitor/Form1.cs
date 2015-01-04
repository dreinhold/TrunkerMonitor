using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TrunkerMonitor
{
    public partial class Form1 : Form
    {
        string last_label = System.String.Empty;
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            byte[] buffer;
            FileStream fileStream;
            try
            {

                string exe_path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                fileStream = new FileStream(exe_path + "/sdrsharptrunking.log", FileMode.Open, FileAccess.Read);
            }
            catch (UnauthorizedAccessException)
            {
                return;
            }
            catch (FileNotFoundException)
            {
                return;
            }
            catch (IOException)
            {
                return;
            }
            try
            {
                int length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                  sum += count;  // sum is a buffer offset for next reading
              }
              finally
              {
                fileStream.Close();
              }
              // Get only second line
              string raw_data = System.Text.Encoding.UTF8.GetString(buffer);
              char[] delimiterChars = {'\n'};
              string[] lines = raw_data.Split('\n');
              string[] line_tmp = lines[1].Split('\t');
              string new_label = System.String.Empty;
              if (line_tmp[0].Equals("Listen"))
              {
                  new_label = "";
                  if (!line_tmp[6].Equals(""))
                  {
                      new_label = line_tmp[6] + " ---> ";
                  }
                  if(line_tmp[4].Equals("")) {
                      new_label = new_label + "GROUP " + line_tmp[3];
                  } else {
                      new_label = new_label + line_tmp[4];
                  }
              }
              else
              {
                  new_label = "Scanning";
              }
              if(! new_label.Equals(this.last_label)) {
                  this.label1.Text = new_label;
                  this.last_label = new_label;
              }
              
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
