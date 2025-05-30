
using IkalaskuriVersio2._0.Services;
using Moq;

namespace IkalaskuriVersio2._0.Tests
{
    public class IkalaskuriIntegraatiotestit
    {
        /* Integraatiotestaus: 
           * Testaa useiden komponenttien (ei yksittäisten metodien) yhdessä toimimista
           * Simuloi todellista käyttötilannetta usean vaiheen läpi.*/

        // Testaa Suorita-metodin läpimenon.
        [Fact]
        public void Suorita_KokoProsessi_TuottaaOikeatTulosteet()
        {
            // Arrange
            var mockUI = new Mock<IKayttoliittyma>();
            var palvelu = new IkalaskuriService(mockUI.Object);

            var syotteet = new Queue<string>(["Nainen", "01.01.2000"]);

            mockUI.Setup(ui => ui.LueSyote()).Returns(() => syotteet.Dequeue());

            // Act
            palvelu.Suorita();

            // Assert
            mockUI.Verify(ui => ui.Tulosta("Kerro sukupuolesi. (Mies / Nainen)."), Times.Once());
            mockUI.Verify(ui => ui.Tulosta("Anna syntymäaika muodossa PP.KK.VVVV."), Times.Once());
            mockUI.Verify(ui => ui.Tulosta(It.Is<string>(s => s.Contains("Odotettua elinaikaa jäljellä"))), Times.Once());
        }

    }
}
