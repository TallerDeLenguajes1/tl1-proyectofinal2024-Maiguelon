using System;
using System.Threading.Tasks;
using EspacioHechizo;
using EspacioPersonaje;
using EspacioDatos;

namespace EspacioCombates
{
    public static class Combates
    {
        private static readonly Random rand = new();

        // Método para calcular el daño provocado en cada turno
        private static int CalcularDaño(Personaje atacante, Personaje defensor)
        {
            int ataque = (atacante.Caracteristicas.Destreza * atacante.Caracteristicas.Fuerza * atacante.Nivel);
            int efectividad = rand.Next(4, 7);
            int defensa = (defensor.Caracteristicas.Armadura * defensor.Caracteristicas.Velocidad) ;
            int dañoProvocado = (ataque * efectividad - defensa); 

            // Asegurar que el daño no sea negativo
            return Math.Max(dañoProvocado, 1);
        }

        // Método para realizar un turno de combate
        public static void TurnoCombate(Personaje atacante, Personaje defensor)
        {
            int daño = CalcularDaño(atacante, defensor);
            defensor.Caracteristicas.Salud -= daño;

            string fraseAtaque = daño > 10
                ? Datos.ObtenerFraseAtaque(atacante.Clase, "alto")
                : Datos.ObtenerFraseAtaque(atacante.Clase, "bajo");

            // Reemplazar "{nombre}" con el nombre del atacante
            fraseAtaque = fraseAtaque.Replace("{nombre}", atacante.Nombre);

            Console.WriteLine($"{atacante.Nombre} ataca a {defensor.Nombre}: {fraseAtaque}\n");
        }


        // Método para usar un hechizo
        private static void UsarHechizo(Personaje lanzador, Personaje objetivo)
        {
            int nivelMagia = lanzador.Caracteristicas.Magia;
            if (nivelMagia <= 0 || nivelMagia > ListaHechizos.Hechizos.Length)
            {
                Console.WriteLine($"{lanzador.Nombre} no tiene hechizos disponibles.");
                return;
            }

            Hechizo hechizo = ListaHechizos.Hechizos[nivelMagia - 1]; // Ajuste para índices de array
            objetivo.Caracteristicas.Salud -= hechizo.Danio;

            // Reemplazar {nombre} en la frase con el nombre del lanzador
            string fraseHechizo = Datos.ObtenerFraseHechizo(lanzador.Clase, hechizo.Nombre);
            fraseHechizo = fraseHechizo.Replace("{nombre}", lanzador.Nombre);

            Console.WriteLine($"{lanzador.Nombre} usa {hechizo.Nombre} y causa {hechizo.Danio} puntos de daño a {objetivo.Nombre}: {fraseHechizo}\n");
        }


        // Método para mostrar el estado actual de un personaje
        private static void MostrarEstado(Personaje personaje)
        {
            Console.WriteLine(personaje);
        }

        // Método principal para realizar un combate
        public static void RealizarCombate(Personaje p1, Personaje p2)
        {
            Console.WriteLine($"El combate entre {p1.Nombre} y {p2.Nombre} ha comenzado!");

            bool p1Turno = true; // Determinar el turno inicial

            // Usar hechizos en el primer turno
            if (p1.Caracteristicas.Magia > 0)
            {
                UsarHechizo(p1, p2);
                p1Turno = false; // Cambiar turno
            }
            else if (p2.Caracteristicas.Magia > 0)
            {
                UsarHechizo(p2, p1);
                p1Turno = true; // Cambiar turno
            }

            // Continuar el combate hasta que uno de los personajes sea vencido
            while (p1.Caracteristicas.Salud > 0 && p2.Caracteristicas.Salud > 0)
            {
                if (p1Turno)
                {
                    TurnoCombate(p1, p2);
                }
                else
                {
                    TurnoCombate(p2, p1);
                }

                // Cambiar turno
                p1Turno = !p1Turno;
            }

            // Determinar el ganador y mostrar el resultado
            if (p1.Caracteristicas.Salud > 0)
            {
                Console.WriteLine($"{p1.Nombre} ha ganado el combate!");
                p1.SubirNivel();
            }
            else
            {
                Console.WriteLine($"{p2.Nombre} ha ganado el combate!");
                p2.SubirNivel();
            }

            Console.WriteLine("Fin del combate.");
        }
    }
}

