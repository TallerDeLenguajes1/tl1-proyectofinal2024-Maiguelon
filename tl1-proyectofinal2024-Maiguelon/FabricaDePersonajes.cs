using System;
using System.Threading.Tasks;
using EspacioPersonaje;
using EspacioNombreAPI;

namespace EspacioFabricaDePersonajes
{
    // Clase para crear personajes aleatoreos
    public class FabricaDePersonajes
    {
        private static readonly Random rand = new();

        // Método asincrónico para crear un personaje aleatoreo
        public async Task<Personaje> CrearPersonajeAsync()
        {
            NombreAPI nombreAPI = new(); // Instancia de la clase para obtener nombres
            string nombreCompleto = await nombreAPI.ObtenerNombreAleatorioAsync(); // Obtiene un nombre aleatorio

            // EPITETOS A ACTUALIZAR
            string[] epitetos = { "El Valiente", "El Sabio", "El Fuerte", "El Ágil", "El Noble" };
            string[] clases = { "Mago", "Guerrero", "Picaro", "Druida" };

            // Selecciona un epíteto y una clase aleatoriamente
            string epiteto = epitetos[rand.Next(epitetos.Length)];
            string clase = clases[rand.Next(clases.Length)];
            int edad = rand.Next(18, 301); // Mayores de edad of course

            // Crea una nueva instancia de Personaje con los valores generados
            Personaje personaje = new(nombreCompleto, epiteto, clase, edad);
            return personaje;
        }
    }
}
