using System;
using System.Threading.Tasks;
using EspacioPersonaje;
using EspacioFabricaDePersonajes;

namespace ProyectoRPG
{
    class Program
    {
        static async Task Main(string[] args)
        {
            FabricaDePersonajes fabrica = new();

            for (int i = 0; i < 5; i++)
            {
                Personaje personaje = await fabrica.CrearPersonajeAsync();
                Console.WriteLine(personaje);
                Console.WriteLine();
            }
        }
    }
}
