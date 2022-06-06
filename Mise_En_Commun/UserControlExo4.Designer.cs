namespace Mise_En_Commun
{
    partial class UserControlExo4
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
            this.pcbImageMot = new System.Windows.Forms.PictureBox();
            this.lblMotEspagnol = new System.Windows.Forms.Label();
            this.lblOrigine = new System.Windows.Forms.Label();
            this.lblMotFrançais = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pcbImageMot)).BeginInit();
            this.SuspendLayout();
            // 
            // pcbImageMot
            // 
            this.pcbImageMot.BackColor = System.Drawing.Color.White;
            this.pcbImageMot.Location = new System.Drawing.Point(0, 0);
            this.pcbImageMot.Name = "pcbImageMot";
            this.pcbImageMot.Size = new System.Drawing.Size(150, 150);
            this.pcbImageMot.TabIndex = 0;
            this.pcbImageMot.TabStop = false;
            // 
            // lblMotEspagnol
            // 
            this.lblMotEspagnol.AutoSize = true;
            this.lblMotEspagnol.BackColor = System.Drawing.Color.Transparent;
            this.lblMotEspagnol.Font = new System.Drawing.Font("Noto Sans Lao", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMotEspagnol.ForeColor = System.Drawing.Color.Black;
            this.lblMotEspagnol.Location = new System.Drawing.Point(32, 208);
            this.lblMotEspagnol.Name = "lblMotEspagnol";
            this.lblMotEspagnol.Size = new System.Drawing.Size(89, 20);
            this.lblMotEspagnol.TabIndex = 1;
            this.lblMotEspagnol.Text = "MotEspagnol";
            // 
            // lblOrigine
            // 
            this.lblOrigine.AutoSize = true;
            this.lblOrigine.BackColor = System.Drawing.Color.Transparent;
            this.lblOrigine.Font = new System.Drawing.Font("Noto Sans Lao", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrigine.Location = new System.Drawing.Point(33, 242);
            this.lblOrigine.Name = "lblOrigine";
            this.lblOrigine.Size = new System.Drawing.Size(52, 20);
            this.lblOrigine.TabIndex = 2;
            this.lblOrigine.Text = "Origine";
            // 
            // lblMotFrançais
            // 
            this.lblMotFrançais.AutoSize = true;
            this.lblMotFrançais.BackColor = System.Drawing.Color.Transparent;
            this.lblMotFrançais.Font = new System.Drawing.Font("Noto Sans Lao", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMotFrançais.Location = new System.Drawing.Point(32, 179);
            this.lblMotFrançais.Name = "lblMotFrançais";
            this.lblMotFrançais.Size = new System.Drawing.Size(83, 20);
            this.lblMotFrançais.TabIndex = 3;
            this.lblMotFrançais.Text = "MotFrançais";
            // 
            // UserControlExo4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Controls.Add(this.lblMotFrançais);
            this.Controls.Add(this.lblOrigine);
            this.Controls.Add(this.lblMotEspagnol);
            this.Controls.Add(this.pcbImageMot);
            this.Name = "UserControlExo4";
            this.Size = new System.Drawing.Size(150, 275);
            this.Load += new System.EventHandler(this.UserControl1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pcbImageMot)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private System.Windows.Forms.PictureBox pcbImageMot;
        private System.Windows.Forms.Label lblMotEspagnol;
        private System.Windows.Forms.Label lblOrigine;
        private System.Windows.Forms.Label lblMotFrançais;
    }
}
