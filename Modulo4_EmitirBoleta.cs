using System;
using System.IO;

// MODULO 4: EMITIR BOLETA (opcion 4 del menu)
class Program
{
    static string[] nombres = {
        "Polo basico", "Camisa manga larga", "Jean clasico", "Short deportivo",
        "Gorra", "Vestido de fiesta largo", "Vestido casual", "Vestido de noche",
        "Falda plisada", "Blusa elegante", "Chompa de lana", "Casaca de jean",
        "Pantalon jogger", "Polera con capucha", "Bufanda"
    };

    static double[] precios = {
        35, 80, 120, 45, 25, 180, 90, 250, 60, 70, 110, 150, 95, 130, 40
    };

    static string[] colores = { "Negro", "Blanco", "Azul", "Rojo", "Verde", "Gris", "Beige" };

    static string[] tallas = { "XS", "S", "M", "L", "XL" };

    static int[,] stock = new int[15, 5];
    static bool[,] comprada = new bool[15, 5];

    static int numBoleta = 1001;
    static string archivoBoletas = "boletas.txt";

    static void Main()
    {
        LlenarStockInicial();
        EmitirBoleta();
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

    static void VerColores()
    {
        Console.WriteLine("Colores disponibles:");

        for (int c = 0; c < colores.Length; c++)
        {
            Console.WriteLine("  " + (c + 1) + ". " + colores[c]);
        }
    }

    static void VerTallas()
    {
        Console.WriteLine("Tallas disponibles:");

        for (int i = 0; i < tallas.Length; i++)
        {
            Console.WriteLine("  " + (i + 1) + ". " + tallas[i]);
        }
    }

    // La gorra (5) y la bufanda (15) no llevan talla
    static bool UsaTalla(int p)
    {
        if (p == 5 || p == 15)
        {
            return false;
        }

        return true;
    }

    static void EmitirBoleta()
    {
        if (numBoleta > 1100)
        {
            Console.WriteLine("Talonario agotado.");
            return;
        }

        Console.Write("Nombre del cliente: ");
        string cliente = Console.ReadLine() ?? "";

        Console.Write("DNI del cliente: ");
        string dni = Console.ReadLine() ?? "";

        Console.Write("Tienda (1-5): ");
        int tienda = LeerEntero();

        Console.Write("Forma de pago (1=Yape, 2=BCP): ");
        int pago = LeerEntero();

        if (tienda < 1 || tienda > 5 || pago < 1 || pago > 2)
        {
            Console.WriteLine("Datos invalidos.");
            return;
        }

        string fecha = DateTime.Now.ToString("dd/MM/yyyy");
        string formaPago = "Yape";

        if (pago == 2) formaPago = "BCP";

        double subtotal = 0;
        string detalle = "";
        int seguir = 1;

        while (seguir == 1)
        {
            Console.Write("Numero de prenda (1-15): ");
            int p = LeerEntero();

            Console.Write("Cantidad: ");
            int cant = LeerEntero();

            if (cant > 50)
            {
                Console.WriteLine("La cantidad maxima es 50.");
            }
            else if (p < 1 || p > 15 || cant <= 0)
            {
                Console.WriteLine("Datos invalidos.");
            }
            else if (ObtenerStock(p, tienda) < cant)
            {
                Console.WriteLine("Stock insuficiente. Solo hay " +
                                  ObtenerStock(p, tienda));
            }
            else
            {
                VerColores();
                Console.Write("Elija el color: ");
                int c = LeerEntero();

                int tl = 0;
                if (UsaTalla(p))
                {
                    VerTallas();
                    Console.Write("Elija la talla: ");
                    tl = LeerEntero();
                }

                if (c < 1 || c > colores.Length)
                {
                    Console.WriteLine("Color invalido. No se agrego la prenda.");
                }
                else if (UsaTalla(p) && (tl < 1 || tl > tallas.Length))
                {
                    Console.WriteLine("Talla invalida. No se agrego la prenda.");
                }
                else
                {
                    string talla = "Unica";
                    if (UsaTalla(p)) talla = tallas[tl - 1];

                    PonerStock(p, tienda, ObtenerStock(p, tienda) - cant);
                    comprada[p - 1, tienda - 1] = true;

                    double importe = precios[p - 1] * cant;
                    subtotal += importe;

                    detalle += nombres[p - 1] + " | Talla " + talla +
                               " | " + colores[c - 1] +
                               " x" + cant + "  S/ " +
                               importe.ToString("0.00") + "\n";

                    Console.WriteLine("Prenda agregada.");
                }
            }

            Console.Write("Agregar otra prenda? (1=Si, 0=No): ");
            seguir = LeerEntero();
        }

        if (subtotal == 0)
        {
            Console.WriteLine("No se agrego ninguna prenda. No se emite boleta.");
            return;
        }

        double igv = subtotal * 0.18;
        double total = subtotal + igv;

        string boleta = "BOLETA N " + numBoleta + "\n";
        boleta += "Fecha:    " + fecha + "\n";
        boleta += "Cliente:  " + cliente + "\n";
        boleta += "DNI:      " + dni + "\n";
        boleta += "Tienda:   " + tienda + "\n";
        boleta += detalle;
        boleta += "Subtotal: S/ " + subtotal.ToString("0.00") + "\n";
        boleta += "IGV 18%:  S/ " + igv.ToString("0.00") + "\n";
        boleta += "TOTAL:    S/ " + total.ToString("0.00") + "\n";
        boleta += "Pago:     " + formaPago + "\n";
        boleta += "----------";

        Console.WriteLine();
        Console.WriteLine(boleta);

        try
        {
            File.AppendAllText(archivoBoletas, boleta + "\n");
            Console.WriteLine("Boleta guardada.");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("No hay permiso para guardar boletas.txt.");
        }
        catch (IOException)
        {
            Console.WriteLine("No se pudo guardar la boleta en el archivo.");
        }
        catch (Exception)
        {
            Console.WriteLine("Este compilador no permite guardar archivos.");
        }

        numBoleta++;
    }
}