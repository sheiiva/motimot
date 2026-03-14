namespace Motimot
{
    /// <summary>
    /// Constants for the word-of-the-day game (standard mode).
    /// </summary>
    public static class GameConstants
    {
        /// <summary>Length of the hidden word and each guess (letters).</summary>
        public const int WordLength = 5;

        /// <summary>Maximum number of guesses per session.</summary>
        public const int MaxAttempts = 6;

        /// <summary>Default open-source word list URL (one word per line, UTF-8). Used when loading the dictionary from the network.</summary>
        public const string DefaultWordListUrl = "https://raw.githubusercontent.com/shawon-majid/WordleCracker/main/fiveLetterWords.txt";
    }
}
