using System;
using EspacioPersonaje;
using EspacioHechizo;

namespace EspacioCombate
{
    public static class Combate
    {
        private static readonly Random rand = new();

        // Método para iniciar el combate
        public static Personaje IniciarCombate(Personaje p1, Personaje p2)
        {
            Console.WriteLine($"¡Comienza el combate entre {p1.Nombre} y {p2.Nombre}!");

            // Mostrar las características iniciales
            MostrarEstado(p1);
            MostrarEstado(p2);

            // Alternar turnos hasta que uno de los personajes sea derrotado
            bool turnoP1 = true;
            while (p1.Caracteristicas.Salud > 0 && p2.Caracteristicas.Salud > 0)
            {
                if (turnoP1)
                {
                    RealizarTurno(p1, p2);
                }
                else
                {
                    RealizarTurno(p2, p1);
                }

                // Mostrar el estado después de cada turno
                MostrarEstado(p1);
                MostrarEstado(p2);

                turnoP1 = !turnoP1;
            }

            // Determinar el ganador y retornar
            if (p1.Caracteristicas.Salud > 0)
            {
                Console.WriteLine($"{p1.Nombre} ha ganado el combate!");
                return p1;
            }
            else
            {
                Console.WriteLine($"{p2.Nombre} ha ganado el combate!");
                return p2;
            }
        }

        // Método para realizar un turno de ataque
        private static void RealizarTurno(Personaje atacante, Personaje defensor)
        {
            // Determinar si se usa un hechizo (primer turno del mago o druida)
            if (atacante.Clase == "Mago" || atacante.Clase == "Druida")
            {
                if (atacante.Nivel == 1)
                {
                    UsarHechizo(atacante, defensor);
                    return;
                }
            }

            // Calcular daño normal
            int efectividad = rand.Next(1, 101);
            int ataque = atacante.Caracteristicas.Destreza * atacante.Caracteristicas.Fuerza * atacante.Nivel;
            int defensa = defensor.Caracteristicas.Armadura * defensor.Caracteristicas.Velocidad;
            int dano = ((ataque * efectividad) / 100) - defensa;

            if (dano > 0)
            {
                defensor.Caracteristicas.Salud -= dano;
                Console.WriteLine($"{atacante.Nombre} ataca a {defensor.Nombre} y causa {dano} puntos de daño.");
            }
            else
            {
                Console.WriteLine($"{atacante.Nombre} ataca a {defensor.Nombre}, pero la defensa de {defensor.Nombre} lo bloquea.");
            }
        }

        // Método para usar un hechizo
        private static void UsarHechizo(Personaje lanzador, Personaje objetivo)
        {
            int nivelMagia = lanzador.Caracteristicas.Magia - 1; // índice del hechizo en la lista
            if (nivelMagia < 0 || nivelMagia >= ListaHechizos.Hechizos.Length) return;

            Hechizo hechizo = ListaHechizos.Hechizos[nivelMagia];
            objetivo.Caracteristicas.Salud -= hechizo.Danio;
            Console.WriteLine($"{lanzador.Nombre} usa {hechizo.Nombre} y causa {hechizo.Danio} puntos de daño a {objetivo.Nombre}.");
        }

        // Método para mostrar el estado actual de un personaje
        private static void MostrarEstado(Personaje p)
        {
            Console.WriteLine($"{p.Nombre} - Salud: {p.Caracteristicas.Salud}");
        }
    }
}
