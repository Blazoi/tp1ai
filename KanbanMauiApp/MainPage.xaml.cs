using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml;
using KanbanLibrary;

namespace KanbanMauiApp
{
    public partial class MainPage : ContentPage
    {
      
        
        ManagementTache manager = new();
        Etape selectionEtape;
        Tache selectionTache;
        List<Tache> listeDeTaches;
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
            manager.Taches = liste;
            PlannedTasks.ItemsSource = liste;
            BindingContext = liste;
            listeDeTaches = liste;
        }

        // --- Sauvegarder les tâches dans un fichier ---
        private void Sauvegarder()
        {
            manager.SauvegarderVersXML(chemin, listeDeTaches);
        }
        // --- Rafraîchir les listes ---
        private void RafraichirListes()
        {
            //Réinitialiser et réassigner la liste source
            TaskSteps.ItemsSource = null;
            TaskSteps.ItemsSource = listeDeTaches.Find(x => x.Description == selectionTache.Description).Etapes;
        }

        // --- Sélection d’une tâche ---
        private void OnTaskSelected(object sender, SelectionChangedEventArgs e)
        {
            //Trouver l'index de la tâche sélectionnée
            int index = listeDeTaches.IndexOf(e.CurrentSelection.First() as Tache);
            selectionTache = listeDeTaches[index];

            //Afficher la description
            TaskDescription.Text = selectionTache.Description;
            string format = "dddd MMM dd, yyyy";
            TaskDates.Text = $"Date de création : {selectionTache.DateCreation.ToString(format)}" +
                             $"\nDate de début : {selectionTache.DateDebut?.ToString(format) ?? "non définie"}" +
                             $"\nDate de fin : {selectionTache.DateFin?.ToString(format) ?? "non définie\n"}";

            //Afficher les étapes
            TaskSteps.ItemsSource = selectionTache.Etapes;
            InitialiserSelectionEtape(selectionTache);
        }

        // --- Sélection d’une étape ---
        private void OnStepSelected(object sender, SelectionChangedEventArgs e)
        {
            selectionEtape = e.CurrentSelection.FirstOrDefault() as Etape;
        }
        // Appelé après que ListeEtapes.ItemsSource soit définie
        private void InitialiserSelectionEtape(Tache tache)
        {
            selectionEtape = tache.Etapes.Find(x => x.Termine == false);
        }
        private async void OnAddTask(object sender, EventArgs e)
        {
            
        }

        // --- Suppression d’une tâche ---
        private void OnDeleteTask(object sender, EventArgs e)
        {
            XmlDocument doc = new();
            doc.Load(chemin);

            int index = listeDeTaches.IndexOf(selectionTache);
            XmlNode noeud = doc.SelectSingleNode($"/taches/tache[{index + 1}]");
            if (selectionTache != null)
            {
                listeDeTaches.Remove(selectionTache);
                noeud.ParentNode.RemoveChild(noeud);
                Sauvegarder();
            }
        }

        // --- Ajout d’une étape ---
        private async void OnAddStep(object sender, EventArgs e)
        {
            
        }

        // --- Suppression d’une étape ---
        private void OnDeleteStep(object sender, EventArgs e)
        {
            XmlDocument doc = new();
            doc.Load(chemin);
            XmlNode noeud = doc.SelectSingleNode($"/taches/tache/etapes/etape[@no='{selectionEtape.Numero}']");

            if (selectionEtape != null) {
                selectionTache.Etapes.Remove(selectionEtape);
                noeud.ParentNode.RemoveChild(noeud);
                RafraichirListes();
                selectionEtape = null;
            }
            Sauvegarder();
        }

        // --- Terminer une étape ---
        private void OnCompleteStep(object sender, EventArgs e)
        {
            if (selectionEtape != null)
            {
                selectionEtape.Termine = true;
            }
        }

        // --- À propos ---
        private async void OnAboutClicked(object sender, EventArgs e)
        {
            Sauvegarder();
        }
    }
}
