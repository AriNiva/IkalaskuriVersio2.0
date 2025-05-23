using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IkalaskuriVersio2._0.Services
{
    internal class IkalaskuriService
    {
        private readonly IKayttoliittyma _ui;

        public IkalaskuriService(IKayttoliittyma ui)
        {
            _ui = ui;
        }

        public void Suorita() 
        {
            var sukupuoli = KysyKayttajanSukupuoli();
            var syntymaAika = KysyKayttajanSyntymaAika();
            var elinIanOdote = GetElinIanOdote(sukupuoli);

        }

        internal string KysyKayttajanSukupuoli() 
        {
            string sukupuoli;

            do
            {
                _ui.Tulosta("Kerro sukupuolesi. (Mies / Nainen).");
                sukupuoli = _ui.LueSyote();
                if (sukupuoli != "Mies" && sukupuoli != "Nainen")
                {
                    _ui.Tulosta("Virhe: Syötä Mies tai Nainen.");
                }
            }
            while (sukupuoli != "Mies" && sukupuoli != "Nainen");

            return sukupuoli;
        }

        internal DateTime KysyKayttajanSyntymaAika() 
        {
            DateTime syntymaAika;
            string syote;

            do 
            { 
                _ui.Tulosta("Anna syntymäaika muodossa PP.KK.VVVV");
                syote = _ui.LueSyote();

                if (!DateTime.TryParseExact(syote, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out syntymaAika)) 
                {
                    _ui.Tulosta("Virheellinen päivämäärämuoto! Käytä muotoa PP.KK.VVVV.");
                }
            }
            while (!DateTime.TryParseExact(syote, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out syntymaAika));

            return syntymaAika;
        }



        internal int GetElinIanOdote(string sukupuoli) 
        {
            int elinIanOdote = 0;

            switch (sukupuoli) 
            {
                case "Mies":
                    elinIanOdote = 78;
                    break;
                case "Nainen":
                    elinIanOdote = 84;
                    break;
            }
            return elinIanOdote;
        }

        /* Metodi LaskeJaljellaOlevaAika ottaa vastaan syntymäajan ja elinajan odotteen, laskee arvioidun kuolinpäivän ja kertoo
           kuinka paljon aikaa on jäljellä tänään.

         * (int vuodet, int kuukaudet, int paivat) on C#:n palautustyyppi (tuple with named elements)
           Tämä metodi palauttaa kolme lukuarvoa - ne ovat nimeltään vuodet,kuukaudet ja paivat*/
        internal (int vuodet, int kuukaudet, int paivat) LaskeJaljellaOlevaAika(DateTime syntymaAika, int elinIanOdote) 
        {
            // Lasketaan arvioitu kuolinpäivä ja haetaan tämänhetkinen päivä.
            DateTime kuolinpaiva = syntymaAika.AddYears(elinIanOdote);
            DateTime tanaan = DateTime.Today;

            // Tarkempi selitys laskemisesta Readme-tiedostossa.
            TimeSpan erotus = kuolinpaiva - tanaan;
            int vuodet = (int)(erotus.Days / 365.25);
            int paivatJaljella = erotus.Days - (int)(vuodet * 365.25);
            int kuukaudet = paivatJaljella / 30;
            int paivat = paivatJaljella % 30;

            return (vuodet, kuukaudet, paivat);
        }

        internal void MuodostaViesti(DateTime kuolinpaiva, DateTime tanaan, int vuodet, int kuukaudet, int paivat) 
        { 
            if (kuolinpaiva < tanaan) 
            {
                _ui.Tulosta("Onneksi olkoon - olet ylittänyt odotetun eliniän!");
            }
            else 
            {
                _ui.Tulosta($"Odotettua elinaikaa jäljellä {vuodet} vuotta {kuukaudet} kuukautta ja {paivat} päivää.");
            }
        }
    }
}
