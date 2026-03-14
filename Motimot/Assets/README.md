# Assets layout

Folder structure for the word-of-the-day game.

| Folder | Purpose |
|--------|---------|
| **Scripts/Runtime** | Game logic, data model, and runtime behaviour (builds into the game). |
| **Scripts/Editor** | Editor-only tools and utilities (e.g. dictionary validation, debug menus). |
| **Data** | Word lists, config files, and other non-code data (e.g. dictionary text/JSON). |
| **Prefabs** | Reusable Unity prefabs (tiles, rows, keyboard, etc.). |
| **Scenes** | Game scenes (e.g. main game, menus). |
| **Settings** | URP and other project settings (existing). |

Existing top-level assets (e.g. `DefaultVolumeProfile`, `InputSystem_Actions`) stay at root or in Settings as needed.

---

## Entry point — main game scene

**Main game scene:** `Assets/Scenes/Main.unity`

This scene is the default entry point for the word-of-the-day game. All core gameplay (grid, input, UI) will live in this scene unless a separate menu or loading scene is added later.

**Build Settings:** `Scenes/Main` is listed first (index 0) in `ProjectSettings/EditorBuildSettings.asset`, so it loads on Play and in builds. `SampleScene` remains in the build at index 1 for reference; remove it from Build Settings if not needed.
