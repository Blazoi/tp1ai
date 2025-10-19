using System.Collections.ObjectModel;
using System.Diagnostics;
using Android.AdServices.Measurement;
using KanbanLibrary;

namespace KanbanMauiApp
{
    public partial class MainPage : ContentPage
    {
      
        
        ManagementTache manager = new();
        Etape selectionEtape;

        public MainPage()
        {
            InitializeComponent();
            ChargerTachesDepuisFichier();
            
        }
        // --- Charger les tâches depuis un fichier ---
        private void ChargerTachesDepuisFichier()
        {
            List<Tache> liste = manager.ChargerDepuisXML("C:\\Users\\jackj\\OneDrive\\Desktop\\TP1\\KanbanMauiApp\\Data\\taches.xml");
            manager.Taches = liste;
            PlannedTasks.ItemsSource = liste;
            BindingContext = liste;
        }

        // --- Sauvegarder les tâches dans un fichier ---
        private void Sauvegarder()
        {
            //manager.SauvegarderVersXML("C:\\Users\\jackj\\OneDrive\\Desktop\\TP1\\KanbanMauiApp\\Data\\taches.xml");
        }
        // --- Rafraîchir les listes ---
        private void RafraichirListes()
        {
            manager.ChargerDepuisXML("C:\\Users\\jackj\\OneDrive\\Desktop\\TP1\\KanbanMauiApp\\Data\\taches.xml");
        }

        // --- Sélection d’une tâche ---
        private void OnTaskSelected(object sender, SelectionChangedEventArgs e)
        {
            Tache selection = e.CurrentSelection.FirstOrDefault() as Tache;
            TaskDescription.Text = selection.Description;
            string format = "dddd MMM dd, yyyy";
            TaskDates.Text = $"Date de création : {selection.DateCreation.ToString(format)}\nDate de début : {selection.DateDebut?.ToString(format) ?? "non définie"}\nDate de fin : {selection.DateFin?.ToString(format) ?? "non définie\n"}";

            List<Etape> etapes = selection.Etapes;
            TaskSteps.ItemsSource = etapes;
            BindingContext = etapes;
        }

        // --- Sélection d’une étape ---
        private void OnStepSelected(object sender, SelectionChangedEventArgs e)
        {
            selectionEtape = e.CurrentSelection.FirstOrDefault() as Etape;
        }
        // Appelé après que ListeEtapes.ItemsSource soit définie
        private void InitialiserSelectionEtape(Tache tache)
        {
            Tache t1 = manager.Taches.Find(x => x.Description == tache.Description);
            selectionEtape = t1.Etapes.FirstOrDefault();
        }
        private async void OnAddTask(object sender, EventArgs e)
        {
            
        }

        // --- Suppression d’une tâche ---
        private void OnDeleteTask(object sender, EventArgs e)
        {
            
        }

        // --- Ajout d’une étape ---
        private async void OnAddStep(object sender, EventArgs e)
        {
            
        }

        // --- Suppression d’une étape ---
        private void OnDeleteStep(object sender, EventArgs e)
        {
            
        }

        // --- Terminer une étape ---
        private void OnCompleteStep(object sender, EventArgs e)
        {
        }

        // --- À propos ---
        private async void OnAboutClicked(object sender, EventArgs e)
        {
     
        }
    }
}
