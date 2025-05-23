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

            palvelu.KysyKayttajanSyntymaAika();

            mockUI.Verify(ui => ui.Tulosta("Anna syntym‰aika muodossa PP.KK.VVVV:"), Times.Once());
            mockUI.Verify(ui => ui.Tulosta("Virheellinen p‰iv‰m‰‰r‰muoto! K‰yt‰ muotoa PP.KK.VVVV."), Times.Never());
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
        public void GetElinIanOdote_Palauttaa_Oikean_ElinIanOdotteen(string sukupuoli, int elinIanOdote) 
        {
            var (_, palvelu) = LuoTestiYmparisto();

            int tulos = palvelu.GetElinIanOdote(sukupuoli);

            Assert.Equal(elinIanOdote, tulos);
        }

        [Fact]
        public void LaskeJaljellaOlevaAika_Palauttaa_Oikean_Jaljella_Olevan_Ajan() 
        {
            var (_, palvelu) = LuoTestiYmparisto();

            /* DateTime.Today antaa t‰m‰n p‰iv‰n p‰iv‰m‰‰r‰n, esim 01.05.2025. 
               .AddYears(-20) v‰hent‰‰ 20 vuotta -> 01.05.2005.
               N‰in saadaan testihenkilˆlle syntym‰aika, josta laskettuna h‰n olisi nyt 20-vuotias.*/
            DateTime syntymaAika = DateTime.Today.AddYears(-20);

            // M‰‰ritell‰‰n elini‰n odote 80 vuoteen.
            int odote = 80;

            /* Kutsutaan LaskeJ‰ljell‰OlevaAika-metodia, joka palauttaa tuplen (vuodet, kuukaudet, p‰iv‰t)
               Otetaan k‰yttˆˆn vain vuodet. Muut (kuukaudet, paivat) j‰tet‰‰n huomiotta k‰ytt‰m‰ll‰ _ -sijoituksia (ns. discards)*/
            var (vuodet, _, _) = palvelu.LaskeJaljellaOlevaAika(syntymaAika, odote);

            /* Testi sallii pient‰ vaihtelua vuosissa (59-61), koska:
               Vuodessa ei ole aina tasan 365 p‰iv‰‰.
               Karkausvuodet vaikuttavat DateTime-laskuihin.
               .Days / 365.25 ei aina anna t‰sm‰lleen "vuotta."
               Jos laskettu vuosim‰‰r‰ on 59,60 tai 61 -> testi menee l‰pi.*/
            Assert.True(vuodet >= 59 && vuodet <= 61);
        }
    }
}