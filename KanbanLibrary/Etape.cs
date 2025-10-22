using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

namespace KanbanLibrary
{
    public class Etape : INotifyPropertyChanged
    {
        public int Numero { get; set; }
        public string Description { get; set; }

        private bool termine;
        public bool Termine
        {
            get => termine;
            set
            {
                termine = value;
                OnPropertyChanged("Termine");
            }
        }

        public Etape(int numero, string description, bool termine = false)
        {
            Numero = numero;
            Description = description;
            Termine = termine;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
