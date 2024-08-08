using System;
using System.Threading.Tasks;
using EspacioPersonaje;
using EspacioFabricaDePersonajes;
using EspacioCombate;

namespace ProyectoRPG
{
    class Program
    {
        static async Task Main(string[] args)
        {
            FabricaDePersonajes fabrica = new FabricaDePersonajes();

            // Crear cuatro personajes de forma asincrónica
            Personaje personaje1 = await fabrica.CrearPersonajeAsync();
            Personaje personaje2 = await fabrica.CrearPersonajeAsync();
            Personaje personaje3 = await fabrica.CrearPersonajeAsync();
            Personaje personaje4 = await fabrica.CrearPersonajeAsync();

            // Mostrar información de los personajes
            Console.WriteLine("Personaje 1:");
            Console.WriteLine(personaje1);
            Console.WriteLine();

            Console.WriteLine("Personaje 2:");
            Console.WriteLine(personaje2);
            Console.WriteLine();

            Console.WriteLine("Personaje 3:");
            Console.WriteLine(personaje3);
            Console.WriteLine();

            Console.WriteLine("Personaje 4:");
            Console.WriteLine(personaje4);
            Console.WriteLine();

            // Primera ronda de combates
            Console.WriteLine("Primer combate: Personaje 1 vs Personaje 2");
            Personaje ganador1 = Combate.IniciarCombate(personaje1, personaje2);
            Console.WriteLine($"El ganador del primer combate es: {ganador1.Nombre}");
            Console.WriteLine();

            Console.WriteLine("Segundo combate: Personaje 3 vs Personaje 4");
            Personaje ganador2 = Combate.IniciarCombate(personaje3, personaje4);
            Console.WriteLine($"El ganador del segundo combate es: {ganador2.Nombre}");
            Console.WriteLine();

            // Combate final
            Console.WriteLine("Combate final: Ganador del primer combate vs Ganador del segundo combate");
            Personaje ganadorFinal = Combate.IniciarCombate(ganador1, ganador2);
            Console.WriteLine($"El ganador final es: {ganadorFinal.Nombre}");
        }
    }
}
