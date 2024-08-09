using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using EspacioPersonaje;

public class Apuestas
{
    private Random rand = new Random();

    // Método para obtener las cuotas de apuestas para cada personaje
    public Dictionary<string, double> ObtenerCuotas(List<Personaje> participantes)
    {
        Dictionary<string, double> cuotas = new Dictionary<string, double>();
        foreach (var personaje in participantes)
        {
            double cuota = Math.Round(rand.NextDouble() * (3.5 - 1.1) + 1.1, 2); // Cuotas entre 1.1 y 3.5
            cuotas.Add(personaje.Nombre, cuota);
        }
        return cuotas;
    }

    public double RealizarApuesta(double montoApostado, Personaje personaje, Dictionary<string, double> cuotas)
    {
        if (cuotas.TryGetValue(personaje.Nombre, out double cuota))
        {
            return CalcularGanancia(montoApostado, cuota);
        }
        else
        {
            throw new Exception("El personaje no tiene una cuota asociada.");
        }
    }


    // Método para calcular las ganancias basadas en la apuesta
    public double CalcularGanancia(double montoApostado, double cuota)
    {
        return montoApostado * cuota;
    }

    // Método para actualizar el historial de máximas ganancias
    public void ActualizarHistorialGanancias(double ganancia)
    {
        string path = "historialGanancias.json";
        List<double> historial = new List<double>();

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            historial = JsonSerializer.Deserialize<List<double>>(json) ?? new List<double>();
        }

        historial.Add(ganancia);
        historial.Sort((a, b) => b.CompareTo(a)); // Ordena de mayor a menor

        string nuevoJson = JsonSerializer.Serialize(historial, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, nuevoJson);
    }

    // Método para mostrar el historial de máximas ganancias
    public void MostrarHistorialGanancias()
    {
        string path = "historialGanancias.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            List<double> historial = JsonSerializer.Deserialize<List<double>>(json) ?? new List<double>(); // Si es null, inicializa como una lista vacía
            Console.WriteLine("Historial de máximas ganancias:");
            foreach (var ganancia in historial)
            {
                Console.WriteLine($"$ {ganancia}");
            }
        }
        else
        {
            Console.WriteLine("No hay historial de ganancias registrado.");
        }
    }

    private string montoTotalPath = "montoTotal.json";

    public double CargarMontoTotal()
    {
        if (File.Exists(montoTotalPath))
        {
            string json = File.ReadAllText(montoTotalPath);
            return JsonSerializer.Deserialize<double>(json);
        }
        return 1000; // Monto inicial
    }

    public void GuardarMontoTotal(double montoTotal)
    {
        string json = JsonSerializer.Serialize(montoTotal, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(montoTotalPath, json);
    }


}
