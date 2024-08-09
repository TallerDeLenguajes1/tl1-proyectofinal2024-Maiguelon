using System;
using System.Threading.Tasks;
using EspacioPersonaje;
using EspacioCombates;
using EspacioFabricaDePersonajes;
using EspacioGestionPartida;

namespace ProyectoRPG
{
    class Program
    {
        static async Task Main(string[] args)
        {
            GestionPartida manejadorDePartidas = new GestionPartida();
            EstadoPartida estado = new EstadoPartida();

            bool continuar = true;

            while (continuar)
            {
                Console.Clear();
                Console.WriteLine("Bienvenido al Torneo de Aventureros!");
                Console.WriteLine("1. Nueva Partida");
                Console.WriteLine("2. Cargar Partida");
                Console.WriteLine("3. Mostrar Historial de Ganadores");
                Console.WriteLine("4. Salir");
                Console.Write("Seleccione una opción: ");

                string opcion = Console.ReadLine() ?? "";  // Inicializa la variable opción para asegurarte de que no sea nula

                if (string.IsNullOrWhiteSpace(opcion))
                {
                    Console.WriteLine("Opción no válida. Por favor, ingrese una opción.");
                    continue;
                }

                switch (opcion)
                {
                    case "1":
                        estado = await IniciarNuevoTorneo();
                        manejadorDePartidas.GuardarPartida(estado.Participantes, "cuartos", "partida.json");
                        await JugarTorneo(estado, manejadorDePartidas);
                        break;

                    case "2":
                        if (manejadorDePartidas.Existe("partida.json"))
                        {
                            estado = manejadorDePartidas.CargarPartida("partida.json");
                            Console.WriteLine("Partida cargada exitosamente.");
                            await JugarTorneo(estado, manejadorDePartidas);
                        }
                        else
                        {
                            Console.WriteLine("No se encontró una partida guardada.");
                        }
                        break;

                    case "3":
                        MostrarHistorialDeGanadores();
                        break;

                    case "4":
                        continuar = false;
                        break;

                    default:
                        Console.WriteLine("Opción no válida. Por favor, seleccione una opción del menú.");
                        break;
                }

                if (continuar)
                {
                    Console.WriteLine("Presione cualquier tecla para volver al menú principal...");
                    Console.ReadKey();
                }
            }

            Console.WriteLine("Gracias por jugar. ¡Hasta la próxima!");
        }

        static async Task<EstadoPartida> IniciarNuevoTorneo()
        {
            List<Personaje> participantes = new List<Personaje>();
            FabricaDePersonajes fabrica = new FabricaDePersonajes();

            for (int i = 0; i < 8; i++)
            {
                var personaje = await fabrica.CrearPersonajeAsync();
                participantes.Add(personaje);
                Console.WriteLine($"Participante {i + 1}: {personaje.Epiteto} {personaje.Nombre}, {personaje.Edad} años");
            }

            return new EstadoPartida { Participantes = participantes, RondaActual = 1 };
        }

        static async Task JugarTorneo(EstadoPartida estado, GestionPartida manejadorDePartidas)
        {
            while (estado.Participantes.Count > 1)
            {
                Console.WriteLine("Siguiente ronda del torneo...");
                estado = await JugarRonda(estado, manejadorDePartidas);
            }

            if (estado.Participantes.Count == 1)
            {
                Console.WriteLine($"El ganador del torneo es {estado.Participantes[0].Nombre}, {estado.Participantes[0].Epiteto}.");
                manejadorDePartidas.GuardarPartida(estado.Participantes, "ganadores", "ganadores.json");
                Console.WriteLine("Partida guardada.");
                
                Console.WriteLine("¿Desea jugar de nuevo? (s/n)");
                string respuesta = (Console.ReadLine() ?? "").ToLower();
                if (respuesta != "s")
                {
                    Console.WriteLine("Gracias por jugar. ¡Hasta la próxima!");
                    Environment.Exit(0);  // Salir del programa
                }
            }
        }

        static void MostrarHistorialDeGanadores()
        {
            GestionPartida manejadorDePartidas = new GestionPartida();
            if (manejadorDePartidas.Existe("ganadores.json"))
            {
                var historial = manejadorDePartidas.CargarPartida("ganadores.json").Participantes;
                Console.WriteLine("Historial de Ganadores:");
                foreach (var ganador in historial)
                {
                    Console.WriteLine($"{ganador.Epiteto} {ganador.Nombre}");
                }
            }
            else
            {
                Console.WriteLine("No se ha encontrado ningún ganador registrado.");
            }
        }

        static async Task<EstadoPartida> JugarRonda(EstadoPartida estado, GestionPartida manejadorDePartidas)
        {
            List<Personaje> ganadores = new List<Personaje>();

            for (int i = 0; i < estado.Participantes.Count; i += 2)
            {
                Console.WriteLine($"Combate entre {estado.Participantes[i].Epiteto} y {estado.Participantes[i + 1].Epiteto}");
                
                // Ejecuta el combate de manera asíncrona
                await Task.Run(() => Combates.RealizarCombate(estado.Participantes[i], estado.Participantes[i + 1]));

                if (estado.Participantes[i].Caracteristicas.Salud > 0)
                {
                    ganadores.Add(estado.Participantes[i]);
                }
                else
                {
                    ganadores.Add(estado.Participantes[i + 1]);
                }

                // Preguntar si desea guardar después de cada combate
                if (PreguntarSiGuardar())
                {
                    manejadorDePartidas.GuardarPartida(estado.Participantes, estado.FaseTorneo, "partida.json");
                    Console.WriteLine("Partida guardada.");
                    Environment.Exit(0); // Salir del programa
                }
            }

            estado.Participantes = ganadores;
            estado.RondaActual++;
            return estado;
        }

        static bool PreguntarSiGuardar()
        {
            while (true)
            {
                Console.WriteLine("¿Desea guardar la partida y salir? (s/n)");
                string respuesta = (Console.ReadLine() ?? "").ToLower(); // Asegurar que la entrada no sea nula
                if (respuesta == "s")
                {
                    return true; // Guardar y salir
                }
                else if (respuesta == "n")
                {
                    return false; // Continuar sin guardar
                }
                else
                {
                    Console.WriteLine("Opción no válida. Por favor, ingrese 's' para guardar o 'n' para continuar.");
                }
            }
        }
    }

    public class EstadoPartida
    {
        public List<Personaje> Participantes { get; set; } = new List<Personaje>();
        public int RondaActual { get; set; }
        public string FaseTorneo { get; set; } = "cuartos";
    }
}






