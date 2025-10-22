using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml;
using KanbanLibrary;
using Microsoft.Maui.Controls.Internals;

namespace KanbanMauiApp
{
    public partial class MainPage : ContentPage
    {
      
        
        ManagementTache manager = new();

        Etape selectionEtape;
        Tache selectionTache;

        List<Tache> listeDeTachesPlanifiees;
        List<Tache> listeDeTachesEnCours;
        List<Tache> listeDeTachesCompletees;

        string chemin = "C:\\Users\\jackj\\OneDrive - Collège de Bois-de-Boulogne\\Bureau\\TP1\\KanbanMauiApp\\Data\\taches.xml";
        public MainPage()
        {
            InitializeComponent();
            ChargerTachesDepuisFichier();
            
        }
        // --- Charger les tâches depuis un fichier ---
        private void ChargerTachesDepuisFichier()
        {
            List<Tache> liste = manager.ChargerDepuisXML(chemin);

            //Assigner les différentes tâches à de différentes listes selon leur statut
            List<Tache> liste_p = new();
            List<Tache> liste_ec = new();
            List<Tache> liste_c = new();

            foreach (Tache t in liste)
            {
                switch (t.Statut)
                {
                    case 0:
                        liste_p.Add(t);
                        break;
                    case 1:
                        liste_ec.Add(t);
                        break;
                    case 2:
                        liste_c.Add(t);
                        break;
                }
            }

            manager.Taches = liste;

            PlannedTasks.ItemsSource = liste_p;
            listeDeTachesPlanifiees = liste_p;

            InProgressTasks.ItemsSource = liste_ec;
            listeDeTachesEnCours = liste_ec;

            CompletedTasks.ItemsSource = liste_c;
            listeDeTachesCompletees = liste_c;

            //BindingContext = liste;
        }

        // --- Sauvegarder les tâches dans un fichier ---
        private void Sauvegarder()
        {
            manager.SauvegarderVersXML(chemin, manager.Taches);
        }
        // --- Rafraîchir les listes ---
        private void RafraichirListes()
        {
            Sauvegarder();
            ChargerTachesDepuisFichier();

            //Réinitialiser et réassigner les listes sources
            if (selectionTache != null)
            {
                TaskSteps.ItemsSource = null;
                TaskSteps.ItemsSource = selectionTache.Etapes;
            }

            PlannedTasks.ItemsSource = null;
            PlannedTasks.ItemsSource = listeDeTachesPlanifiees;

            InProgressTasks.ItemsSource = null;
            InProgressTasks.ItemsSource = listeDeTachesEnCours;

            CompletedTasks.ItemsSource = null;
            CompletedTasks.ItemsSource = listeDeTachesCompletees;
        }

        // --- Sélection d’une tâche ---

        //À REFAIRE
        private void OnTaskSelected(object sender, SelectionChangedEventArgs e)
        {
            selectionTache = e.CurrentSelection.FirstOrDefault() as Tache;

            if (selectionTache == null)
                return;

            //Afficher la description
            TaskDescription.Text = selectionTache.Description;
            string format = "dddd MMM dd, yyyy";
            TaskDates.Text = $"Date de création : {selectionTache.DateCreation.ToString(format)}" +
                             $"\nDate de début : {selectionTache.DateDebut?.ToString(format) ?? "non définie"}" +
                             $"\nDate de fin : {selectionTache.DateFin?.ToString(format) ?? "non définie\n"}";

            //Afficher les étapes
            TaskSteps.ItemsSource = selectionTache.Etapes;
            InitialiserSelectionEtape();
        }

        // --- Sélection d’une étape ---
        private void OnStepSelected(object sender, SelectionChangedEventArgs e)
        {
            selectionEtape = e.CurrentSelection.FirstOrDefault() as Etape;
        }
        // Appelé après que ListeEtapes.ItemsSource soit définie
        private void InitialiserSelectionEtape()
        {
            selectionEtape = selectionTache.Etapes?.Find(x => x.Termine == false) ?? null;
        }
        private async void OnAddTask(object sender, EventArgs e)
        {
            string desc = Taches_Entry.Text;
            DateOnly dateCreation = DateOnly.FromDateTime(DateTime.Now);
            
            if (desc != null)
            {
                listeDeTachesPlanifiees.Add(new Tache(desc, dateCreation, null, null, new List<Etape>()));
                manager.Taches.Add(new Tache(desc, dateCreation, null, null, new List<Etape>()));
                RafraichirListes();
            }

        }

        // --- Suppression d’une tâche ---
        private void OnDeleteTask(object sender, EventArgs e)
        {
            //Retirer la tâche du doc
            //Retirer la tâche de la liste
            XmlDocument doc = new();
            doc.Load(chemin);

            int index = manager.Taches.IndexOf(selectionTache);
            XmlNode noeud = doc.SelectSingleNode($"/taches/tache[{index + 1}]");

            if (selectionTache != null)
            {
                manager.Taches.Remove(selectionTache);

                if (listeDeTachesPlanifiees.Contains(selectionTache))
                {
                    listeDeTachesPlanifiees.Remove(selectionTache);
                } else if (listeDeTachesEnCours.Contains(selectionTache))
                {
                    listeDeTachesEnCours.Remove(selectionTache);
                } else
                {
                    listeDeTachesCompletees.Remove(selectionTache);
                }
                noeud.ParentNode.RemoveChild(noeud);
                TaskSteps.ItemsSource = null;
                TaskSteps.ItemsSource = selectionTache.Etapes;
                RafraichirListes();
            }
        }

        // --- Ajout d’une étape ---
        private async void OnAddStep(object sender, EventArgs e)
        {
            if (Etapes_Entry.Text != null)
            {
                int numero = selectionTache.Etapes.Count + 1;
                string desc = Etapes_Entry.Text;
                Etape etape = new Etape(numero, desc);

                manager.Taches.Find(x => x == selectionTache).Etapes.Add(etape);

                RafraichirListes();
            }
        }

        // --- Suppression d’une étape ---
        private void OnDeleteStep(object sender, EventArgs e)
        {
            XmlDocument doc = new();
            doc.Load(chemin);
            XmlNode noeud = doc.SelectSingleNode($"/taches/tache/etapes/etape[@no='{selectionEtape.Numero}']");

            if (selectionEtape != null) {
                selectionTache.Etapes.Remove(selectionEtape);
                manager.Taches.Find(x => x.Description == selectionTache.Description).Etapes = selectionTache.Etapes;
                noeud.ParentNode.RemoveChild(noeud);
                RafraichirListes();
                InitialiserSelectionEtape();
            }
        }

        // --- Terminer une étape ---
        private void OnCompleteStep(object sender, EventArgs e)
        {
            if (selectionEtape != null)
            {
                manager.TerminerEtape(selectionTache, selectionEtape);
                RafraichirListes();
            }
        }

        // --- À propos ---
        private async void OnAboutClicked(object sender, EventArgs e)
        {

        }
    }
}
