using System;

namespace MagazinParis
{
    public class Client
    {
        public string Nume;
        public double Buget;

        public Client(string nume, double buget)
        {
            Nume = nume;
            Buget = buget;
        }

        public void AfisareInfo()
        {
            Console.WriteLine($"Client: {Nume} | Buget disponibil: {Buget} RON");
        }

        public void Cumpara(Produs produsDorit, int cantitate)
        {
            double costTotal = produsDorit.Pret * cantitate;

            Console.WriteLine($"\n{Nume} incearca sa cumpere {cantitate} x {produsDorit.Nume}");

            if (produsDorit.Cantitate >= cantitate)
            {
                if (Buget >= costTotal)
                {
                    produsDorit.Cantitate -= cantitate;
                    Buget -= costTotal;

                    Console.WriteLine($"[SUCCES] Tranzactie reusita! Cost: {costTotal} RON. Buget ramas: {Buget} RON.");
                }
                else
                {
                    Console.WriteLine($"[RESPINS] Fonduri insuficiente! {Nume} are doar {Buget} RON, dar costa {costTotal} RON.");
                }
            }
            else
            {
                Console.WriteLine($"[RESPINS] Stoc insuficient in magazin. Mai sunt doar {produsDorit.Cantitate} bucati.");
            }
        }
    }
}