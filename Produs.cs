using System;

namespace MagazinParis
{
    public class Produs
    {
        public string CodUnic;
        public string Nume;
        public double Pret;
        public int Cantitate;

        public Produs(string codUnic, string nume, double pret, int cantitate)
        {
            CodUnic = codUnic;
            Nume = nume;
            Pret = pret;
            Cantitate = cantitate;
        }

        public void AfisareInfo()
        {
            Console.WriteLine($"[{CodUnic}] {Nume} | Pret: {Pret} RON | Stoc: {Cantitate} buc.");
        }
    }
}