using FlashCards.Models;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text.Json;

namespace FlashCards.ViewModels
{
    public class StartViewModel : ViewModelBase
    {
        public ObservableCollection<Flashcard> Flashcards { get; } = new();
        public ReactiveCommand<Unit, Unit> AddFlashcardCommand { get; }
        private string _newQuestion;
        public string NewQuestion
        {
            get => _newQuestion;
            set
            {
                if (_newQuestion != value)
                {
                    _newQuestion = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _newAnswer;
        public string NewAnswer
        {
            get => _newAnswer;
            set
            {
                if (_newAnswer != value)
                {
                    _newAnswer = value;
                    OnPropertyChanged();
                }
            }
        }


        public void AddFlashcard()
        {
            if (!string.IsNullOrWhiteSpace(NewQuestion) && !string.IsNullOrWhiteSpace(NewAnswer))
            {
                Flashcards.Add(new Flashcard { Question = NewQuestion, Answer = NewAnswer });
                NewQuestion = string.Empty;
                NewAnswer = string.Empty;
                OnPropertyChanged(nameof(NewQuestion));
                OnPropertyChanged(nameof(NewAnswer));
            }
        }

        public void LoadFromJson(string path)
        {
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var loaded = JsonSerializer.Deserialize<List<Flashcard>>(json);
                if (loaded != null)
                {
                    Flashcards.Clear();
                    foreach (var card in loaded)
                        Flashcards.Add(card);
                }
            }
        }

        public void SaveToJson(string path)
        {
            var json = JsonSerializer.Serialize(Flashcards.ToList());
            File.WriteAllText(path, json);
        }
    }
}
