﻿namespace GarminSensorsDisplayGUI
{
    partial class Form1
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.button2 = new System.Windows.Forms.Button();
            this.textBoxCadenza = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxGeneralLog = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxLogCadence = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxLogSpeed = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxVelocita = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxLogPower = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxPower = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.Location = new System.Drawing.Point(307, 384);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "INIZIO";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBoxCadenza
            // 
            this.textBoxCadenza.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBoxCadenza.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCadenza.Location = new System.Drawing.Point(264, 22);
            this.textBoxCadenza.Name = "textBoxCadenza";
            this.textBoxCadenza.Size = new System.Drawing.Size(135, 38);
            this.textBoxCadenza.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(261, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Cadence";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // textBoxGeneralLog
            // 
            this.textBoxGeneralLog.Location = new System.Drawing.Point(14, 22);
            this.textBoxGeneralLog.Name = "textBoxGeneralLog";
            this.textBoxGeneralLog.Size = new System.Drawing.Size(225, 20);
            this.textBoxGeneralLog.TabIndex = 3;
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(388, 384);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "FINE";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(621, 471);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(75, 23);
            this.buttonExit.TabIndex = 7;
            this.buttonExit.Text = "EXIT";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "general log";
            // 
            // textBoxLogCadence
            // 
            this.textBoxLogCadence.Location = new System.Drawing.Point(15, 79);
            this.textBoxLogCadence.Name = "textBoxLogCadence";
            this.textBoxLogCadence.Size = new System.Drawing.Size(224, 20);
            this.textBoxLogCadence.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "cadence sensor log";
            // 
            // textBoxLogSpeed
            // 
            this.textBoxLogSpeed.Location = new System.Drawing.Point(15, 136);
            this.textBoxLogSpeed.Name = "textBoxLogSpeed";
            this.textBoxLogSpeed.Size = new System.Drawing.Size(224, 20);
            this.textBoxLogSpeed.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 120);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "speed sensor log";
            // 
            // textBoxVelocita
            // 
            this.textBoxVelocita.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxVelocita.Location = new System.Drawing.Point(452, 22);
            this.textBoxVelocita.Name = "textBoxVelocita";
            this.textBoxVelocita.Size = new System.Drawing.Size(141, 38);
            this.textBoxVelocita.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(449, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Speed";
            // 
            // textBoxLogPower
            // 
            this.textBoxLogPower.Location = new System.Drawing.Point(15, 188);
            this.textBoxLogPower.Name = "textBoxLogPower";
            this.textBoxLogPower.Size = new System.Drawing.Size(224, 20);
            this.textBoxLogPower.TabIndex = 15;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 172);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(134, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "virtual power sensor debug";
            // 
            // textBoxPower
            // 
            this.textBoxPower.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPower.Location = new System.Drawing.Point(264, 104);
            this.textBoxPower.Name = "textBoxPower";
            this.textBoxPower.Size = new System.Drawing.Size(135, 38);
            this.textBoxPower.TabIndex = 17;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(261, 88);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(37, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Power";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Items.AddRange(new object[] {
            "icoa",
            "afewfo"});
            this.listBox1.Location = new System.Drawing.Point(15, 240);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(120, 95);
            this.listBox1.TabIndex = 19;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(699, 497);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBoxPower);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBoxLogPower);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxVelocita);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxLogSpeed);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxLogCadence);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBoxGeneralLog);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxCadenza);
            this.Controls.Add(this.button2);
            this.Name = "Form1";
            this.Text = "Garmin Sensors Display GUI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBoxCadenza;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxGeneralLog;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxLogCadence;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxLogSpeed;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxVelocita;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxLogPower;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxPower;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ListBox listBox1;
    }
}

