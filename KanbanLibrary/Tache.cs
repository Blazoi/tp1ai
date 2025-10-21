using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanLibrary
{
    public class Tache : INotifyPropertyChanged
    {
        public string Description { get; set; }
        public DateOnly DateCreation { get; set; }
        public DateOnly? DateDebut { get; set; }
        public DateOnly? DateFin { get; set; }

        private List<Etape> etapes;
        public List<Etape> Etapes
        {
            get => etapes;
            set
            {
                etapes = value;
                OnPropertyChanged("Etapes");
            }
        }

        public int Statut { get; set; }

        public Tache(string description, DateOnly dateCreation, DateOnly? dateDebut, DateOnly? dateFin, List<Etape> etapes, int statut = 0)
        {
            Description = description;
            DateCreation = dateCreation;
            DateDebut = dateDebut;
            DateFin = dateFin;
            Etapes = etapes;
            Statut = statut;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
