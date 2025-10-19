using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanLibrary
{
    public interface IConversionXML
    {
        List<Tache> ChargerDepuisXML(string cheminFichier);
        void SauvegarderVersXML(string cheminFichier, List<Tache> taches);
    }
}
