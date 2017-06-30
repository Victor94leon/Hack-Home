using Entities;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace SAL
{
    public class MicrosoftServiceClient
    {
        // Cliente para acceder al servicio Mobile
        MobileServiceClient Client;
        // Objeto para realizar operaciones con Tablas de Mobile Service
        private IMobileServiceTable<LabItem> LabItemTable;

        // Enviar una evidencia
        public async Task SendEvidence(LabItem userEvidence)
        {
            Client =
                new MobileServiceClient(@"http://xamarin-diplomado.azurewebsites.net/");
            LabItemTable = Client.GetTable<LabItem>();
            await LabItemTable.InsertAsync(userEvidence);
        }
    }
}