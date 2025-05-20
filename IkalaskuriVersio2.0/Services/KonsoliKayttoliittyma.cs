using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IkalaskuriVersio2._0.Services
{
    internal class KonsoliKayttoliittyma : IKayttoliittyma
    {
        public string LueSyote() 
        {
            string syote = Console.ReadLine() ?? string.Empty;

            return syote;
        }

        public void Tulosta(string viesti) 
        { 
            Console.WriteLine(viesti);
        }
    }
}
