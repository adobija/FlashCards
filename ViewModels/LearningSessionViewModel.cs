using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FlashCards.Models;

namespace FlashCards.ViewModels
{
    public class LearningSessionViewModel : ViewModelBase
    {
        private Stack<Flashcard> _activeStack;
        private List<Flashcard> _incorrect;
        private Random _rand = new();

        private Flashcard? _current;
        public Flashcard? Current
        {
            get => _current;
            private set
            {
                _current = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsFinished));
            }
        }

        private bool _isAnswerVisible;
        public bool IsAnswerVisible
        {
            get => _isAnswerVisible;
            private set
            {
                _isAnswerVisible = value;
                OnPropertyChanged();
            }
        }

        public int CorrectCount { get; private set; }
        public int IncorrectCount => _incorrect.Count;
        public IEnumerable<Flashcard> IncorrectFlashcards => _incorrect;
        public bool IsFinished => Current == null;

        public LearningSessionViewModel(IEnumerable<Flashcard> flashcards)
        {
            // Enhanced randomization with Fisher-Yates shuffle
            var shuffledCards = flashcards.ToList();
            for (int i = shuffledCards.Count - 1; i > 0; i--)
            {
                int j = _rand.Next(i + 1);
                var temp = shuffledCards[i];
                shuffledCards[i] = shuffledCards[j];
                shuffledCards[j] = temp;
            }

            _activeStack = new Stack<Flashcard>(shuffledCards);
            _incorrect = new List<Flashcard>();
            NextCard();
        }

        public void Reveal()
        {
            IsAnswerVisible = true;
        }

        public void MarkCorrect()
        {
            CorrectCount++;
            OnPropertyChanged(nameof(CorrectCount));
            NextCard();
        }

        public void MarkIncorrect()
        {
            if (Current != null)
                _incorrect.Add(Current);
            OnPropertyChanged(nameof(IncorrectCount));
            NextCard();
        }

        private void NextCard()
        {
            IsAnswerVisible = false;

            if (_activeStack.Count == 0 && _incorrect.Count > 0)
            {
                // Shuffle incorrect cards before re-adding them
                var shuffledIncorrect = _incorrect.OrderBy(_ => _rand.Next()).ToList();
                _activeStack = new Stack<Flashcard>(shuffledIncorrect);
                _incorrect.Clear();
                OnPropertyChanged(nameof(IncorrectCount));
            }

            Current = _activeStack.Count > 0 ? _activeStack.Pop() : null;
        }


    }
}