using System;

// MODULO 2: STOCK (opcion 2 del menu)
class Program
{
    static string[] nombres = {
        "Polo basico", "Camisa manga larga", "Jean clasico", "Short deportivo",
        "Gorra", "Vestido de fiesta largo", "Vestido casual", "Vestido de noche",
        "Falda plisada", "Blusa elegante", "Chompa de lana", "Casaca de jean",
        "Pantalon jogger", "Polera con capucha", "Bufanda"
    };

    static int[,] stock = new int[15, 5];

    static void Main()
    {
        Console.WriteLine("Modulo 2: Gestion de Stock iniciado.");
    }
}
