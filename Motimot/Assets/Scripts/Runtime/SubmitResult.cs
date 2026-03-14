namespace Motimot
{
    /// <summary>Result of a submit attempt.</summary>
    public enum SubmitResult
    {
        /// <summary>Submit was accepted; state was updated (new row added, possibly won/lost).</summary>
        Accepted,

        /// <summary>Session is over (won or lost); input ignored.</summary>
        IgnoredGameOver,

        /// <summary>Current row is not complete (length &lt; WordLength).</summary>
        RowIncomplete,

        /// <summary>Current row spells a word not in the dictionary.</summary>
        InvalidWord
    }
}
