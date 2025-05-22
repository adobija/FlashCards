using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FlashCards.Models;
using System.Collections.Generic;

namespace FlashCards.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private LearningSessionViewModel? _session;
        public LearningSessionViewModel? Session
        {
            get => _session;
            set
            {
                _session = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsInLearningMode));
            }
        }

        public bool IsInLearningMode => Session != null;

        public ObservableCollection<Flashcard> Flashcards { get; } = new();

        private string? _newQuestion;
        public string? NewQuestion
        {
            get => _newQuestion;
            set
            {
                _newQuestion = value;
                OnPropertyChanged();
            }
        }

        private string? _newAnswer;
        public string? NewAnswer
        {
            get => _newAnswer;
            set
            {
                _newAnswer = value;
                OnPropertyChanged();
            }
        }

        public void AddFlashcard()
        {
            if (!string.IsNullOrWhiteSpace(NewQuestion) && !string.IsNullOrWhiteSpace(NewAnswer))
            {
                Flashcards.Add(new Flashcard { Question = NewQuestion!, Answer = NewAnswer! });
                NewQuestion = string.Empty;
                NewAnswer = string.Empty;
            }
        }

        public void LoadFromJson(string filePath)
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var loaded = JsonSerializer.Deserialize<List<Flashcard>>(json);
                if (loaded != null)
                {
                    Flashcards.Clear();
                    foreach (var card in loaded)
                        Flashcards.Add(card);
                }
            }
        }

        public void StartLearning()
        {
            if (Flashcards.Any())
            {
                Session = new LearningSessionViewModel(Flashcards.ToList());
            }
        }
    }
}