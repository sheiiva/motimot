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
    }
}
