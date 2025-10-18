using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Biblio_de_classes
{
    internal class ManagementTache : IConversionXML
    {
        List<Tache> ChargerDepuisXML(string cheminFichier)
        {
            //Variable à retourner
            List<Tache> liste = new();
            
            //Charger le document
            XmlDocument doc = new();
            doc.Load(cheminFichier);

            //Créer la liste de noeuds
            XmlNodeList listeNoeuds = doc.GetElementsByTagName("tache");

            //Ajouter les taches à la liste
            foreach (XmlNode n in listeNoeuds)
            {
                //Valeurs du constructeur
                string desc = n["description"].InnerText;
                DateOnly dateCreation = DateOnly.Parse(n.Attributes["creation"].Value);
                DateOnly? dateDebut = DateOnly.Parse(n.Attributes["debut"].Value);
                DateOnly? dateFin = DateOnly.Parse(n.Attributes["fin"].Value);
                List<Etape> etapes = new();

                //Ajouter les étapes à la liste
                foreach (XmlNode etape in n["etapes"])
                {
                    //int num = (n["etapes"].ChildNodes.Count > 0) ? n["etapes"].ChildNodes.Count + 1 : 0;
                    etapes.Add(new Etape(int.Parse(etape.Attributes["no"].Value), etape.InnerText, Boolean.Parse(etape.Attributes["terminee"].Value)));
                }

                liste.Add(new Tache( desc, dateCreation, dateDebut, dateFin, etapes, 0));
            }

            return liste;
        }
        void SauvegarderVersXML(string cheminFichier, List<Tache> taches)
        {

        }
    }
}
