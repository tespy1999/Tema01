using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MagazinParis
{
    public enum CategorieProdus
    {
        Necunoscut = 0,
        Patiserie = 1,
        Bauturi = 2,
        Dulciuri = 3,
        Gustari = 4
    }

    [Flags]
    public enum CaracteristiciProdus
    {
        Niciuna = 0,
        Bio = 1,
        Vegan = 2,
        FaraZahar = 4,
        FaraGluten = 8
    }

    public class Produs : INotifyPropertyChanged, IDataErrorInfo
    {
        private string codUnic;
        private string nume;
        private double pret;
        private int cantitate;
        private CategorieProdus categorie;
        private CaracteristiciProdus caracteristici;
        private DateTime dataAdaugarii;

        public string CodUnic
        {
            get { return codUnic; }
            set
            {
                if (codUnic != value)
                {
                    codUnic = value;
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

        public double Pret
        {
            get { return pret; }
            set
            {
                if (pret != value)
                {
                    pret = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Cantitate
        {
            get { return cantitate; }
            set
            {
                if (cantitate != value)
                {
                    cantitate = value;
                    OnPropertyChanged();
                }
            }
        }

        public CategorieProdus Categorie
        {
            get { return categorie; }
            set
            {
                if (categorie != value)
                {
                    categorie = value;
                    OnPropertyChanged();
                }
            }
        }

        public CaracteristiciProdus Caracteristici
        {
            get { return caracteristici; }
            set
            {
                if (caracteristici != value)
                {
                    caracteristici = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime DataAdaugarii
        {
            get { return dataAdaugarii; }
            set
            {
                if (dataAdaugarii != value)
                {
                    dataAdaugarii = value;
                    OnPropertyChanged();
                }
            }
        }

        public Produs() 
        {
            DataAdaugarii = DateTime.Now;
        }

        public Produs(string codUnic, string nume, double pret, int cantitate, CategorieProdus categorie, CaracteristiciProdus caracteristici)
        {
            CodUnic = codUnic;
            Nume = nume;
            Pret = pret;
            Cantitate = cantitate;
            Categorie = categorie;
            Caracteristici = caracteristici;
            DataAdaugarii = DateTime.Now;
        }

        public Produs(string linieFisier)
        {
            string[] date = linieFisier.Split(';');
            CodUnic = date[0].Trim();
            Nume = date[1].Trim();
            Pret = double.Parse(date[2].Trim(), System.Globalization.CultureInfo.InvariantCulture);
            Cantitate = int.Parse(date[3].Trim(), System.Globalization.CultureInfo.InvariantCulture);
            Categorie = (CategorieProdus)Enum.Parse(typeof(CategorieProdus), date[4].Trim(), true);
            Caracteristici = (CaracteristiciProdus)Enum.Parse(typeof(CaracteristiciProdus), date[5].Trim(), true);
            
            if (date.Length > 6)
                DataAdaugarii = DateTime.Parse(date[6].Trim(), System.Globalization.CultureInfo.InvariantCulture);
            else
                DataAdaugarii = DateTime.Now;
        }

        public string ConversieLaSir_PentruFisier()
        {
            return $"{CodUnic};{Nume};{Pret.ToString(System.Globalization.CultureInfo.InvariantCulture)};{Cantitate};{Categorie};{Caracteristici};{DataAdaugarii:yyyy-MM-dd}";
        }

        public void AfisareInfo()
        {
            Console.WriteLine($"[{CodUnic}] {Nume} | Categorie: {Categorie} | Info: {Caracteristici} | Pret: {Pret} RON | Stoc: {Cantitate} buc.");
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
                    case nameof(CodUnic):
                        if (string.IsNullOrWhiteSpace(CodUnic))
                            error = "Codul unic este obligatoriu!";
                        break;
                    case nameof(Nume):
                        if (string.IsNullOrWhiteSpace(Nume))
                            error = "Numele produsului este obligatoriu!";
                        break;
                    case nameof(Pret):
                        if (Pret <= 0)
                            error = "Prețul trebuie să fie un număr pozitiv!";
                        break;
                    case nameof(Cantitate):
                        if (Cantitate < 0)
                            error = "Cantitatea trebuie să fie un număr întreg pozitiv sau zero!";
                        break;
                    case nameof(Categorie):
                        if (Categorie == CategorieProdus.Necunoscut)
                            error = "Selectați o categorie!";
                        break;
                }
                return error;
            }
        }
    }
}