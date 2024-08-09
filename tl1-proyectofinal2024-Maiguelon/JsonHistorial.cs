using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using EspacioPersonaje;

namespace EspacioHistorialJson
{
    public class HistorialJson
    {
        // Método para guardar un nuevo ganador en el historial sin sobrescribir los anteriores
        public void GuardarGanador(Personaje ganador, string nombreArchivo)
        {
            List<Personaje> ganadores = new List<Personaje>();

            // Si el archivo ya existe, leer los ganadores actuales
            if (File.Exists(nombreArchivo))
            {
                string jsonExistente = File.ReadAllText(nombreArchivo);
                ganadores = JsonSerializer.Deserialize<List<Personaje>>(jsonExistente) ?? new List<Personaje>();
            }

            // Agregar el nuevo ganador al historial
            ganadores.Add(ganador);

            // Guardar el historial actualizado en el archivo JSON
            string json = JsonSerializer.Serialize(ganadores, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(nombreArchivo, json);
        }

        // Método para leer los ganadores desde el archivo JSON
        public List<Personaje> LeerGanadores(string nombreArchivo)
        {
            try
            {
                if (!File.Exists(nombreArchivo))
                    return new List<Personaje>();

                string json = File.ReadAllText(nombreArchivo);
                return JsonSerializer.Deserialize<List<Personaje>>(json) ?? new List<Personaje>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine("Error al leer el archivo de ganadores: " + ex.Message);
                return new List<Personaje>(); // Retorna una lista vacía en caso de error
            }
        }


        // Método para verificar si un archivo existe y no está vacío
        public bool Existe(string nombreArchivo)
        {
            return File.Exists(nombreArchivo) && new FileInfo(nombreArchivo).Length > 0;
        }
    }

    // Clase para representar el historial de un ganador
    public class Historial
    {
        public required Personaje Ganador { get; set; }
        public required double Ganancias { get; set; }
    }
}
