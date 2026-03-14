using UnityEngine;
using UnityEngine.UI;

namespace Motimot
{
    /// <summary>
    /// Displays the guess grid: MaxAttempts rows × WordLength tiles (1.1).
    /// Tiles are indexed [row, col] with row 0 at top, col 0 at left.
    /// Add to a GameObject (adds Canvas automatically if needed).
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public sealed class GameGridView : MonoBehaviour
    {
        private SessionController _controller;

        [SerializeField] private float _cellSize = 64f;
        [SerializeField] private float _spacing = 8f;
        [SerializeField] private float _verticalMargin = 40f;
        [SerializeField] private Color _tileDefaultColor = new Color(0.22f, 0.22f, 0.22f);

        private GridLayoutGroup _gridLayout;
        private RectTransform _gridRootRect;
        private Image[][] _tileBackgrounds;
        private Text[][] _tileTexts;

        /// <summary>Tile [row, col]. Row 0 = first guess row, col 0 = first letter.</summary>
        public Image GetTileBackground(int row, int col) =>
            _tileBackgrounds != null && row >= 0 && row < GameConstants.MaxAttempts && col >= 0 && col < GameConstants.WordLength
                ? _tileBackgrounds[row][col]
                : null;

        /// <summary>Tile [row, col] text component for the letter.</summary>
        public Text GetTileText(int row, int col) =>
            _tileTexts != null && row >= 0 && row < GameConstants.MaxAttempts && col >= 0 && col < GameConstants.WordLength
                ? _tileTexts[row][col]
                : null;

        /// <summary>Binds to session controller and subscribes to state changes (1.2). Call from bootstrap.</summary>
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

        /// <summary>Unsubscribes from controller. Called automatically on destroy.</summary>
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

        /// <summary>Updates grid from game state. Called when state changes.</summary>
        public void RefreshFromState(GameState state)
        {
            if (state == null || _tileBackgrounds == null || _tileTexts == null) return;
            for (int row = 0; row < GameConstants.MaxAttempts; row++)
            {
                for (int col = 0; col < GameConstants.WordLength; col++)
                {
                    string letter = GetLetterForTile(state, row, col);
                    _tileTexts[row][col].text = letter;
                    _tileBackgrounds[row][col].color = _tileDefaultColor;
                }
            }
        }

        private static string GetLetterForTile(GameState state, int row, int col)
        {
            if (row < state.AttemptsCount)
            {
                var attempt = state.Attempts[row];
                if (col < attempt.Tiles.Count)
                    return char.ToUpperInvariant(attempt.Tiles[col].Letter).ToString();
            }
            else if (row == state.AttemptsCount && col < state.CurrentRowLetters.Length)
            {
                return char.ToUpperInvariant(state.CurrentRowLetters[col]).ToString();
            }
            return "";
        }

        private void Awake()
        {
            EnsureRootRectTransform();
            BuildGrid();
        }

        private void Start()
        {
            ScaleGridToFit();
        }

        private void ScaleGridToFit()
        {
            if (_gridRootRect == null) return;
            var rootRect = transform as RectTransform;
            if (rootRect == null) return;

            Canvas.ForceUpdateCanvases();
            var rootSize = rootRect.rect.size;
            var availableHeight = Mathf.Max(0, rootSize.y - 2f * _verticalMargin);
            var availableWidth = rootSize.x;
            if (availableHeight <= 0 || availableWidth <= 0) return;

            var gridSize = _gridRootRect.sizeDelta;
            var scaleX = availableWidth / gridSize.x;
            var scaleY = availableHeight / gridSize.y;
            var scale = Mathf.Min(scaleX, scaleY, 1f);
            _gridRootRect.localScale = new Vector3(scale, scale, 1f);
        }

        private void EnsureRootRectTransform()
        {
            var rect = transform as RectTransform;
            if (rect == null) return;
            if (rect.anchorMin != Vector2.zero || rect.anchorMax != Vector2.one)
            {
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
            }
        }

        private void BuildGrid()
        {
            var gridRoot = new GameObject("Grid");
            gridRoot.transform.SetParent(transform, false);

            _gridRootRect = gridRoot.AddComponent<RectTransform>();
            _gridRootRect.anchorMin = new Vector2(0.5f, 0.5f);
            _gridRootRect.anchorMax = new Vector2(0.5f, 0.5f);
            _gridRootRect.pivot = new Vector2(0.5f, 0.5f);
            _gridRootRect.anchoredPosition = Vector2.zero;
            _gridRootRect.sizeDelta = new Vector2(
                GameConstants.WordLength * _cellSize + (GameConstants.WordLength - 1) * _spacing,
                GameConstants.MaxAttempts * _cellSize + (GameConstants.MaxAttempts - 1) * _spacing);

            _gridLayout = gridRoot.AddComponent<GridLayoutGroup>();
            _gridLayout.cellSize = new Vector2(_cellSize, _cellSize);
            _gridLayout.spacing = new Vector2(_spacing, _spacing);
            _gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _gridLayout.constraintCount = GameConstants.WordLength;
            _gridLayout.childAlignment = TextAnchor.MiddleCenter;
            _gridLayout.startCorner = GridLayoutGroup.Corner.UpperLeft;
            _gridLayout.startAxis = GridLayoutGroup.Axis.Horizontal;

            _tileBackgrounds = new Image[GameConstants.MaxAttempts][];
            _tileTexts = new Text[GameConstants.MaxAttempts][];

            for (int row = 0; row < GameConstants.MaxAttempts; row++)
            {
                _tileBackgrounds[row] = new Image[GameConstants.WordLength];
                _tileTexts[row] = new Text[GameConstants.WordLength];

                for (int col = 0; col < GameConstants.WordLength; col++)
                {
                    var tile = CreateTile(row, col);
                    tile.transform.SetParent(gridRoot.transform, false);
                    _tileBackgrounds[row][col] = tile.GetComponent<Image>();
                    _tileTexts[row][col] = tile.GetComponentInChildren<Text>();
                }
            }
        }

        private GameObject CreateTile(int row, int col)
        {
            var tile = new GameObject($"Tile_{row}_{col}");
            var rect = tile.AddComponent<RectTransform>();

            var img = tile.AddComponent<Image>();
            img.color = _tileDefaultColor;

            var textObj = new GameObject("Letter");
            textObj.transform.SetParent(tile.transform, false);
            var textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            var text = textObj.AddComponent<Text>();
            text.text = "";
            text.alignment = TextAnchor.MiddleCenter;
            text.fontSize = 32;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.color = Color.white;

            return tile;
        }
    }
}
