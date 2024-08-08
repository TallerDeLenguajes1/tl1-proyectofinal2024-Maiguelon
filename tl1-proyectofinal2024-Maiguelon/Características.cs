using System;

namespace EspacioCaracteristica 
{
    public class Caracteristica 
    {
        private static readonly Random rand = new();

        public int Velocidad { get; set; }
        public int Destreza { get; set; }
        public int Fuerza { get; set; }
        public int Armadura { get; set; }
        public int Salud { get; set; }
        public int Magia { get; set; }

        public Caracteristica()
        {
            Salud = 100;
        }

        public void GenerarCaracteristicas(string clase)
        {
            switch (clase)
            {
                case "Mago":
                    Velocidad = rand.Next(1, 4);
                    Destreza = rand.Next(2, 4);
                    Fuerza = 2;
                    Armadura = rand.Next(3, 5);
                    Magia = rand.Next(5, 8);
                    break;

                case "Guerrero":
                    Velocidad = rand.Next(3, 5);
                    Destreza = rand.Next(2, 4);
                    Fuerza = rand.Next(4, 5);
                    Armadura = rand.Next(4, 6);
                    Magia = 0;
                    break;

                case "Picaro":
                    Velocidad = rand.Next(4, 6);
                    Destreza = rand.Next(3, 6);
                    Fuerza = rand.Next(2, 3);
                    Armadura = rand.Next(2, 4);
                    Magia = 0;
                    break;

                case "Druida":
                    Velocidad = rand.Next(3, 6);
                    Destreza = rand.Next(2, 4);
                    Fuerza = rand.Next(2, 5);
                    Armadura = rand.Next(2, 4);
                    Magia = rand.Next(2, 6);
                    break;
            }
        }
    }
}
