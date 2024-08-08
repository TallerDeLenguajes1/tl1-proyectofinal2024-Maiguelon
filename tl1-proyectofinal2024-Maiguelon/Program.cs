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


    // Verificar si hay una partida guardada
    if (manejadorDePartidas.Existe("partida.json"))
    {
        Console.WriteLine("¿Desea continuar la partida guardada? (s/n)");
        string respuesta = (Console.ReadLine() ?? "").ToLower(); // Asegurar que la entrada no sea nula
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
        manejadorDePartidas.GuardarPartida(estado.Participantes, "cuartos", "partida.json");
        Console.WriteLine("Partida guardada.");
    }

    // Continuar el torneo
    while (estado.Participantes.Count > 1)
    {
        Console.WriteLine("Siguiente ronda del torneo...");
        estado = await JugarRonda(estado);
        if (PreguntarSiContinuar())
        {
            manejadorDePartidas.GuardarPartida(estado.Participantes, estado.FaseTorneo, "partida.json");
            Console.WriteLine("Partida guardada.");
            return;
        }
    }

    // Anunciar el ganador
    if (estado.Participantes.Count == 1)
    {
        Console.WriteLine($"El ganador del torneo es {estado.Participantes[0].Nombre}, {estado.Participantes[0].Epiteto}.");
        manejadorDePartidas.GuardarPartida(estado.Participantes, "ganadores", "ganadores.json");
    }
}

// Método para iniciar un nuevo torneo
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

// Método para jugar una ronda del torneo
public static async Task<EstadoPartida> JugarRonda(EstadoPartida estado)
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
    }

    estado.Participantes = ganadores;
    estado.RondaActual++;
    return estado;
}

// Método para preguntar si se desea continuar el torneo
static bool PreguntarSiContinuar()
{
    Console.WriteLine("¿Desea continuar el torneo o guardar y salir? (continuar/guardar)");
    string respuesta = (Console.ReadLine() ?? "").ToLower(); // Asegurar que la entrada no sea nula
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


