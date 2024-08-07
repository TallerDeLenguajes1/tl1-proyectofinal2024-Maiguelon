using System;
using EspacioPersonaje;
using EspacioFabricaDePersonajes;

namespace ProyectoRPG
{
    class Program
    {
        static void Main(string[] args)
        {
            FabricaDePersonajes fabrica = new();
            
            for (int i = 0; i < 5; i++)
            {
                Personaje personaje = fabrica.CrearPersonaje();
                Console.WriteLine(personaje);
                Console.WriteLine();
            }
        }
    }
}
