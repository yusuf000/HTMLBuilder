
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
            this.numberOfPages = new System.Windows.Forms.TextBox();
            this.SKT_button = new System.Windows.Forms.Button();
            this.keywordTemplets = new System.Windows.Forms.TextBox();
            this.SRT_button = new System.Windows.Forms.Button();
            this.ruleTemplets = new System.Windows.Forms.TextBox();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.Run = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.continuousTime = new System.Windows.Forms.CheckBox();
            this.percentage = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.isPaginator = new System.Windows.Forms.CheckBox();
            this.outputAsTxt = new System.Windows.Forms.CheckBox();
            this.continuousOutputCSV = new System.Windows.Forms.CheckBox();
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
            // numberOfPages
            // 
            this.numberOfPages.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numberOfPages.Location = new System.Drawing.Point(408, 43);
            this.numberOfPages.Name = "numberOfPages";
            this.numberOfPages.Size = new System.Drawing.Size(144, 30);
            this.numberOfPages.TabIndex = 1;
            this.numberOfPages.Text = "1";
            // 
            // SKT_button
            // 
            this.SKT_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SKT_button.Location = new System.Drawing.Point(43, 145);
            this.SKT_button.Name = "SKT_button";
            this.SKT_button.Size = new System.Drawing.Size(236, 56);
            this.SKT_button.TabIndex = 2;
            this.SKT_button.Text = "Select Keyword Templates";
            this.SKT_button.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.SKT_button.UseVisualStyleBackColor = true;
            this.SKT_button.Click += new System.EventHandler(this.SRT_button_Click);
            // 
            // keywordTemplets
            // 
            this.keywordTemplets.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keywordTemplets.Location = new System.Drawing.Point(297, 160);
            this.keywordTemplets.Name = "keywordTemplets";
            this.keywordTemplets.Size = new System.Drawing.Size(221, 30);
            this.keywordTemplets.TabIndex = 3;
            // 
            // SRT_button
            // 
            this.SRT_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SRT_button.Location = new System.Drawing.Point(43, 225);
            this.SRT_button.Name = "SRT_button";
            this.SRT_button.Size = new System.Drawing.Size(236, 56);
            this.SRT_button.TabIndex = 4;
            this.SRT_button.Text = "Select Rule Templates";
            this.SRT_button.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.SRT_button.UseVisualStyleBackColor = true;
            this.SRT_button.Click += new System.EventHandler(this.SKT_button_Click);
            // 
            // ruleTemplets
            // 
            this.ruleTemplets.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ruleTemplets.Location = new System.Drawing.Point(297, 238);
            this.ruleTemplets.Name = "ruleTemplets";
            this.ruleTemplets.Size = new System.Drawing.Size(221, 30);
            this.ruleTemplets.TabIndex = 5;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker2.Location = new System.Drawing.Point(43, 304);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(475, 30);
            this.dateTimePicker2.TabIndex = 7;
            // 
            // Run
            // 
            this.Run.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Run.Location = new System.Drawing.Point(379, 463);
            this.Run.Name = "Run";
            this.Run.Size = new System.Drawing.Size(139, 44);
            this.Run.TabIndex = 8;
            this.Run.Text = "Run";
            this.Run.UseVisualStyleBackColor = true;
            this.Run.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.Location = new System.Drawing.Point(43, 365);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(82, 29);
            this.checkBox1.TabIndex = 10;
            this.checkBox1.Text = "Votes";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // continuousTime
            // 
            this.continuousTime.AutoSize = true;
            this.continuousTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.continuousTime.Location = new System.Drawing.Point(43, 400);
            this.continuousTime.Name = "continuousTime";
            this.continuousTime.Size = new System.Drawing.Size(180, 29);
            this.continuousTime.TabIndex = 11;
            this.continuousTime.Text = "Continuous Time";
            this.continuousTime.UseVisualStyleBackColor = true;
            this.continuousTime.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // percentage
            // 
            this.percentage.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.percentage.Location = new System.Drawing.Point(408, 83);
            this.percentage.Name = "percentage";
            this.percentage.Size = new System.Drawing.Size(144, 30);
            this.percentage.TabIndex = 13;
            this.percentage.Text = "50";
            this.percentage.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(38, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(349, 25);
            this.label3.TabIndex = 12;
            this.label3.Text = "NON-mandatory keyword probability %";
            // 
            // isPaginator
            // 
            this.isPaginator.AutoSize = true;
            this.isPaginator.Checked = true;
            this.isPaginator.CheckState = System.Windows.Forms.CheckState.Checked;
            this.isPaginator.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.isPaginator.Location = new System.Drawing.Point(43, 431);
            this.isPaginator.Name = "isPaginator";
            this.isPaginator.Size = new System.Drawing.Size(114, 29);
            this.isPaginator.TabIndex = 14;
            this.isPaginator.Text = "Paginator";
            this.isPaginator.UseVisualStyleBackColor = true;
            // 
            // outputAsTxt
            // 
            this.outputAsTxt.AutoSize = true;
            this.outputAsTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputAsTxt.Location = new System.Drawing.Point(43, 466);
            this.outputAsTxt.Name = "outputAsTxt";
            this.outputAsTxt.Size = new System.Drawing.Size(146, 29);
            this.outputAsTxt.TabIndex = 15;
            this.outputAsTxt.Text = "Output as .txt";
            this.outputAsTxt.UseVisualStyleBackColor = true;
            // 
            // continuousOutputCSV
            // 
            this.continuousOutputCSV.AutoSize = true;
            this.continuousOutputCSV.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.continuousOutputCSV.Location = new System.Drawing.Point(43, 504);
            this.continuousOutputCSV.Name = "continuousOutputCSV";
            this.continuousOutputCSV.Size = new System.Drawing.Size(243, 29);
            this.continuousOutputCSV.TabIndex = 16;
            this.continuousOutputCSV.Text = "Continuous Output CSV";
            this.continuousOutputCSV.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 545);
            this.Controls.Add(this.continuousOutputCSV);
            this.Controls.Add(this.outputAsTxt);
            this.Controls.Add(this.isPaginator);
            this.Controls.Add(this.percentage);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.continuousTime);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.Run);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.ruleTemplets);
            this.Controls.Add(this.SRT_button);
            this.Controls.Add(this.keywordTemplets);
            this.Controls.Add(this.SKT_button);
            this.Controls.Add(this.numberOfPages);
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
        private System.Windows.Forms.TextBox numberOfPages;
        private System.Windows.Forms.Button SKT_button;
        private System.Windows.Forms.TextBox keywordTemplets;
        private System.Windows.Forms.Button SRT_button;
        private System.Windows.Forms.TextBox ruleTemplets;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Button Run;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox continuousTime;
        private System.Windows.Forms.TextBox percentage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox isPaginator;
        private System.Windows.Forms.CheckBox outputAsTxt;
        private System.Windows.Forms.CheckBox continuousOutputCSV;
    }
}

