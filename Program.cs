using System;

class Program
{
    static string[] codigo = new string[10];
    static string[] nombre = new string[10];
    static int[] categoria = new int[10];
    static double[] precio = new double[10];
    static int[,] stock = new int[10, 3];          
    static string[] nomCat = new string[3];
    static int nProd = 0;

    static int[] bolNum = new int[50];
    static int[] bolProd = new int[50];
    static int[] bolCant = new int[50];
    static int[] bolTienda = new int[50];
    static double[] bolSub = new double[50];
    static int[] bolPago = new int[50];
    static string[] bolEstado = new string[50];
    static string[] nomPago = { "Yape", "BCP" };
    static int nVen = 0;
    static int nextBol = 1001;

    static void Main()
    {
       