using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace EspacioDatos
{
    // Clase estática para manejar loa Json de frases y epítetos
    public static class Datos
    {
        // Diccionarios para almacenar epítetos, frases de ataque y frases de hechizos
        public static Dictionary<string, List<string>> Epitetos { get; private set; } = new Dictionary<string, List<string>>();
        public static Dictionary<string, Dictionary<string, List<string>>> FrasesAtaque { get; private set; } = new Dictionary<string, Dictionary<string, List<string>>>();
        public static Dictionary<string, List<string>> FrasesHechizos { get; private set; } = new Dictionary<string, List<string>>();

        // Constructor para cargar los datos, de forma que no se desreference un null
        static Datos()
        {
            CargarEpitetos();        
            CargarFrasesAtaque();    
            CargarFrasesHechizos();  
        }

        // Método privado para cargar epítetos desde un archivo JSON
        private static void CargarEpitetos()
        {
            string json = File.ReadAllText("epitetos.json");  // Lee el contenido del archivo JSON
            var epitetos = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);  // Deserializar el contenido JSON 
            if (epitetos != null)  // Verificar que el resultado de la deserialización no sea nulo
            {
                Epitetos = epitetos;  // Asignar el diccionario deserializado a la propiedad estática
            }
        }

        // Ataque
        private static void CargarFrasesAtaque()
        {
            string json = File.ReadAllText("frasesAtaque.json");  // Lee el Json
            var frasesAtaque = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, List<string>>>>(json);  // Deserializar el JSON
            if (frasesAtaque != null)  // Verificar que el resultado de la deserialización no sea nulo
            {
                FrasesAtaque = frasesAtaque;  // Asignar el diccionario deserializado a la propiedad estática
            }
        }

        // Hechizos
        private static void CargarFrasesHechizos()
        {
            string json = File.ReadAllText("frasesHechizos.json");  // Lee el JSON
            var frasesHechizos = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);  // Deserializar JSON
            if (frasesHechizos != null)  // Verificar que el resultado de la deserialización no sea nulo
            {
                FrasesHechizos = frasesHechizos;  // Asignar el diccionario deserializado a la propiedad estática
            }
        }

        // Epítetos
        public static string ObtenerEpítetoAleatorio(string clase)
        {
            var epitetos = Epitetos.ContainsKey(clase) ? Epitetos[clase] : new List<string> { "El Desconocido" };  // Obtener la lista de epítetos para la clase o un epíteto predeterminado
            return epitetos[new Random().Next(epitetos.Count)];  // Devolver un epíteto aleatorio de la lista
        }

        // Fradse de ataque personalizada
        public static string ObtenerFraseAtaque(string clase, string tipo)
        {
            var frases = FrasesAtaque.ContainsKey(clase) && FrasesAtaque[clase].ContainsKey(tipo)
                ? FrasesAtaque[clase][tipo]
                : new List<string> { "Hace un ataque." };  // Obtener la lista de frases de ataque para la clase y tipo, o una frase predeterminada
            return frases[new Random().Next(frases.Count)];  // Devolver una frase aleatoria de la lista
        }

        // Frase de Hechizo personalizada
        public static string ObtenerFraseHechizo(string clase, string hechizo)
        {
            var frases = FrasesHechizos.ContainsKey(clase) ? FrasesHechizos[clase] : new List<string> { "Usa un hechizo." };  // Obtener la lista de frases de hechizo para la clase o una frase predeterminada
            return frases[new Random().Next(frases.Count)].Replace("{hechizo}", hechizo);  // Devolver una frase aleatoria de la lista, reemplazando el marcador de posición por el nombre del hechizo
        }
    }
}
