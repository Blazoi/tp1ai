namespace Biblio_de_classes
{
    public class Etape
    {
        public int Numero {  get; set; }
        public string Description { get; set; }
        public bool Terminee { get; set; }

        public Etape (int numero, string description, bool terminee)
        {
            Numero = numero;
            Description = description;
            Terminee = terminee;
        }
    }
}
