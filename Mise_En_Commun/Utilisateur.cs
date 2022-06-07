using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace Mise_En_Commun
{

    class Utilisateur
    {
        private const string chcon = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=U:\A21\frm_EspagnolTrad-1.git\baseLangue.mdb";
        private OleDbConnection connec = new OleDbConnection();
        public string Username;
        private string codeUtil;


        public Utilisateur(string nom)
        {
            this.Username = nom;
        }

        public bool isAdmin()
        {
            connec.ConnectionString = chcon;
            connec.Open();

            string requete = @"select [codeUtil] from [Utilisateurs] where [nomUtil] = '" + this.Username + "'";

            OleDbCommand cmd = new OleDbCommand();

            cmd.Connection = connec;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = requete;

            this.codeUtil = cmd.ExecuteScalar().ToString();

            if (this.codeUtil == "6" || this.codeUtil == "5")
            {
                connec.Close();
                return true;
            }

            else
            {
                connec.Close();
                return false;

            }

        }

        public string[,] getExosInfo()
        {
            connec.ConnectionString = chcon;
            connec.Open();

            string[,] infos = new string[1, 3];
            string requete = @"select [codeExo], [codeLeçon], [codeCours] from [Utilisateurs] where [nomUtil] = '" + this.Username + "'";

            OleDbCommand cmd = new OleDbCommand();

            cmd.Connection = connec;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = requete;

            OleDbDataReader da = cmd.ExecuteReader();

            if (da.HasRows)
            {
                while (da.Read())
                {

                    infos[0, 0] = da.GetInt32(0).ToString();
                    infos[0, 1] = da.GetInt32(1).ToString();
                    infos[0, 2] = da.GetString(2);
                }
            }

            connec.Close();
            return infos;
        }

        public string[,] getInfos()
        {
            string[,] res = getExosInfo();

            connec.ConnectionString = chcon;
            connec.Open();

            string requete = @"select [titreLecon] from [Lecons] where [numCours] = '"
                    + res[0, 2] + "'" + " and [numLecon] = " + res[0, 1];
            OleDbCommand cmd = new OleDbCommand();

            cmd.Connection = connec;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = requete;

            string lecon = cmd.ExecuteScalar().ToString();
            res[0, 1] = lecon;

            string requete4 = @"select [titreCours] from [Cours] where [numCours] = '"
                    + res[0, 2] + "'";
            cmd.CommandText = requete4;

            string cours = cmd.ExecuteScalar().ToString();
            res[0, 2] = cours;

            connec.Close();
            return res;
        }

        public int getNbExos()
        {
            string[,] res = getExosInfo();

            connec.ConnectionString = chcon;
            connec.Open();

            string requete = @"select count([numExo]) from [Exercices] where [numCours] = '"
                    + res[0, 2] + "'" + " and [numLecon] = " + res[0, 1];
            OleDbCommand cmd = new OleDbCommand();

            cmd.Connection = connec;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = requete;

            int lecon = (int)cmd.ExecuteScalar();
            connec.Close();
            return lecon;
        }

        public string getEnonceExo()
        {
            string[,] res = getExosInfo();

            connec.ConnectionString = chcon;
            connec.Open();

            string requete = @"select [enonceExo] from [Exercices] where [numCours] = '"
                    + res[0, 2] + "'" + " and [numLecon] = " + res[0, 1] + " and [numExo] = " + res[0, 0];

            OleDbCommand cmd = new OleDbCommand();

            cmd.Connection = connec;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = requete;

            string Enonce = cmd.ExecuteScalar().ToString();

            connec.Close();
            return Enonce;
        }

        public int NemUser()
        {
            connec.ConnectionString = chcon;
            connec.Open();

            string requete = @"select [codeUtil] from [Utilisateurs] where [nomUtil] = '" + this.Username + "'";

            OleDbCommand cmd = new OleDbCommand();

            cmd.Connection = connec;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = requete;

            this.codeUtil = cmd.ExecuteScalar().ToString();
            connec.Close();
            return Int32.Parse(this.codeUtil);
        }
        

    }
}
