using System;
using System.Threading.Tasks;  
using EspacioPersonaje;
using EspacioFabricaDePersonajes;
using EspacioCombates;

namespace ProyectoRPG
{
    class Program
    {
        static async Task Main(string[] args)
        {
            FabricaDePersonajes fabrica = new FabricaDePersonajes();

            // Crear personajes
            Personaje p1 = await fabrica.CrearPersonajeAsync();
            Personaje p2 = await fabrica.CrearPersonajeAsync();
            Personaje p3 = await fabrica.CrearPersonajeAsync();
            Personaje p4 = await fabrica.CrearPersonajeAsync();

            // Primera ronda de combates
            Combates.RealizarCombate(p1, p2);
            Combates.RealizarCombate(p3, p4);

            // Combate final entre ganadores
            Personaje ganador1 = p1.Caracteristicas.Salud > 0 ? p1 : p2;
            Personaje ganador2 = p3.Caracteristicas.Salud > 0 ? p3 : p4;

            Combates.RealizarCombate(ganador1, ganador2);

            // Mostrar el ganador final
            Personaje ganadorFinal = ganador1.Caracteristicas.Salud > 0 ? ganador1 : ganador2;
            Console.WriteLine($"El ganador final es {ganadorFinal.Nombre}");
        }
    }
}
