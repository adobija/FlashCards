using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.IO;
using System.Text.Json;
using Avalonia;
using System.Linq;

namespace FlashCards.Views
{
    public partial class LearningSessionView : UserControl
    {
        public LearningSessionView()
        {
            InitializeComponent();
        }

        private void OnRevealClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.LearningSessionViewModel vm)
            {
                vm.Reveal();
            }
        }

        private void OnCorrectClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.LearningSessionViewModel vm)
            {
                vm.MarkCorrect();
            }
        }

        private void OnIncorrectClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.LearningSessionViewModel vm)
            {
                vm.MarkIncorrect();
            }
        }

        private void OnContinueClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.LearningSessionViewModel vm && vm.IncorrectCount > 0)
            {
                // Shuffle incorrect cards before continuing
                var shuffledIncorrect = vm.IncorrectFlashcards.OrderBy(_ => new Random().Next()).ToList();
                var newSession = new ViewModels.LearningSessionViewModel(shuffledIncorrect);

                if (this.VisualRoot is Window window &&
                    window.DataContext is ViewModels.MainWindowViewModel mainWindowVm)
                {
                    mainWindowVm.CurrentView = new LearningSessionView()
                    {
                        DataContext = newSession
                    };
                }
            }
        }

        private async void OnExportClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.LearningSessionViewModel vm)
            {
                var dialog = new SaveFileDialog()
                {
                    Title = "Export Incorrect Flashcards",
                    Filters = { new FileDialogFilter { Name = "JSON Files", Extensions = { "json" } } },
                    DefaultExtension = "json"
                };

                var result = await dialog.ShowAsync((Window)this.VisualRoot);
                if (result != null)
                {
                    var json = JsonSerializer.Serialize(vm.IncorrectFlashcards);
                    await File.WriteAllTextAsync(result, json);
                }
            }
        }

        private void OnEndSessionClick(object sender, RoutedEventArgs e)
        {
            if (this.VisualRoot is Window window &&
                window.DataContext is ViewModels.MainWindowViewModel mainWindowVm)
            {
                mainWindowVm.CurrentView = new StartView()
                {
                    DataContext = new ViewModels.MainViewModel()
                };
            }
        }
    }
}