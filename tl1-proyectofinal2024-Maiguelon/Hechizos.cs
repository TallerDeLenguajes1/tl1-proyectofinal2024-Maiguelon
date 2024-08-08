using System;

namespace EspacioHechizo
{
    public class Hechizo
    {
        public string Nombre { get; set; }
        public int Danio { get; set; }

        public Hechizo(string nombre, int danio)
        {
            Nombre = nombre;
            Danio = danio;
        }
    }

    public static class ListaHechizos
    {
        public static Hechizo[] Hechizos = new Hechizo[]
        {
            new Hechizo("Proyectil Arcano", 20),
            new Hechizo("Rayo de Escarcha", 25),
            new Hechizo("Haz Lunar", 30),
            new Hechizo("Invocación: Lobo Divino", 35),
            new Hechizo("Rocío Ácido", 40),
            new Hechizo("Bola de Fuego", 50),
            new Hechizo("Ventisca", 60),
            new Hechizo("Erupción Telúrica", 70),
            new Hechizo("Invocación: General Divino Mahoraga", 80),
            new Hechizo("Extensión de Dominio", 100)
        };
    }
}
