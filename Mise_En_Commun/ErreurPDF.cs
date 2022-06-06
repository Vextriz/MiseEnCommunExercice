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
    public partial class ErreurPDF : UserControl
    {
        public int Num
        {
            get;
            set;
        }
        public List<string> ls
        {
            get;
            set;
        }
        public string image
        {
            get;
            set;
        }
        public ErreurPDF()
        {
            InitializeComponent();
        }

        private void ErreurPDF_Load(object sender, EventArgs e)
        {
            groupBox1.Size = new System.Drawing.Size(550, 175 );
            this.Size = new System.Drawing.Size(550, 175 );
            groupBox1.Text = "Exercice n° " + Num.ToString();
            pictureBox1.Image = Image.FromFile(@"../../Photo/BaseFautes/" + image);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            int r = 0;
            int f = 0;
            for (int i = 0; i < ls.Count(); i++)
            {
                Label l = new Label();
                l.Location = new System.Drawing.Point(15, 40 + r);
                l.ForeColor = Color.Red;
                l.Text = ls[i];
                r = r + 25;
                f = f + 1;
                if (f == 2)
                {
                    r = r + 25;
                }
                if (f > 2)
                {
                    f = 0;
                    groupBox1.Size = new System.Drawing.Size(550, 150 + r);
                    this.Size = new System.Drawing.Size(550, 150 + r);
                }
                groupBox1.Controls.Add(l);

            }
        }
    }
}
