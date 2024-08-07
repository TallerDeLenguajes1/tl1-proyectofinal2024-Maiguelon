using System;
using EspacioPersonaje;

namespace EspacioFabricaDePersonajes
{
    public class FabricaDePersonajes
    {
        private static readonly Random rand = new();

        // Método para generar un personaje aleatorio
        public Personaje CrearPersonaje()
        {
            string[] nombres = { "Aragon", "Legolas", "Gandalf", "Frodo", "Boromir" };
            string[] epitetos = { "El Valiente", "El Sabio", "El Fuerte", "El Ágil", "El Noble" };
            string[] clases = { "Mago", "Guerrero", "Picaro", "Druida" };

            string nombre = nombres[rand.Next(nombres.Length)];
            string epiteto = epitetos[rand.Next(epitetos.Length)];
            string clase = clases[rand.Next(clases.Length)];
            int edad = rand.Next(18, 101); // Edad entre 18 y 100

            // Crear una nueva instancia de Personaje
            Personaje personaje = new(nombre, epiteto, clase, edad);

            return personaje;
        }
    }
}
