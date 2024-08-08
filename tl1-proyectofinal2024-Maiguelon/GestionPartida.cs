using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using EspacioPersonaje;

namespace EspacioGestionPartida
{
    // Clase para gestionar el guardado y la carga de la partida
    public class GestionPartida
    {
        // Método para guardar el estado de la partida en un archivo JSON
        public void GuardarPartida(List<Personaje> personajes, string faseTorneo, string nombreArchivo)
        {
            var estado = new EstadoPartida
            {
                Participantes = personajes,
                FaseTorneo = faseTorneo
            };

            string json = JsonSerializer.Serialize(estado, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(nombreArchivo, json);
        }

        // Método para cargar la partida
        public EstadoPartida CargarPartida(string nombreArchivo)
        {
            if (!File.Exists(nombreArchivo))
                throw new FileNotFoundException("El archivo de partida guardada no existe.");

            string json = File.ReadAllText(nombreArchivo);
            return JsonSerializer.Deserialize<EstadoPartida>(json) ?? new EstadoPartida();
        }

        public bool Existe(string nombreArchivo)
        {
            return File.Exists(nombreArchivo) && new FileInfo(nombreArchivo).Length > 0;
        }
    }

    // Clase del estado del juego guardado
    public class EstadoPartida
    {
        public List<Personaje> Participantes { get; set; } = new List<Personaje>();
        public string FaseTorneo { get; set; } = "cuartos"; // Estado inicial, puede ser "cuartos", "semis", "final"
    }
}
