using System;

public static class Utilidades
{
    public static void CambiarColorSegunClase(string clase)
    {
        switch (clase)
        {
            case "Guerrero":
                Console.ForegroundColor = ConsoleColor.Red;
                break;
            case "Mago":
                Console.ForegroundColor = ConsoleColor.Blue;
                break;
            case "Picaro":
                Console.ForegroundColor = ConsoleColor.Green;
                break;
            case "Druida":
                Console.ForegroundColor = ConsoleColor.Cyan;
                break;
            default:
                Console.ForegroundColor = ConsoleColor.White;
                break;
        }
    }
}
