# Assets layout

Project layout for the word-of-the-day game — use this as the reference when adding or finding code and assets.

**Folder structure**

| Folder | Purpose |
|--------|---------|
| **Scripts/Runtime** | Game logic, data model, and runtime behaviour; assembly **Motimot** (`Motimot.asmdef`). |
| **Scripts/Editor** | Editor-only tools and utilities; assembly **Motimot.Editor** (`Motimot.Editor.asmdef`), references Motimot. |
| **Data** | Word lists, config files, and other non-code data (e.g. dictionary text/JSON). |
| **Prefabs** | Reusable Unity prefabs (tiles, rows, keyboard, etc.). |
| **Scenes** | Game scenes (e.g. main game, menus). |
| **Settings** | URP and other project settings (existing). |

Existing top-level assets (e.g. `DefaultVolumeProfile`, `InputSystem_Actions`) stay at root or in Settings as needed.

---

## Where game logic lives

| Concern | Location | Notes |
|--------|----------|--------|
| **Core game state** | `Scripts/Runtime/` | Hidden word, attempts, current row, win/lose. Types like `GameState`, session controller. |
| **UI (logic & binding)** | `Scripts/Runtime/` | Presenters, input handling, updating grid/keyboard from state. Optionally a `Runtime/UI/` subfolder if it grows. |
| **UI (prefabs & hierarchy)** | `Prefabs/`, scene hierarchy | Tile, row, virtual keyboard prefabs; layout in `Scenes/Main.unity`. |
| **Data (dictionary)** | `Data/` for word lists and assets; `Scripts/Runtime/` for loading, validation, and daily-word selection | Dictionary file(s) in Data; C# that loads and validates lives in Runtime. |
| **Editor tools** | `Scripts/Editor/` | Dictionary validation, debug menus, any Editor-only utilities. |

---

## Entry point — main game scene

**Main game scene:** `Assets/Scenes/Main.unity`

This scene is the default entry point for the word-of-the-day game. All core gameplay (grid, input, UI) will live in this scene unless a separate menu or loading scene is added later.

**Build Settings:** `Scenes/Main` is listed first (index 0) in `ProjectSettings/EditorBuildSettings.asset`, so it loads on Play and in builds. `SampleScene` remains in the build at index 1 for reference; remove it from Build Settings if not needed.
