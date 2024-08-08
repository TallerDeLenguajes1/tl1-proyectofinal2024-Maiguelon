using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EspacioNombreAPI
{
    // Clase para interactuar con la API de Random User y obtener nombres aleatorios
    public class NombreAPI
    {
        // HttpClient es estático para reutilizar la instancia y mejorar el rendimiento
        private static readonly HttpClient client = new HttpClient();

        // Método asincrónico
        public async Task<string> ObtenerNombreAleatorioAsync()
        {
            // Realiza una solicitud GET y deserializa el JSON recibido
            var response = await client.GetFromJsonAsync<NombreResponse>("https://randomuser.me/api/");
            
            // Si la respuesta es válida y contiene resultados...
            if (response != null && response.results.Length > 0)
            {
                return response.results[0].name.first + " " + response.results[0].name.last; // "Ensambla" el nombre
            }
            return "Nombre Desconocido"; // Si falla, sale esto
        }
    }

    // Clases auxiliares para mapear la respuesta JSON 
    public class NombreResponse
    {
        public Result[] results { get; set; } = Array.Empty<Result>();
    }

    public class Result
    {
        public Name name { get; set; } = new Name();
    }

    public class Name
    {
        public string first { get; set; } = string.Empty;
        public string last { get; set; } = string.Empty;
    }
}
