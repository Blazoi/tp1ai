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
                DateOnly dateCreation = DateOnly.ParseExact(n.Attributes["creation"].Value, "dd/MM/yyyy");
                DateOnly? dateDebut = null;
                DateOnly? dateFin = null;
                if (n.Attributes["debut"].Value != "")
                {
                    dateDebut = DateOnly.ParseExact(n.Attributes["debut"].Value, "dd/MM/yyyy");
                }
                if (n.Attributes["fin"].Value != "")
                {
                    dateFin = DateOnly.ParseExact(n.Attributes["fin"].Value, "dd/MM/yyyy");
                }
                List<Etape> etapes = new();

                //Ajouter les étapes à la liste
                foreach (XmlNode etape in n["etapes"].ChildNodes)
                {
                    etapes.Add(new Etape(int.Parse(etape.Attributes["no"].Value),
                                         etape.InnerText,
                                         Boolean.Parse(etape.Attributes["termine"].Value)
                                         ));
                }

                liste.Add(new Tache(desc, dateCreation, dateDebut, dateFin, etapes));
            }

            return liste;
        }
        public void SauvegarderVersXML(string cheminFichier, List<Tache> taches)
        {
            XmlDocument doc = new();
            XmlElement TachesElement = doc.CreateElement("taches");

            foreach (Tache tache in taches)
            {
                XmlElement tacheElement = doc.CreateElement("tache");
                string format = "dd/MM/yyyy";
                //Ajouté attributs
                tacheElement.SetAttribute("creation", tache.DateCreation.ToString(format));
                tacheElement.SetAttribute("debut", tache.DateDebut?.ToString(format) ?? "works");
                tacheElement.SetAttribute("fin", tache.DateFin?.ToString(format) ?? "properly");

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
                TachesElement.AppendChild(tacheElement);
            }

            // Ajouté root et sauvegardé
            doc.AppendChild(TachesElement);
            doc.Save(cheminFichier);
        }

        public void AjouterTache(string description)
        {
            Tache tache = new Tache(description, DateOnly.FromDateTime(DateTime.Now), null, null, new List<Etape>());
            Taches.Add(tache);
        }

        public void SupprimerTache(Tache tache)
        {
            Tache tacheAEnlever = Taches.Find(element => element.Description == tache.Description);
            Taches.Remove(tacheAEnlever);
        }

        public void AjouterEtape(Tache tache, string description)
        {
            //Créé l'étape
            Etape etape = new Etape(Taches.Count + 1, description);

            //Ajouté l'étape
            Taches.Find(element => element.Description == tache.Description).Etapes.Add(etape);
        }

        public void SupprimerEtape(Tache tache, Etape etape)
        {
            Taches.Find(element => element.Description == tache.Description).Etapes.Remove(etape);
        }

        public void TerminerEtape(Tache tache, Etape etape)
        {
            List<Etape> EtapesListe = Taches.Find(element => element.Description == tache.Description).Etapes;
            EtapesListe.Find(element => element.Description == etape.Description).Termine = true;
        }
    }
}