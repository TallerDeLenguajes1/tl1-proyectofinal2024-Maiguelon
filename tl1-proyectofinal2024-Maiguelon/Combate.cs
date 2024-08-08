using System;
using EspacioPersonaje;
using EspacioHechizo;

namespace EspacioCombate
{
    public static class Combate
    {

        // Método para iniciar un combate
        public static Personaje IniciarCombate(Personaje p1, Personaje p2)
        {
            Random rand = new();
            bool turnoP1 = true;
            bool hechizoUsadoP1 = false;
            bool hechizoUsadoP2 = false;

            while (p1.Caracteristicas.Salud > 0 && p2.Caracteristicas.Salud > 0)
            {
                if (turnoP1)
                {
                    if ((p1.Clase == "Mago" || p1.Clase == "Druida") && !hechizoUsadoP1)
                    {
                        UsarHechizo(p1, p2);
                        hechizoUsadoP1 = true;
                    }
                    else
                    {
                        Atacar(p1, p2);
                    }
                }
                else
                {
                    if ((p2.Clase == "Mago" || p2.Clase == "Druida") && !hechizoUsadoP2)
                    {
                        UsarHechizo(p2, p1);
                        hechizoUsadoP2 = true;
                    }
                    else
                    {
                        Atacar(p2, p1);
                    }
                }

                turnoP1 = !turnoP1; // Cambiar de turno

                // Mostrar estado de salud de los personajes
                Console.WriteLine($"{p1.Nombre}: Salud = {p1.Caracteristicas.Salud}");
                Console.WriteLine($"{p2.Nombre}: Salud = {p2.Caracteristicas.Salud}");
                Console.WriteLine();
            }

            Personaje ganador = p1.Caracteristicas.Salud > 0 ? p1 : p2;
            Console.WriteLine($"El ganador del combate es: {ganador.Nombre}");

            // El ganador sube de nivel
            ganador.SubirNivel();
            return ganador;
        }

        // Método para realizar un ataque
        private static void Atacar(Personaje atacante, Personaje defensor)
        {
            Random rand = new();
            int efectividad = rand.Next(1, 11);
            int ataque = (atacante.Caracteristicas.Destreza * atacante.Caracteristicas.Fuerza * atacante.Nivel);
            int defensa = (defensor.Caracteristicas.Armadura * defensor.Caracteristicas.Velocidad);
            int danio = ((ataque * efectividad) - defensa );

            // Capear a 50 y agregar esquivada
            defensor.Caracteristicas.Salud -= Math.Max(danio, 1); // Asegurar que el daño no sea negativo ni cero
            Console.WriteLine($"{atacante.Nombre} ataca a {defensor.Nombre} y causa {danio} puntos de daño.");
        }

        // Método para usar un hechizo
        private static void UsarHechizo(Personaje lanzador, Personaje objetivo)
        {
            int nivelMagia = lanzador.Caracteristicas.Magia;
            if (nivelMagia < 0 || nivelMagia >= ListaHechizos.Hechizos.Length)
            {
                nivelMagia = ListaHechizos.Hechizos.Length - 1;
            }

            Hechizo hechizo = ListaHechizos.Hechizos[nivelMagia];
            objetivo.Caracteristicas.Salud -= hechizo.Danio;
            Console.WriteLine($"{lanzador.Nombre} usa {hechizo.Nombre} y causa {hechizo.Danio} puntos de daño a {objetivo.Nombre}.");
        }
    }
}
