
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using FlashCards.Models;

public class MainViewModel : ViewModelBase
{
    public ObservableCollection<Flashcard> Flashcards { get; } = new();
    public string NewQuestion { get; set; }
    public string NewAnswer { get; set; }

    

    // Nowe w³aœciwoœci do nauki:
    private LearningSessionViewModel _learningSession;
    public LearningSessionViewModel LearningSession
    {
        get => _learningSession;
        private set
        {
            _learningSession = value;
            OnPropertyChanged();
        }
    }

    private bool _isLearningActive;
    public bool IsLearningActive
    {
        get => _isLearningActive;
        set
        {
            if (_isLearningActive != value)
            {
                _isLearningActive = value;
                OnPropertyChanged();
            }
        }
    }

    // Dodaj metody do startu nauki
    public void StartLearning()
    {
        if (Flashcards.Count == 0)
            return;

        LearningSession = new LearningSessionViewModel(Flashcards);
        IsLearningActive = true;
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
