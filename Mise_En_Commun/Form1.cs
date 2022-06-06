using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Image = System.Drawing.Image;

namespace Mise_En_Commun
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        OleDbConnection connec = new OleDbConnection();
        string chcon = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=U:\A21\frm_EspagnolTrad-1.git\baseLangue.mdb";
        int exoNum = 0;
        // Exercice Conjugaison
        GroupBox Exercice = new GroupBox();
        GroupBox grbFr = new GroupBox();
        GroupBox grbEsp = new GroupBox();
        Button Valider = new Button();
        // Exercice Vocabulaire
        GroupBox groupBoxVoc = new GroupBox();
        Button Finalisation = new Button();
        Button Recommencer_Conjugaison = new Button();
        Button AideConjugaison = new Button();
        FlowLayoutPanel panelJuste = new FlowLayoutPanel();
        FlowLayoutPanel panelFautes = new FlowLayoutPanel();
        Button btnGenererPDF = new Button();
        Label ExoJuste = new Label();
        Label ExoFaux = new Label();
        DataSet dsLocal = new DataSet();
        // partiKarim
        Label lblTraductionEspagnol = new Label();
        Label lblPhraseATraduire = new Label();
        Label lblEnonce = new Label();
        Button btnCommencerExo = new Button();
        Button btnSolution = new Button();
        Button btnValider = new Button();
        DataSet DsLocalexo3 = new DataSet();
        /* Ceci nous servira à savoir combien de fois l'utilisateur à demander de l'aide (en réalité, l'utilisateur a le droit à 4 fois d'être aidé au max, mais par 
         soucis d'un label qui ne se positionne pas correctement, je le mets à 5 (+1)*/
        int cptAide = 3;




        private void Form1_Load(object sender, EventArgs e)
        {
            Finalisation.Click += new System.EventHandler(Exo_Suivant);
            Finalisation.Location = new System.Drawing.Point(1220, 678);
            Finalisation.Size = new System.Drawing.Size(120, 54);
            Finalisation.Text = "Finalisation de l\'exercice";
            Exercice.Location = new System.Drawing.Point(12, 89);
            Exercice.Size = new System.Drawing.Size(1345, 751);
            Exercice.BackColor = System.Drawing.Color.Transparent;
            Exercice.Font = new System.Drawing.Font("Noto Sans Lao", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Controls.Add(Finalisation);

        }
        public Dictionary<int, List<string>> dico()
        {
            List<string> ls = new List<string>();
            Dictionary<int, List<string>> lsf = new Dictionary<int, List<string>>();
            ls.Clear();
            lsf.Add(1, null);
            lsf.Add(4, null);
            lsf.Add(5, null);
            ls.Clear();
            lsf.Add(7, null);
            ls.Clear();
            lsf.Add(14, null);
            ls.Clear();
            lsf.Add(6, null);
            ls.Clear();
            lsf.Add(8, null);
            ls.Clear();
            lsf.Add(9, null);
            ls.Clear();
            lsf.Add(10, null);
            ls.Clear();
            lsf.Add(11, null);
            ls.Clear();
            lsf.Add(12, null);
            ls.Clear();
            lsf.Add(13, null);
            ls.Clear();

            ls.Add("Coma esto");
            ls.Add("Coma esta");
            ls.Add("Léo est chauve");
            ls.Add("Léo n'est pas chauve");
            lsf.Add(2, ls);
            List<string> lsd = new List<string>();

            lsd.Add("Mbappe va au Réal");
            lsd.Add("Mbappe reste au PSG");
            lsf.Add(3, lsd);
            return lsf;
        }
        private void Exo_Conjugaison()
        {
            Exercice.Controls.Add(Finalisation);

            //GroupBox grbFr = new GroupBox();
            grbFr.Location = new System.Drawing.Point(50, 50);
            grbFr.Size = new System.Drawing.Size(1200, 200);
            //  GroupBox grbEsp = new GroupBox();
            grbEsp.Location = new System.Drawing.Point(50, 280);
            grbEsp.Size = new System.Drawing.Size(1200, 300);
            Exercice.Controls.Add(grbEsp);
            Exercice.Controls.Add(grbFr);

            connec.ConnectionString = chcon;
            connec.Open();
            int NuméroExo = 2;
            int NuméroLeçon = 1;
            string filtre = " WHERE [ConcerneMots.numExo] = " + NuméroExo + " AND " +
               "[ConcerneMots.numLecon] = " + NuméroLeçon + "AND" +
               "[Exercices.codeVerbe] IS NOT NULL AND [Exercices.codetemps] > 0";
            string requêteESP = @"SELECT Mots.libMot
            FROM ((ConcerneMots INNER JOIN
                         Exercices ON ConcerneMots.numExo = Exercices.numExo) INNER JOIN
                         Mots ON ConcerneMots.numMot = Mots.numMot) " + filtre;
            OleDbCommand cd = new OleDbCommand();
            cd.Connection = connec;
            cd.CommandType = CommandType.Text;
            cd.CommandText = requêteESP;
            string motsESP = cd.ExecuteScalar().ToString();
            grbEsp.Text = motsESP;

            string requêteFR = @"SELECT Mots.traducMot
            FROM ((ConcerneMots INNER JOIN
                         Exercices ON ConcerneMots.numExo = Exercices.numExo) INNER JOIN
                         Mots ON ConcerneMots.numMot = Mots.numMot) " + filtre;
            cd.CommandText = requêteFR;
            string motsFR = cd.ExecuteScalar().ToString();
            grbFr.Text = motsFR;

            int nterm = -1;
            string term = motsESP.Substring(motsESP.Length - 2, 2);
            if (term == "ar")
            {
                nterm = 1;
            }
            else if (term == "er")
            {
                nterm = 2;
            }
            else
            {
                nterm = 3;
            }
            string requêteTemp = @"SELECT libPersonne,traducPersonne FROM Personne";
            OleDbCommand cd1 = new OleDbCommand(requêteTemp, connec);
            OleDbDataAdapter da = new OleDbDataAdapter();
            da.SelectCommand = cd1;
            DataTable dt = new DataTable();
            da.Fill(dt);
            int r = 0;
            int v = 0;
            int y = 0;
            int x = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (j == 1)
                    {
                        Label l = new Label();
                        TextBox t = new TextBox();
                        string so = dt.Rows[i][j].ToString() + " " + motsESP.Substring(0, motsESP.Length - 2);
                        l.Text = so;
                        t.Location = new System.Drawing.Point(205 + y, 64 + x);
                        l.Size = new System.Drawing.Size(155, 13);
                        l.Location = new System.Drawing.Point(43 + y, 64 + x);
                        y = y + 400;
                        if (y >= 1200)
                        {
                            x = x + 100;
                            y = 0;
                        }
                        grbEsp.Controls.Add(l);
                        grbEsp.Controls.Add(t);
                    }
                }
            }

            string requeteRecupTemps = @"SELECT Exercices.codetemps
            FROM ((ConcerneMots INNER JOIN
                         Exercices ON ConcerneMots.numExo = Exercices.numExo) INNER JOIN
                         Mots ON ConcerneMots.numMot = Mots.numMot) " + filtre;
            cd.CommandText = requeteRecupTemps;
            string codeTemps = cd.ExecuteScalar().ToString();
            string requeteFinal = @"SELECT term
            FROM Terminaisons
          WHERE [Groupe] = " + nterm + "AND [numTemps] = '" + codeTemps + "'";

            string RequeteLibTemps = @"SELECT libTemps 
            FROM Temps WHERE [codeTemps] = " + codeTemps + "";
            cd.CommandText = RequeteLibTemps;
            string LibTemps = cd.ExecuteScalar().ToString();
            Label LTemps = new Label();
            LTemps.Size = new System.Drawing.Size(300, 13);
            LTemps.Location = new System.Drawing.Point(43, 64);
            LTemps.Text = " Cet Exercices est au : " + LibTemps;
            grbFr.Controls.Add(LTemps);
            string RequeteTraducTemps = @"SELECT traducTemps 
            FROM Temps WHERE [codeTemps] = " + codeTemps + "";
            cd.CommandText = RequeteTraducTemps;
            string TraducTemps = cd.ExecuteScalar().ToString();
            Label TTemps = new Label();
            TTemps.Size = new System.Drawing.Size(300, 13);
            TTemps.Location = new System.Drawing.Point(743, 64);
            TTemps.Text = "Este ejercicio está en  : " + TraducTemps;
            grbFr.Controls.Add(TTemps);

            OleDbCommand cd2 = new OleDbCommand(requeteFinal, connec);
            OleDbDataAdapter da2 = new OleDbDataAdapter();
            da2.SelectCommand = cd2;
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            int u = 0;
            foreach (TextBox t in grbEsp.Controls.OfType<TextBox>())
            {
                t.Tag = dt2.Rows[u][0].ToString();
                // t.Text = dt2.Rows[u][0].ToString();
                u++;
            }

            AideConjugaison.Location = new System.Drawing.Point(50, 678);
            AideConjugaison.Size = new System.Drawing.Size(120, 54);
            AideConjugaison.Text = "Aide de l\'exercice";
            AideConjugaison.Click += new System.EventHandler(Aide_Conjugaison);
            Valider.Click += new System.EventHandler(Valider_Click);
            Valider.Location = new System.Drawing.Point(438, 626);
            Valider.Text = "Valider";
            Valider.Size = new System.Drawing.Size(308, 23);
            Recommencer_Conjugaison.Click += new System.EventHandler(Recommencer_Exo);
            Recommencer_Conjugaison.Location = new System.Drawing.Point(1100, 678);
            Recommencer_Conjugaison.Size = new System.Drawing.Size(120, 54);
            Recommencer_Conjugaison.Text = "Recommencer l\'exercice";
            Exercice.Controls.Add(Valider);
            Exercice.Controls.Add(AideConjugaison);

            Exercice.Controls.Add(Recommencer_Conjugaison);
            Exercice.Text = "Conjugaison";
            this.Controls.Add(Exercice);

            connec.Close();
        

        }
        private void Exerices_Vocabulaire()
        {
            this.Controls.Add(Exercice);
            Exercice.Controls.Add(Finalisation);
            
            Finalisation.Text = "Finalisation de l\'exercice";
            groupBoxVoc.BackColor = System.Drawing.Color.Transparent;
            groupBoxVoc.Font = new System.Drawing.Font("Noto Sans Lao", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            groupBoxVoc.Location = new System.Drawing.Point(50, 50);
            groupBoxVoc.Size = new System.Drawing.Size(1200, 418);
            groupBoxVoc.Text = "Vocabulaire";
            Exercice.Controls.Add(groupBoxVoc);

            connec.ConnectionString = chcon;
            connec.Open();
            int NuméroExo = 1;
            int NuméroLeçon = 2;
            string NuméroCours = "PAYSCULT";
            string filtre = " WHERE [ConcerneMots.numExo] = " + NuméroExo + " AND " +
                "[ConcerneMots.numLecon] = " + NuméroLeçon + "AND" +
                "[ConcerneMots.numCours] = '" + NuméroCours + "'";
            string RequeteNombreDeTrucACréer = @" SELECT count(*)  " +
                " FROM Mots INNER JOIN ConcerneMots ON Mots.numMot = ConcerneMots.numMot";
            OleDbCommand cd = new OleDbCommand();
            cd.Connection = connec;
            cd.CommandType = CommandType.Text;
            cd.CommandText = RequeteNombreDeTrucACréer;
            int NbTrucs = (int)cd.ExecuteScalar();

            string RequeteNomEspagnol = @" SELECT Mots.libMot, Mots.traducMot, Mots.origine, Mots.cheminPhoto  " +
               " FROM Mots INNER JOIN ConcerneMots ON Mots.numMot = ConcerneMots.numMot" + filtre;

            OleDbCommand cd1 = new OleDbCommand(RequeteNomEspagnol, connec);
            OleDbDataAdapter da = new OleDbDataAdapter();
            da.SelectCommand = cd1;
            List<string> tabExo4 = new List<string>();
            DataTable dt = new DataTable();
            da.Fill(dt);
            int r = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    tabExo4.Add(dt.Rows[i][j].ToString());
                   // MessageBox.Show(dt.Rows[i][j].ToString());
                }
                tabExo4.Add("Séparateur");
                UserControlExo4 us = new UserControlExo4();
                us.tabExo = tabExo4;
                //MessageBox.Show(us.tabExo[i].ToString() + " range : " + i);
                us.Location = new System.Drawing.Point(23 + r, 20);
                us.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                groupBoxVoc.Controls.Add(us);
                if (dt.Rows.Count == 2)
                {
                    r = r + 400;
                }
                else if (dt.Rows.Count == 3)
                {
                    r = r + 350;
                }
                else if (dt.Rows.Count == 4)
                {
                    r = r + 250;
                }
                else
                {
                    r = r + 135;
                }
                tabExo4.Clear();
            }
            connec.Close();
        }
        private void PDF()
        {
            ExoJuste.Location = new System.Drawing.Point(650, 60);
            ExoJuste.Size = new System.Drawing.Size(150, 17);
            ExoJuste.Text = "Exercice Juste";
            ExoJuste.ForeColor = Color.Green;
            ExoFaux.Location = new System.Drawing.Point(12, 60);
            ExoFaux.Size = new System.Drawing.Size(150, 17);
            ExoFaux.Text = "Exercice Faux";
            ExoFaux.ForeColor = Color.Red;
            panelJuste.Location = new System.Drawing.Point(650, 80);
            panelJuste.Size = new System.Drawing.Size(600, 500);
            panelJuste.BorderStyle = BorderStyle.Fixed3D;
            panelFautes.Location = new System.Drawing.Point(12, 80);
            panelFautes.Size = new System.Drawing.Size(600, 500);
            panelFautes.BorderStyle = BorderStyle.Fixed3D;
            btnGenererPDF.Location = new System.Drawing.Point(1150, 15);
            btnGenererPDF.Size = new System.Drawing.Size(100, 33);
            btnGenererPDF.Text = "Generer PDF";
            btnGenererPDF.Click += new System.EventHandler(btnGenererPDF_Click);
            List<string> ls = new List<string>();
            Dictionary<int, List<string>> lsf = dico();
            int r = 0;
            int v = 0;
            int nbimageE = 1;
            int nbimageJ = 1;
            foreach (KeyValuePair<int, List<string>> kvp in lsf)
            {
                if (kvp.Value != null)
                {
                    ErreurPDF epdf = new ErreurPDF();
                    epdf.Location = new System.Drawing.Point(15, 15 + r);
                    epdf.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                    epdf.Num = kvp.Key;
                    epdf.ls = kvp.Value;
                    epdf.image = "e" + nbimageE.ToString() + ".jpg";
                    panelFautes.Controls.Add(epdf);
                    for (int s = 0; s < kvp.Value.Count(); s++)
                    {
                        if (s > 2)
                            r = r + 100;
                    }
                    r = r + 125;
                    nbimageE = nbimageE + 1;
                    if (nbimageE > 4)
                    {
                        nbimageE = 1;
                    }
                }
                if (kvp.Value == null)
                {
                    JustePDF jpdf = new JustePDF();
                    jpdf.Location = new System.Drawing.Point(15, 15 );
                    jpdf.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                    jpdf.Num = kvp.Key;
                    jpdf.image = "j" + nbimageJ.ToString() + ".jpg";
                    panelJuste.Controls.Add(jpdf);
                   // v = v + 125;
                    nbimageJ = nbimageJ + 1;
                    if (nbimageJ > 4)
                    {
                        nbimageJ = 1;
                    }
                }

            }
            Exercice.Controls.Add(panelJuste);
            Exercice.Controls.Add(panelFautes);
            Exercice.Controls.Add(btnGenererPDF);
            Exercice.Controls.Add(ExoFaux);
            Exercice.Controls.Add(ExoJuste);
            panelJuste.AutoScroll = true;
            panelFautes.AutoScroll = true;
            Exercice.Text = "PDF";
            this.Controls.Add(Exercice);

        }
        private void btnGenererPDF_Click(object sender, EventArgs e)
        {
            //Bitmap b =  CaptureScreen();
            //var codeBitmap = new Bitmap(b);
            //Image image = (Image)codeBitmap;
            FileStream fs = new FileStream("Liste_des_erreurs.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            Document document = new Document(PageSize.A4);
            PdfWriter pdf = PdfWriter.GetInstance(document, fs);
            //iTextSharp.text.Image bof = iTextSharp.text.Image.GetInstance(image, System.Drawing.Imaging.ImageFormat.Jpeg);
            document.Open();
            //document.Add(bof);

            PdfContentByte pbtext = pdf.DirectContent;
            Dictionary<int, List<string>> lsf = dico();
            BaseFont bf = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.TTF", BaseFont.WINANSI, true);
            document.Add(new Paragraph("Voici la list de vos erreurs ainsi que leur correction"));
            foreach (KeyValuePair<int, List<string>> kvp in lsf)
            {
                if (kvp.Value != null)
                {
                    document.Add(new Paragraph("Exercice " + kvp.Key.ToString()));
                    for (int i = 0; i < kvp.Value.Count; i++)
                    {
                        document.Add(new Paragraph(kvp.Value[i]));
                    }
                }
            }
            document.Add(new Paragraph("Voici la liste des exercices que vous avez réussi sans commettre de fautes"));

            foreach (KeyValuePair<int, List<string>> kvp in lsf)
            {
                if (kvp.Value == null)
                {

                    document.Add(new Paragraph("Exercice " + kvp.Key.ToString()));
                    document.Add(new Paragraph("Exercice réussi bravo "));
                }
            }

            MessageBox.Show("Creation du PDF");

            document.Close();
        }
    
    private void Valider_Click(object sender, EventArgs e)
        {
            bool juste = false;
            bool gagné = false;
            int just = 0;
            foreach (TextBox c in grbEsp.Controls.OfType<TextBox>())
            {

                if (c.Text == c.Tag.ToString())
                {
                    c.Text = "Good";
                    //   c.BackColor = Color.Green;
                    juste = true;

                }
                if (c.Text == "Good")
                {
                    gagné = true;
                    c.BackColor = Color.Green;

                }
                else
                {
                    gagné = false;
                }
                if (c.Text != c.Tag.ToString() && c.Text != "Good" & c.Text is null)
                {
                    c.BackColor = Color.Red;
                }
            }
            if (juste == false)
            {
                MessageBox.Show("Erreur");
            }
            if (gagné == true)
            {
                MessageBox.Show("Bravo");
            }

        }
        private void Aide_Conjugaison(object sender, EventArgs e)
        {
            bool sort = true;
            foreach (TextBox c in grbEsp.Controls.OfType<TextBox>())
            {
                if (c.Text == "" && c.BackColor != Color.Green && c.Text != c.Tag.ToString() && sort == true)
                {
                    c.Text = c.Tag.ToString();
                    sort = false;
                }
               
            }
            Exercice.Controls.Remove(AideConjugaison);
        }

        // ExoKArim 
        
        private void btnCommencerExo_Click(object sender, EventArgs e)
        {

            connec.ConnectionString = chcon;

            RamenerToutesLesTablesEnLocale();
            //Pour vérifier si y a toutes les tables (si c'est 12 --> c'est bon)
            int nbTableDansDataSet = dsLocal.Tables.Count;
          //  MessageBox.Show("Nb de tables dans le DataSet : " + nbTableDansDataSet.ToString());

            // Rechercher l'énoncé
            // Pour ça, il faut d'abord savoir à quelle cours et quelle leçon je suis. Mais vu que jsp encore comment faire je le fais avec des trucs choisis moi
            // Je récup le nom cours (dans un string), le num (dans un int) de la leçon et le numéro de l'exo (dans un int)
            string cours = "GRAMM1";   // J'initialise avec des valeurs choisis par moi (non au hasard) pck je ne sais pas encore comment récup là où on s'est arrêté
            int numLecon = 2;
            int numExo = 4;
            // Une fois récup, je parcours toutes les ligne de la table Exercices
            for (int i = 0; i < dsLocal.Tables["Exercices"].Rows.Count; i++)
            {
                // "Si le cours récup est le même que dans la ligne ET le num de la leçon récup est le même que la ligne ET le num de l'exercice est le même que dans la ligne ALORS"
                if (cours == dsLocal.Tables["Exercices"].Rows[i]["numCours"].ToString() && numLecon.ToString() == dsLocal.Tables["Exercices"].Rows[i]["numLecon"].ToString() && numExo.ToString() == dsLocal.Tables["Exercices"].Rows[i]["numExo"].ToString())
                {
                    foreach (Label lbl in Exercice.Controls.OfType<Label>())
                    {
                        // Nous voulons changer seulement le label de l'énoncé
                        if (lbl.Name == "lblEnonce")
                        {
                            string enonce = dsLocal.Tables["Exercices"].Rows[i]["enonceExo"].ToString();
                            lbl.Text = "Énoncé : " + enonce;
                           // MessageBox.Show(enonce);
                        }
                    }
                }
            }


            // Rechercher une phrase à traduire
            // On récup le num du cours, leçon etc... (déjà fais en haut)
            //Ensuite je parcours la table Exercices et je récup le codePhrase
            int codePhrase = 0; // Tjrs initialiser à 0
            for (int i = 0; i < dsLocal.Tables["Exercices"].Rows.Count; i++)
            {
                // "Si le cours récup est le même que dans la ligne ET le num de la leçon récup est le même que la ligne ET le num de l'exercice est le même que dans la ligne ALORS"
                if (cours == dsLocal.Tables["Exercices"].Rows[i]["numCours"].ToString() && numLecon.ToString() == dsLocal.Tables["Exercices"].Rows[i]["numLecon"].ToString() && numExo.ToString() == dsLocal.Tables["Exercices"].Rows[i]["numExo"].ToString())
                {
                    codePhrase = (int)dsLocal.Tables["Exercices"].Rows[i]["codePhrase"];
                }
            }

            string phraseEnFrancais = "";
            string traductionPhraseEnEspagnol = "";

            //Parcourt de toutes les lignes de la table "Phrases"
            for (int i = 0; i < dsLocal.Tables["Phrases"].Rows.Count; i++)
            {
                if ((int)dsLocal.Tables["Phrases"].Rows[i]["codePhrase"] == codePhrase)
                {
                    phraseEnFrancais = dsLocal.Tables["Phrases"].Rows[i]["traducPhrase"].ToString();
                    traductionPhraseEnEspagnol = dsLocal.Tables["Phrases"].Rows[i]["textePhrase"].ToString();
                }
            }

            //  Affichage de la phrase à traduire
            foreach (Label lbl in Exercice.Controls.OfType<Label>())
            {
                if (lbl.Name == "lblPhraseATraduire")
                {
                    lbl.Text = "Veuillez traduire la phrase suivante : " + phraseEnFrancais;
                }
            }

            //Affichage de la phrase en espagnol
            AffichePhraseEspagnolACompleter(traductionPhraseEnEspagnol);       // CHANGER LE NOM DE LA PROCEDURE
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        // Déclaration en globale pck je l'utilise dans d'autres évènements :
        // Nous stockons dedans tous les mots AVEC les caractères comme une virgule ou un point à la fin du mot
        string[] motsPhraseEspagnol;
        // Nous stockons dedans tous les mots SANS les caractères comme une virgule ou un point à la fin du mot. Utilisable QUE pour les TextBox
        List<string> nouveauTableau = new List<string>();
        List<string> listeMotsEspagnolCaches = new List<string>();

        public void AffichePhraseEspagnolACompleter(string XtraductionPhraseEnEspagnol)
        {

            // Tableau de string contenant tous les mots de la phrase en espagnol (utiliser la méthode split)
            motsPhraseEspagnol = XtraductionPhraseEnEspagnol.Split(' ');

            // Parcourt des mots de la phrase en espagnol pour ranger dans une liste tous les mots SANS les caractères comme une virgule ou un point à la fin du mot.
            for (int i = 0; i < motsPhraseEspagnol.Length; i++)
            {
                string motActu = motsPhraseEspagnol[i];
                string nouveauMot = "";     // Représente le mot que nous allons stocker dans le tableau NE contenant PAS de caractères tel qu'une virgule ou un point à la fin du mot

                if (motActu[motActu.Length - 1] == ',')  // Dernière lettre == ','
                {
                    // Je parcours les lettres du mots sauf la dernière
                    for (int j = 0; j < motActu.Length - 1; j++)
                    {
                        nouveauMot = nouveauMot + motActu[j];
                    }

                    nouveauTableau.Add(nouveauMot);
                    nouveauTableau.Add(",");
                }
                else if (motActu[motActu.Length - 1] == '.')
                {
                    // Je parcours les lettres du mots sauf la dernière
                    for (int j = 0; j < motActu.Length - 1; j++)
                    {
                        nouveauMot = nouveauMot + motActu[j];
                    }

                    nouveauTableau.Add(nouveauMot);
                    nouveauTableau.Add(".");
                }
                else
                {
                    nouveauMot = motActu;
                    nouveauTableau.Add(nouveauMot);
                }



            }

            int nbMotsDansPhraseEspagnol = nouveauTableau.Count;
            // Déclaration + initialisation d'une variable aléatoire qui servira à cacher une des valeur dans la phrase
            Random aleatoire = new Random();
            int posAleatoire = aleatoire.Next(0, nbMotsDansPhraseEspagnol);

            // Il ne faudrait pas que le mot caché soit une virgule ou un point etc... --> donc on tire un numéro aléatoirement jusqu'à qu'on obtient un mot
            while (nouveauTableau[posAleatoire] == "," || nouveauTableau[posAleatoire] == "." || nouveauTableau[posAleatoire] == ":")
            {
                posAleatoire = aleatoire.Next(0, nbMotsDansPhraseEspagnol);
            }

            // Une fois que nous avons fixé la 1ère position aléatoire, on fixe la 2ème
            int posAleatoire2 = aleatoire.Next(0, nbMotsDansPhraseEspagnol);
            while (posAleatoire2 == posAleatoire || nouveauTableau[posAleatoire2] == "," || nouveauTableau[posAleatoire2] == "." || nouveauTableau[posAleatoire2] == ":")
            {
                posAleatoire2 = aleatoire.Next(0, nbMotsDansPhraseEspagnol);
            }

            MessageBox.Show("La 1ère position aléatoire est : " + posAleatoire.ToString());     // Test
            MessageBox.Show("La 2ème position aléatoire est : " + posAleatoire2.ToString());     // Test


            //Position de ma TextBox si elle est en 1ère position
            int leftTxt = 406;
            int topTxt = 531;

            //Position de mon label s'il est en 1ère position
            int leftLabel = 406;
            int topLabel = 538;

            // Représente le tag des TextBox (afin de les différencier)
            int valTag = 0;

            for (int i = 0; i < nouveauTableau.Count; i++)
            {
                string motActuel = nouveauTableau[i];
                // "Si la position actuelle du tableau que l'on parcourt  est différent de la position aléatoire"
                if (i != posAleatoire && i != posAleatoire2)
                {
                    for (int j = 0; j < nouveauTableau.Count; j++)      // J'AI CHANGE ICI LA LENGTH
                    {
                        if (i == j)
                        {

                            // Génération d'un label dynamiquement

                            Label lblPhraseTraductionEspagnol = new Label();

                            lblPhraseTraductionEspagnol.Left = leftLabel;
                            lblPhraseTraductionEspagnol.Top = topLabel;
                            lblPhraseTraductionEspagnol.Text = motActuel;
                            lblPhraseTraductionEspagnol.AutoSize = true;

                            // Ajout à la collection des Controls du form
                            Exercice.Controls.Add(lblPhraseTraductionEspagnol);


                            // Nous n'allons pas incrémenter la même chose suivant la nature du prochain composant (Label ou TextBox)
                            int posProchainComposant = i + 1;
                            if (posProchainComposant == posAleatoire || posProchainComposant == posAleatoire2)  // Si le prochain composant est une TextBox
                            {
                                // On incrémente la position de la TextBox
                                int longueurLabel = lblPhraseTraductionEspagnol.Width;
                                leftTxt = leftLabel + longueurLabel + 10;
                            }
                            else
                            {
                                int longueurLabel = lblPhraseTraductionEspagnol.Width;
                                leftLabel = leftLabel + longueurLabel + 10;
                            }

                        }

                    }







                }
                // On a atteint la position aléatoire --> au lieu d'écrire le mot, on va mettre une TextBox à la place
                else if (i == posAleatoire)
                {
                    string motCache = nouveauTableau[i];
                    listeMotsEspagnolCaches.Add(motCache);

                    for (int j = 0; j < nouveauTableau.Count; j++)
                    {
                        if (i == j)
                        {
                            // Génération d'une TextBox dynamiquement :
                            TextBox txtMotATrouver = new TextBox();
                            // Paramétrage de la TextBox
                            txtMotATrouver.Top = topTxt;
                            txtMotATrouver.Left = leftTxt;
                            txtMotATrouver.Size = new System.Drawing.Size(100, 35);
                            txtMotATrouver.Tag = valTag;       // Pour pouvoir différencier les TextBox puisqu'elles ne vont pas avoir le même traitement

                            // Génération d'un évènement KeyPress dynamiquement afin de pouvoir interdir certains caractères
                            txtMotATrouver.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtMotATrouver_KeyPress);

                            // Ajout à la collection des Controls du formulaire (pour l'afficher)
                            Exercice.Controls.Add(txtMotATrouver);

                            // Incrémentation du tag pour la prochaine TextBox
                            valTag++;

                            // Nous n'allons pas incrémenter la même chose suivant la nature du prochain composant (Label ou TextBox)
                            int posProchainComposant = i + 1;
                            if (posProchainComposant == posAleatoire || posProchainComposant == posAleatoire2)  // Si le prochain composant est une TextBox
                            {
                                // On incrémente la position de la TextBox
                                int longueurTextBox = txtMotATrouver.Width; // à déclarer en haut pck je l'initialise 2 fois
                                leftTxt = leftTxt + longueurTextBox + 10;      // Besoin de stocker la longueur (pas la largeur) du label
                            }
                            else
                            {
                                int longueurTextBox = txtMotATrouver.Width;
                                leftLabel = leftTxt + longueurTextBox + 10;
                            }

                        }

                    }



                }
                else if (i == posAleatoire2)
                {
                    string motCache = nouveauTableau[i];
                    listeMotsEspagnolCaches.Add(motCache);

                    for (int j = 0; j < nouveauTableau.Count; j++)
                    {
                        if (i == j)
                        {
                            // Génération d'une TextBox dynamiquement :
                            TextBox txtMotATrouver = new TextBox();
                            // Paramétrage de la TextBox
                            txtMotATrouver.Top = topTxt;
                            txtMotATrouver.Left = leftTxt;
                            txtMotATrouver.Size = new System.Drawing.Size(100, 35);
                            txtMotATrouver.Tag = valTag;       // Pour pouvoir différencier les TextBox puisqu'elles ne vont pas avoir le même traitement

                            // Génération d'un évènement KeyPress dynamiquement afin de pouvoir interdir certains caractères
                            txtMotATrouver.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtMotATrouver_KeyPress);

                            // Ajout à la collection des Controls du formulaire (pour l'afficher)
                            Exercice.Controls.Add(txtMotATrouver);

                            // Incrémentation du tag pour la prochaine TextBox
                            valTag++;

                            // Nous n'allons pas incrémenter la même chose suivant la nature du prochain composant (Label ou TextBox)
                            int posProchainComposant = i + 1;
                            if (posProchainComposant == posAleatoire || posProchainComposant == posAleatoire2)  // Si le prochain composant est une TextBox
                            {
                                // On incrémente la position de la TextBox
                                int longueurTextBox = txtMotATrouver.Width; // à déclarer en haut pck je l'initialise 2 fois
                                leftTxt = leftTxt + longueurTextBox + 10;
                            }
                            else
                            {
                                int longueurTextBox = txtMotATrouver.Width;
                                leftLabel = leftTxt + longueurTextBox + 10;
                            }

                        }

                    }



                }


            }

        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public void RamenerToutesLesTablesEnLocale()
        {
            // On ouvre la connexion (obligé sinon ne fonctionne pas)
            connec.Open();

            //Copié sur internet (sauf le "connec") ; dans la 3ème colonne ce tableau on a le nom des toutes les tables (voir sujet)
            DataTable schemaTable = connec.GetOleDbSchemaTable(
            OleDbSchemaGuid.Tables,
            new object[] { null, null, null, "TABLE" });

            // Ferme la co
            connec.Close();


            foreach (DataRow dr in schemaTable.Rows)
            {
                // Je récup le nom de la table
                string nomTable = dr[2].ToString();

                // Je formule ma requête pour récup les données de la table
                string requete = @"SELECT * FROM " + nomTable;

                OleDbCommand cd = new OleDbCommand();
                cd.Connection = connec;
                cd.CommandType = CommandType.Text;
                cd.CommandText = requete;

                // Mtn on veut charger les données dans le DataSet local donc on crée un DataAdapter
                OleDbDataAdapter da = new OleDbDataAdapter();
                da.SelectCommand = cd;
                da.Fill(dsLocal, nomTable);
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private void btnValider_Click(object sender, EventArgs e)
        {
            // A REMPLACER PAR UNE FONCTION PCK JE L'ECRIS UNE 2EME FOIS EN BAS (KeyPress des TextBox)
            ReponsesJustesOuFausses();
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public void ReponsesJustesOuFausses()
        {
            string texteBrute;
            string texteBruteSansAccents;
            string texteUtilisateurFinal = "";
            // On vérifie si la réponse entré par l'utilisateur est juste
            // Quand je clique sur le bouton, je dois vérifier pour chaque TextBox si la réponse est juste
            bool justeOuPas = false;

            for (int i = 0; i < listeMotsEspagnolCaches.Count; i++)      // Pour que les modifications qui vont suivre ne se fassent que sur UNE seule TextBox et pas toutes les autres (grâce au if qui utilise le Tag)
            {
                foreach (TextBox t in Exercice.Controls.OfType<TextBox>())      // Faut essayer de régler le problème de casse
                {
                    // Le code suivant est fait pour gérer les problèmes de casses

                    texteBrute = t.Text;
                    texteBruteSansAccents = EnleverAccents(texteBrute.Trim());

                    // Si le mot entré par l'utilisateur est le premier mot de la phrase
                    string premierMot = EnleverAccents(nouveauTableau[0].Trim().ToLower());
                    if (texteBruteSansAccents.ToLower() == premierMot)
                    {
                        // Tous les caractères sont stockés dans un tableau 1D
                        char[] tabTexteSplit = texteBruteSansAccents.ToLower().ToCharArray();

                        // Transformation de la 1ère lettre en majuscule
                        texteUtilisateurFinal = tabTexteSplit[0].ToString().ToUpper();

                        // Concaténation de toutes les lettres afin de former le mot avec la bonne casse
                        int nbLettre = tabTexteSplit.Length;
                        for (int j = 1; j < nbLettre; j++)
                        {
                            texteUtilisateurFinal += tabTexteSplit[j];
                        }
                    }
                    else
                    {
                        // Nous mettons tout en minuscule (sachant que les accents ont déjà été enlevé)
                        texteUtilisateurFinal = texteBruteSansAccents.ToLower();
                    }

                    if (t.Tag.ToString() == i.ToString())
                    {
                        if (texteUtilisateurFinal == EnleverAccents(listeMotsEspagnolCaches[i].Trim()))
                        {
                            justeOuPas = true;
                        }
                        else
                        {
                            justeOuPas = false;
                        }

                        if (justeOuPas == true)
                        {
                            // Si la réponse est juste alors : couleur d'arrière plan = vert
                            t.BackColor = System.Drawing.Color.LawnGreen;
                        }
                        else
                        {
                            // Si la réponse est fausse alors : couleur d'arrière plan = rouge
                            t.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
                        }
                    }

                }

            }

        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private void btnSolution_Click(object sender, EventArgs e)
        {
            AfficheSolution();
        }


        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public void AfficheSolution()
        {
            int nbMotsCaches = listeMotsEspagnolCaches.Count;

            string reponse;

            for (int i = 0; i < nbMotsCaches; i++)
            {
                foreach (TextBox t in Exercice.Controls.OfType<TextBox>())
                {
                    reponse = listeMotsEspagnolCaches[i];

                    if (t.Tag.ToString() == i.ToString())
                    {
                        // Sous forme de MessageBox comme ça l'utilisateur pourra analyser son/ses erreur(s)
                        MessageBox.Show("Réponse de la case n°" + (i + 1).ToString() + " : " + reponse);        // Fonctionne 100% avec plusieurs cases j'ai très bien vérifié te casse pas la tête dessus si y a un problème
                    }
                }
            }

        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void txtMotATrouver_KeyPress(object sender, KeyPressEventArgs e)
        {
            // On vérifie si ce n'est pas une lettre
            if (!(char.IsLetter(e.KeyChar)))
            {
                // Si j'appuis sur entrer faut que je fasse en sorte que ça me renvoie sur l'évènement btnValider_Click
                if (e.KeyChar == (char)Keys.Enter)
                {
                    ReponsesJustesOuFausses();
                }
                // On vérifie si c'est la flèche de retour
                else if (!(e.KeyChar == (char)Keys.Back))
                {
                    e.Handled = true;
                }
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public string EnleverAccents(string Xchaine)
        {
            // Déclaration de variables
            string accent = "ÀÁÂÃÄÅàáâãäåÒÓÔÕÖØòóôõöøÈÉÊËèéêëÌÍÎÏìíîïÙÚÛÜùúûüÿÑñÇç";
            string sansAccent = "AAAAAAaaaaaaOOOOOOooooooEEEEeeeeIIIIiiiiUUUUuuuuyNnCc";

            // Conversion des chaines en tableaux de caractères
            char[] tableauSansAccent = sansAccent.ToCharArray();
            char[] tableauAccent = accent.ToCharArray();

            // Pour chaque accent
            for (int i = 0; i < accent.Length; i++)
            {
                // Remplacement de l'accent par son équivalent sans accent dans la chaîne de caractères
                Xchaine = Xchaine.Replace(tableauAccent[i].ToString(), tableauSansAccent[i].ToString());    // à changer à ma manière
            }

            // Retour du résultat
            return Xchaine;
        }

        private void Exo_Mot_à_trou()
        {
           //Génération dynamique du bouton commencer exo/
            btnCommencerExo.Name = "btnCommencerExo";
            btnCommencerExo.Left = 948;
            btnCommencerExo.Top = 40;
            btnCommencerExo.Text = "Commencer exo";
            btnCommencerExo.Width = 335;
            btnCommencerExo.Height = 149;
            // Génération de l'évènement
            btnCommencerExo.Click += new System.EventHandler(btnCommencerExo_Click);
            // Ajout au form
            Exercice.Controls.Add(btnCommencerExo);

           //Génération dynamique du bouton valider/
            btnValider.Name = "btnValider";
            btnValider.Left = 987;
            btnValider.Top = 640;
            btnValider.Text = "Valider";
            btnValider.Width = 156;
            btnValider.Height = 68;
            // Génération de l'évènement
            btnValider.Click += new System.EventHandler(btnValider_Click);
            // Ajout au form
            Exercice.Controls.Add(btnValider);

           //Génération dynamique du bouton Solution/ //796; 640 156; 68
            btnSolution.Name = "btnSolution";
            btnSolution.Left = 796;
            btnSolution.Top = 640;
            btnSolution.Text = "Solution";
            btnSolution.Width = 156;
            btnSolution.Height = 68;
            // Génération de l'évènement
            btnSolution.Click += new System.EventHandler(btnSolution_Click);
            // Ajout au form
            Exercice.Controls.Add(btnSolution);

            // Génération dynamique du label contenant l'énoncé/
            lblEnonce.Name = "lblEnonce";
            lblEnonce.Left = 193;
            lblEnonce.Top = 277;
            lblEnonce.AutoSize = true;
            lblEnonce.Text = "Énoncé : ";
            // Ajout au form
            Exercice.Controls.Add(lblEnonce);

           // Génération dynamique du label contenant la phrase qu'il faudra traduire/
            lblPhraseATraduire.Name = "lblPhraseATraduire";
            lblPhraseATraduire.Left = 193;
            lblPhraseATraduire.Top = 422;
            lblPhraseATraduire.AutoSize = true;
            lblPhraseATraduire.Text = "Phrase à traduire :  ";
            // Ajout au form
            Exercice.Controls.Add(lblPhraseATraduire);

           // Génération dynamique du label contenant la phrase qu'il faudra traduire/
            lblTraductionEspagnol.Name = "lblTraductionEspagnol";
            lblTraductionEspagnol.Location = new System.Drawing.Point(12,50);
            lblTraductionEspagnol.AutoSize = true;
            lblTraductionEspagnol.Text = "Traduction en espagnol : ";
            // Ajout au form
            Exercice.Controls.Add(lblTraductionEspagnol);
            Exercice.Controls.Add(Finalisation);
            Exercice.Text = "Mot à trou";
            this.Controls.Add(Exercice);



        }
        private void btnCommencerExo_Click3(object sender, EventArgs e)
        {
            connec.ConnectionString = chcon;

            RamenerToutesLesTablesEnLocale3();
            //Pour vérifier si y a toutes les tables (si c'est 12 --> c'est bon)
            int nbTableDansDataSet = DsLocalexo3.Tables.Count;
          //  MessageBox.Show("Nb de tables dans le DataSet : " + nbTableDansDataSet.ToString());

            // Rechercher l'énoncé
            // Pour ça, il faut d'abord savoir à quelle cours et quelle leçon je suis. Mais vu que jsp encore comment faire je le fais avec des trucs choisis moi
            // Je récup le nom cours (dans un string), le num (dans un int) de la leçon et le numéro de l'exo (dans un int)
            string cours = "DEBUT1";   // J'initialise avec des valeurs choisis par moi (non au hasard) pck je ne sais pas encore comment récup là où on s'est arrêté
            int numLecon = 5;
            int numExo = 8;

            // Une fois récup, je parcours toutes les ligne de la table Exercices pour trouver la ligne (dans la B.D.) correspondant au cours, leçon et exercice actuel
            for (int i = 0; i < DsLocalexo3.Tables["Exercices"].Rows.Count; i++)
            {
                // "Si le cours récup est le même que dans la ligne ET le num de la leçon récup est le même que la ligne ET le num de l'exercice est le même que dans la ligne ALORS"
                if (cours == DsLocalexo3.Tables["Exercices"].Rows[i]["numCours"].ToString() && numLecon.ToString() == DsLocalexo3.Tables["Exercices"].Rows[i]["numLecon"].ToString() && numExo.ToString() == DsLocalexo3.Tables["Exercices"].Rows[i]["numExo"].ToString())
                {
                    foreach (Label lbl in Exercice.Controls.OfType<Label>())
                    {
                        if (lbl.Name == "lblEnonce")
                        {
                            lbl.Text = "Énoncé : " + DsLocalexo3.Tables["Exercices"].Rows[i]["enonceExo"].ToString() + ".";
                        }
                    }
                }
            }

            // Rechercher une phrase à traduire
            // On récup le num du cours, leçon etc... (déjà fais en haut)
            //Ensuite je parcours la table Exercices et je récup le codePhrase
            int codePhrase = 0; // Tjrs initialiser à 0
            for (int i = 0; i < DsLocalexo3.Tables["Exercices"].Rows.Count; i++)
            {
                // "Si le cours récup est le même que dans la ligne ET le num de la leçon récup est le même que la ligne ET le num de l'exercice est le même que dans la ligne ALORS"
                if (cours == DsLocalexo3.Tables["Exercices"].Rows[i]["numCours"].ToString() && numLecon.ToString() == DsLocalexo3.Tables["Exercices"].Rows[i]["numLecon"].ToString() && numExo.ToString() == DsLocalexo3.Tables["Exercices"].Rows[i]["numExo"].ToString())
                {
                    codePhrase = (int)DsLocalexo3.Tables["Exercices"].Rows[i]["codePhrase"];
                }
            }

            string phraseEnFrancais = "";
            string traductionPhraseEnEspagnol = "";

            //Parcourt de toutes les lignes de la table "Phrases"
            for (int i = 0; i < DsLocalexo3.Tables["Phrases"].Rows.Count; i++)
            {
                if ((int)DsLocalexo3.Tables["Phrases"].Rows[i]["codePhrase"] == codePhrase)
                {
                    phraseEnFrancais = DsLocalexo3.Tables["Phrases"].Rows[i]["traducPhrase"].ToString();
                    traductionPhraseEnEspagnol = DsLocalexo3.Tables["Phrases"].Rows[i]["textePhrase"].ToString();
                }
            }

            //  Affichage de la phrase à traduire
            foreach (Label lbl in Exercice.Controls.OfType<Label>())
            {
                if (lbl.Name == "lblPhraseEnFrancais")
                {
                    lbl.Text = "Traduction française de la phrase en espagnole à réordonner : " + phraseEnFrancais;
                }
            }

            // Affichage des label désordonnés
            AffichageLabelDesordonnes(traductionPhraseEnEspagnol);

            // Affichage du nombre d'aide(s) disponible pour l'utilisateur
            foreach (Label lbl in Exercice.Controls.OfType<Label>())
            {
                if (lbl.Name == "lblNbAideDispo")
                {
                    lbl.Text = "Nombre d'aide(s) disponible : " + (cptAide).ToString();
                }
            }


        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        // Création d'un tableau qui contiendra dans chaque case un mot de la phrase en espagnole
        string[] motsPhraseEspagnolExo3;
        List<string> motsPhraseEspagnolExo3Desordonnes = new List<string>();
        List<int> val_aleatoire_deja_utilise = new List<int>();

        // Déclaration en globale puisqu'on l'utilise dans d'autres fonctions ; à causqe de ça ça me bouge que le dernier
        //Label lblPhraseTraductionEspagnol;

        public void AffichageLabelDesordonnes(string XtraductionPhraseEnEspagnol)
        {
            motsPhraseEspagnolExo3 = XtraductionPhraseEnEspagnol.Split(' ');
            // Test : je vérifie si le tableau contient bien tous les mots
            for (int i = 0; i < motsPhraseEspagnolExo3.Length; i++)
            {
                string motActuelle = motsPhraseEspagnolExo3[i];
              //  MessageBox.Show(motActuelle);
            }

            // Pour chaque mot dans le tableau "motsPhraseEspagnolExo3", on crée un label à une certaine position ET un sous-panel
            int leftLabel = 29;
            int topLabel = 515;

            int left_sous_panel = 17;
            int top_sous_panel = 21;
            int val_tag_sous_panel = 0;
            foreach (string motEspagnol in motsPhraseEspagnolExo3)
            {
                // petit test des familles : MessageBox.Show(motEspagnol);
                /*//Création des Label
                lblPhraseTraductionEspagnol = new Label();
                lblPhraseTraductionEspagnol.Left = leftLabel;
                lblPhraseTraductionEspagnol.Top = topLabel;
                lblPhraseTraductionEspagnol.Name = "lblPhraseTraductionEspagnol";
                lblPhraseTraductionEspagnol.Text = motEspagnol;
                lblPhraseTraductionEspagnol.BackColor = System.Drawing.Color.Orange;
                lblPhraseTraductionEspagnol.Size = new System.Drawing.Size(155, 34);
                lblPhraseTraductionEspagnol.Tag = val_tag_label;    // nous le redéfinirons après pour récup les réponses
                //MessageBox.Show("TabIndex label : " + lblPhraseTraductionEspagnol.TabIndex.ToString());
                val_tag_label++;

                ControlExtension.Draggable(lblPhraseTraductionEspagnol, true);
                //Code dans le test cliquer glisser  //Pour que le Label puisse bouger
                //lblPhraseTraductionEspagnol.MouseDown += new System.Windows.Forms.MouseEventHandler(lblPhraseTraductionEspagnol_MouseDown);
                //lblPhraseTraductionEspagnol.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblPhraseTraductionEspagnol_MouseMove);
                // Génération de son évènement "Move" dynamiquement
                lblPhraseTraductionEspagnol.Move += new System.EventHandler(lblPhraseTraductionEspagnol_Move);

                // Ajout à la collection de Control du panel
                this.Controls.Add(lblPhraseTraductionEspagnol);*/


                //Création des sous-panel
                Panel sous_panel = new Panel();
                sous_panel.Left = left_sous_panel;
                sous_panel.Top = top_sous_panel;
                sous_panel.Size = new System.Drawing.Size(155, 34);
                sous_panel.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
                sous_panel.Name = "sous_panel";
                sous_panel.Tag = val_tag_sous_panel;
                sous_panel.SendToBack();
                //MessageBox.Show("TabIndex sous-panel = " + sous_panel.TabIndex.ToString());
                val_tag_sous_panel++;   //Nous incrémentons directement la valeur du Tag du prochain sous panel

                //Ajout à la collection des contrôles
                Exercice.Controls.Add(sous_panel);

                // Incrémentation pour le prochain sous panel
                if (left_sous_panel >= 776)
                {
                    top_sous_panel = top_sous_panel + 58;
                    left_sous_panel = 17;
                }
                else
                {
                    left_sous_panel = left_sous_panel + 192;
                }
            }


            foreach (string motEspagnol in motsPhraseEspagnolExo3)
            {
                //Création des Label
                generer_label_desordonnes(leftLabel, topLabel, motEspagnol);

                // Incrémentation du Left et du Top pour le prochain label
                if (leftLabel >= 797)
                {
                    // Nous avons atteint la limite --> réinitialisation du Left et incrémentation la hauteur
                    topLabel = topLabel + 58;
                    leftLabel = 29;
                }
                else
                {
                    // Nous incrémentons seulement la Left
                    leftLabel = leftLabel + 192;
                }
            }


        }

        int valTagLabel = 0;
        public void generer_label_desordonnes(int XleftLabel, int XtopLabel, string XmotEspagnol)
        {
            Label lblPhraseTraductionEspagnol = new Label();
            lblPhraseTraductionEspagnol.Left = XleftLabel;
            lblPhraseTraductionEspagnol.Top = XtopLabel;
            lblPhraseTraductionEspagnol.Name = "lblPhraseTraductionEspagnol";   
            lblPhraseTraductionEspagnol.BackColor = System.Drawing.Color.Orange;
            lblPhraseTraductionEspagnol.Size = new System.Drawing.Size(155, 34);
            lblPhraseTraductionEspagnol.Tag = valTagLabel;
            valTagLabel++;

            ControlExtension.Draggable(lblPhraseTraductionEspagnol, true);
            //Code dans le test cliquer glisser  //Pour que le Label puisse bouger
            //lblPhraseTraductionEspagnol.MouseDown += new System.Windows.Forms.MouseEventHandler(lblPhraseTraductionEspagnol_MouseDown);
            //lblPhraseTraductionEspagnol.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblPhraseTraductionEspagnol_MouseMove);
            // Génération de son évènement "Move" dynamiquement
            lblPhraseTraductionEspagnol.Move += new System.EventHandler(lblPhraseTraductionEspagnol_Move);

            int nbLabel = motsPhraseEspagnolExo3.Length - 1;
            Random rnd = new Random();
            int valAleatoire = rnd.Next(0, (nbLabel + 1));

            // Tant que la valeur tiré est déjà utilisée, nous retirons une autre valeur
            while (val_aleatoire_deja_utilise.Contains(valAleatoire))
            {
                valAleatoire = rnd.Next(0, (nbLabel + 1));
            }
            val_aleatoire_deja_utilise.Add(valAleatoire);


            //Nous ajoutons dans la nouvelle liste le texte présent à la position "valAleatoire" du tableau où les phrases sont dans l'ordre
            string mot_a_ajouter = motsPhraseEspagnolExo3[valAleatoire];
            motsPhraseEspagnolExo3Desordonnes.Add(mot_a_ajouter);
            lblPhraseTraductionEspagnol.Text = mot_a_ajouter;

            // Ajout à la collection de Control du panel
            Exercice.Controls.Add(lblPhraseTraductionEspagnol);
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public void RamenerToutesLesTablesEnLocale3()
        {
            // On ouvre la connexion (obligé sinon ne fonctionne pas)
            connec.Open();

            //Copié sur internet (sauf le "connec") ; dans la 3ème colonne ce tableau on a le nom des toutes les tables (voir sujet)
            DataTable schemaTable = connec.GetOleDbSchemaTable(
            OleDbSchemaGuid.Tables,
            new object[] { null, null, null, "TABLE" });

            // Ferme la co
            connec.Close();


            foreach (DataRow dr in schemaTable.Rows)
            {
                // Je récup le nom de la table
                string nomTable = dr[2].ToString();

                // Je formule ma requête pour récup les données de la table
                string requete = @"SELECT * FROM " + nomTable;

                OleDbCommand cd = new OleDbCommand();
                cd.Connection = connec;
                cd.CommandType = CommandType.Text;
                cd.CommandText = requete;

                // Mtn on veut charger les données dans le DataSet local donc on crée un DataAdapter
                OleDbDataAdapter da = new OleDbDataAdapter();
                da.SelectCommand = cd;
                da.Fill(DsLocalexo3, nomTable);
            }
        }


        List<Panel> sous_panel_deja_cache = new List<Panel>();
        List<Panel> sous_panel_deja_occupe = new List<Panel>();
        List<Label> label_qui_vient_de_sortir_dun_sous_panel = new List<Label>();    // Il n'y en a qu'un seul à la fois, je vais donc l'effacer plusieurs fois
        private void lblPhraseTraductionEspagnol_Move(object sender, EventArgs e)
        {

            /*Maintenant, nous voulons faire en sorte que lorsque le label est très proche d'un sous panel, alors nous insérons directement le label dans le sous panel*/
            foreach (Panel sous_panel in Exercice.Controls.OfType<Panel>())
            {
                foreach (Label lbl in Exercice.Controls.OfType<Label>())
                {
                    /*if (bonneZonePourCacherSousPanel(lbl, sous_panel) == true)
                    {
                        //MessageBox.Show("Bonne zone pour cacher");
                        if (!sous_panel_deja_cache.Contains(sous_panel))
                        {
                            sous_panel.Visible = false;
                            sous_panel_deja_cache.Add(sous_panel);
                        }
                    }
                    else
                    {
                        sous_panel.Visible = true;
                    }*/

                    // Condition nécessaire pour rentrer le lbl dans sous panel
                    if (bonneZonePourInsererDansSousPanel(lbl, sous_panel) == true)
                    {
                        // Si je trouve pas comment régler le problème (quand je retire un label d'un sous panel et je le remet dedans ça me dit qu'il est déjà occupé), j'enlève la condition seulement
                        //if (!sous_panel_deja_occupe.Contains(sous_panel))
                        //{

                        //Rentrer label dans sous panel
                        rentrerLabelDansSousPanel(lbl, sous_panel);
                        //sous_panel_deja_occupe.Add(sous_panel);     // PEUT ETRE CHANGER POUR METTRE LE TAG A LA PLACE
                        //MessageBox.Show("Bonne zone !"); //Petit test (il fonctionne)
                        //lblNecessairePourInsererLblDansPnl.Text = "C'est bon";    Ne fonctionne pas lui, messagebox fonctionne
                        // Après que le label ai atteint la bonne zone pour l'insertion, il ne faut plus qu'il bouge, donc on désactive
                        ControlExtension.Draggable(lbl, false);
                        //}



                        /*else
                        {
                            MessageBox.Show("la case "+ sous_panel.Tag.ToString() +" est déjà occupé !");
                        }*/
                    }
                }

            }

            /*Nous voulons pouvoir faire passer un label contenu dans un panel au form*/
            foreach (Panel sous_panel in Exercice.Controls.OfType<Panel>())
            {
                foreach (Label lbl in sous_panel.Controls.OfType<Label>())
                {
                    // Nous remettons au label la capacité de bouger, car nous l'avons désactivé précédemment
                    ControlExtension.Draggable(lbl, true);

                    // A partir d'une certaine hauteur du label dans le sous panel, nos considérons que l'utilisateur veut l'ajouter au formulaire
                    if (lbl.Top >= 25)
                    {
                        Exercice.Controls.Add(lbl);
                    }
                }
            }
        }


        //Fonction permettant de savoir si nous avons le droit de rentrer le labl dans le sous panel
        public bool bonneZonePourInsererDansSousPanel(Label Xlbl, Panel Xsous_panel)
        {
            bool res = false;

            int limite_a_gauche_sous_panel = Xsous_panel.Left - 5; // de base 18; FAIRE ATTENTION AUX CONFUSIONS
            int limite_a_droite_sous_panel = Xsous_panel.Left + 5;
            int limite_en_haut_sous_panel = Xsous_panel.Top - 5;
            int limite_en_bas_sous_panel = Xsous_panel.Top + 5;

            int left_label = Xlbl.Left;
            int top_label = Xlbl.Top;

            bool bon_left = false;
            bool bon_top = false;

            // Nous regardons si le Left du label est bon
            if (limite_a_gauche_sous_panel < left_label && left_label < limite_a_droite_sous_panel)
            {
                bon_left = true;
            }
            else
            {
                bon_left = false;
            }

            // Nous regardons si le Top du label est bon
            if (limite_en_haut_sous_panel < top_label && top_label < limite_en_bas_sous_panel)
            {
                bon_top = true;
            }
            else
            {
                bon_top = false;
            }

            if (bon_left == true && bon_top == true)
            {
                res = true;
            }
            else
            {
                res = false;
            }

            return res;
        }

        // A CHANGER SINON CA MARCHERA PAS
        public bool estDansSousPanel(Label Xlbl, Panel Xsous_panel)
        {
            bool res = false;

            int left_label = Xlbl.Left;
            int top_label = Xlbl.Top;

            //Même Left et même Top (on doit donc mettre la même taille partout mais c'est déjà fait quand on crée de base)
            int left_sous_panel = Xsous_panel.Left;
            int top_sous_panel = Xsous_panel.Top;

            // Si le sous panel et le label ont les mêmes coordonnées (sachant qu'ils font la même taille), alors le label est dans le sous panel
            if (left_label == left_sous_panel && top_label == top_sous_panel)
            {
                res = true;
            }
            else
            {
                res = false;
            }

            return res;
        }


        public void rentrerLabelDansSousPanel(Label Xlbl, Panel Xsous_panel)
        {
            // 
            Xsous_panel.Controls.Add(Xlbl);

            Xlbl.Left = 0;
            Xlbl.Top = 0;

            Xsous_panel.Controls.Add(Xlbl);
        }

        public void sortirLabelDuSousPanel(Label Xlbl, Panel Xsous_panel)
        {
            //Ajout dans le Form
            Exercice.Controls.Add(Xlbl);

            // On l'enlève de la collection des Controls du sous panel
            Xsous_panel.Controls.Remove(Xlbl);
        }



        public bool bonneZonePourCacherSousPanel(Label Xlbl, Panel Xsous_panel)
        {
            bool res = false;

            int left_label = Xlbl.Left;
            int top_label = Xlbl.Top;

            int limite_a_gauche_sous_panel = Xsous_panel.Left - 5;
            int limite_a_droite_sous_panel = Xsous_panel.Left + 5;
            int limite_en_haut_sous_panel = Xsous_panel.Top - Xlbl.Height;
            int limite_en_bas_sous_panel = Xsous_panel.Top + Xsous_panel.Height;

            bool bon_left = false;
            bool bon_top = false;

            /*if( (Xlbl.Left == Xsous_panel.Left) && (Xlbl.Top == Xsous_panel.Top) )
            {
                res = true;
            }
            else
            {
                res = false;
            }

            return res;*/

            if (limite_a_gauche_sous_panel < left_label && left_label < limite_a_droite_sous_panel)
            {
                bon_left = true;
            }
            else
            {
                bon_left = false;
            }

            if (limite_en_haut_sous_panel < top_label && top_label < limite_en_bas_sous_panel)
            {
                bon_top = true;
            }
            else
            {
                bon_top = false;
            }

            if (bon_left == true && bon_top == true)
            {
                res = true;
            }
            else
            {
                res = false;
            }

            return res;
        }

        public void ajouterLabelDansForm(Label Xlbl, GroupBox XExercice, Panel Xsous_panel)
        {
            // Le sous panel n'est plus occupé
            sous_panel_deja_occupe.Remove(Xsous_panel);

            int left_label_dans_sous_panel = Xlbl.Left;

            XExercice.Controls.Add(Xlbl);
            // Peut etre faut désactiver Draggable
            ControlExtension.Draggable(Xlbl, false);
            //ControlExtension.Draggable(Xlbl, true);

            // Si nous ne définissons pas une position au label une fois sorti du sous panel, le label se mettra en haut à gauche du form
            /*Xlbl.Left = Xsous_panel.Left + left_label_dans_sous_panel;
            Xlbl.Top = 300; // A tester*/
            /*Xlbl.Left = 300;
            Xlbl.Top = 200;*/
        }



        private void Exo_Glissage_Mot()
        {
            /*Génération dynamique du bouton commencer exo*/
            Button btnCommencerExo = new Button();
            btnCommencerExo.Location = new System.Drawing.Point(50, 171);
            btnCommencerExo.Name = "btnCommencerExo";
            btnCommencerExo.Size = new System.Drawing.Size(205, 74);
            btnCommencerExo.Text = "Commencer exo";
            btnCommencerExo.Click += new System.EventHandler(this.btnCommencerExo_Click3);
            Exercice.Controls.Add(btnCommencerExo);


            Button btnSolution = new Button();
            btnSolution.Location = new System.Drawing.Point(1005, 501);
            btnSolution.Name = "btnSolution";
            btnSolution.Size = new System.Drawing.Size(125, 72);
            btnSolution.Text = "Solution";
            btnSolution.Click += new System.EventHandler(this.btnSolution_Click3);
            Exercice.Controls.Add(btnSolution);


            Button btnValider = new Button();
            btnValider.Location = new System.Drawing.Point(806, 301);
            btnValider.Name = "btnValider";
            btnValider.Size = new System.Drawing.Size(155, 72);
            btnValider.Text = "Valider";
            btnValider.Click += new System.EventHandler(this.btnValider_Click3);
            Exercice.Controls.Add(btnValider);


            Button btnAide = new Button();
            btnAide.Location = new System.Drawing.Point(1136, 501);
            btnAide.Name = "btnAide";
            btnAide.Size = new System.Drawing.Size(125, 72);
            btnAide.Text = "Aide";
            btnAide.Click += new System.EventHandler(this.btnAide_Click3);
            Exercice.Controls.Add(btnAide);


            Label lblEnonce = new Label();
            lblEnonce.AutoSize = true;
            lblEnonce.Location = new System.Drawing.Point(83, 367);
            lblEnonce.Name = "lblEnonce";
            lblEnonce.Size = new System.Drawing.Size(100, 29);
            lblEnonce.Text = "Enoncé";
            Exercice.Controls.Add(lblEnonce);


            Label lblPhraseEnFrancais = new Label();
            lblPhraseEnFrancais.AutoSize = true;
            lblPhraseEnFrancais.Location = new System.Drawing.Point(83, 430);
            lblPhraseEnFrancais.Name = "lblPhraseEnFrancais";
            lblPhraseEnFrancais.Size = new System.Drawing.Size(113, 29);
            lblPhraseEnFrancais.Text = "Phrase : ";
            Exercice.Controls.Add(lblPhraseEnFrancais);


            Label lblNbAideDispo = new Label();
            lblNbAideDispo.AutoSize = true;
            lblNbAideDispo.Location = new System.Drawing.Point(75, 752);
            lblNbAideDispo.Name = "lblNbAideDispo";
            lblNbAideDispo.Size = new System.Drawing.Size(358, 29);
            lblNbAideDispo.Text = "Nombre d\'aide(s) disponible : ";
            Exercice.Controls.Add(lblNbAideDispo);
            Exercice.Controls.Add(Finalisation);
            Exercice.Text = " Phrases dans le désordre";

            this.Controls.Add(Exercice);


        }

        private void lblPhraseTraductionEspagnol_MouseDown(object sender, MouseEventArgs e)
        {
            /*if(e.Button == MouseButtons.Left)
            {
                // Ici on met control.Draggable(lbltatata , true) et du coup on enc=lève le Point point, évènement MouseMove et enlever en haut l'event(s) enlevé(s)
                ControlExtension.Draggable(lblPhraseTraductionEspagnol, true);
            }*/
        }
        private void lblPhraseTraductionEspagnol_MouseMove(object sender, MouseEventArgs e)
        {
            // Seulement si on appui sur clique gauche
            /*if(e.Button == MouseButtons.Left)
            {
                for(int i=0; i<motsPhraseEspagnolExo3.Length; i++)
                {
                    foreach( Label lab in this.Controls.OfType<Label>() )
                    {
                        if( lab.Name == "lblPhraseTraductionEspagnol" && (int)lab.Tag == i) // Aucun rapport mais une solution pour passer au 1er plan serait de générer le grand panel dynamiquement peut être
                        {
                            MessageBox.Show("le label selectionné est : " + lab.Tag.ToString());

                            lab.Left = lab.Left + e.X - point.X;
                            lab.Top = lab.Top + e.Y - point.Y;
                        }
                        else
                        {

                        }
                    }
                }
            }*/
        }

        private void btnCacherForm_Click(object sender, EventArgs e)
        {
            foreach (Control cont in Exercice.Controls)
            {
                //MessageBox.Show("Control : " + cont.Name);
            }
        }

        private void btnCacherSousPanel_Click(object sender, EventArgs e)
        {
            foreach (Panel sous_panel in Exercice.Controls.OfType<Panel>())
            {
                sous_panel.Visible = !sous_panel.Visible;
            }
        }

        List<string> reponse_utilisateur = new List<string>();
        private void btnValider_Click3(object sender, EventArgs e)
        {

            /*Nous rajoutons dans une liste la réponse finale de l'utilisateur*/
            recupererReponseUtilisateur();

            /*Pour vérifier j'affiche la liste (ca marche très bien)*/
            /*for (int i=0; i<reponse_utilisateur.Count; i++)
            {
                MessageBox.Show(reponse_utilisateur[i]);
            }*/

            verifierReponseUtilisateur();

            // Pour chaque label on va essayer de lui donner un Tag qui est = au Tag du sous-panel dans lequel le label est (PEUT ETRE QUE J'EN AURAI BESOIN)
            /*foreach (Panel sous_panel in this.Controls.OfType<Panel>())
            {
                foreach (Label lbl in sous_panel.Controls.OfType<Label>())
                {
                    MessageBox.Show("tkt2");        // ça me le fait 12 fois, voir sur papier exécution exact
                    // Si le label actuel "labelActu" est dans le sous-panel actuel "sous_panel_actu" (trouvé grâce à une fonction à faire encore), alors on assigne le Tag du sous-panel_actu au labelActu
                    if ( sous_panel.Contains(lbl) )   // ça rentre pas dans la boucle donc c à cause de la fonction
                    {
                        //MessageBox.Show("Normalement ça m'affiche ça 2 fois");
                        lbl.Tag = sous_panel.Tag;
                        //MessageBox.Show("tkt");

                        // Petit test
                        MessageBox.Show("Tag du sous panel '" + sous_panel.Name + "' : " + sous_panel.Tag.ToString() + " -- Tag du label '" + lbl.Name + "' : " + lbl.Tag.ToString());
                    }
                }
            }*/

        }

        public void recupererReponseUtilisateur()
        { 
            foreach (Panel sous_panel in Exercice.Controls.OfType<Panel>())
            {
                foreach (Label lbl in sous_panel.Controls.OfType<Label>())
                {
                    // Nous exécutons le code seulement si le label est présent dans le sous panel
                    if (sous_panel.Contains(lbl))
                    {
                        string texte_du_label = lbl.Text;
                        reponse_utilisateur.Add(texte_du_label);
                    }
                }
            }

        }

        public void verifierReponseUtilisateur()
        {
            bool bonne_reponse = false;

            bool mauvaise_reponse = false;
            /*Nous vérifions pour chaque position si le mot est le même : si oui --> c'est juste, sinon c'est faux*/
            int nbMots = motsPhraseEspagnolExo3.Length;


            // Il faut que l'utilisateur ait au moins mit une réponse
            if (reponse_utilisateur.Count >= 1)
            {
                for (int i = 0; i < nbMots && mauvaise_reponse == false; i++)
                {

                    string motReponse = motsPhraseEspagnolExo3[i];
                    string motUtilisateur = reponse_utilisateur[i];

                    if (motUtilisateur == motReponse)
                    {
                        bonne_reponse = true;
                    }
                    else
                    {
                        bonne_reponse = false;
                        mauvaise_reponse = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez proposer une réponse.");
            }


            if (bonne_reponse == true)
            {
                MessageBox.Show("La réponse est juste !");
            }
            else
            {
                MessageBox.Show("La réponse est fausse !");
            }

            /*J'efface la liste de la réponse de l'utilisateur pour un autre essai*/
            reponse_utilisateur.Clear();
        }

        private void btnSolution_Click3(object sender, EventArgs e)
        {
            int left_label_replace = 29;
            int top_label_replace = 515;
            /*Nous replaçons tous les label au bon endroit (qu'ils soient dans le form ou dans les sous panel*/

            // Nous les enlèvons du sous panel et les ajoutons au form
            foreach (Panel sous_panel in Exercice.Controls.OfType<Panel>())
            {
                foreach (Label lbl in sous_panel.Controls.OfType<Label>())
                {
                    Exercice.Controls.Add(lbl);
                }
            }


            // Nous les replaçons à la même position de départ (sauf les label de l'énoncé, la phrase en français et le nombre d'aide restant)
            foreach (Label lbl in Exercice.Controls.OfType<Label>())
            {
                if (lbl.Name != "lblEnonce" && lbl.Name != "lblPhraseEnFrancais" && lbl.Name != "lblNbAideDispo")
                {
                    lbl.Left = left_label_replace;
                    lbl.Top = top_label_replace;

                    // Nous incrémentons pour le prochain
                    if (left_label_replace >= 797)
                    {
                        // Nous avons atteint la limite --> réinitialisation du Left et incrémentation la hauteur
                        top_label_replace = top_label_replace + 58;
                        left_label_replace = 29;
                    }
                    else
                    {
                        // Nous incrémentons seulement la Left
                        left_label_replace = left_label_replace + 192;
                    }
                }

            }

            // Nous les ajoutons dans l'ordre dans les bon sous panel ET nous réglons à la bonne position (0,0)
            foreach (Panel sous_panel in Exercice.Controls.OfType<Panel>())
            {
                foreach (Label lbl in Exercice.Controls.OfType<Label>())
                {
                    // JE PENSE QUE CETTE CONDITION NE SERT RIEN A VOIR PLUS TARD SI JE L'ENLEVE OU PAS
                    if (lbl.Name != "lblEnonce" && lbl.Name != "lblPhraseEnFrancais" && lbl.Name != "lblNbAideDispo")
                    {
                        placerLabelDansBonSousPanel(lbl, sous_panel);
                    }

                }
            }

            // Il reste toujours un label qui n'est pas inséré, donc on l'insère ici avec la même stratégie qu'en haut
            foreach (Panel sous_panel in Exercice.Controls.OfType<Panel>())
            {
                foreach (Label lbl in Exercice.Controls.OfType<Label>())
                {
                    // JE PENSE QUE CETTE CONDITION NE SERT RIEN A VOIR PLUS TARD SI JE L'ENLEVE OU PAS
                    if (lbl.Name != "lblEnonce" && lbl.Name != "lblPhraseEnFrancais" && lbl.Name != "lblNbAideDispo")
                    {
                        placerLabelDansBonSousPanel(lbl, sous_panel);
                    }

                }
            }


        }

        public void placerLabelDansBonSousPanel(Label Xlbl, Panel Xsous_panel)
        {
            // JE PENSE QUE CETTE CONDITION NE SERT RIEN A VOIR PLUS TARD SI JE L'ENLEVE OU PAS
            if (Xlbl.Name != "lblEnonce" && Xlbl.Name != "lblPhraseEnFrancais" && Xlbl.Name != "lblNbAideDispo")
            {
                // Nous récupérons l'indice de position dans le tableau des réponses, dans lequel se trouve le mot examiné
                int indice_mot_recherche_dans_tableau_des_reponses = 0;

                for (int i = 0; i < motsPhraseEspagnolExo3.Length; i++)
                {
                    string mot_recherche = Xlbl.Text;
                    string mot_dans_tableau = motsPhraseEspagnolExo3[i];

                    if (mot_recherche == mot_dans_tableau)
                    {
                        indice_mot_recherche_dans_tableau_des_reponses = i;
                    }
                }

                // Nous vérifions pour si la position récupéré correspond au tag du sous panel
                if ((int)Xsous_panel.Tag == indice_mot_recherche_dans_tableau_des_reponses)
                {
                    // Nous insérons le label dans le sous panel et réglons sa position dedans
                    Xsous_panel.Controls.Add(Xlbl);
                    Xlbl.Left = 0;
                    Xlbl.Top = 0;
                    //Test
                   // MessageBox.Show("Le mot '" + Xlbl.Text + "' va dans le panel '" + Xsous_panel.Tag.ToString() + "'");
                }
            }

            nb_label_qui_devait_etre_depose++;

        }


        int nb_label_qui_devait_etre_depose = 1;
        private void btnAide_Click3(object sender, EventArgs e)
        {
            /*Le bouton aide consiste à prendre un label au hasard dans le FORM, puis de le placer dans le bon sous panel*/




            if (cptAide <= 0)
            {
                /*Quelques fois, il reste un label qui n'est pas déposé dans son sous panel*/
                int nbLabelDepose = CompteNbLabelDepose();
                if (nbLabelDepose < 3)
                {
                    // Nous récupérons le Label qui n'a pas encore été placé alors qu'il devait l'être puis nous le mettons au bon endroit
                    placerLabelQuiDevaitLetre();

                    foreach (Label lbl in Exercice.Controls.OfType<Label>())
                    {
                        if (lbl.Name == "lblNbAideDispo")
                        {
                            lbl.Text = "Nombre d'aide(s) disponible : 0";
                        }
                    }
                }
                else
                {
                    cptAide = 0;    // JE CROIS SERT A RIEN
                    // Affichage à l'utilisateur du nombre d'aide qu'il lui reste ( -1 car en réalité l'utilisateur a le droit à 4 aides maximum )
                    foreach (Label lbl in Exercice.Controls.OfType<Label>())
                    {
                        if (lbl.Name == "lblNbAideDispo")
                        {
                            lbl.Text = "Nombre d'aide(s) disponible : 0";
                        }
                    }
                    MessageBox.Show("Vous avez épuisé toutes vos aides.");
                }

            }
            else
            {

                cptAide--;

                rangerUnLabelDansBonSousPanel();

                foreach (Label lbl in Exercice.Controls.OfType<Label>())
                {
                    if (lbl.Name == "lblNbAideDispo")
                    {
                        lbl.Text = "Nombre d'aide(s) disponible : " + cptAide.ToString();
                    }
                }

                /*Quelques fois, il reste un label qui n'est pas déposé dans son sous panel*/
                int nb_label_vraiment_depose = CompteNbLabelDepose();
                if (nb_label_qui_devait_etre_depose > nb_label_vraiment_depose)
                {
                    MessageBox.Show("Le label n'est pas bien placé.");
                    placerLabelQuiDevaitLetre();
                    nb_label_qui_devait_etre_depose++;
                }
                else
                {
                    nb_label_qui_devait_etre_depose++;
                }

            }

        }

        public int CompteNbLabelDepose()
        {
            int res = 0;

            foreach (Panel sous_panel in Exercice.Controls.OfType<Panel>())
            {
                foreach (Label lbl in sous_panel.Controls.OfType<Label>())
                {
                    if (sous_panel.Contains(lbl))
                    {
                        res++;
                    }
                }
            }

            return res;
        }

        public void placerLabelQuiDevaitLetre()
        {
            foreach (Label lbl in Exercice.Controls.OfType<Label>())
            {
                if (lbl.Left == 0 && lbl.Top == 0)
                {
                    foreach (Panel sous_panel in Exercice.Controls.OfType<Panel>())
                    {
                        placerLabelDansBonSousPanel(lbl, sous_panel);
                    }
                }
            }
        }


        public void rangerUnLabelDansBonSousPanel()
        {
            // Tirage d'une valeur aléatoire
            Random rnd = new Random();
            int nbMots = motsPhraseEspagnolExo3.Length;
            int valTagAleatoire = 0;

            bool label_recupere = false;

            // Tant que nous n'avons pas récupéré un label, nous recommençons le tirage
            while (label_recupere == false)
            {
                valTagAleatoire = rnd.Next(0, nbMots);

                // Parcourt des label dans le form pour sélectionner celui qui a pour Tag la valeur aléatoire ci-dessus
                foreach (Label lbl in Exercice.Controls.OfType<Label>())
                {
                    if (lbl.Name != "lblEnonce" && lbl.Name != "lblPhraseEnFrancais" && lbl.Name != "lblNbAideDispo")
                    {
                        if ((int)lbl.Tag == valTagAleatoire)
                        {
                            label_recupere = true;
                            // Nous le plaçons dans le bon sous panel
                            foreach (Panel sous_panel in Exercice.Controls.OfType<Panel>())
                            {
                                placerLabelDansBonSousPanel(lbl, sous_panel);
                            }
                        }
                    }
                }
            }

            nb_label_qui_devait_etre_depose++;

            // Tant que nous n'avons pas récupéré un label, nous recommençons le tirage
            /*while (label_manquant_recupere == false)
            {
                MessageBox.Show("Valeur label_maquant_rec = " + valTagAleatoire);
                // Parcourt des label dans le form pour sélectionner celui qui a pour Tag la valeur aléatoire ci-dessus
                foreach (Label lbl in this.Controls.OfType<Label>())
                {
                    if (lbl.Name != lblEnonce.Name && lbl.Name != lblPhraseEnFrancais.Name && lbl.Name != lblNbAideDispo.Name)
                    {
                        if ((int)lbl.Tag == valTagAleatoire)
                        {
                            label_manquant_recupere = true;
                            // Nous le plaçons dans le bon sous panel
                            foreach (Panel sous_panel in this.Controls.OfType<Panel>())
                            {
                                placerLabelDansBonSousPanel(lbl, sous_panel);
                            }
                        }
                    }
                }
            }*/


        }


    private void Exo_Suivant(object sender, EventArgs e)
        {
            Exercice.Controls.Clear();
            exoNum = exoNum + 1;
            // MessageBox.Show(exoNum.ToString());

            if (exoNum == 1)
            {
                Exerices_Vocabulaire();
            }
            else if (exoNum == 2)
            {
                Exo_Conjugaison();
            }
            else if (exoNum == 3)
            {
                Exo_Mot_à_trou();
            }
            else if (exoNum == 4)
            {
                Exo_Glissage_Mot();
            }
            else
                PDF();
            
        }
        private void Recommencer_Exo(object sender, EventArgs e)
        {
            foreach (TextBox c in grbEsp.Controls.OfType<TextBox>())
            {
                c.Text = "";
                c.BackColor = Color.White;
            }
            Exercice.Controls.Add(AideConjugaison);
        }

    }
}
