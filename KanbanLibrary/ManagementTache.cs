using KanbanLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KanbanLibrary
{
    public class ManagementTache : IConversionXML
    {
        public List<Tache> Taches { get; set; }

        public List<Tache> ChargerDepuisXML(string cheminFichier)
        {
            //Variable à retourner
            List<Tache> liste = new();

            XmlDocument doc = new();
            doc.Load(cheminFichier);
            XmlNodeList listeNoeuds = doc.GetElementsByTagName("tache");


            //Ajouter les taches à la liste
            foreach (XmlNode noeud in listeNoeuds)
            {
                string desc = noeud["description"].InnerText;
                DateOnly dateCreation = DateOnly.ParseExact(noeud.Attributes["creation"].Value, "dd/MM/yyyy");
                DateOnly? dateDebut = null;
                DateOnly? dateFin = null;
                if (noeud.Attributes["debut"].Value != "")
                {
                    dateDebut = DateOnly.ParseExact(noeud.Attributes["debut"].Value, "dd/MM/yyyy");
                }
                if (noeud.Attributes["fin"].Value != "")
                {
                    dateFin = DateOnly.ParseExact(noeud.Attributes["fin"].Value, "dd/MM/yyyy");
                }


                //Ajouter les étapes à la liste
                List<Etape> etapes = new();
                foreach (XmlNode etape in noeud["etapes"])
                {
                    int num = int.Parse(etape.Attributes["no"].Value);
                    string etape_desc = etape.InnerText;
                    bool termine = bool.Parse(etape.Attributes["termine"].Value);

                    etapes.Add(new Etape(num, etape_desc, termine));
                }

                liste.Add(new Tache(desc, dateCreation, dateDebut, dateFin, etapes));

            }

            return liste;
        }
        public void SauvegarderVersXML(string cheminFichier, List<Tache> taches)
        {
            XmlDocument doc = new();
            XmlElement root = doc.CreateElement("taches");

            foreach (Tache tache in taches)
            {
                XmlElement tacheElement = doc.CreateElement("tache");
                string format = "dd/MM/yyyy";
                //Ajouté attributs
                tacheElement.SetAttribute("creation", tache.DateCreation.ToString(format));
                tacheElement.SetAttribute("debut", tache.DateDebut?.ToString(format) ?? "");
                tacheElement.SetAttribute("fin", tache.DateFin?.ToString(format) ?? "");

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
                    etapeElement.SetAttribute("termine", etape.Termine.ToString());
                    etapeElement.SetAttribute("no", etape.Numero.ToString());
                    etapeElement.InnerText = etape.Description;

                    //Ajouté l'élément
                    etapes.AppendChild(etapeElement);
                }

                tacheElement.AppendChild(etapes);

                //Ajouté la tâche au root
                root.AppendChild(tacheElement);
            }

            // Ajouté root et sauvegardé
            doc.AppendChild(root);
            doc.Save(cheminFichier);
        }

        public void AjouterTache(string description)
        {
            Tache tache = new Tache(description, DateOnly.FromDateTime(DateTime.Now), null, null, new List<Etape>());
            Taches.Add(tache);
        }

        public void SupprimerTache(Tache tache)
        {
            Taches.Remove(tache);
        }

        public void AjouterEtape(Tache tache, string description)
        {
            Etape etape = new Etape(Taches.Count + 1, description);
            tache.Etapes.Add(etape);
        }

        public void SupprimerEtape(Tache tache, Etape etape)
        {
            tache.Etapes.Remove(etape);
        }

        public void TerminerEtape(Tache tache, Etape etape)
        {
            tache.Etapes.Find(x => x == etape).Termine = true;
        }
    }
}