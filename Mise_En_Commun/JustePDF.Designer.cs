namespace Mise_En_Commun
{
    partial class JustePDF
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbJuste = new System.Windows.Forms.GroupBox();
            this.pbJuste = new System.Windows.Forms.PictureBox();
            this.labelJuste = new System.Windows.Forms.Label();
            this.gbJuste.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbJuste)).BeginInit();
            this.SuspendLayout();
            // 
            // gbJuste
            // 
            this.gbJuste.Controls.Add(this.pbJuste);
            this.gbJuste.Controls.Add(this.labelJuste);
            this.gbJuste.ForeColor = System.Drawing.Color.Green;
            this.gbJuste.Location = new System.Drawing.Point(0, 0);
            this.gbJuste.Name = "gbJuste";
            this.gbJuste.Size = new System.Drawing.Size(550, 175);
            this.gbJuste.TabIndex = 1;
            this.gbJuste.TabStop = false;
            this.gbJuste.Text = "groupBox1";
            // 
            // pbJuste
            // 
            this.pbJuste.Location = new System.Drawing.Point(283, 19);
            this.pbJuste.Name = "pbJuste";
            this.pbJuste.Size = new System.Drawing.Size(89, 88);
            this.pbJuste.TabIndex = 1;
            this.pbJuste.TabStop = false;
            // 
            // labelJuste
            // 
            this.labelJuste.AutoSize = true;
            this.labelJuste.Location = new System.Drawing.Point(45, 64);
            this.labelJuste.Name = "labelJuste";
            this.labelJuste.Size = new System.Drawing.Size(35, 13);
            this.labelJuste.TabIndex = 0;
            this.labelJuste.Text = "label1";
            // 
            // JustePDF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbJuste);
            this.Name = "JustePDF";
            this.Size = new System.Drawing.Size(550, 175);
            this.Load += new System.EventHandler(this.JustePDF_Load);
            this.gbJuste.ResumeLayout(false);
            this.gbJuste.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbJuste)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbJuste;
        private System.Windows.Forms.PictureBox pbJuste;
        private System.Windows.Forms.Label labelJuste;
    }
}
