
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
            DateTime tanaan = DateTime.Today;
            var sukupuoli = KysyKayttajanSukupuoli();
            var syntymaAika = KysyKayttajanSyntymaAika();
            var elinIanOdote = Odote(sukupuoli);
            var kuolinpaiva = LaskeKuolinPaiva(syntymaAika, elinIanOdote);
            var erotus = LaskeErotus(kuolinpaiva, tanaan);
            var vuodet = LaskeKokonaisetVuodet(erotus);
            var paivatJaljella = LaskePaivatJaljella(erotus, vuodet);
            var kuukaudet = LaskeKuukaudet(paivatJaljella);
            var paivat = LaskePaivat(paivatJaljella);
            MuodostaViesti(kuolinpaiva, tanaan, vuodet, kuukaudet, paivat);
        }

        internal string KysyKayttajanSukupuoli() 
        {
            string sukupuoli;

            do
            {
                _ui.Tulosta("Kerro sukupuolesi. (Mies / Nainen).");
                sukupuoli = _ui.LueSyote();
                
                if (!OnKelvollinenSukupuoli(sukupuoli))
                {
                    _ui.Tulosta("Virhe: Syötä Mies tai Nainen.");
                }
            }
            while (!OnKelvollinenSukupuoli(sukupuoli));

            var s = sukupuoli.Trim().ToLower();

            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static bool OnKelvollinenSukupuoli(string syote) 
        { 
            if (string.IsNullOrWhiteSpace(syote)) 
                return false;

            var s = syote.Trim().ToLower();
            return s == "mies" || s == "nainen";
        }

        internal DateTime KysyKayttajanSyntymaAika() 
        {
            string syote;
            DateTime syntymaAika;
            bool ok;

            do
            {
                _ui.Tulosta("Anna syntymäaika muodossa PP.KK.VVVV.");
                syote = _ui.LueSyote();

                ok = OnKelvollinenSyntymaAika(syote, out syntymaAika);

                if (!ok)
                {
                    _ui.Tulosta("Virheellinen päivämäärämuoto! Käytä muotoa PP.KK.VVVV.");
                }
            }
            while (!ok);

            return syntymaAika;
        }

        public static bool OnKelvollinenSyntymaAika(string syote, out DateTime syntymaAika)
        {
            return DateTime.TryParseExact(syote, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out syntymaAika);
        }

        internal int Odote(string sukupuoli) 
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

        // Esimerkki laskemisesta Readme-tiedostossa.
        internal DateTime LaskeKuolinPaiva(DateTime syntymaAika, int elinIanOdote) 
        {
            // Lisätään syntymäaikaan eliniänodotteen verran vuosia -> saadaan odotettu kuolinpäivä.
            DateTime kuolinpaiva = syntymaAika.AddYears(elinIanOdote);

            return kuolinpaiva;
        }

        internal TimeSpan LaskeErotus(DateTime kuolinpaiva, DateTime tanaan) 
        {
            /* Lasketaan erotus tänään ja odotetun kuolinpäivän välillä
               * erotus on TimeSpan, joka kertoo montako päivää on jäljellä */
            TimeSpan erotus;

            erotus = kuolinpaiva - tanaan;
            
            return erotus;
        }

        internal int LaskeKokonaisetVuodet(TimeSpan erotus) 
        {
            // Tässä lasketaan montako kokonaista vuotta jäljellä olevista päivistä saadaan

            int vuodet = (int)(erotus.Days / 365.25);

            return vuodet;
        }

        internal int LaskePaivatJaljella(TimeSpan erotus, int vuodet) 
        {
            // Lasketaan, montako päivää jää vielä jäljelle, kun täydet vuodet on ensin otettu pois
            int paivatJaljella = erotus.Days - (int)(vuodet * 365.25);

            return paivatJaljella;
        }

        internal int LaskeKuukaudet(int paivatJaljella) 
        {
            // Nyt jäljelle jääneet päivät jaetaan kuukausiksi
            int kuukaudet = paivatJaljella / 30;

            return kuukaudet;
        }

        internal int LaskePaivat(int paivatJaljella) 
        {
            // Lopuksi katsotaan, montako päivää jää vielä jäljelle, kun kuukaudet on otettu pois
            int paivat = paivatJaljella % 30;

            return paivat;
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
