using System;
using System.IO;

namespace MagazinParis
{
    public class AdministrareClienti_FisierText
    {
        private string numeFisier;

        public AdministrareClienti_FisierText(string numeFisier)
        {
            this.numeFisier = numeFisier;
            if (!File.Exists(numeFisier))
            {
                File.Create(numeFisier).Close();
            }
        }

        public void SalveazaClientiInFisier(Clienti clienti)
        {
            using (StreamWriter sw = new StreamWriter(numeFisier, false))
            {
                for (int i = 0; i < clienti.numarClienti; i++)
                {
                    sw.WriteLine(clienti.clienti[i].ConversieLaSir_PentruFisier());
                }
            }
        }

        public void CitesteDinFisierInClienti(Clienti clienti)
        {
            if (!File.Exists(numeFisier))
                return;

            string[] linii = File.ReadAllLines(numeFisier);
            foreach (string linie in linii)
            {
                if (!string.IsNullOrEmpty(linie))
                {
                    Client c = new Client(linie);
                    clienti.AdaugaClient(c);
                }
            }
        }

        public void AdaugaClient(Client client)
        {
            using (StreamWriter sw = new StreamWriter(numeFisier, true))
            {
                sw.WriteLine(client.ConversieLaSir_PentruFisier());
            }
        }

        public Client[] GetClienti(out int nrClienti)
        {
            string[] linii = File.ReadAllLines(numeFisier);
            Client[] clienti = new Client[linii.Length];
            nrClienti = 0;

            foreach (string linie in linii)
            {
                if (!string.IsNullOrEmpty(linie))
                {
                    clienti[nrClienti] = new Client(linie);
                    nrClienti++;
                }
            }
            return clienti;
        }

        public Client CautaClient(string idUnic)
        {
            int nrClienti;
            Client[] clienti = GetClienti(out nrClienti);

            for (int i = 0; i < nrClienti; i++)
            {
                if (clienti[i].IdUnic == idUnic)
                {
                    return clienti[i];
                }
            }
            return null;
        }

        public void ModificaClient(string idUnic, Client clientNou)
        {
            int nrClienti;
            Client[] clienti = GetClienti(out nrClienti);

            using (StreamWriter sw = new StreamWriter(numeFisier, false))
            {
                for (int i = 0; i < nrClienti; i++)
                {
                    if (clienti[i].IdUnic == idUnic)
                    {
                        sw.WriteLine(clientNou.ConversieLaSir_PentruFisier());
                    }
                    else
                    {
                        sw.WriteLine(clienti[i].ConversieLaSir_PentruFisier());
                    }
                }
            }
        }
    }
}
