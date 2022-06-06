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
    public partial class UserControlExo4 : UserControl
    {
        public List<string> tabExo
        {
            get;
            set;
        }
        public UserControlExo4()
        {
            InitializeComponent();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {

            lblMotEspagnol.Text = tabExo[0];
            lblMotFrançais.Text = tabExo[1];
            lblOrigine.Text = tabExo[2];
            if (tabExo[3] == "")
            {
                pcbImageMot.Image = Image.FromFile(@"../../Photo/DrapeauCatalogne.png");
                pcbImageMot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

            }
            else
            {

                pcbImageMot.Image = Image.FromFile(@"../../Photo/baseImages/" + tabExo[3]);
                pcbImageMot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            }
        }

    }
}
