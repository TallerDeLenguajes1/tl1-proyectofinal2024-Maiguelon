using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using EspacioPersonaje;

namespace EspacioHistorialJson
{
    public class HistorialJson
    {
        // Método para guardar un ganador en el archivo JSON
        public void GuardarGanador(Personaje ganador, double ganancias, string nombreArchivo)
        {
            List<Historial> historial;

            // Si hay historial, lo lee
            if (Existe(nombreArchivo))
            {
                historial = LeerGanadores(nombreArchivo);
            }
            else
            {
                historial = new List<Historial>();
            }

            // Agregar nuevo ganador al historial
            historial.Add(new Historial { Ganador = ganador, Ganancias = ganancias });

            // Serializar el historial actualizado a JSON y guardarlo
            string json = JsonSerializer.Serialize(historial);
            File.WriteAllText(nombreArchivo, json);
        }

        // Método para leer los ganadores desde JSON
        public List<Historial> LeerGanadores(string nombreArchivo)
        {
            // Control para evitar que devuelva NULL
            if (!File.Exists(nombreArchivo))
                return new List<Historial>();

            // Leer el contenido del archivo
            string json = File.ReadAllText(nombreArchivo);

            // Deserializar el contenido JSON a una lista de historial
            var historial = JsonSerializer.Deserialize<List<Historial>>(json);

            // Retorna lista vacía si fuera a devolver NULL
            return historial ?? new List<Historial>();
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
