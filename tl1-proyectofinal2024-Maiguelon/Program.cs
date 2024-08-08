using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EspacioPersonaje;
using EspacioFabricaDePersonajes;
using EspacioCombates;
using EspacioHistorialJson;

namespace ProyectoRPG
{
    class Program
    {
        static async Task Main(string[] args)
        {
            FabricaDePersonajes fabrica = new FabricaDePersonajes();
            List<Personaje> participantes = new List<Personaje>();

            // Generar 8 personajes para el torneo
            for (int i = 0; i < 8; i++)
            {
                participantes.Add(await fabrica.CrearPersonajeAsync());
            }

            // Presentar los enfrentamientos de cuartos de final
            Console.WriteLine("Cuartos de Final:");
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine($"{participantes[i * 2].Epiteto} vs {participantes[i * 2 + 1].Epiteto}");
            }

            // Ejecutar cuartos de final
            List<Personaje> ganadoresCuartos = new List<Personaje>();
            for (int i = 0; i < 4; i++)
            {
                Combates.RealizarCombate(participantes[i * 2], participantes[i * 2 + 1]);
                Personaje ganador = participantes[i * 2].Caracteristicas.Salud > 0 ? participantes[i * 2] : participantes[i * 2 + 1];
                ganadoresCuartos.Add(ganador);

                // Preguntar si continuar o guardar y salir
                if (!PreguntarSiContinuar()) return;
            }

            // Presentar semifinales
            Console.WriteLine("Semifinales:");
            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine($"{ganadoresCuartos[i * 2].Epiteto} vs {ganadoresCuartos[i * 2 + 1].Epiteto}");
            }

            // Ejecutar semifinales
            List<Personaje> ganadoresSemis = new List<Personaje>();
            for (int i = 0; i < 2; i++)
            {
                Combates.RealizarCombate(ganadoresCuartos[i * 2], ganadoresCuartos[i * 2 + 1]);
                Personaje ganador = ganadoresCuartos[i * 2].Caracteristicas.Salud > 0 ? ganadoresCuartos[i * 2] : ganadoresCuartos[i * 2 + 1];
                ganadoresSemis.Add(ganador);

                // Preguntar si continuar o guardar y salir
                if (!PreguntarSiContinuar()) return;
            }

            // Presentar la final
            Console.WriteLine("Final:");
            Console.WriteLine($"{ganadoresSemis[0].Epiteto} vs {ganadoresSemis[1].Epiteto}");

            // Ejecutar la final
            Combates.RealizarCombate(ganadoresSemis[0], ganadoresSemis[1]);
            Personaje campeon = ganadoresSemis[0].Caracteristicas.Salud > 0 ? ganadoresSemis[0] : ganadoresSemis[1];

            // Anunciar y guardar el campeón
            Console.WriteLine($"¡{campeon.Nombre}, {campeon.Epiteto}, es el nuevo campeón!");
            GuardarCampeon(campeon);
        }

        static bool PreguntarSiContinuar()
        {
            Console.WriteLine("¿Desea continuar el torneo o guardar y salir? (continuar/guardar)");
            string? respuesta = Console.ReadLine();
            if (respuesta?.ToLower() == "guardar")
            {
                // Implementar la lógica de guardar aquí
                Console.WriteLine("Guardando partida...");
                // Guardar el estado actual del torneo
                return false;
            }
            return true;

        }

        static void GuardarCampeon(Personaje campeon)
        {
            HistorialJson historialJson = new HistorialJson();
            historialJson.GuardarGanador(campeon, 0, "ganadores.json");
        }
    }
}
