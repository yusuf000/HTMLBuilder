
namespace Automation_interface
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SRT_button = new System.Windows.Forms.Button();
            this.ruleTempletsFile = new System.Windows.Forms.TextBox();
            this.SKT_button = new System.Windows.Forms.Button();
            this.keywordTemplets = new System.Windows.Forms.TextBox();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.Run = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(38, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(241, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Number of pages to create";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(297, 43);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(175, 30);
            this.textBox1.TabIndex = 1;
            // 
            // SRT_button
            // 
            this.SRT_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SRT_button.Location = new System.Drawing.Point(43, 160);
            this.SRT_button.Name = "SRT_button";
            this.SRT_button.Size = new System.Drawing.Size(236, 56);
            this.SRT_button.TabIndex = 2;
            this.SRT_button.Text = "Select Rules Temples";
            this.SRT_button.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.SRT_button.UseVisualStyleBackColor = true;
            this.SRT_button.Click += new System.EventHandler(this.SRT_button_Click);
            // 
            // ruleTempletsFile
            // 
            this.ruleTempletsFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ruleTempletsFile.Location = new System.Drawing.Point(297, 175);
            this.ruleTempletsFile.Name = "ruleTempletsFile";
            this.ruleTempletsFile.Size = new System.Drawing.Size(221, 30);
            this.ruleTempletsFile.TabIndex = 3;
            // 
            // SKT_button
            // 
            this.SKT_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SKT_button.Location = new System.Drawing.Point(43, 240);
            this.SKT_button.Name = "SKT_button";
            this.SKT_button.Size = new System.Drawing.Size(236, 56);
            this.SKT_button.TabIndex = 4;
            this.SKT_button.Text = "Select keywords Temples";
            this.SKT_button.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.SKT_button.UseVisualStyleBackColor = true;
            this.SKT_button.Click += new System.EventHandler(this.SKT_button_Click);
            // 
            // keywordTemplets
            // 
            this.keywordTemplets.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keywordTemplets.Location = new System.Drawing.Point(297, 253);
            this.keywordTemplets.Name = "keywordTemplets";
            this.keywordTemplets.Size = new System.Drawing.Size(221, 30);
            this.keywordTemplets.TabIndex = 5;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker2.Location = new System.Drawing.Point(43, 322);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(475, 30);
            this.dateTimePicker2.TabIndex = 7;
            // 
            // Run
            // 
            this.Run.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Run.Location = new System.Drawing.Point(379, 429);
            this.Run.Name = "Run";
            this.Run.Size = new System.Drawing.Size(139, 44);
            this.Run.TabIndex = 8;
            this.Run.Text = "Run";
            this.Run.UseVisualStyleBackColor = true;
            this.Run.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 458);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "label2";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.Location = new System.Drawing.Point(43, 375);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(82, 29);
            this.checkBox1.TabIndex = 10;
            this.checkBox1.Text = "Votes";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 545);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Run);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.keywordTemplets);
            this.Controls.Add(this.SKT_button);
            this.Controls.Add(this.ruleTempletsFile);
            this.Controls.Add(this.SRT_button);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Form1";
            this.Text = "AutomationVOTES -";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button SRT_button;
        private System.Windows.Forms.TextBox ruleTempletsFile;
        private System.Windows.Forms.Button SKT_button;
        private System.Windows.Forms.TextBox keywordTemplets;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Button Run;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

