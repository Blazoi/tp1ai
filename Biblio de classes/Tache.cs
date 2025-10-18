using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio_de_classes
{
    public class Tache
    {
        public string Description {  get; set; }
        public DateOnly DateCreation { get; set; }
        public DateOnly? DateDebut { get; set; }
        public DateOnly? DateFin { get; set; }
        public List<Etape> Etapes { get; set; }
        public int Statut { get; set; } = 0;

        public Tache (string description, DateOnly dateCreation, DateOnly? dateDebut, DateOnly? dateFin, List<Etape> etapes, int statut)
        {
            Description = description;
            DateCreation = dateCreation;
            DateDebut = dateDebut;
            DateFin = dateFin;
            Etapes = etapes;
            Statut = statut;
        }
    }
}
