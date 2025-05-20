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
            KysyKayttajanSukupuoli(); 
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
    }
}
