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
        private List<Tache> Taches = new List<Tache>();
        public List<Tache> ChargerDepuisXML(string cheminFichier)
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
                    //int num = (n["etapes"].ChildNodes.Count > 0) ? n["etapes"].ChildNodes.Count + 1 : 0; -- Je vais peut-être réutiliser
                    etapes.Add(new Etape(int.Parse(etape.Attributes["no"].Value), etape.InnerText, Boolean.Parse(etape.Attributes["terminee"].Value)));
                }

                liste.Add(new Tache( desc, dateCreation, dateDebut, dateFin, etapes, 0));
            }

            return liste;
        }
        public void SauvegarderVersXML(string cheminFichier, List<Tache> taches)
        {
            XmlDocument doc = new();
            XmlElement TachesElement = doc.CreateElement("Taches");

            foreach (Tache tache in taches)
            {
                XmlElement tacheElement = doc.CreateElement("tache");

                //Ajouté attributs
                tacheElement.SetAttribute("creation", tache.DateCreation.ToString());
                tacheElement.SetAttribute("debut", tache.DateDebut.ToString());
                tacheElement.SetAttribute("fin", tache.DateFin.ToString());

                //Ajouté description
                XmlElement description = doc.CreateElement("description");
                description.InnerText = tache.Description;
                tacheElement.AppendChild(description);

                //Ajouté étapes
                XmlElement etapes = doc.CreateElement("etapes");

                foreach (Etape etape in tache.Etapes)
                {
                    //Créé l'élément
                    XmlElement etapeElement = doc.CreateElement("etape");
                    etapeElement.SetAttribute("terminee", etape.Terminee.ToString());
                    etapeElement.SetAttribute("no", etape.Numero.ToString());
                    etapeElement.InnerText = etape.Description;

                    //Ajouté l'élément
                    etapes.AppendChild(etapeElement);
                }

                tacheElement.AppendChild(etapes);

                //Ajouté la tâche au root
                TachesElement.AppendChild(tacheElement);
            }

            // Ajouté root et sauvegardé
            doc.AppendChild(TachesElement);
            doc.Save(cheminFichier);
        }

        public void AjouterTache(string description)
        {
            Tache tache = new Tache(description, DateOnly.FromDateTime(DateTime.Now), null, null, new List<Etape>(), 0);
            Taches.Add(tache);
        }

        public void SupprimerTache(Tache tache)
        {
            Tache tacheAEnlever = Taches.Find(element => element.Description == tache.Description);
            Taches.Remove(tacheAEnlever);
        }

        public void AjouterEtape(Tache tache, string description)
        {

        }

        public void SupprimerEtape (Tache tache, Etape etape)
        {

        }

        public void TerminerEtape (Tache tache, Etape etape)
        {

        }
    }
}