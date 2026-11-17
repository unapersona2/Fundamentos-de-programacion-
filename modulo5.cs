using System;
using System.IO;

class Program
{
    static string archivoBoletas = "boletas.txt";

    static void Main()
    {
        CrearBoletaDeEjemplo();
        BuscarBoleta();
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

    // Solo para poder probar el modulo: crea la boleta 1001 si no existe el archivo
    static void CrearBoletaDeEjemplo()
    {
        try
        {
            if (!File.Exists(archivoBoletas))
            {
                string boleta = "BOLETA N 1001\n";
                boleta += "Fecha:    01/01/2026\n";
                boleta += "Cliente:  Cliente de prueba\n";
                boleta += "DNI:      12345678\n";
                boleta += "Tienda:   1\n";
                boleta += "Polo basico | Talla M | Negro x2  S/ 70.00\n";
                boleta += "Subtotal: S/ 70.00\n";
                boleta += "IGV 18%:  S/ 12.60\n";
                boleta += "TOTAL:    S/ 82.60\n";
                boleta += "Pago:     Yape\n";
                boleta += "----------";

                File.AppendAllText(archivoBoletas, boleta + "\n");
            }
        }
        catch (Exception)
        {
            Console.WriteLine("No se pudo crear la boleta de ejemplo.");
        }
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