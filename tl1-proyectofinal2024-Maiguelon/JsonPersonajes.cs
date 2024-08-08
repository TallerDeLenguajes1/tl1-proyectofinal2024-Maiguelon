using System; 
using System.Collections.Generic; 
using System.IO; 
using System.Text.Json; 
using EspacioPersonaje; 

namespace EspacioPersonajesJson
{
    // Clase para manejar la persistencia en JSON
    public class PersonajesJson
    {
        // Método para guardar una lista de personajes en un archivo JSON
        public void GuardarPersonajes(List<Personaje> personajes, string nombreArchivo)
        {
            // Serializar la lista de personajes a una cadena JSON fácil de leer
            string json = JsonSerializer.Serialize(personajes, new JsonSerializerOptions { WriteIndented = true });
            // Escribir la cadena JSON en el archivo
            File.WriteAllText(nombreArchivo, json);
        }

        // Método para leer una lista de personajes desde un archivo JSON
        public List<Personaje> LeerPersonajes(string nombreArchivo)
        {
            // Control para evitar devoluciones posibles de NULL
            if (!File.Exists(nombreArchivo))
                return new List<Personaje>();

            // Leer el archivo
            string json = File.ReadAllText(nombreArchivo);

            // Deserializar el contenido JSON a una lista de personajes
            var personajes = JsonSerializer.Deserialize<List<Personaje>>(json);

            // Retornar la lista deserializada o una lista vacía si el resultado es nulo
            return personajes ?? new List<Personaje>();
        }

        // Método para verificar si un archivo existe y tiene contenido
        public bool Existe(string nombreArchivo)
        {
            // Verificar si el archivo existe y no está vacío
            return File.Exists(nombreArchivo) && new FileInfo(nombreArchivo).Length > 0;
        }
    }
}
