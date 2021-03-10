using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Automation_interface.model;

namespace Automation_interface
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void SRT_button_Click(object sender, EventArgs e)
        {
            int size = -1;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                try
                {
                    keywordTemplets.Text = file;
                }
                catch (IOException)
                {
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string keyWordTempletePath = keywordTemplets.Text.Trim();
                string ruleTempletePath = ruleTemplets.Text.Trim();
                List<List<string>> rules = new List<List<string>>();
                using (var rd = new StreamReader(ruleTempletePath))
                {
                    while (!rd.EndOfStream)
                    {
                        List<string> list = new List<string>();
                        var splits = rd.ReadLine().Split(',');
                        for (int i = 0; i < splits.Length; i++)
                        {
                            string s = splits[i].Trim();
                            if(s.Length > 0)
                                list.Add(s);
                        }
                        rules.Add(list);
                    }
                }

                for (int i = 1; i < rules.Count; i++)
                {
                    PageBuilder pageBuilder = new PageBuilder();
                    model.KeyWords rule = Util.getRule(keyWordTempletePath);
                    string html = pageBuilder.createpage(rules[1], rule, dateTimePicker2.Value, checkBox1.Checked);
                    File.WriteAllText("web_" + i + ".html", html);
                }
                

                MessageBox.Show("Done", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void SKT_button_Click(object sender, EventArgs e)
        {
            int size = -1;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                try
                {
                    ruleTemplets.Text = file;
                }
                catch (IOException)
                {
                }
            }
        }
    }
}
