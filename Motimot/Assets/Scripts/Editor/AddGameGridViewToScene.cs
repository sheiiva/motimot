using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using Motimot;
using UnityEngine.UI;

namespace Motimot.Editor
{
    /// <summary>
    /// Editor menu: Add GameGridView (Canvas + grid) to the active scene.
    /// Use Tools → Motimot → Add GameGridView to Scene.
    /// </summary>
    public static class AddGameGridViewToScene
    {
        [MenuItem("Tools/Motimot/Add GameGridView to Scene")]
        public static void Execute()
        {
            var go = new GameObject("GameGridView", typeof(RectTransform));
            var rootRect = go.GetComponent<RectTransform>();
            rootRect.anchorMin = Vector2.zero;
            rootRect.anchorMax = Vector2.one;
            rootRect.offsetMin = Vector2.zero;
            rootRect.offsetMax = Vector2.zero;

            go.AddComponent<Canvas>();
            go.AddComponent<CanvasScaler>();
            go.AddComponent<GraphicRaycaster>();
            go.AddComponent<GameGridView>();

            var canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            var scaler = go.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(400, 600);
            scaler.matchWidthOrHeight = 0.5f;

            Undo.RegisterCreatedObjectUndo(go, "Add GameGridView");
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
