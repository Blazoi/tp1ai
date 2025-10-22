using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanLibrary
{
    public class Tache
    {
        public string Description { get; set; }
        public DateOnly DateCreation { get; set; }
        public DateOnly? DateDebut { get; set; }
        public DateOnly? DateFin { get; set; }

        public List<Etape> Etapes { get; set; }

        public int Statut { get; set; }

        public Tache(string description, DateOnly dateCreation, DateOnly? dateDebut, DateOnly? dateFin, List<Etape> etapes)
        {
            Description = description;
            DateCreation = dateCreation;
            DateDebut = dateDebut;
            DateFin = dateFin;
            Etapes = etapes;

            DefinirStatut();
        }

        private void DefinirStatut()
        {
            int compte = Etapes.Count;
            int nmb = Etapes.Count(x => x.Termine == true);

            if (nmb == compte)
            {
                Statut = 2;
            } else if (nmb > 0 && nmb < compte)
            {
                Statut = 1;
            } else
            {
                Statut = 0;
            }
        }
    }
}
