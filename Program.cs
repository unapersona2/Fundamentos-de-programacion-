using System;

class Program
{
    static string[] nombres = {
        "Polo basico",
        "Camisa manga larga",
        "Jean clasico",
        "Short deportivo",
        "Gorra",
        "Vestido de fiesta largo",
        "Vestido casual",
        "Vestido de noche",
        "Falda plisada",
        "Blusa elegante",
        "Chompa de lana",
        "Casaca de jean",
        "Pantalon jogger",
        "Polera con capucha",
        "Bufanda"
    };

    static double[] precios = {
        35, 80, 120, 45, 25, 180, 90, 250, 60, 70, 110, 150, 95, 130, 40
    };

    static string[] colores = {
        "Negro", "Blanco", "Azul", "Rojo", "Verde", "Gris", "Beige"
    };

    static string[] tallas = {
        "XS", "S", "M", "L", "XL"
    };

    static int[,] stock = new int[15, 5];

    static bool[,] comprada = new bool[15, 5];

    static int numBoleta = 1001;
    static string archivoDatos = "datos.txt";
    static string archivoBoletas = "boletas.txt";

    static void Main()
    {
        int opcion;

        LlenarStockInicial();
        CargarDatos();

        do
        {
            MostrarMenu();
            opcion = LeerEntero();

            if (opcion == 1) VerCatalogo();
            else if (opcion == 2) VerStock();
            else if (opcion == 3) Traslado();
            else if (opcion == 4) EmitirBoleta();
            else if (opcion == 5) BuscarBoleta();
            else if (opcion == 6) VerPrendasCompradas();
            else if (opcion != 0) Console.WriteLine("Opcion invalida.");

        } while (opcion != 0);

        GuardarDatos();
        Console.WriteLine("FIN DEL PROGRAMA");
    }

    static void MostrarMenu()
    {
        Console.WriteLine();
        Console.WriteLine("       TIENDA DE ROPA");
        Console.WriteLine("          CATALOGO");
        Console.WriteLine("  1. Ver catalogo");
        Console.WriteLine("           STOCK");
        Console.WriteLine("  2. Ver stock");
        Console.WriteLine("  3. Traslado entre tiendas");
        Console.WriteLine("          VENTAS");
        Console.WriteLine("  4. Emitir boleta");
        Console.WriteLine("  5. Buscar boleta");
        Console.WriteLine("  6. Ver prendas compradas");
        Console.WriteLine("  0. Salir");
        Console.Write("Elija una opcion: ");
    }

    static int LeerEntero()
    {
        int n;
        string texto = Console.ReadLine();

        while (!int.TryParse(texto, out n))
        {
            Console.Write("Ingrese un numero valido: ");
            texto = Console.ReadLine();
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


    static void GuardarDatos()
    {
        string[] lineas = new string[31];
        lineas[0] = numBoleta.ToString();

        // Guardar stock
        for (int p = 0; p < 15; p++)
        {
            string fila = "";

            for (int t = 0; t < 5; t++)
            {
                fila += stock[p, t];

                if (t < 4) fila += ",";
            }

            lineas[p + 1] = fila;
        }

        for (int p = 0; p < 15; p++)
        {
            string fila = "";

            for (int t = 0; t < 5; t++)
            {
                fila += comprada[p, t] ? "1" : "0";

                if (t < 4) fila += ",";
            }

            lineas[p + 16] = fila;
        }

        try
        {
            File.WriteAllLines(archivoDatos, lineas);
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("No hay permiso para guardar " + archivoDatos + ".");
        }
        catch (IOException)
        {
            Console.WriteLine("No se pudo guardar el archivo " + archivoDatos + ".");
        }
    }


    static void CargarDatos()
    {
        if (!File.Exists(archivoDatos))
        {
            return;
        }

        try
        {
            string[] lineas = File.ReadAllLines(archivoDatos);

            if (lineas.Length < 16)
            {
                return;
            }

            int numeroGuardado;
            if (int.TryParse(lineas[0], out numeroGuardado))
            {
                numBoleta = numeroGuardado;
            }

            // Cargar stock
            for (int p = 0; p < 15; p++)
            {
                string[] partes = lineas[p + 1].Split(',');

                if (partes.Length >= 5)
                {
                    for (int t = 0; t < 5; t++)
                    {
                        int cantidad;
                        if (int.TryParse(partes[t], out cantidad))
                        {
                            stock[p, t] = cantidad;
                        }
                    }
                }
            }

            // Cargar compras si el archivo ya tiene el nuevo formato.
            // Si es un archivo antiguo, comprada se queda en false.
            if (lineas.Length >= 31)
            {
                for (int p = 0; p < 15; p++)
                {
                    string[] partes = lineas[p + 16].Split(',');

                    if (partes.Length >= 5)
                    {
                        for (int t = 0; t < 5; t++)
                        {
                            comprada[p, t] = (partes[t] == "1");
                        }
                    }
                }
            }
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("No hay permiso para leer " + archivoDatos + ".");
        }
        catch (IOException)
        {
            Console.WriteLine("No se pudo leer el archivo " + archivoDatos + ".");
        }
    }


    static void VerCatalogo()
    {
        Console.WriteLine();
        Console.WriteLine("N".PadRight(4) + "Prenda".PadRight(26) + "Precio sin IGV");
        Console.WriteLine(new string('-', 46));

        for (int p = 1; p <= 15; p++)
        {
            Console.WriteLine(p.ToString().PadRight(4) +
                              nombres[p - 1].PadRight(26) +
                              "S/ " + precios[p - 1].ToString("0.00"));
        }

        Console.WriteLine(new string('-', 46));
        Console.WriteLine("Tallas: XS, S, M, L, XL (se elige al comprar)");
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

    static void VerPrendasCompradas()
    {
        Console.WriteLine();
        Console.WriteLine("N".PadRight(4) + "Prenda".PadRight(26) +
                          "T1".PadLeft(6) + "T2".PadLeft(6) + "T3".PadLeft(6) +
                          "T4".PadLeft(6) + "T5".PadLeft(6));
        Console.WriteLine(new string('-', 60));

        bool encontrado = false;

        for (int p = 0; p < 15; p++)
        {
            string linea = (p + 1).ToString().PadRight(4) + nombres[p].PadRight(26);

            for (int t = 0; t < 5; t++)
            {
                if (comprada[p, t])
                {
                    linea += "X".PadLeft(6);
                    encontrado = true;
                }
                else
                {
                    linea += "-".PadLeft(6);
                }
            }

            Console.WriteLine(linea);
        }

        Console.WriteLine(new string('-', 60));
        Console.WriteLine("X = prenda comprada en esa tienda");

        if (!encontrado)
        {
            Console.WriteLine("Todavia no se han comprado prendas.");
        }
    }


    static void Traslado()
    {
        Console.Write("Numero de prenda (1-15): ");
        int p = LeerEntero();

        Console.Write("Tienda origen (1-5): ");
        int origen = LeerEntero();

        Console.Write("Tienda destino (1-5): ");
        int destino = LeerEntero();

        Console.Write("Cantidad: ");
        int cant = LeerEntero();

        if (cant > 50)
        {
            Console.WriteLine("La cantidad maxima es 50.");
        }
        else if (p < 1 || p > 15 || origen < 1 || origen > 5 ||
                 destino < 1 || destino > 5 || origen == destino || cant <= 0)
        {
            Console.WriteLine("Datos invalidos.");
        }
        else if (ObtenerStock(p, origen) < cant)
        {
            Console.WriteLine("Stock insuficiente. Solo hay " +
                              ObtenerStock(p, origen));
        }
        else
        {
            PonerStock(p, origen, ObtenerStock(p, origen) - cant);
            PonerStock(p, destino, ObtenerStock(p, destino) + cant);

            GuardarDatos();
            Console.WriteLine("Traslado realizado.");
        }
    }


    static void EmitirBoleta()
    {
        if (numBoleta > 1100)
        {
            Console.WriteLine("Talonario agotado.");
            return;
        }

        Console.Write("Nombre del cliente: ");
        string cliente = Console.ReadLine();

        Console.Write("DNI del cliente: ");
        string dni = Console.ReadLine();

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

        numBoleta++;
        GuardarDatos();
    }

    static void BuscarBoleta()
    {
        if (!File.Exists(archivoBoletas))
        {
            Console.WriteLine("Todavia no hay boletas guardadas.");
            return;
        }

        Console.Write("Numero de boleta: ");
        int num = LeerEntero();

        try
        {
            string[] lineas = File.ReadAllLines(archivoBoletas);
            bool mostrando = false;
            bool hallada = false;

            for (int i = 0; i < lineas.Length; i++)
            {
                if (lineas[i] == "BOLETA N " + num)
                {
                    mostrando = true;
                    hallada = true;
                }

                if (mostrando)
                {
                    Console.WriteLine(lineas[i]);

                    if (lineas[i] == "----------")
                    {
                        mostrando = false;
                    }
                }
            }

            if (!hallada)
            {
                Console.WriteLine("Boleta no encontrada.");
            }
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("No hay permiso para leer boletas.txt.");
        }
        catch (IOException)
        {
            Console.WriteLine("No se pudo leer el archivo de boletas.");
        }
    }
}
