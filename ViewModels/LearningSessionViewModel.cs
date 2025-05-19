
using FlashCards.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class LearningSessionViewModel : ViewModelBase
{
    private Stack<Flashcard> activeStack;
    private List<Flashcard> correct;
    private List<Flashcard> incorrect;
    private Random rand = new();

    public Flashcard Current { get; private set; }
    public bool IsAnswerVisible { get; private set; }

    public IReadOnlyList<Flashcard> IncorrectFlashcards => incorrect;

    public LearningSessionViewModel(IEnumerable<Flashcard> flashcards)
    {
        activeStack = new Stack<Flashcard>(flashcards.OrderBy(_ => rand.Next()));
        correct = new();
        incorrect = new();
        NextCard();
    }

    public void Reveal() => IsAnswerVisible = true;

    public void MarkCorrect()
    {
        if (Current != null)
            correct.Add(Current);
        NextCard();
    }

    public void MarkIncorrect()
    {
        if (Current != null)
            incorrect.Add(Current);
        NextCard();
    }

    private void NextCard()
    {
        IsAnswerVisible = false;

        if (activeStack.Count == 0 && incorrect.Count > 0)
        {
            activeStack = new Stack<Flashcard>(incorrect.OrderBy(_ => rand.Next()));
            incorrect.Clear();
        }

        Current = activeStack.Count > 0 ? activeStack.Pop() : null;
        OnPropertyChanged(nameof(Current));
        OnPropertyChanged(nameof(IsAnswerVisible));
    }

    public bool IsFinished => Current == null && incorrect.Count == 0;
    public int CorrectCount => correct.Count;
    public int IncorrectCount => incorrect.Count;

    public void ExportIncorrect(string path)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(IncorrectFlashcards);
        File.WriteAllText(path, json);
    }
}
