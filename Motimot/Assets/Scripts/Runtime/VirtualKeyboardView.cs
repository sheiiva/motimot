using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Motimot
{
    /// <summary>
    /// Renders an on-screen QWERTY keyboard. Keys trigger AddLetter, Backspace, Submit.
    /// Key colors update from feedback: green > yellow > gray > default (2.1–2.3).
    /// </summary>
    public sealed class VirtualKeyboardView : MonoBehaviour
    {
        private SessionController _controller;

        [SerializeField] private float _keyHeight = 42f;
        [SerializeField] private float _keySpacing = 6f;
        [SerializeField] private float _rowSpacing = 8f;
        [SerializeField] private Color _keyDefaultColor = new Color(0.35f, 0.35f, 0.38f);
        [SerializeField] private Color _correctPositionColor = new Color(0.4f, 0.62f, 0.35f);
        [SerializeField] private Color _wrongPositionColor = new Color(0.78f, 0.68f, 0.24f);
        [SerializeField] private Color _absentColor = new Color(0.4f, 0.4f, 0.4f);

        private readonly Dictionary<char, Image> _letterKeyImages = new Dictionary<char, Image>();
        private RectTransform _keyboardRootRect;

        private static readonly string[] Row1 = { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P" };
        private static readonly string[] Row2 = { "A", "S", "D", "F", "G", "H", "J", "K", "L" };
        private static readonly string[] Row3 = { "Z", "X", "C", "V", "B", "N", "M" };

        /// <summary>Binds to session controller. Call from bootstrap.</summary>
        public void Bind(SessionController controller)
        {
            Unbind();
            _controller = controller;
            if (_controller != null)
            {
                _controller.OnStateChanged += RefreshFromState;
                RefreshFromState(_controller.State);
            }
        }

        public void Unbind()
        {
            if (_controller != null)
            {
                _controller.OnStateChanged -= RefreshFromState;
                _controller = null;
            }
        }

        private void OnDestroy()
        {
            Unbind();
        }

        private void Awake()
        {
            BuildKeyboard();
        }

        private void Start()
        {
            ScaleKeyboardToFit();
        }

        private void ScaleKeyboardToFit()
        {
            if (_keyboardRootRect == null) return;
            var parentRect = transform as RectTransform;
            if (parentRect == null) return;

            Canvas.ForceUpdateCanvases();
            var parentWidth = parentRect.rect.width;
            var keyboardWidth = 10f * _keyHeight + 9f * _keySpacing + 24f; // widest row + padding
            if (parentWidth <= 0 || keyboardWidth <= 0) return;

            var scale = Mathf.Min(parentWidth / keyboardWidth, 1f);
            _keyboardRootRect.localScale = new Vector3(scale, scale, 1f);
        }

        private void BuildKeyboard()
        {
            var root = new GameObject("Keyboard");
            root.transform.SetParent(transform, false);

            _keyboardRootRect = root.AddComponent<RectTransform>();
            _keyboardRootRect.anchorMin = new Vector2(0f, 0f);
            _keyboardRootRect.anchorMax = new Vector2(1f, 0f);
            _keyboardRootRect.pivot = new Vector2(0.5f, 0f);
            _keyboardRootRect.anchoredPosition = Vector2.zero;
            _keyboardRootRect.sizeDelta = new Vector2(0f, 180f);

            var vLayout = root.AddComponent<VerticalLayoutGroup>();
            vLayout.spacing = _rowSpacing;
            vLayout.childAlignment = TextAnchor.MiddleCenter;
            vLayout.childControlWidth = true;
            vLayout.childControlHeight = true;
            vLayout.childForceExpandWidth = false;
            vLayout.childForceExpandHeight = false;
            vLayout.padding = new RectOffset(12, 12, 12, 24);

            CreateRow(root.transform, Row1);
            CreateRow(root.transform, Row2);
            CreateRowWithSpecialKeys(root.transform, Row3);
        }

        private void CreateRow(Transform parent, string[] letters)
        {
            var row = CreateRowHost(parent);
            foreach (string letter in letters)
            {
                char c = letter.ToLowerInvariant()[0];
                CreateLetterKey(row.transform, c);
            }
        }

        private void CreateRowWithSpecialKeys(Transform parent, string[] letters)
        {
            var row = CreateRowHost(parent);
            CreateActionKey(row.transform, "ENTER", () => _controller?.Submit(), 1.5f);
            foreach (string letter in letters)
            {
                char c = letter.ToLowerInvariant()[0];
                CreateLetterKey(row.transform, c);
            }
            CreateActionKey(row.transform, "⌫", () => _controller?.Backspace(), 1.5f);
        }

        private GameObject CreateRowHost(Transform parent)
        {
            var row = new GameObject("Row");
            row.transform.SetParent(parent, false);
            var hLayout = row.AddComponent<HorizontalLayoutGroup>();
            hLayout.spacing = _keySpacing;
            hLayout.childAlignment = TextAnchor.MiddleCenter;
            hLayout.childControlWidth = false;
            hLayout.childControlHeight = true;
            hLayout.childForceExpandWidth = false;
            hLayout.childForceExpandHeight = true;
            return row;
        }

        private void CreateLetterKey(Transform parent, char letter)
        {
            var key = CreateKeyButton(parent, letter.ToString().ToUpperInvariant(), 1f);
            key.onClick.AddListener(() =>
            {
                if (_controller != null)
                    _controller.AddLetter(letter);
            });
            var img = key.GetComponent<Image>();
            _letterKeyImages[letter] = img;
        }

        private void CreateActionKey(Transform parent, string label, UnityEngine.Events.UnityAction action, float widthMultiplier)
        {
            var key = CreateKeyButton(parent, label, widthMultiplier);
            key.onClick.AddListener(action);
        }

        private Button CreateKeyButton(Transform parent, string label, float widthMultiplier)
        {
            var go = new GameObject($"Key_{label}");
            go.transform.SetParent(parent, false);

            var layoutElement = go.AddComponent<LayoutElement>();
            layoutElement.minWidth = _keyHeight * widthMultiplier;
            layoutElement.preferredWidth = _keyHeight * widthMultiplier;
            layoutElement.preferredHeight = _keyHeight;

            var img = go.AddComponent<Image>();
            img.color = _keyDefaultColor;

            var button = go.AddComponent<Button>();

            var textObj = new GameObject("Text");
            textObj.transform.SetParent(go.transform, false);
            var textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            var text = textObj.AddComponent<Text>();
            text.text = label;
            text.alignment = TextAnchor.MiddleCenter;
            text.fontSize = 18;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.color = Color.white;

            return button;
        }

        private void RefreshFromState(GameState state)
        {
            if (state == null) return;

            var bestFeedback = ComputeBestFeedbackPerLetter(state);
            foreach (var kv in _letterKeyImages)
            {
                char c = kv.Key;
                var img = kv.Value;
                img.color = bestFeedback.TryGetValue(c, out var fb)
                    ? FeedbackToColor(fb)
                    : _keyDefaultColor;
            }
        }

        private static Dictionary<char, LetterFeedback> ComputeBestFeedbackPerLetter(GameState state)
        {
            var best = new Dictionary<char, LetterFeedback>();
            if (state.Attempts == null) return best;

            foreach (var row in state.Attempts)
            {
                foreach (var tile in row.Tiles)
                {
                    char c = char.ToLowerInvariant(tile.Letter);
                    if (!best.TryGetValue(c, out var current) || IsBetter(tile.Feedback, current))
                        best[c] = tile.Feedback;
                }
            }
            return best;
        }

        private static bool IsBetter(LetterFeedback a, LetterFeedback b)
        {
            if (a == LetterFeedback.CorrectPosition) return true;
            if (b == LetterFeedback.CorrectPosition) return false;
            if (a == LetterFeedback.WrongPosition) return true;
            if (b == LetterFeedback.WrongPosition) return false;
            return a == LetterFeedback.Absent && b != LetterFeedback.Absent;
        }

        private Color FeedbackToColor(LetterFeedback feedback)
        {
            return feedback switch
            {
                LetterFeedback.CorrectPosition => _correctPositionColor,
                LetterFeedback.WrongPosition => _wrongPositionColor,
                LetterFeedback.Absent => _absentColor,
                _ => _keyDefaultColor
            };
        }
    }
}
