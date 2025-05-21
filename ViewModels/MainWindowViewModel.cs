using System;
using FlashCards.Models;

namespace FlashCards.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public ViewModelBase CurrentViewModel { get; set; }

        public MainWindowViewModel()
        {
            CurrentViewModel = new StartViewModel(); 
        }
    }
}
