using System;

namespace MagazinParis
{
    public class Clienti
    {
        public Client[] clienti;
        public int numarClienti;

        public Clienti(int capacitateMaxima)
        {
            clienti = new Client[capacitateMaxima];
            numarClienti = 0;
        }

        public void AdaugaClient(Client clientNou)
        {
            if (numarClienti < clienti.Length)
            {
                clienti[numarClienti] = clientNou;
                numarClienti++;
            }
            else
            {
                Console.WriteLine("Lista de clienti este plina!");
            }
        }

        public void AfiseazaToti()
        {
            for (int i = 0; i < numarClienti; i++)
            {
                clienti[i].AfisareInfo();
            }
        }

        public Client CautaDupaId(string idCautat)
        {
            for (int i = 0; i < numarClienti; i++)
            {
                if (clienti[i].IdUnic == idCautat)
                {
                    return clienti[i];
                }
            }
            return null;
        }

        public bool StergeClient(string idUnic)
        {
            for (int i = 0; i < numarClienti; i++)
            {
                if (clienti[i].IdUnic == idUnic)
                {
                    for (int j = i; j < numarClienti - 1; j++)
                    {
                        clienti[j] = clienti[j + 1];
                    }
                    numarClienti--;
                    clienti[numarClienti] = null;
                    return true;
                }
            }
            return false;
        }
    }
}
