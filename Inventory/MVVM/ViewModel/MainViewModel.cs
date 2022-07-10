using Inventory.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.MVVM.ViewModel
{
    class MainViewModel: ObservableObject
    {
        public RelayCommand TShirtDetailsViewCommand{ get; set; }
        public RelayCommand StocksViewCommand { get; set; }
        public RelayCommand DeliveriesViewCommand { get; set; }
        public RelayCommand LogViewCommand { get; set; }
        public TShirtDetailsViewModel TShirtDetailsVM { get; set; }
        public StocksViewModel StocksVM { get; set; }
        public DeliveriesViewModel DeliveriesVM { get; set; }
        public LogViewModel LogVM { get; set; }
        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertChanged();
            }
        }

        public MainViewModel()
        {
            TShirtDetailsVM = new TShirtDetailsViewModel();
            StocksVM = new StocksViewModel();
            DeliveriesVM = new DeliveriesViewModel();
            LogVM = new LogViewModel();

            CurrentView = TShirtDetailsVM;

            TShirtDetailsViewCommand = new RelayCommand(o =>
            {
                CurrentView = TShirtDetailsVM;
            });

            StocksViewCommand = new RelayCommand(o =>
            {
                CurrentView = StocksVM;
            });

            DeliveriesViewCommand = new RelayCommand(o =>
            {
                CurrentView = DeliveriesVM;
            });

            LogViewCommand = new RelayCommand(o =>
            {
                CurrentView = LogVM;
            });
        }
    }
}
