using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MagazinParis
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Produs> _produse;
        private ObservableCollection<Client> _clienti;
        private Produs _produsForm;
        private Client _clientForm;
        private Produs _produsSelectat;
        private Client _clientSelectat;
        private string _textCautareProdus;
        private string _textCautareClient;

        private const string NumeFisierProduse = "Produse.txt";
        private const string NumeFisierClienti = "Clienti.txt";

        public ObservableCollection<Produs> Produse
        {
            get { return _produse; }
            set
            {
                _produse = value;
                OnPropertyChanged();
                ActualizeazaStatisticiProduse();
            }
        }

        public ObservableCollection<Client> Clienti
        {
            get { return _clienti; }
            set
            {
                _clienti = value;
                OnPropertyChanged();
                ActualizeazaStatisticiClienti();
            }
        }

        public Produs ProdusForm
        {
            get { return _produsForm; }
            set
            {
                _produsForm = value;
                OnPropertyChanged();
            }
        }

        public Client ClientForm
        {
            get { return _clientForm; }
            set
            {
                _clientForm = value;
                OnPropertyChanged();
            }
        }

        public Produs ProdusSelectat
        {
            get { return _produsSelectat; }
            set
            {
                _produsSelectat = value;
                OnPropertyChanged();
                if (_produsSelectat != null)
                {
                    ProdusForm = new Produs(
                        _produsSelectat.CodUnic,
                        _produsSelectat.Nume,
                        _produsSelectat.Pret,
                        _produsSelectat.Cantitate,
                        _produsSelectat.Categorie,
                        _produsSelectat.Caracteristici)
                    {
                        DataAdaugarii = _produsSelectat.DataAdaugarii
                    };
                    ActualizeazaCheckBoxuriCaracteristici();
                }
            }
        }

        public Client ClientSelectat
        {
            get { return _clientSelectat; }
            set
            {
                _clientSelectat = value;
                OnPropertyChanged();
                if (_clientSelectat != null)
                {
                    ClientForm = new Client(
                        _clientSelectat.IdUnic,
                        _clientSelectat.Nume,
                        _clientSelectat.Email,
                        _clientSelectat.Buget)
                    {
                        DataInregistrarii = _clientSelectat.DataInregistrarii
                    };
                }
            }
        }

        public string TextCautareProdus
        {
            get { return _textCautareProdus; }
            set
            {
                _textCautareProdus = value;
                OnPropertyChanged();
                FiltreazaProduse();
            }
        }

        public string TextCautareClient
        {
            get { return _textCautareClient; }
            set
            {
                _textCautareClient = value;
                OnPropertyChanged();
                FiltreazaClienti();
            }
        }

        public Array CategoriiProduse => Enum.GetValues(typeof(CategorieProdus));

        private int _totalProduse;
        private double _valoareStoc;
        private int _totalBucati;
        private int _totalCategorii;
        private int _totalClienti;
        private double _bugetTotal;

        public int TotalProduse
        {
            get { return _totalProduse; }
            set { _totalProduse = value; OnPropertyChanged(); }
        }

        public double ValoareStoc
        {
            get { return _valoareStoc; }
            set { _valoareStoc = value; OnPropertyChanged(); }
        }

        public int TotalBucati
        {
            get { return _totalBucati; }
            set { _totalBucati = value; OnPropertyChanged(); }
        }

        public int TotalCategorii
        {
            get { return _totalCategorii; }
            set { _totalCategorii = value; OnPropertyChanged(); }
        }

        public int TotalClienti
        {
            get { return _totalClienti; }
            set { _totalClienti = value; OnPropertyChanged(); }
        }

        public double BugetTotal
        {
            get { return _bugetTotal; }
            set { _bugetTotal = value; OnPropertyChanged(); }
        }

        private ICollectionView _produseView;
        private ICollectionView _clientiView;

        public ICollectionView ProduseView => _produseView;

        public ICollectionView ClientiView => _clientiView;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Produse = new ObservableCollection<Produs>();
            Clienti = new ObservableCollection<Client>();

            Produse.CollectionChanged += Produse_CollectionChanged;
            Clienti.CollectionChanged += Clienti_CollectionChanged;

            IncarcaDate();

            _produseView = CollectionViewSource.GetDefaultView(Produse);
            _produseView.Filter = FiltrareProdus;

            _clientiView = CollectionViewSource.GetDefaultView(Clienti);
            _clientiView.Filter = FiltrareClient;

            ProdusForm = new Produs();
            ClientForm = new Client();

            foreach (CheckBox cb in pnlCaracteristici.Children.OfType<CheckBox>())
            {
                cb.Checked += Caracteristici_CheckedChanged;
                cb.Unchecked += Caracteristici_CheckedChanged;
            }

            Closing += MainWindow_Closing;
        }

        private void Produse_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ActualizeazaStatisticiProduse();
        }

        private void Clienti_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ActualizeazaStatisticiClienti();
        }

        private void IncarcaDate()
        {
            if (File.Exists(NumeFisierProduse))
            {
                string[] linii = File.ReadAllLines(NumeFisierProduse);
                foreach (string linie in linii)
                {
                    if (!string.IsNullOrWhiteSpace(linie))
                    {
                        Produse.Add(new Produs(linie));
                    }
                }
            }

            if (File.Exists(NumeFisierClienti))
            {
                string[] linii = File.ReadAllLines(NumeFisierClienti);
                foreach (string linie in linii)
                {
                    if (!string.IsNullOrWhiteSpace(linie))
                    {
                        Clienti.Add(new Client(linie));
                    }
                }
            }
        }

        private void SalveazaDate()
        {
            using (StreamWriter sw = new StreamWriter(NumeFisierProduse, false))
            {
                foreach (Produs p in Produse)
                {
                    sw.WriteLine(p.ConversieLaSir_PentruFisier());
                }
            }

            using (StreamWriter sw = new StreamWriter(NumeFisierClienti, false))
            {
                foreach (Client c in Clienti)
                {
                    sw.WriteLine(c.ConversieLaSir_PentruFisier());
                }
            }
        }

        private void ActualizeazaStatisticiProduse()
        {
            TotalProduse = Produse.Count;
            ValoareStoc = Produse.Sum(p => p.Pret * p.Cantitate);
            TotalBucati = Produse.Sum(p => p.Cantitate);
            TotalCategorii = Produse.Select(p => p.Categorie).Distinct().Count();
        }

        private void ActualizeazaStatisticiClienti()
        {
            TotalClienti = Clienti.Count;
            BugetTotal = Clienti.Sum(c => c.Buget);
        }

        private bool FiltrareProdus(object obj)
        {
            if (string.IsNullOrWhiteSpace(TextCautareProdus))
                return true;

            Produs p = obj as Produs;
            if (p == null)
                return false;

            return p.Nume.ToLower().Contains(TextCautareProdus.ToLower());
        }

        private bool FiltrareClient(object obj)
        {
            if (string.IsNullOrWhiteSpace(TextCautareClient))
                return true;

            Client c = obj as Client;
            if (c == null)
                return false;

            return c.Nume.ToLower().Contains(TextCautareClient.ToLower());
        }

        private void FiltreazaProduse()
        {
            _produseView?.Refresh();
        }

        private void FiltreazaClienti()
        {
            _clientiView?.Refresh();
        }

        private void ActualizeazaCheckBoxuriCaracteristici()
        {
            if (ProdusForm == null) return;

            foreach (CheckBox cb in pnlCaracteristici.Children.OfType<CheckBox>())
            {
                CaracteristiciProdus val = (CaracteristiciProdus)Enum.Parse(typeof(CaracteristiciProdus), cb.Tag.ToString());
                cb.IsChecked = ProdusForm.Caracteristici.HasFlag(val);
            }
        }

        private void Caracteristici_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (ProdusForm == null) return;

            ProdusForm.Caracteristici = CaracteristiciProdus.Niciuna;
            foreach (CheckBox cb in pnlCaracteristici.Children.OfType<CheckBox>())
            {
                if (cb.IsChecked == true)
                {
                    CaracteristiciProdus val = (CaracteristiciProdus)Enum.Parse(typeof(CaracteristiciProdus), cb.Tag.ToString());
                    ProdusForm.Caracteristici |= val;
                }
            }
        }

        private void AdaugaProdus_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProdusForm.CodUnic) || string.IsNullOrWhiteSpace(ProdusForm.Nume) || ProdusForm.Pret <= 0)
            {
                MessageBox.Show("Completați corect toate câmpurile obligatorii!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Produse.Any(p => p.CodUnic == ProdusForm.CodUnic))
            {
                MessageBox.Show("Există deja un produs cu acest cod unic!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Produse.Add(new Produs(
                ProdusForm.CodUnic,
                ProdusForm.Nume,
                ProdusForm.Pret,
                ProdusForm.Cantitate,
                ProdusForm.Categorie,
                ProdusForm.Caracteristici)
            {
                DataAdaugarii = ProdusForm.DataAdaugarii
            });

            ActualizeazaStatisticiProduse();
            ProdusForm = new Produs();
            ActualizeazaCheckBoxuriCaracteristici();
        }

        private void ModificaProdus_Click(object sender, RoutedEventArgs e)
        {
            if (ProdusSelectat == null)
            {
                MessageBox.Show("Selectați un produs pentru a-l modifica!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int index = Produse.IndexOf(ProdusSelectat);
            if (index != -1)
            {
                Produse[index] = new Produs(
                    ProdusForm.CodUnic,
                    ProdusForm.Nume,
                    ProdusForm.Pret,
                    ProdusForm.Cantitate,
                    ProdusForm.Categorie,
                    ProdusForm.Caracteristici)
                {
                    DataAdaugarii = ProdusForm.DataAdaugarii
                };
                ActualizeazaStatisticiProduse();
                MessageBox.Show("Produs modificat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void StergeProdus_Click(object sender, RoutedEventArgs e)
        {
            if (ProdusSelectat == null)
            {
                MessageBox.Show("Selectați un produs pentru a-l șterge!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                $"Sigur doriți să ștergeți produsul '{ProdusSelectat.Nume}'?",
                "Confirmare ștergere",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Produse.Remove(ProdusSelectat);
                ActualizeazaStatisticiProduse();
                ProdusForm = new Produs();
                ActualizeazaCheckBoxuriCaracteristici();
            }
        }

        private void CurataCampuriProdus_Click(object sender, RoutedEventArgs e)
        {
            ProdusForm = new Produs();
            ActualizeazaCheckBoxuriCaracteristici();
            ProdusSelectat = null;
        }

        private void AdaugaClient_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ClientForm.Nume) || string.IsNullOrWhiteSpace(ClientForm.Email) || ClientForm.Buget <= 0)
            {
                MessageBox.Show("Completați corect toate câmpurile obligatorii!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Clienti.Add(new Client(
                Guid.NewGuid().ToString(),
                ClientForm.Nume,
                ClientForm.Email,
                ClientForm.Buget)
            {
                DataInregistrarii = ClientForm.DataInregistrarii
            });

            ActualizeazaStatisticiClienti();
            ClientForm = new Client();
        }

        private void ModificaClient_Click(object sender, RoutedEventArgs e)
        {
            if (ClientSelectat == null)
            {
                MessageBox.Show("Selectați un client pentru a-l modifica!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int index = Clienti.IndexOf(ClientSelectat);
            if (index != -1)
            {
                Clienti[index] = new Client(
                    ClientSelectat.IdUnic,
                    ClientForm.Nume,
                    ClientForm.Email,
                    ClientForm.Buget)
                {
                    DataInregistrarii = ClientForm.DataInregistrarii
                };
                ActualizeazaStatisticiClienti();
                MessageBox.Show("Client modificat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void StergeClient_Click(object sender, RoutedEventArgs e)
        {
            if (ClientSelectat == null)
            {
                MessageBox.Show("Selectați un client pentru a-l șterge!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                $"Sigur doriți să ștergeți clientul '{ClientSelectat.Nume}'?",
                "Confirmare ștergere",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Clienti.Remove(ClientSelectat);
                ActualizeazaStatisticiClienti();
                ClientForm = new Client();
            }
        }

        private void CurataCampuriClient_Click(object sender, RoutedEventArgs e)
        {
            ClientForm = new Client();
            ClientSelectat = null;
        }

        private void Salveaza_Click(object sender, RoutedEventArgs e)
        {
            SalveazaDate();
            MessageBox.Show("Date salvate cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Iesire_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            SalveazaDate();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
