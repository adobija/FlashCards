using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using Avalonia;

namespace FlashCards.Views
{
    public partial class StartView : UserControl
    {
        public StartView()
        {
            InitializeComponent();
            DataContext = new ViewModels.MainViewModel();
        }

        private void OnAddFlashcardClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.MainViewModel vm)
            {
                vm.AddFlashcard();
            }
        }

        private async void OnLoadFromJsonClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.MainViewModel vm)
            {
                var dialog = new OpenFileDialog()
                {
                    Filters = { new FileDialogFilter { Name = "JSON Files", Extensions = { "json" } } },
                    AllowMultiple = false
                };

                var result = await dialog.ShowAsync((Window)this.VisualRoot);
                if (result != null && result.Length > 0)
                {
                    vm.LoadFromJson(result[0]);
                }
            }
        }

        private void OnStartLearningClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.MainViewModel vm)
            {
                vm.StartLearning();

                if (this.VisualRoot is Window window &&
                    window.DataContext is ViewModels.MainWindowViewModel mainWindowVm)
                {
                    mainWindowVm.CurrentView = new LearningSessionView()
                    {
                        DataContext = vm.Session
                    };
                }
            }
        }
    }
}