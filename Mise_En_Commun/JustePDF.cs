using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mise_En_Commun
{
    public partial class JustePDF : UserControl
    {
        public int Num
        {
            get;
            set;
        }
        public string image
        {
            get;
            set;
        }

        public JustePDF()
        {
            InitializeComponent();
        }

        private void JustePDF_Load(object sender, EventArgs e)
        {
            gbJuste.Text = "Exercice : " + Num.ToString();
            labelJuste.Text = "Bravo vous avez réussi sans fautes !";
            pbJuste.Image = Image.FromFile(@"../../Photo/BasesJustes/" + image);
            pbJuste.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            gbJuste.Size =  new System.Drawing.Size(550, 150 );
            this.Size = new System.Drawing.Size(550, 150 );
        }
    }
}
