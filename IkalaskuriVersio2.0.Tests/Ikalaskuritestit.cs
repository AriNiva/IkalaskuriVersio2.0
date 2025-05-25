using IkalaskuriVersio2._0.Services;
using Moq;

namespace IkalaskuriVersio2._0.Tests
{
    public class Ikalaskuritestit
    {
        private static (Mock<IKayttoliittyma>, IkalaskuriService) LuoTestiYmparisto()
        {
            var mockUI = new Mock<IKayttoliittyma>();
            var palvelu = new IkalaskuriService(mockUI.Object);
            return (mockUI, palvelu);
        }

        [Theory]
        [InlineData("Mies")]
        [InlineData("Nainen")]
        public void KysyKayttajanSukupuoli_Syote_Kelvollinen(string syote)
        {
            // Otetaan k‰yttˆˆn LuoTestiYmparisto - metodin palauttamat arvot tuplena
            var (mockUI, palvelu) = LuoTestiYmparisto();

            mockUI.Setup(ui => ui.LueSyote()).Returns(syote);

            var sukupuoli = palvelu.KysyKayttajanSukupuoli();

            Assert.Equal(syote, sukupuoli);
            mockUI.Verify(ui => ui.Tulosta("Kerro sukupuolesi. (Mies / Nainen)."), Times.Once());
        }

        [Fact]
        public void KysyKayttajanSukupuoli_Nayttaa_Virheviestin_Jos_Syote_Ei_Kelvollinen() 
        {
            var (mockUI, palvelu) = LuoTestiYmparisto();

            var syotteet = new Queue<string>(["x", "Mies"]);
            
            mockUI.Setup(ui => ui.LueSyote()).Returns(()  => syotteet.Dequeue());

            var sukupuoli = palvelu.KysyKayttajanSukupuoli();

            Assert.Equal("Mies", sukupuoli);
            mockUI.Verify(ui => ui.Tulosta("Virhe: Syˆt‰ Mies tai Nainen."), Times.Once());
        }

        [Fact]
        public void KysyKayttajanSyntymaAika_Syote_Kelvollinen() 
        {
            var (mockUI, palvelu) = LuoTestiYmparisto();

            mockUI.Setup(ui => ui.LueSyote()).Returns("01.05.2025");

            var syntymaAika = palvelu.KysyKayttajanSyntymaAika();

            Assert.Equal(new DateTime(2025, 5, 1), syntymaAika); // Vertailee DateTime vs DateTime
            mockUI.Verify(ui => ui.Tulosta("Anna syntym‰aika muodossa PP.KK.VVVV."), Times.Once());
            mockUI.Verify(ui => ui.Tulosta("Virheellinen p‰iv‰m‰‰r‰muoto! K‰yt‰ muotoa PP.KK.VVVV."), Times.Never());

            /* Miksi Assert.Equal(new DateTime(2025, 5, 1), syntymaAika) on oikein:
               T‰m‰ on oikein, koska molemmat ovat DateTime-tyyppisi‰. DateTime ei tallenna p‰iv‰m‰‰r‰‰ miss‰‰n merkkijonon muodossa 
               vaan strukturoituna p‰iv‰m‰‰r‰- ja aikadatana. Kun teet new DateTime(2025, 5, 1), se vastaa 01.05.2025.*/
        }

        [Fact]
        public void KysyKayttajanSyntymaAika_Nayttaa_Virheviestin_Jos_Syote_Ei_Kelvollinen() 
        {
            var (mockUI, palvelu) = LuoTestiYmparisto();

            var syotteet = new Queue<string>(["2025-05-01", "01.05.2025"]);

            mockUI.Setup(ui => ui.LueSyote()).Returns(() => syotteet.Dequeue());

            palvelu.KysyKayttajanSyntymaAika();

            mockUI.Verify(ui => ui.Tulosta("Virheellinen p‰iv‰m‰‰r‰muoto! K‰yt‰ muotoa PP.KK.VVVV."), Times.Once());
        }

        [Theory]
        [InlineData("Mies", 78)]
        [InlineData("Nainen", 84)]
        public void Odote_Palauttaa_Oikean_ElinIanOdotteen(string sukupuoli, int elinIanOdote) 
        {
            var (_, palvelu) = LuoTestiYmparisto();

            int tulos = palvelu.Odote(sukupuoli);

            Assert.Equal(elinIanOdote, tulos);
        }


    }
}