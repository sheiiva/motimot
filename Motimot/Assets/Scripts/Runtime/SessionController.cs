using System;
using System.Collections.Generic;

namespace Motimot
{
    /// <summary>
    /// Controls a game session: handles input (letter, backspace, submit), validation, and state updates.
    /// </summary>
    public sealed class SessionController
    {
        private readonly WordListLoader _wordListLoader;
        private GameState _state;

        /// <summary>Current game state. Updated when input is processed.</summary>
        public GameState State => _state;

        /// <summary>Raised when state changes (5.4). Subscribe from presentation layer to update grid, keyboard, win/lose UI.</summary>
        public event Action<GameState> OnStateChanged;

        public SessionController(WordListLoader wordListLoader, string hiddenWord)
        {
            _wordListLoader = wordListLoader ?? new WordListLoader();
            _state = new GameState(hiddenWord ?? string.Empty, new List<Row>(), string.Empty, GamePhase.InProgress);
        }

        /// <summary>
        /// Adds a letter to the current row (1.1). Only when session is in progress and row has space.
        /// </summary>
        /// <returns>True if the letter was added; false if ignored (game over or row full).</returns>
        public bool AddLetter(char letter)
        {
            if (_state.Phase != GamePhase.InProgress)
            {
                return false;
            }

            if (_state.CurrentRowLetters.Length >= GameConstants.WordLength)
            {
                return false;
            }

            string newRow = _state.CurrentRowLetters + char.ToLowerInvariant(letter);
            _state = new GameState(_state.HiddenWord, _state.Attempts, newRow, _state.Phase);
            OnStateChanged?.Invoke(_state);
            return true;
        }

        /// <summary>
        /// Removes the last letter from the current row (1.2). Only when session is in progress and row has at least one letter.
        /// </summary>
        /// <returns>True if a letter was removed; false if ignored (game over or row empty).</returns>
        public bool Backspace()
        {
            if (_state.Phase != GamePhase.InProgress)
            {
                return false;
            }

            if (_state.CurrentRowLetters.Length == 0)
            {
                return false;
            }

            string newRow = _state.CurrentRowLetters.Substring(0, _state.CurrentRowLetters.Length - 1);
            _state = new GameState(_state.HiddenWord, _state.Attempts, newRow, _state.Phase);
            return true;
        }

        /// <summary>
        /// Submits the current row (1.3). Triggers validation and feedback when the row is complete.
        /// Only when Phase is InProgress, row length equals WordLength, and word is valid.
        /// </summary>
        public SubmitResult Submit()
        {
            if (_state.Phase != GamePhase.InProgress)
            {
                return SubmitResult.IgnoredGameOver;
            }

            if (_state.CurrentRowLetters.Length != GameConstants.WordLength)
            {
                return SubmitResult.RowIncomplete;
            }

            string guess = _state.CurrentRowLetters.Trim().ToLowerInvariant();
            if (!_wordListLoader.IsValidWord(guess))
            {
                return SubmitResult.InvalidWord;
            }

            Tile[] tiles = FeedbackComputer.Compute(guess, _state.HiddenWord);
            Row row = new Row(tiles);
            var newAttempts = new List<Row>(_state.Attempts) { row };

            GamePhase newPhase = _state.Phase;
            if (guess == _state.HiddenWord.ToLowerInvariant())
            {
                newPhase = GamePhase.Won;
            }
            else if (newAttempts.Count >= GameConstants.MaxAttempts)
            {
                newPhase = GamePhase.Lost;
            }

            _state = new GameState(_state.HiddenWord, newAttempts, string.Empty, newPhase);
            return SubmitResult.Accepted;
        }
    }
}
