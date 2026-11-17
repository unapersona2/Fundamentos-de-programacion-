using System;
using System.IO;

// MODULO 3: TRASLADO ENTRE TIENDAS (Adaptado con nombre de prenda, stock restante y registro)
class Program
{
    static string[] nombres = {
        "Polo basico", "Camisa manga larga", "Jean clasico", "Short deportivo",
        "Gorra", "Vestido de fiesta largo", "Vestido casual", "Vestido de noche",
        "Falda plisada", "Blusa elegante", "Chompa de lana", "Casaca de jean",
        "Pantalon jogger", "Polera con capucha", "Bufanda"
    };

    static int[,] stock = new int[15, 5];
    static string archivoTraslados = "traslados.txt";

    static void Main()
    {
        LlenarStockInicial();
        Traslado();
        VerStock();
    }

    static int LeerEntero()
    {
        int n;
        string texto = Console.ReadLine() ?? "";

        while (!int.TryParse(texto, out n))
        {
            Console.Write("Ingrese un numero valido: ");
            texto = Console.ReadLine() ?? "";
        }

        return n;
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

    static void PonerStock(int p, int t, int valor)
    {
        stock[p - 1, t - 1] = valor;
    }

    static void Traslado()
    {
        Console.WriteLine("\n--- MODULO DE TRASLADO ENTRE TIENDAS ---");
        
        Console.Write("Numero de prenda (1-15): ");
        int p = LeerEntero();

        if (p < 1 || p > 15)
        {
            Console.WriteLine("[FALLO] Numero de prenda invalido. Debe estar entre 1 y 15.");
            return;
        }

        // Muestra el nombre de la prenda seleccionada
        string nombrePrenda = nombres[p - 1];
        Console.WriteLine("Prenda seleccionada: " + nombrePrenda);

        Console.Write("Tienda origen (1-5): ");
        int origen = LeerEntero();

        Console.Write("Tienda destino (1-5): ");
        int destino = LeerEntero();

        Console.Write("Cantidad a trasladar: ");
        int cant = LeerEntero();

        int stockActualOrigen = ObtenerStock(p, origen);

        // Validaciones de negocio
        if (cant > 50)
        {
            Console.WriteLine("[FALLO] La cantidad maxima permitida por traslado es 50.");
        }
        else if (origen < 1 || origen > 5 || destino < 1 || destino > 5)
        {
            Console.WriteLine("[FALLO] Las tiendas deben estar en el rango de 1 a 5.");
        }
        else if (origen == destino)
        {
            Console.WriteLine("[FALLO] La tienda de origen y destino no pueden ser la misma.");
        }
        else if (cant <= 0)
        {
            Console.WriteLine("[FALLO] La cantidad a trasladar debe ser mayor a cero.");
        }
        else if (stockActualOrigen < cant)
        {
            // Alerta detallada si no hay suficiente stock
            Console.WriteLine("[FALLO] Stock insuficiente en la Tienda " + origen + ".");
            Console.WriteLine("-> Stock actual disponible: " + stockActualOrigen + " unidades.");
            Console.WriteLine("-> Cantidad solicitada: " + cant + " unidades.");
        }
        else
        {
            // Ejecutar traslado
            PonerStock(p, origen, stockActualOrigen - cant);
            int nuevoStockOrigen = ObtenerStock(p, origen);
            
            int stockActualDestino = ObtenerStock(p, destino);
            PonerStock(p, destino, stockActualDestino + cant);

            // Mostrar resultado y stock restante
            Console.WriteLine("\n[EXITO] Traslado realizado correctamente.");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Prenda: " + nombrePrenda);
            Console.WriteLine("Cantidad trasladada: " + cant);
            Console.WriteLine("De Tienda " + origen + " -> A Tienda " + destino);
            Console.WriteLine("Stock restante en Tienda " + origen + ": " + nuevoStockOrigen + " unidades.");
            Console.WriteLine("----------------------------------------");

            // Guardar registro en archivo de texto traslados.txt
            string fechaHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            string registro = "[" + fechaHora + "] Traslado de " + cant + " und(s) de '" + nombrePrenda + 
                              "' (Tienda " + origen + " -> Tienda " + destino + "). Stock restante origen: " + nuevoStockOrigen + "\n";

            try
            {
                File.AppendAllText(archivoTraslados, registro);
                Console.WriteLine("Registro de traslado guardado en " + archivoTraslados);
            }
            catch (Exception)
            {
                Console.WriteLine("Aviso: No se pudo escribir en el archivo de registro de traslados.");
            }
        }
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