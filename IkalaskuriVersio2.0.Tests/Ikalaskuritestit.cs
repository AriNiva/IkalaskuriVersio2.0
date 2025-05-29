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

            var tulos = palvelu.Odote(sukupuoli);

            Assert.Equal(tulos, elinIanOdote);
        }

        [Fact]
        public void LaskeKuolinPaiva_Palauttaa_Oikean_Paivan() 
        {
            var (_, palvelu) = LuoTestiYmparisto();

            DateTime syntymaAika = new DateTime(2025, 5, 1);
            int odote = 80;
            DateTime tulos = new DateTime(2105, 5, 1); 
            
            var kuolinpaiva = palvelu.LaskeKuolinPaiva(syntymaAika, odote);

            Assert.Equal(tulos, kuolinpaiva); // expected, actual
        }

        [Theory]
        [InlineData("2025-05-23", "2026-05-23", 365)]
        [InlineData("2024-02-28", "2024-02-29", 1)] // karkausp‰iv‰
        public void LaskeErotus_Palauttaa_Oikean_Paivamaaraerotuksen(string tanaanStr, string kuolinpaivaStr, int odotetutPaivat)
        {
            // Arrange
            var (_, palvelu) = LuoTestiYmparisto();

            // Parsitaan merkkijonot oikeiksi DateTime-olioiksi.
            DateTime tanaan = DateTime.Parse(tanaanStr);
            DateTime kuolinpaiva = DateTime.Parse(kuolinpaivaStr);

            // Lasketaan odotettu erotus TimeSpan.FromDays(...) avulla.
            TimeSpan odotettu = TimeSpan.FromDays(odotetutPaivat);

            // Act
            var erotus = palvelu.LaskeErotus(kuolinpaiva, tanaan);

            // Assert
            Assert.Equal(odotettu, erotus);
        }

        [Fact]
        public void LaskeKokonaisetVuodet_Palauttaa_Vuodet_Oikein() 
        {
            var (_, palvelu) = LuoTestiYmparisto();

            // M‰‰ritell‰‰n kaksi p‰iv‰m‰‰r‰‰.
            DateTime paiva1 = new DateTime(2015, 5, 1);
            DateTime paiva2 = new DateTime(2025, 5, 1);

            // Lasketaan p‰iv‰m‰‰rien v‰linen erotus.
            TimeSpan erotus = paiva2 - paiva1;

            int odotettu = 10;

            var tulos = palvelu.LaskeKokonaisetVuodet(erotus);

            Assert.Equal(odotettu, tulos);
        }

        [Fact]
        public void LaskePaivatJaljella_Palauttaa_Paivat_Oikein() 
        {
            var (_, palvelu) = LuoTestiYmparisto();

            // M‰‰ritell‰‰n kaksi p‰iv‰m‰‰r‰‰.
            DateTime paiva1 = new DateTime(2010, 1, 1);
            DateTime paiva2 = new DateTime(2020, 2, 10); // 10 vuotta + 40 p‰iv‰‰

            // Lasketaan p‰iv‰m‰‰rien v‰linen erotus.
            TimeSpan erotus = paiva2 - paiva1;

            int vuodet = 10;
            int odotettu = 40;

            var tulos = palvelu.LaskePaivatJaljella(erotus, vuodet);

            Assert.Equal(odotettu, tulos);
        }

        [Fact]
        public void LaskeKuukaudet_Palauttaa_Kuukaudet_Oikein()
        {
            var (_, palvelu) = LuoTestiYmparisto();

            int paivatJaljella = 60;
            int odotettu = 2;

            var tulos = palvelu.LaskeKuukaudet(paivatJaljella);

            Assert.Equal(odotettu, tulos);
        }

        [Fact] 
        public void LaskePaivat_Palauttaa_Paivat_Oikein() 
        {
            var (_, palvelu) = LuoTestiYmparisto();

            int paivatJaljella = 40;
            int odotettu = 10;

            var tulos = palvelu.LaskePaivat(paivatJaljella);

            Assert.Equal(odotettu, tulos);
        }

        [Fact]
        public void MuodostaViesti_Palauttaa_Elinaikaviestin_Jos_Elinika_Ei_Ylittynyt() 
        {
            var (mockUI, palvelu) = LuoTestiYmparisto();

            DateTime syntymaAika = new DateTime(2000, 5, 1);
            DateTime tanaan = new DateTime(2020, 5, 1);
            DateTime kuolinpaiva = new DateTime(2080, 5, 1);
            int vuodet = 60;
            int kuukaudet = 0;
            int paivat = 0;
           
            palvelu.MuodostaViesti(kuolinpaiva, tanaan, vuodet, kuukaudet, paivat);
            
            mockUI.Verify(ui => ui.Tulosta("Odotettua elinaikaa j‰ljell‰ 60 vuotta 0 kuukautta ja 0 p‰iv‰‰."), Times.Once());
        }

        [Fact] 
        public void MuodostaViesti_Palauttaa_Onnittelun_Jos_Elinika_On_Ylittynyt() 
        {
            var (mockUI, palvelu) = LuoTestiYmparisto();

            DateTime syntymaAika = new DateTime(1925, 5, 1);
            DateTime tanaan = new DateTime(2025, 5, 1);
            DateTime kuolinpaiva = new DateTime(2005, 5, 1);
            int vuodet = 0;
            int kuukaudet = 0;
            int paivat = 0;

            palvelu.MuodostaViesti(kuolinpaiva, tanaan, vuodet, kuukaudet, paivat);

            mockUI.Verify(ui => ui.Tulosta("Onneksi olkoon - olet ylitt‰nyt odotetun elini‰n!"), Times.Once());
        }

    }
}