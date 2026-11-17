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
        LlenarStockInicial();
        VerStock();
    }

    static void LlenarStockInicial()
    {
        for (int p = 0; p < 15; p++)
        {
            for (int t = 0; t < 5; t++)
            {
                stock[p, t] = 50;
            }
        }
    }

    static int ObtenerStock(int p, int t)
    {
        return stock[p - 1, t - 1];
    }

    static void VerStock()
    {
        int[] totalTienda = new int[5];

        Console.WriteLine();
        Console.WriteLine("N".PadRight(4) + "Prenda".PadRight(26) +
                          "T1".PadLeft(6) + "T2".PadLeft(6) + "T3".PadLeft(6) +
                          "T4".PadLeft(6) + "T5".PadLeft(6) + "Total".PadLeft(8));
        Console.WriteLine(new string('-', 68));

        for (int p = 1; p <= 15; p++)
        {
            string linea = p.ToString().PadRight(4) + nombres[p - 1].PadRight(26);
            int totalPrenda = 0;

            for (int t = 1; t <= 5; t++)
            {
                int cantidad = ObtenerStock(p, t);
                linea += cantidad.ToString().PadLeft(6);
                totalPrenda += cantidad;
                totalTienda[t - 1] += cantidad;
            }

            linea += totalPrenda.ToString().PadLeft(8);
            Console.WriteLine(linea);
        }

        Console.WriteLine(new string('-', 68));

        string pie = "TOTAL".PadRight(30);
        int totalGeneral = 0;

        for (int t = 0; t < 5; t++)
        {
            pie += totalTienda[t].ToString().PadLeft(6);
            totalGeneral += totalTienda[t];
        }

        pie += totalGeneral.ToString().PadLeft(8);
        Console.WriteLine(pie);
    }
}
