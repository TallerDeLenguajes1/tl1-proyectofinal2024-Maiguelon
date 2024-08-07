using System;
using EspacioCaracteristica;

namespace EspacioPersonaje
{
    public class Personaje
    {
        // Propiedades básicas del personaje
        public string Nombre { get; set; }
        public string Epiteto { get; set; }
        public int Edad { get; set; }
        public string Clase { get; set; }
        public Caracteristica Caracteristicas { get; set; }
        public int Nivel { get; set; }

        // Constructor que inicializa las propiedades y genera las características
        public Personaje(string nombre, string epiteto, string clase, int edad)
        {
            Nombre = nombre;
            Epiteto = epiteto;
            Clase = clase;
            Edad = edad;
            Caracteristicas = new Caracteristica();
            Caracteristicas.GenerarCaracteristicas(clase);
            Nivel = 1;
        }

        // Método para subir de nivel
        public void SubirNivel()
        {
            Nivel++;
            Caracteristicas.Salud += 15;

            Random rnd = new();
            for (int i = 0; i < 2; i++)
            {
                int stat = rnd.Next(1, 6);
                switch (stat)
                {
                    case 1:
                        Caracteristicas.Velocidad++;
                        break;
                    case 2:
                        Caracteristicas.Destreza++;
                        break;
                    case 3:
                        Caracteristicas.Fuerza++;
                        break;
                    case 4:
                        Caracteristicas.Armadura++;
                        break;
                    case 5:
                        Caracteristicas.Magia++;
                        break;
                }
            }
        }

        // Método para representar el personaje como una cadena de texto
        public override string ToString()
        {
            return $"Nombre: {Nombre}\n" +
                   $"Epiteto: {Epiteto}\n" +
                   $"Clase: {Clase}\n" +
                   $"Edad: {Edad}\n" +
                   $"Nivel: {Nivel}\n" +
                   $"Velocidad: {Caracteristicas.Velocidad}\n" +
                   $"Destreza: {Caracteristicas.Destreza}\n" +
                   $"Fuerza: {Caracteristicas.Fuerza}\n" +
                   $"Armadura: {Caracteristicas.Armadura}\n" +
                   $"Salud: {Caracteristicas.Salud}\n" +
                   $"Magia: {Caracteristicas.Magia}";
        }
    }
}
