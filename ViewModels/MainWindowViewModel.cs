using FlashCards.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FlashCards.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private object? _currentView;
        public object? CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            var mainViewModel = new MainViewModel();
            CurrentView = new StartView { DataContext = mainViewModel };
        }
    }
}