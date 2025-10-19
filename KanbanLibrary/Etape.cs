namespace KanbanLibrary
{
    public class Etape
    {
        public int Numero { get; set; }
        public string Description { get; set; }
        public bool Termine { get; set; }

        public Etape(int numero, string description, bool termine = false)
        {
            Numero = numero;
            Description = description;
            Termine = termine;
        }
    }
}
