using IkalaskuriVersio2._0.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Authentication.ExtendedProtection;

namespace IkalaskuriVersio2._0
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddTransient<IKayttoliittyma, KonsoliKayttoliittyma>()
                .AddTransient<IkalaskuriService>()
                .BuildServiceProvider();

            var ikalaskuriService = serviceProvider.GetRequiredService<IkalaskuriService>();
            ikalaskuriService.Suorita();
        }
    }
}
