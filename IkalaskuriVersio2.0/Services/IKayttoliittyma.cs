using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IkalaskuriVersio2._0.Services
{
    internal interface IKayttoliittyma
    {
        string LueSyote();

        void Tulosta(string viesti);
    }
}
