using System;

namespace MagazinParis
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("BUN VENIT LA MAGAZINUL PARIS\n");

            Produs produs1 = new Produs("P01", "Suc de mere", 4.50, 50);
            Produs produs2 = new Produs("P02", "Pringles", 11.00, 30);

            Client client1 = new Client("Andrei", 50.00);
            Client client2 = new Client("Maria", 10.00);

            Console.WriteLine("STARE INITIALA");
            produs1.AfisareInfo();
            produs2.AfisareInfo();
            client1.AfisareInfo();
            client2.AfisareInfo();

            client1.Cumpara(produs1, 5);

            client2.Cumpara(produs2, 3);

            client1.Cumpara(produs2, 100);

            Console.WriteLine("\nSTARE DUPA TRANZACTII");
            produs1.AfisareInfo();
            produs2.AfisareInfo();
            client1.AfisareInfo();
            client2.AfisareInfo();

            Console.ReadKey();
        }
    }
}