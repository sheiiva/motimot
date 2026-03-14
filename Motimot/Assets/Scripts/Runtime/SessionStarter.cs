using System;

namespace Motimot
{
    /// <summary>
    /// Creates a new session: gets daily word and builds initial GameState (5.2).
    /// </summary>
    public static class SessionStarter
    {
        /// <summary>
        /// Starts a new session. Loader must already be loaded (call LoadFromUrlCoroutine or LoadFromText first).
        /// </summary>
        /// <param name="loader">Loaded word list (e.g. from URL or fallback).</param>
        /// <param name="dateForWord">Date for daily word; defaults to today (UTC).</param>
        /// <returns>New SessionController with empty attempts, empty current row, Phase.InProgress.</returns>
        public static SessionController StartSession(WordListLoader loader, DateTime? dateForWord = null)
        {
            if (loader == null)
            {
                throw new ArgumentNullException(nameof(loader));
            }

            DateTime date = dateForWord ?? DateTime.UtcNow.Date;
            string hiddenWord = loader.GetDailyWord(date) ?? "apple";
            return new SessionController(loader, hiddenWord);
        }
    }
}
