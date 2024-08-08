using System;
using EspacioPersonaje;

namespace EspacioCombate
{
    public class Combate
    {
        private static readonly Random rand = new Random();

        public static Personaje RealizarCombate(Personaje p1, Personaje p2)
        {
            while (p1.Caracteristicas.Salud > 0 && p2.Caracteristicas.Salud > 0)
            {
                // p1 ataca a p2
                int danioProvocado = CalcularDanio(p1, p2);
                p2.Caracteristicas.Salud -= danioProvocado;
                if (p2.Caracteristicas.Salud <= 0) break;

                // p2 ataca a p1
                danioProvocado = CalcularDanio(p2, p1);
                p1.Caracteristicas.Salud -= danioProvocado;
            }

            if (p1.Caracteristicas.Salud > 0)
            {
                p1.SubirNivel();
                return p1;
            }
            else
            {
                p2.SubirNivel();
                return p2;
            }
        }

        private static int CalcularDanio(Personaje atacante, Personaje defensor)
        {
            double ataque = atacante.Caracteristicas.Destreza * atacante.Caracteristicas.Fuerza * atacante.Nivel;
            double efectividad = rand.Next(1, 101);
            double defensa = defensor.Caracteristicas.Armadura * defensor.Caracteristicas.Velocidad;
            double constanteAjuste = 500;

            int danioProvocado = (int)(((ataque * efectividad) - defensa) / constanteAjuste);
            return Math.Max(danioProvocado, 0); // Evitar daño negativo
        }
    }
}
