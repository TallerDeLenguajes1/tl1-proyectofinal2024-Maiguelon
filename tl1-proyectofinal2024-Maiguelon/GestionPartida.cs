using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using EspacioPersonaje;
using ProyectoRPG;

namespace EspacioGestionPartida
{
    public class GestionPartida
    {
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
}

