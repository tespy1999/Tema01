using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MagazinParis
{
    public class Client : INotifyPropertyChanged, IDataErrorInfo
    {
        private string nume;
        private double buget;
        private string idUnic;
        private string email;
        private DateTime dataInregistrarii;

        public string IdUnic
        {
            get { return idUnic; }
            set
            {
                if (idUnic != value)
                {
                    idUnic = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Nume
        {
            get { return nume; }
            set
            {
                if (nume != value)
                {
                    nume = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Buget
        {
            get { return buget; }
            set
            {
                if (buget != value)
                {
                    buget = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime DataInregistrarii
        {
            get { return dataInregistrarii; }
            set
            {
                if (dataInregistrarii != value)
                {
                    dataInregistrarii = value;
                    OnPropertyChanged();
                }
            }
        }

        public Client()
        {
            DataInregistrarii = DateTime.Now;
            IdUnic = Guid.NewGuid().ToString();
        }

        public Client(string nume, double buget)
        {
            IdUnic = Guid.NewGuid().ToString();
            Nume = nume;
            Email = "";
            Buget = buget;
            DataInregistrarii = DateTime.Now;
        }

        public Client(string idUnic, string nume, string email, double buget)
        {
            IdUnic = idUnic;
            Nume = nume;
            Email = email;
            Buget = buget;
            DataInregistrarii = DateTime.Now;
        }

        public Client(string linieFisier)
        {
            string[] date = linieFisier.Split(';');
            IdUnic = date[0].Trim();
            Nume = date[1].Trim();
            Email = date[2].Trim();
            Buget = double.Parse(date[3].Trim(), System.Globalization.CultureInfo.InvariantCulture);
            if (date.Length > 4)
                DataInregistrarii = DateTime.Parse(date[4].Trim(), System.Globalization.CultureInfo.InvariantCulture);
            else
                DataInregistrarii = DateTime.Now;
        }

        public string ConversieLaSir_PentruFisier()
        {
            return $"{IdUnic};{Nume};{Email};{Buget.ToString(System.Globalization.CultureInfo.InvariantCulture)};{DataInregistrarii:yyyy-MM-dd}";
        }

        public void AfisareInfo()
        {
            Console.WriteLine($"Client: {Nume} | Email: {Email} | Buget disponibil: {Buget} RON");
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string error = null;
                switch (columnName)
                {
                    case nameof(Nume):
                        if (string.IsNullOrWhiteSpace(Nume))
                            error = "Numele clientului este obligatoriu!";
                        break;
                    case nameof(Email):
                        if (string.IsNullOrWhiteSpace(Email))
                            error = "Email-ul este obligatoriu!";
                        else if (!Email.Contains("@"))
                            error = "Email-ul trebuie să conțină @!";
                        break;
                    case nameof(Buget):
                        if (Buget <= 0)
                            error = "Bugetul trebuie să fie un număr pozitiv!";
                        break;
                }
                return error;
            }
        }
    }
}
