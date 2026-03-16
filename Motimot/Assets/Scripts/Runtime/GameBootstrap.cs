using UnityEngine;

namespace Motimot
{
    /// <summary>
    /// Bootstrap: load word list (URL or fallback), create session, wire KeyboardInputBridge and GameGridView (3.1, 3.2).
    /// Add to a GameObject in the Main scene. Assign fallback WordList if you want offline support.
    /// </summary>
    public sealed class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private TextAsset _fallbackWordList;
        [SerializeField] private KeyboardInputBridge _keyboardInputBridge;
        [SerializeField] private GameGridView _gameGridView;
        [SerializeField] private VirtualKeyboardView _virtualKeyboard;

        private WordListLoader _loader;
        private SessionController _controller;

        private void Start()
        {
            _loader = new WordListLoader();
            StartCoroutine(LoadAndStart());
        }

        private System.Collections.IEnumerator LoadAndStart()
        {
            yield return _loader.LoadFromUrlCoroutine(
                GameConstants.DefaultWordListUrl,
                OnLoadSuccess,
                OnLoadError);

            if (_loader.IsLoaded)
                StartSession();
            else
                TryFallbackAndStart();
        }

        private void OnLoadSuccess()
        {
            // Handled in LoadAndStart
        }

        private void OnLoadError(string error)
        {
            Debug.LogWarning($"[GameBootstrap] URL load failed: {error}. Trying fallback.");
        }

        private void TryFallbackAndStart()
        {
            if (_fallbackWordList != null)
            {
                _loader.LoadFromTextAsset(_fallbackWordList);
                if (_loader.IsLoaded)
                {
                    StartSession();
                    return;
                }
            }
            Debug.LogError("[GameBootstrap] No word list loaded. Assign a fallback TextAsset in the Inspector.");
        }

        private void StartSession()
        {
            _controller = SessionStarter.StartSession(_loader);

            if (_keyboardInputBridge != null)
                _keyboardInputBridge.Controller = _controller;

            if (_gameGridView != null)
                _gameGridView.Bind(_controller);

            if (_virtualKeyboard != null)
                _virtualKeyboard.Bind(_controller);
        }
    }
}
