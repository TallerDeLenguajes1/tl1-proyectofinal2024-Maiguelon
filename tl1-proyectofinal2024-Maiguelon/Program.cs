using System;
using System.Threading.Tasks;
using EspacioPersonaje;
using EspacioCombates;
using EspacioFabricaDePersonajes;
using EspacioGestionPartida;
using EspacioHistorialJson;

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
                        manejadorDePartidas.GuardarPartida(estado.Participantes, "cuartos", "partida.json", 1, 0);
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
                Console.WriteLine($"Participante {i + 1}: {personaje.Nombre} {personaje.Epiteto} , {personaje.Edad} años, {personaje.Clase}");
            }

            return new EstadoPartida { Participantes = participantes, RondaActual = 1 };
        }

        static async Task JugarTorneo(EstadoPartida estado, GestionPartida manejadorDePartidas)
{
    HistorialJson historialManejador = new HistorialJson();

    while (estado.Participantes.Count > 1)
    {
        // Verificar si ya se completaron todos los combates en la fase actual
        if (estado.IndiceCombateActual >= estado.Participantes.Count / 2)
        {
            Console.WriteLine("Todos los combates de esta fase ya se han completado.");
            estado.RondaActual++;
            estado.IndiceCombateActual = 0;

            // Verifica si la fase del torneo debe cambiar
            if (estado.Participantes.Count == 4)
            {
                estado.FaseTorneo = "semifinales";
            }
            else if (estado.Participantes.Count == 2)
            {
                estado.FaseTorneo = "final";
            }
            else if (estado.Participantes.Count == 1)
            {
                Console.WriteLine($"El ganador del torneo es {estado.Participantes[0].Nombre}, {estado.Participantes[0].Epiteto}.");
                historialManejador.GuardarGanador(estado.Participantes[0], "ganadores.json");
                Console.WriteLine("Partida guardada.");
                return; // Salir del método ya que el torneo ha terminado
            }
        }

        Console.WriteLine($"Fase actual: {estado.FaseTorneo}, Combate {estado.IndiceCombateActual + 1}");
        estado = await JugarRonda(estado, manejadorDePartidas);
    }

    if (estado.Participantes.Count == 1)
    {
        Console.WriteLine($"El ganador del torneo es {estado.Participantes[0].Nombre}, {estado.Participantes[0].Epiteto}.");
        historialManejador.GuardarGanador(estado.Participantes[0], "ganadores.json");
        Console.WriteLine("Partida guardada.");
    }
}


        static void MostrarHistorialDeGanadores()
        {
            HistorialJson historialManejador = new HistorialJson();

            if (historialManejador.Existe("ganadores.json"))
            {
                var ganadores = historialManejador.LeerGanadores("ganadores.json");
                Console.WriteLine("Historial de Ganadores:");
                foreach (var ganador in ganadores)
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

            // Verifica si la fase del torneo debe cambiar
            if (estado.Participantes.Count == 4)
            {
                estado.FaseTorneo = "semifinales";
            }
            else if (estado.Participantes.Count == 2)
            {
                estado.FaseTorneo = "final";
            }

            // Procesar solo los combates necesarios
            for (int i = estado.IndiceCombateActual; i < estado.Participantes.Count; i += 2)
            {
                Console.WriteLine($"{estado.FaseTorneo} - Combate {i / 2 + 1}: {estado.Participantes[i].Epiteto} vs {estado.Participantes[i + 1].Epiteto}");

                // Ejecuta el combate de manera asíncrona
                await Task.Run(() => Combates.RealizarCombate(estado.Participantes[i], estado.Participantes[i + 1]));

                // Determinar el ganador del combate y agregarlo a la lista de ganadores
                if (estado.Participantes[i].Caracteristicas.Salud > 0)
                {
                    ganadores.Add(estado.Participantes[i]);
                }
                else
                {
                    ganadores.Add(estado.Participantes[i + 1]);
                }

                estado.IndiceCombateActual++;

                // Preguntar si desea guardar después de cada combate
                if (estado.FaseTorneo != "final" && PreguntarSiGuardar())
                {
                    // Guardar el estado actual incluyendo tanto los ganadores como los que no han combatido aún
                    List<Personaje> participantesRestantes = new List<Personaje>(ganadores);

                    // Añadir los personajes que aún no han combatido
                    for (int j = i + 2; j < estado.Participantes.Count; j++)
                    {
                        participantesRestantes.Add(estado.Participantes[j]);
                    }

                    manejadorDePartidas.GuardarPartida(participantesRestantes, estado.FaseTorneo, "partida.json", estado.RondaActual, estado.IndiceCombateActual);
                    Console.WriteLine("Partida guardada.");
                    Environment.Exit(0); // Salir del programa
                }
            }

            // Actualizar la lista de participantes con los ganadores
            estado.Participantes = ganadores;
            estado.RondaActual++;
            estado.IndiceCombateActual = 0; // Reiniciar para la próxima ronda

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
}







