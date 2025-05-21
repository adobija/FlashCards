using Avalonia.Controls;
using Avalonia.Interactivity;
using FlashCards.ViewModels;
using System;
using System.Collections.Generic;

namespace FlashCards.Views
{
    public partial class StartView : UserControl
    {
        public StartView()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
        private async void OnLoadFromFileClick(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog()
            {
                Filters = new List<FileDialogFilter>()
        {
            new FileDialogFilter() { Name = "JSON Files", Extensions = { "json" } }
        }
            };
            var result = await dlg.ShowAsync((Window)this.VisualRoot);
            if (result != null && result.Length > 0)
            {
                if (DataContext is MainViewModel vm)
                {
                    vm.LoadFromJson(result[0]);
                }
            }
        }

        private void OnAddFlashcardClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.AddFlashcard();
            }
        }

        private async void LoadFromFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filters = { new FileDialogFilter { Name = "JSON Files", Extensions = { "json" } } }
            };

            var result = await dialog.ShowAsync((Window)this.VisualRoot);
            if (result != null && result.Length > 0)
            {
                (DataContext as StartViewModel)?.LoadFromJson(result[0]);
            }
        }

        private void AddFlashcard_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as StartViewModel)?.AddFlashcard();
        }

        private void StartLearning_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
