using KanbanLibrary;

namespace KanbanTestProject
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestStatutTache()
        {
            Tache tache = new Tache("", DateOnly.FromDateTime(DateTime.Now), null, null, new List<Etape>());
            Assert.That(tache.Statut, Is.EqualTo(0));
        }
        [Test]
        public void TestEnCours()
        {
            Tache tache = new Tache("", DateOnly.FromDateTime(DateTime.Now), null, null,
                new List<Etape>
                { new Etape(0, "", true),
                    new Etape(0, "", false),
                    new Etape(0, "", false) });
            Assert.That(tache.Statut, Is.EqualTo(1));
        }
        [Test]
        public void TestEtapesTerminees()
        {
            Tache tache = new Tache("", DateOnly.FromDateTime(DateTime.Now), null, null,
                new List<Etape>
                { new Etape(0, "", true),
                    new Etape(0, "", true),
                    new Etape(0, "", true) });
            Assert.That(tache.Statut, Is.EqualTo(2));
        }

    }
}
