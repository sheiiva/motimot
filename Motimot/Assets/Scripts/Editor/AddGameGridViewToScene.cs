using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using Motimot;
using UnityEngine.UI;

namespace Motimot.Editor
{
    /// <summary>
    /// Editor menu: Add GameGridView, Bootstrap, and wiring to the active scene.
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

        [MenuItem("Tools/Motimot/Add Game Bootstrap")]
        public static void AddBootstrap()
        {
            var bootstrap = Object.FindObjectOfType<GameBootstrap>();
            if (bootstrap != null)
            {
                Debug.Log("GameBootstrap already exists in scene.");
                Selection.activeGameObject = bootstrap.gameObject;
                return;
            }

            var go = new GameObject("GameBootstrap");
            var comp = go.AddComponent<GameBootstrap>();
            go.AddComponent<KeyboardInputBridge>();

            var grid = Object.FindObjectOfType<GameGridView>();
            if (grid != null)
            {
                var so = new SerializedObject(comp);
                so.FindProperty("_gameGridView").objectReferenceValue = grid;
                so.FindProperty("_keyboardInputBridge").objectReferenceValue = go.GetComponent<KeyboardInputBridge>();
                var fallback = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Data/words-fallback.txt");
                if (fallback != null)
                    so.FindProperty("_fallbackWordList").objectReferenceValue = fallback;
                so.ApplyModifiedPropertiesWithoutUndo();
            }

            Undo.RegisterCreatedObjectUndo(go, "Add Game Bootstrap");
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            Selection.activeGameObject = go;
        }
    }
}
