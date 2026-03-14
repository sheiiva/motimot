using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Motimot
{
    /// <summary>
    /// Loads the word list from an open-source URL (or fallback text) and provides validation.
    /// Words are stored in memory; use <see cref="LoadFromText"/> for fallback when offline.
    /// </summary>
    public sealed class WordListLoader
    {
        private readonly HashSet<string> _wordSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly List<string> _wordList = new List<string>();

        /// <summary>Whether at least one load has succeeded and the list is usable.</summary>
        public bool IsLoaded => _wordList.Count > 0;

        /// <summary>Number of words currently loaded (for validation and daily word selection).</summary>
        public int WordCount => _wordList.Count;

        /// <summary>
        /// Load words from raw text (one word per line, UTF-8). Use for fallback or local asset.
        /// Only words of length <see cref="GameConstants.WordLength"/> are kept; leading/trailing whitespace is trimmed.
        /// </summary>
        public void LoadFromText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            _wordSet.Clear();
            _wordList.Clear();

            string[] lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                string word = line.Trim();
                if (word.Length != GameConstants.WordLength)
                {
                    continue;
                }

                string normalized = word.ToLowerInvariant();
                if (_wordSet.Add(normalized))
                {
                    _wordList.Add(normalized);
                }
            }
        }

        /// <summary>
        /// Coroutine: load words from <paramref name="url"/>. On success, fills the word list and calls <paramref name="onSuccess"/>; on failure, calls <paramref name="onError"/> with the error message.
        /// </summary>
        public System.Collections.IEnumerator LoadFromUrlCoroutine(string url, Action onSuccess, Action<string> onError)
        {
            if (string.IsNullOrEmpty(url))
            {
                onError?.Invoke("URL is null or empty.");
                yield break;
            }

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    onError?.Invoke(request.error ?? "Unknown error.");
                    yield break;
                }

                string text = request.downloadHandler?.text;
                if (string.IsNullOrEmpty(text))
                {
                    onError?.Invoke("Downloaded text was empty.");
                    yield break;
                }

                LoadFromText(text);
                onSuccess?.Invoke();
            }
        }

        /// <summary>
        /// Returns true if <paramref name="word"/> has length <see cref="GameConstants.WordLength"/> and is in the loaded word list (case-insensitive).
        /// </summary>
        public bool IsValidWord(string word)
        {
            if (word == null || word.Length != GameConstants.WordLength)
            {
                return false;
            }

            return _wordSet.Contains(word.Trim().ToLowerInvariant());
        }

        /// <summary>
        /// Returns the word at <paramref name="index"/> (for deterministic daily word selection). Valid only if 0 &lt;= index &lt; <see cref="WordCount"/>.
        /// </summary>
        public string GetWordByIndex(int index)
        {
            if (index < 0 || index >= _wordList.Count)
            {
                return null;
            }

            return _wordList[index];
        }
    }
}
