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

        string chemin = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "taches.xml");
        public MainPage()
        {
            InitializeComponent();
            ChargerTachesDepuisFichier();
            
        }
        // --- Charger les tâches depuis un fichier ---
        private void ChargerTachesDepuisFichier()
        {
            manager.Taches = manager.ChargerDepuisXML(chemin); ;

            PlannedTasks.ItemsSource = manager.Taches.Where(x => x.Statut == 0);
            InProgressTasks.ItemsSource = manager.Taches.Where(x => x.Statut == 1);
            CompletedTasks.ItemsSource = manager.Taches.Where(x => x.Statut == 2);
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
            PlannedTasks.ItemsSource = manager.Taches.Where(x => x.Statut == 0);

            InProgressTasks.ItemsSource = null;
            InProgressTasks.ItemsSource = manager.Taches.Where(x => x.Statut == 1);

            CompletedTasks.ItemsSource = null;
            CompletedTasks.ItemsSource = manager.Taches.Where(x => x.Statut == 2);
        }

        // --- Sélection d’une tâche ---
        private void OnTaskSelected(object sender, SelectionChangedEventArgs e)
        {
            selectionTache = e.CurrentSelection.FirstOrDefault() as Tache;

            if (selectionTache.Statut == 0)
            {
                InProgressTasks.SelectionChanged -= OnTaskSelected;
                InProgressTasks.SelectedItem = null;
                InProgressTasks.SelectionChanged += OnTaskSelected;

                CompletedTasks.SelectionChanged -= OnTaskSelected;
                CompletedTasks.SelectedItem = null;
                CompletedTasks.SelectionChanged += OnTaskSelected;
            }
            else if (selectionTache.Statut == 1)
            {
                PlannedTasks.SelectionChanged -= OnTaskSelected;
                PlannedTasks.SelectedItem = null;
                PlannedTasks.SelectionChanged += OnTaskSelected;

                CompletedTasks.SelectionChanged -= OnTaskSelected;
                CompletedTasks.SelectedItem = null;
                CompletedTasks.SelectionChanged += OnTaskSelected;
            }
            else if (selectionTache.Statut == 2)
            {
                PlannedTasks.SelectionChanged -= OnTaskSelected;
                PlannedTasks.SelectedItem = null;
                PlannedTasks.SelectionChanged += OnTaskSelected;

                InProgressTasks.SelectionChanged -= OnTaskSelected;
                InProgressTasks.SelectedItem = null;
                InProgressTasks.SelectionChanged += OnTaskSelected;
            }

            //Description
            TaskDescription.Text = selectionTache.Description;
            string format = "dddd MMM dd, yyyy";
            TaskDates.Text = $"Date de création : {selectionTache.DateCreation.ToString(format)}" +
                             $"\nDate de début : {selectionTache.DateDebut?.ToString(format) ?? "non définie"}" +
                             $"\nDate de fin : {selectionTache.DateFin?.ToString(format) ?? "non définie\n"}";

            //Étapes
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
                manager.Taches.Add(new Tache(desc, dateCreation, null, null, new List<Etape>()));
                RafraichirListes();
            }

        }

        // --- Suppression d’une tâche ---
        private void OnDeleteTask(object sender, EventArgs e)
        {
            XmlDocument doc = new();
            doc.Load(chemin);

            int index = manager.Taches.IndexOf(selectionTache);
            XmlNode noeud = doc.SelectSingleNode($"/taches/tache[{index + 1}]");

            if (selectionTache != null)
            {
                manager.Taches.Remove(selectionTache);
                noeud.ParentNode.RemoveChild(noeud);
                selectionTache = null;
                TaskSteps.ItemsSource = null;
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
            await DisplayAlert("Système de gestion de tâches", "Version 3.2\nJack Adam's Dieujuste", "Fermer");
        }
    }
}
