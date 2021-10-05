
namespace WindowsFormsApp1
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.resizaileBox = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resizaileBox)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.resizaileBox);
            this.panel1.Location = new System.Drawing.Point(12, 52);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(681, 339);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // resizaileBox
            // 
            this.resizaileBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.resizaileBox.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.resizaileBox.Cursor = System.Windows.Forms.Cursors.SizeNESW;
            this.resizaileBox.Location = new System.Drawing.Point(21, 3);
            this.resizaileBox.Name = "resizaileBox";
            this.resizaileBox.Size = new System.Drawing.Size(675, 346);
            this.resizaileBox.TabIndex = 0;
            this.resizaileBox.TabStop = false;
            this.resizaileBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.resizaileBox_MouseDown);
            this.resizaileBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.resizaileBox_MouseMove);
            this.resizaileBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.resizaileBox_MouseUp);
            this.resizaileBox.Resize += new System.EventHandler(this.resizaileBox_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resizaileBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox resizaileBox;
    }
}

