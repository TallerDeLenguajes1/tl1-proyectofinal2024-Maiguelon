using System;
using System.Threading.Tasks;
using EspacioPersonaje;
using EspacioCombates;
using EspacioGestionPartida;

namespace ProyectoRPG
{
    class Program
    {
        static async Task Main(string[] args)
        {
            GestionPartida manejadorDePartidas = new GestionPartida();
            EstadoPartida estado = null;

            // Verificar si hay una partida guardada
            if (manejadorDePartidas.Existe("partida.json"))
            {
                Console.WriteLine("¿Desea continuar la partida guardada? (s/n)");
                string respuesta = Console.ReadLine()?.ToLower();
                if (respuesta == "s")
                {
                    estado = manejadorDePartidas.CargarPartida("partida.json");
                    Console.WriteLine("Partida cargada exitosamente.");
                }
            }

            // Si no hay partida guardada o el usuario no desea continuar, iniciar un nuevo torneo
            if (estado == null)
            {
                estado = await IniciarNuevoTorneo();
                manejadorDePartidas.GuardarPartida(estado.Participantes, estado.FaseTorneo, "partida.json");
                Console.WriteLine("Partida guardada.");
            }

            // Continuar el torneo
            while (estado.Participantes.Count > 1)
            {
                Console.WriteLine("Siguiente ronda del torneo...");
                estado = await JugarRonda(estado);
                if (!PreguntarSiContinuar())
                {
                    manejadorDePartidas.GuardarPartida(estado.Participantes, estado.FaseTorneo, "partida.json");
                    Console.WriteLine("Partida guardada.");
                    return;
                }
            }

            // Anunciar el ganador
            if (estado.Participantes.Count == 1)
            {
                Console.WriteLine($"El ganador del torneo es {estado.Participantes[0].Nombre} ({estado.Participantes[0].Epiteto})");
                manejadorDePartidas.GuardarPartida(estado.Participantes, "ganadores.json");
            }
        }

        static async Task<EstadoPartida> IniciarNuevoTorneo()
        {
            List<Personaje> participantes = new List<Personaje>();
            FabricaDePersonajes fabrica = new FabricaDePersonajes();

            for (int i = 0; i < 8; i++)
            {
                var personaje = await fabrica.CrearPersonajeAsync();
                participantes.Add(personaje);
                Console.WriteLine($"{personaje.Epiteto} {personaje.Nombre}, {personaje.Edad} años");
            }

            return new EstadoPartida { Participantes = participantes, FaseTorneo = "cuartos" };
        }

        static async Task<EstadoPartida> JugarRonda(EstadoPartida estado)
        {
            List<Personaje> ganadores = new List<Personaje>();

            for (int i = 0; i < estado.Participantes.Count; i += 2)
            {
                Console.WriteLine($"Combate entre {estado.Participantes[i].Epiteto} y {estado.Participantes[i + 1].Epiteto}");
                Combates.RealizarCombate(estado.Participantes[i], estado.Participantes[i + 1]);
                if (estado.Participantes[i].Caracteristicas.Salud > 0)
                {
                    ganadores.Add(estado.Participantes[i]);
                }
                else
                {
                    ganadores.Add(estado.Participantes[i + 1]);
                }
            }

            estado.Participantes = ganadores;
            estado.RondaActual++;
            return estado;
        }

        static bool PreguntarSiContinuar()
        {
            Console.WriteLine("¿Desea continuar el torneo o guardar y salir? (continuar/guardar)");
            string respuesta = Console.ReadLine()?.ToLower();
            return respuesta != "guardar";
        }
    }

    public class EstadoPartida
    {
        public List<Personaje> Participantes { get; set; } = new List<Personaje>();
        public int RondaActual { get; set; }
        public string FaseTorneo { get; set; } = "cuartos";
    }
}


