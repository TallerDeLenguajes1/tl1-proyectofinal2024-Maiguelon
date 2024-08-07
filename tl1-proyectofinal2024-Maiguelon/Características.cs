using System;

namespace EspacioCaracteristica
{
    public class Caracteristica 
    {
        // Creo una instancia pública para la aleatoriedad, por comodidad y para evitar que "se repita la semilla"
        private static readonly Random rand = new(); 

        public int Velocidad { get; set; }
        public int Destreza { get; set; }
        public int Fuerza { get; set; }
        public int Armadura { get; set; }
        public int Salud { get; set; }
        public int Magia { get; set; }

        // Como todos tienen 100 de salud, no es necesario repetir el constructor
        public Caracteristica()
        {
            Salud = 100;
        }

        // Creo un método que asigna propiedades según la clase
        public void GenerarCaracteristicas(string clase)
        {
            // Solo los magos y los druidas cuentan con magia
            switch (clase)
            {
                case "Mago":
                    Velocidad = rand.Next(1, 4);
                    Destreza = rand.Next(1, 3);
                    Fuerza = 2;
                    Armadura = rand.Next(2, 4);
                    Magia = rand.Next(5, 8);
                    break;

                case "Guerrero":
                    Velocidad = rand.Next(4, 7);
                    Destreza = rand.Next(1, 3);
                    Fuerza = rand.Next(5, 8);
                    Armadura = rand.Next(3, 6);
                    Magia = 0;
                    break;

                case "Picaro":
                    Velocidad = rand.Next(4, 7);
                    Destreza = rand.Next(2, 4);
                    Fuerza = rand.Next(2, 5);
                    Armadura = rand.Next(1, 3);
                    Magia = 0;
                    break;

                case "Druida":
                    Velocidad = rand.Next(4, 7);
                    Destreza = rand.Next(1, 3);
                    Fuerza = rand.Next(3, 6);
                    Armadura = rand.Next(1, 3);
                    Magia = rand.Next(2, 7);
                    break;
            }
        }
    }
}
