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
using LumenWorks.Framework.IO.Csv;

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
                
                using (var rd = new CsvReader(new StreamReader(ruleTempletePath), false))
                {
                    while (rd.ReadNextRecord())
                    {
                        List<string> list = new List<string>();
                        for (int i = 0; i < rd.FieldCount; i++)
                        {
                            string s = rd[i].Trim();
                            list.Add(s);
                        }
                        rules.Add(list);
                    }
                }

                int numberOfPages = Int32.Parse(this.numberOfPages.Text.Trim());
                int percentate = Int32.Parse(this.percentage.Text.Trim());
                Util.currentDate = dateTimePicker2.Value;
                for (int page = 1; page <= numberOfPages; page++)
                {
                    for (int i = 1; i < rules.Count; i++)
                    {
                        if (!continuousTime.Checked)
                        {
                            Util.currentDate = dateTimePicker2.Value;
                        }
                        PageBuilder pageBuilder = new PageBuilder();
                        model.KeyWords keywords = Util.getRule(keyWordTempletePath, checkBox1.Checked);
                        string html = pageBuilder.createpage(rules[i], keywords, checkBox1.Checked, rules[0].ToArray(), percentate);
                        File.WriteAllText("web_" + page + "_" + i + ".html", html);
                    }
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

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
