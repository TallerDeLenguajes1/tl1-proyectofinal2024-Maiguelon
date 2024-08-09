using System;
using System.Collections.Generic;
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
            Apuestas gestionApuestas = new Apuestas();
            double montoTotal = gestionApuestas.CargarMontoTotal();

            bool continuar = true;

            while (continuar)
            {
                Console.Clear();
                Console.WriteLine("Bet for Thrones");
                Console.WriteLine("1. Nueva Partida");
                Console.WriteLine("2. Cargar Partida");
                Console.WriteLine("3. Mostrar Historial de Ganancias");
                Console.WriteLine("4. Mostrar Historial de Ganadores");
                Console.WriteLine("5. Salir");
                Console.Write("Seleccione una opción: ");

                string opcion = Console.ReadLine() ?? "";

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
                        montoTotal = await JugarTorneo(estado, manejadorDePartidas, gestionApuestas, montoTotal);
                        break;

                    case "2":
                        if (manejadorDePartidas.Existe("partida.json"))
                        {
                            estado = manejadorDePartidas.CargarPartida("partida.json");
                            Console.WriteLine("Partida cargada exitosamente.");
                            montoTotal = await JugarTorneo(estado, manejadorDePartidas, gestionApuestas, montoTotal);
                        }
                        else
                        {
                            Console.WriteLine("No se encontró una partida guardada.");
                        }
                        break;


                    case "3":
                        gestionApuestas.MostrarHistorialGanancias();
                        break;

                    case "4":
                        MostrarHistorialDeGanadores();
                        break;

                    case "5":
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

        static async Task<double> JugarTorneo(EstadoPartida estado, GestionPartida manejadorDePartidas, Apuestas gestionApuestas, double montoTotal)
{
    HistorialJson historialManejador = new HistorialJson();
    bool salirPorDinero = false;

    while (estado.Participantes.Count > 1 && !salirPorDinero)
    {
        if (estado.Participantes.Count == 4 && estado.FaseTorneo != "semifinales")
        {
            estado.FaseTorneo = "semifinales";
            estado.IndiceCombateActual = 0;
        }
        else if (estado.Participantes.Count == 2 && estado.FaseTorneo != "final")
        {
            estado.FaseTorneo = "final";
            estado.IndiceCombateActual = 0;
        }

        Dictionary<string, double> cuotas = gestionApuestas.ObtenerCuotas(estado.Participantes);
        Console.WriteLine("Vas a la quiniela, las apuestas están:");

        foreach (var cuota in cuotas)
        {
            Console.WriteLine($"{cuota.Key}: {cuota.Value}");
        }

        Console.Write("¿Cuánto desea apostar? ");
        double montoApostado = double.Parse(Console.ReadLine() ?? "0");
        Console.Write("¿Por cuál personaje desea apostar? ");
        string nombrePersonaje = Console.ReadLine() ?? "";

        if (!string.IsNullOrEmpty(nombrePersonaje))
        {
            // Busca el personaje en la lista de participantes
            Personaje personajeApostado = estado.Participantes.FirstOrDefault(p => p.Nombre == nombrePersonaje);

            if (personajeApostado != null)
            {
                double ganancia = gestionApuestas.RealizarApuesta(montoApostado, personajeApostado, cuotas);
                montoTotal += ganancia;
            }
            else
            {
                Console.WriteLine("El personaje seleccionado no existe.");
            }
        }
        else
        {
            Console.WriteLine("Debe ingresar un nombre válido para realizar la apuesta.");
        }

        if (montoTotal <= 0)
        {
            Console.WriteLine("Perdiste todo tu dinero, el torneo no te interesa más.");
            salirPorDinero = true;
            break;
        }

        (estado, montoTotal) = await JugarRonda(estado, manejadorDePartidas, gestionApuestas, montoTotal);
    }

    if (estado.Participantes.Count == 1 && !salirPorDinero)
    {
        Console.WriteLine($"El ganador del torneo es {estado.Participantes[0].Nombre}, {estado.Participantes[0].Epiteto}.");
        historialManejador.GuardarGanador(estado.Participantes[0], "ganadores.json");
        gestionApuestas.ActualizarHistorialGanancias(montoTotal);
        Console.WriteLine($"Tu monto total acumulado es: ${montoTotal}");
        Console.WriteLine("Gracias por jugar. ¡Hasta la próxima!");
        Environment.Exit(0);
    }
    return montoTotal;
}


        static async Task<(EstadoPartida, double)> JugarRonda(EstadoPartida estado, GestionPartida manejadorDePartidas, Apuestas gestionApuestas, double montoTotal)
        {
            List<Personaje> ganadores = new List<Personaje>();

            for (int i = estado.IndiceCombateActual * 2; i < estado.Participantes.Count; i += 2)
            {
                Console.WriteLine($"{estado.FaseTorneo} - Combate {estado.IndiceCombateActual + 1}: {estado.Participantes[i].Epiteto} vs {estado.Participantes[i + 1].Epiteto}");

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
                    Environment.Exit(0);
                }
            }

            estado.Participantes = ganadores;
            estado.RondaActual++;
            estado.IndiceCombateActual = 0;

            return (estado, montoTotal);
        }

        static bool PreguntarSiGuardar()
        {
            while (true)
            {
                Console.WriteLine("¿Desea guardar la partida y salir? (s/n)");
                string respuesta = (Console.ReadLine() ?? "").ToLower();
                if (respuesta == "s")
                {
                    return true;
                }
                else if (respuesta == "n")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Opción no válida. Por favor, ingrese 's' para guardar o 'n' para continuar.");
                }
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
    }
}
