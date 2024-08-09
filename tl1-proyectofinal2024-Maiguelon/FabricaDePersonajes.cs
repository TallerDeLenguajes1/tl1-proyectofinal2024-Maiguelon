using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using EspacioPersonaje;
using EspacioNombreAPI;

namespace EspacioFabricaDePersonajes
{
    public class FabricaDePersonajes
    {
        private static readonly Random rand = new();
        private static readonly Dictionary<string, List<string>> epitetos = new(); // Inicialización para evitar nulos

        static FabricaDePersonajes()
        {
            string json = File.ReadAllText("epitetos.json"); // Carga el archivo JSON
            epitetos = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json) ?? new Dictionary<string, List<string>>(); // Deserializa el JSON a un diccionario o crea uno vacío si es null
        }

        // Método asincrónico para crear un personaje aleatorio
        public async Task<Personaje> CrearPersonajeAsync()
        {
            NombreAPI nombreAPI = new(); // Instancia de la clase para obtener nombres
            string nombreCompleto = await nombreAPI.ObtenerNombreAleatorioAsync(); // Obtiene un nombre aleatorio

            string[] clases = { "Mago", "Guerrero", "Picaro", "Druida" };
            string clase = clases[rand.Next(clases.Length)]; // Selecciona una clase aleatoriamente

            // Selecciona un epíteto aleatorio basado en la clase
            string epiteto = epitetos[clase][rand.Next(epitetos[clase].Count)];

            int edad = rand.Next(18, 301); // Mayores de edad

            // Crea una nueva instancia de Personaje con los valores generados
            Personaje personaje = new(nombreCompleto, epiteto, clase, edad);
            return personaje;
        }
    }
}


