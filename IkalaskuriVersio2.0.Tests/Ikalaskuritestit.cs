using IkalaskuriVersio2._0.Services;
using Moq;

namespace IkalaskuriVersio2._0.Tests
{
    public class Ikalaskuritestit
    {
        [Theory]
        [InlineData("Mies")]
        [InlineData("Nainen")]
        public void KysyKayttajanSukupuoli_Syote_Kelvollinen(string syote)
        {
            var mockUI = new Mock<IKayttoliittyma>();
            var palvelu = new IkalaskuriService(mockUI.Object);

            mockUI.Setup(ui => ui.LueSyote()).Returns(syote);

            var sukupuoli = palvelu.KysyKayttajanSukupuoli();

            Assert.Equal(syote, sukupuoli);
            mockUI.Verify(ui => ui.Tulosta("Kerro sukupuolesi. (Mies / Nainen)."), Times.Once());
        }
    }
}