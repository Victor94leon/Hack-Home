using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using Entities;
using System.Collections.Generic;
using System.Net;

namespace SAL
{
    public class ServiceClient
    {
        //Direccion base de la Web API
        string WebAPIBaseAddress = "https://ticapacitacion.com/hackathome/";

        public ServiceClient()
        {

        }

        //Realiza la autenticacion al servicio Web API
        public async Task<ResultInfo> AutenticateAsync(
            string studentEmail, string studentPassword)
        {
            ResultInfo Result = null;

            
            // ID del diplomado.
            string EventID = "xamarin30";

            string RequestUri = "api/evidence/Authenticate";

            // El servicio requiere un objeto UserInfo con los datos del usuario y evento.
            UserInfo User = new UserInfo
            {
                Email = studentEmail,
                Password = studentPassword,
                EventID = EventID
            };
            // Utilizamos el objeto System.Net.Http.HttpClient para consumir el servicio REST
            // Debe instalarse el paquete Nuget System.Net.Http
            using (var Client = new HttpClient())
            {
                // Establecemos la direccion base del servicio REST
                Client.BaseAddress = new Uri(WebAPIBaseAddress);

                // Limpiamos encabezados de la petición.
                Client.DefaultRequestHeaders.Accept.Clear();

                // Indicamos al servicio que envie los datos en formato JSON.
                Client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    // Serializamos a formato JSON el objeto a enviar.
                    // Debe instalarse el paquete NuGet Newtonsoft.Json.
                    var JSONUserInfo = JsonConvert.SerializeObject(User);

                    // Hacemos una peticion POST al servicio enviando el objeto JSON
                    HttpResponseMessage Response =
                        await Client.PostAsync(RequestUri,
                        new StringContent(JSONUserInfo.ToString(),
                                          Encoding.UTF8, "application/json"));

                    // Leemos el resultado devuelto.
                    var ResultWebAPI = await Response.Content.ReadAsStringAsync();

                    // Deserializamos el resultado JSON obtenido
                    Result = JsonConvert.DeserializeObject<ResultInfo>(ResultWebAPI);
                }
                catch (System.Exception)
                {

                }
            }
            return Result;
        }
        // Lista de evidencias
        public async Task<List<Evidence>> GetEvidencesAsync(string token)
        {
            List<Evidence> Evidences = null;

            // URI completo
            string URI = $"{WebAPIBaseAddress}api/evidence/getevidences?token={token}";

            using (var Client = new HttpClient())
            {
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    // Realizamos una peticion GET
                    var Response =
                        await Client.GetAsync(URI);

                    if(Response.StatusCode == HttpStatusCode.OK)
                    {
                        // Si el estatus de la respuesta HTTP fue exitosa, leemos
                        // el valor devuelto.

                        var ResultWebAPI = await Response.Content.ReadAsStringAsync();

                        Evidences =
                            JsonConvert.DeserializeObject<List<Evidence>>(ResultWebAPI);
                    }
                }
                catch(System.Exception)
                {

                }
            }
            return Evidences;
        }

        // Informacion de la evidencia
        public async Task<EvidenceDetail> GetEvidenceByIDAsync(string token, int evidenceID)
        {
            EvidenceDetail Result = null;

            // URI de la evidencia
            string URI = 
                $"{WebAPIBaseAddress}api/evidence/getevidencebyid?token={token}&&evidenceid={evidenceID}";

            using (var Client = new HttpClient())
            {
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    // Realizamos una peticion GET
                    var Response = await Client.GetAsync(URI);

                    var ResultWebAPI = await Response.Content.ReadAsStringAsync();

                    if (Response.StatusCode == HttpStatusCode.OK)
                    {
                        // Si el estatus de la respuesta HTTP fue exitosa, leemos
                        // el valor devuelto.
                        Result = JsonConvert.DeserializeObject<EvidenceDetail>(ResultWebAPI);
                    }
                }
                catch (System.Exception)
                {

                }
            }
            return Result;
        }
    }
}