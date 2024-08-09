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
                ImprimirDecoracion();
                CentrarTexto("=====================================", true);
                CentrarTexto("TORNEO DE CANDIDATOS", true);
                CentrarTexto("=====================================", true);
                Console.WriteLine();
                CentrarTexto("1. Nueva Partida", true);
                CentrarTexto("2. Cargar Partida", true);
                CentrarTexto("3. Mostrar Historial de Ganadores", true);
                CentrarTexto("4. Salir", true);
                ImprimirDecoracion();
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

        static void ImprimirDecoracion()
        {
            Console.WriteLine(new string('*', Console.WindowWidth));
        }

        static void CentrarTexto(string texto, bool saltoLinea = false)
        {
            int espacios = (Console.WindowWidth - texto.Length) / 2;
            Console.Write(new string(' ', espacios) + texto);
            if (saltoLinea)
                Console.WriteLine();
        }

        static async Task<EstadoPartida> IniciarNuevoTorneo()
        {
            List<Personaje> participantes = new List<Personaje>();
            FabricaDePersonajes fabrica = new FabricaDePersonajes();

            Console.Clear();
            ImprimirDecoracion();
            CentrarTexto("Los siguientes candidatos lucharán por el trono:", true);
            ImprimirDecoracion();

            for (int i = 0; i < 8; i++)
            {
                var personaje = await fabrica.CrearPersonajeAsync();
                participantes.Add(personaje);
                CentrarTexto($"Participante {i + 1}: {personaje.Nombre} {personaje.Epiteto} , {personaje.Edad} años, {personaje.Clase}\n");
            }

            Console.WriteLine();
            ImprimirDecoracion();

            return new EstadoPartida { Participantes = participantes, RondaActual = 1 };
        }

        static async Task JugarTorneo(EstadoPartida estado, GestionPartida manejadorDePartidas)
        {
            HistorialJson historialManejador = new HistorialJson();

            while (estado.Participantes.Count > 1)
            {
                if (estado.Participantes.Count == 4 && estado.FaseTorneo != "semifinales")
                {
                    estado.FaseTorneo = "semifinales";
                    estado.IndiceCombateActual = 0; // Reiniciar índice al zar una nueva fase
                }
                else if (estado.Participantes.Count == 2 && estado.FaseTorneo != "final")
                {
                    estado.FaseTorneo = "final";
                }

                if (estado.IndiceCombateActual < estado.Participantes.Count / 2)
                {
                    estado = await JugarRonda(estado, manejadorDePartidas);
                }
                else
                {
                    // Si todos los combates en la fase actual ya se completaron, pasa a la siguiente fase
                    if (estado.Participantes.Count == 4)
                    {
                        estado.FaseTorneo = "semifinales";
                        estado.RondaActual = 2;
                        estado.IndiceCombateActual = 0;
                    }
                    else if (estado.Participantes.Count == 2)
                    {
                        estado.FaseTorneo = "final";
                        estado.RondaActual = 3;
                        estado.IndiceCombateActual = 0;
                    }
                    else
                    {
                        // Terminó el torneo
                        break;
                    }
                }
            }

            if (estado.Participantes.Count == 1)
            {
                ImprimirDecoracion();

                string mensajeGanador = "¡EL GANADOR DEL TRONO ES!";
                CentrarTexto(mensajeGanador, true);
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow; // Color dorado

                string textoGanador = $"{estado.Participantes[0].Nombre} {estado.Participantes[0].Epiteto}";
                CentrarTexto(textoGanador, true);

                Console.ResetColor();
                historialManejador.GuardarGanador(estado.Participantes[0], "ganadores.json");
                Console.WriteLine();
                CentrarTexto("¡Gracias por jugar!", true);
                ImprimirDecoracion();

                Console.WriteLine();
                Environment.Exit(0); // Salir del programa después de la final
            }

        }

        static void MostrarHistorialDeGanadores()
{
    HistorialJson historialManejador = new HistorialJson();

    if (historialManejador.Existe("ganadores.json")) // Si hay ganadores
    {
        var ganadores = historialManejador.LeerGanadores("ganadores.json");
        CentrarTexto("Historial de Ganadores:\n", true);
        foreach (var ganador in ganadores)
        {
            Utilidades.CambiarColorSegunClase(ganador.Clase);
            CentrarTexto($"{ganador.Epiteto} {ganador.Nombre}\n");
            Console.ResetColor(); // Restablecer el color por defecto después de cada impresión
        }
    }
    else
    {
        CentrarTexto("No se ha encontrado ningún ganador registrado.");
    }
}

        static async Task<EstadoPartida> JugarRonda(EstadoPartida estado, GestionPartida manejadorDePartidas)
        {
            List<Personaje> ganadores = new List<Personaje>();

            for (int i = estado.IndiceCombateActual * 2; i < estado.Participantes.Count; i += 2)
            {
                CentrarTexto($"{estado.FaseTorneo} - Combate {estado.IndiceCombateActual + 1}: {estado.Participantes[i].Epiteto} vs {estado.Participantes[i + 1].Epiteto}");

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

                estado.IndiceCombateActual++;

                if (estado.FaseTorneo != "final" && PreguntarSiGuardar())
                {
                    List<Personaje> participantesRestantes = new List<Personaje>(ganadores);

                    for (int j = i + 2; j < estado.Participantes.Count; j++)
                    {
                        participantesRestantes.Add(estado.Participantes[j]);
                    }

                    manejadorDePartidas.GuardarPartida(participantesRestantes, estado.FaseTorneo, "partida.json", estado.RondaActual, estado.IndiceCombateActual);
                    Console.WriteLine("Partida guardada.");
                    Environment.Exit(0); // Salir del programa
                }
            }

            estado.Participantes = ganadores;
            estado.RondaActual++;
            estado.IndiceCombateActual = 0;

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
