namespace GarminSensorsDisplayGUI
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
            this.listViewTrainers = new System.Windows.Forms.ListView();
            this.label11 = new System.Windows.Forms.Label();
            this.trainerSpeeds = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxResistanceLevel = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cartesianChart1 = new LiveCharts.WinForms.CartesianChart();
            this.button4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trainerSpeeds)).BeginInit();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.Location = new System.Drawing.Point(15, 462);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "START";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBoxCadenza
            // 
            this.textBoxCadenza.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBoxCadenza.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCadenza.Location = new System.Drawing.Point(264, 22);
            this.textBoxCadenza.Name = "textBoxCadenza";
            this.textBoxCadenza.ReadOnly = true;
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
            this.button3.Location = new System.Drawing.Point(106, 462);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "STOP";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(888, 471);
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
            this.textBoxVelocita.ReadOnly = true;
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
            this.textBoxPower.ReadOnly = true;
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
            // listViewTrainers
            // 
            this.listViewTrainers.HideSelection = false;
            this.listViewTrainers.Location = new System.Drawing.Point(15, 241);
            this.listViewTrainers.Name = "listViewTrainers";
            this.listViewTrainers.Size = new System.Drawing.Size(169, 184);
            this.listViewTrainers.TabIndex = 19;
            this.listViewTrainers.UseCompatibleStateImageBehavior = false;
            this.listViewTrainers.View = System.Windows.Forms.View.List;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 225);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(56, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "trainers list";
            // 
            // trainerSpeeds
            // 
            this.trainerSpeeds.Location = new System.Drawing.Point(202, 241);
            this.trainerSpeeds.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.trainerSpeeds.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.trainerSpeeds.Name = "trainerSpeeds";
            this.trainerSpeeds.Size = new System.Drawing.Size(37, 20);
            this.trainerSpeeds.TabIndex = 21;
            this.trainerSpeeds.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.trainerSpeeds.ValueChanged += new System.EventHandler(this.trainerSpeeds_ValueChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(199, 225);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(79, 13);
            this.label12.TabIndex = 22;
            this.label12.Text = "selected speed";
            // 
            // textBoxResistanceLevel
            // 
            this.textBoxResistanceLevel.Location = new System.Drawing.Point(202, 307);
            this.textBoxResistanceLevel.Name = "textBoxResistanceLevel";
            this.textBoxResistanceLevel.ReadOnly = true;
            this.textBoxResistanceLevel.Size = new System.Drawing.Size(76, 20);
            this.textBoxResistanceLevel.TabIndex = 23;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(199, 291);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(80, 13);
            this.label13.TabIndex = 24;
            this.label13.Text = "resistance level";
            // 
            // cartesianChart1
            // 
            this.cartesianChart1.Location = new System.Drawing.Point(354, 172);
            this.cartesianChart1.Name = "cartesianChart1";
            this.cartesianChart1.Size = new System.Drawing.Size(588, 260);
            this.cartesianChart1.TabIndex = 25;
            this.cartesianChart1.Text = "cartesianChart1";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(490, 462);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 26;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(965, 497);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.cartesianChart1);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.textBoxResistanceLevel);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.trainerSpeeds);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.listViewTrainers);
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
            ((System.ComponentModel.ISupportInitialize)(this.trainerSpeeds)).EndInit();
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
        private System.Windows.Forms.ListView listViewTrainers;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown trainerSpeeds;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxResistanceLevel;
        private System.Windows.Forms.Label label13;
        private LiveCharts.WinForms.CartesianChart cartesianChart1;
        private System.Windows.Forms.Button button4;
    }
}

