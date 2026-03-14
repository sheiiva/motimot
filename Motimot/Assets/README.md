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
