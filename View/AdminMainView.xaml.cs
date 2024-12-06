using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace FinancniInformacniSystemBanky.View
{
    public partial class AdminMainView : UserControl, INotifyPropertyChanged
    {
        private string _greeting;
        private DispatcherTimer _timer;

        public string Greeting
        {
            get => _greeting;
            set
            {
                _greeting = value;
                OnPropertyChanged(nameof(Greeting));
            }
        }

        public AdminMainView()
        {
            InitializeComponent();
            DataContext = this; // Nastavení DataContext

            SetGreeting(); 
            StartClock();  // Spuštění hodin
        }

        private void SetGreeting()
        {
            int hour = DateTime.Now.Hour;

            if (hour < 12)
                Greeting = "Dobré ráno";
            else if (hour < 18)
                Greeting = "Dobré odpoledne";
            else
                Greeting = "Dobrý večer";
        }

        private void StartClock()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1); // Nastavení intervalu na 1 sekundu
            _timer.Tick += UpdateClock; // Při každém tiknutí timeru aktualizovat hodiny
            _timer.Start(); // Spuštění timeru

            // Nastavení počátečního času tak, aby hodiny nebyly prázdné
            UpdateClock(this, null);
        }

        private void UpdateClock(object sender, EventArgs e)
        {
            // Aktualizace textu hodin každou sekundu
            ClockTextBlock.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}