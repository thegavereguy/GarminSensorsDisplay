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
            this.textBoxInitDebug = new System.Windows.Forms.TextBox();
            this.textBoxChannelTypeCadenza = new System.Windows.Forms.TextBox();
            this.textBoxChannelTypeVelocita = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxDebugCadenza = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxDebugVelocita = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
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
            this.textBoxCadenza.Location = new System.Drawing.Point(307, 79);
            this.textBoxCadenza.Name = "textBoxCadenza";
            this.textBoxCadenza.Size = new System.Drawing.Size(100, 20);
            this.textBoxCadenza.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(304, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "cadenza";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // textBoxInitDebug
            // 
            this.textBoxInitDebug.Location = new System.Drawing.Point(14, 22);
            this.textBoxInitDebug.Name = "textBoxInitDebug";
            this.textBoxInitDebug.Size = new System.Drawing.Size(225, 20);
            this.textBoxInitDebug.TabIndex = 3;
            // 
            // textBoxChannelTypeCadenza
            // 
            this.textBoxChannelTypeCadenza.Location = new System.Drawing.Point(569, 160);
            this.textBoxChannelTypeCadenza.Name = "textBoxChannelTypeCadenza";
            this.textBoxChannelTypeCadenza.Size = new System.Drawing.Size(100, 20);
            this.textBoxChannelTypeCadenza.TabIndex = 4;
            // 
            // textBoxChannelTypeVelocita
            // 
            this.textBoxChannelTypeVelocita.Location = new System.Drawing.Point(569, 203);
            this.textBoxChannelTypeVelocita.Name = "textBoxChannelTypeVelocita";
            this.textBoxChannelTypeVelocita.Size = new System.Drawing.Size(100, 20);
            this.textBoxChannelTypeVelocita.TabIndex = 5;
            // 
            // button3
            // 
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
            this.label5.Size = new System.Drawing.Size(81, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "debug generale";
            // 
            // textBoxDebugCadenza
            // 
            this.textBoxDebugCadenza.Location = new System.Drawing.Point(15, 79);
            this.textBoxDebugCadenza.Name = "textBoxDebugCadenza";
            this.textBoxDebugCadenza.Size = new System.Drawing.Size(224, 20);
            this.textBoxDebugCadenza.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(121, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "debug sensore cadenza";
            // 
            // textBoxDebugVelocita
            // 
            this.textBoxDebugVelocita.Location = new System.Drawing.Point(15, 136);
            this.textBoxDebugVelocita.Name = "textBoxDebugVelocita";
            this.textBoxDebugVelocita.Size = new System.Drawing.Size(224, 20);
            this.textBoxDebugVelocita.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 120);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(123, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "debbug sensore velocita";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(699, 497);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxDebugVelocita);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxDebugCadenza);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBoxChannelTypeVelocita);
            this.Controls.Add(this.textBoxChannelTypeCadenza);
            this.Controls.Add(this.textBoxInitDebug);
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
        private System.Windows.Forms.TextBox textBoxInitDebug;
        private System.Windows.Forms.TextBox textBoxChannelTypeCadenza;
        private System.Windows.Forms.TextBox textBoxChannelTypeVelocita;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxDebugCadenza;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxDebugVelocita;
        private System.Windows.Forms.Label label7;
    }
}

