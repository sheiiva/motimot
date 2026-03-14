# Coding style and assembly layout

Canonical reference for C# in this Unity project. Follow these rules in all game and editor code.

---

## 1. Namespaces

- **Runtime (game code):** `Motimot` for game logic, data model, and runtime behaviour.
- **Editor (tools, debug):** `Motimot.Editor` for editor-only scripts.
- Use one namespace per file; match the folder hierarchy when splitting (e.g. `Motimot.Data` if a `Data` subfolder is added under `Scripts/Runtime`).

**Example**

```csharp
namespace Motimot
{
    public sealed class GameState { }
}
```

```csharp
namespace Motimot.Editor
{
    public sealed class DictionaryValidator : UnityEditor.Editor { }
}
```

---

## 2. Naming

| Element | Convention | Example |
|--------|------------|--------|
| Types (class, struct, enum, interface, delegate) | PascalCase | `GameState`, `LetterFeedback`, `IWordProvider` |
| Public / protected / internal members | PascalCase | `HiddenWord`, `MaxAttempts`, `SubmitGuess()` |
| Private fields | camelCase (optionally `_` prefix) | `_dictionary` or `currentRow` |
| Parameters and locals | camelCase | `word`, `attemptIndex` |
| Constants (field or local) | PascalCase | `WordLength`, `MaxAttempts` |
| Static readonly | PascalCase | `DefaultWordLength` |

- **Interfaces:** prefix with `I` (PascalCase). Example: `IWordProvider`.
- **Enums:** PascalCase for type and values. Example: `LetterFeedback { CorrectPosition, WrongPosition, Absent }`.
- **Async methods:** suffix with `Async`. Example: `LoadDictionaryAsync()`.

---

## 3. File and type layout

- **One main type per file;** file name = type name (e.g. `GameState.cs` for `class GameState`).
- Order inside a file: fields → constructors → public API → private/static helpers.
- Prefer `sealed` for classes that are not base types.
- Prefer `readonly` for fields that are set only in constructor or init.

---

## 4. Style preferences

- **Braces:** always use `{ }` for blocks (including single-line `if`).
- **Indentation:** 4 spaces.
- **Line length:** keep under ~120 characters; wrap and indent logically.
- **Nulls:** use nullable reference types where useful; avoid unnecessary null checks when the type guarantees non-null.
- **Unity:** use `[SerializeField]` for inspector-only fields; avoid public fields for serialized data when a property or method is preferable for API.

---

## 5. Assembly layout

| Location | Assembly | Purpose |
|----------|----------|--------|
| `Assets/Scripts/Runtime/` | **Motimot** | Game logic, data model, dictionary, everything that runs in builds. |
| `Assets/Scripts/Editor/` | **Motimot.Editor** | Editor-only tools (e.g. dictionary validation, debug menus). References `Motimot`. |

- **Runtime:** `Motimot.asmdef` in `Scripts/Runtime/`. No platform restriction (included in all builds).
- **Editor:** `Motimot.Editor.asmdef` in `Scripts/Editor/`. `includePlatforms: ["Editor"]`. References `Motimot`.

Scripts in `Scripts/Runtime/` must not depend on UnityEditor. Scripts in `Scripts/Editor/` may use UnityEditor and `Motimot`.

---

## 6. Runtime state storage

Session state (hidden word, attempts, current row, phase) is stored in **plain C# types** only:

- **GameState**, **Row**, **Tile**, **LetterFeedback**, **GamePhase** — all in `Motimot` namespace under `Scripts/Runtime/`.
- **No ScriptableObject** for game/session state: ScriptableObjects are for project assets (e.g. config, word-list references), not for per-session data.
- **Immutable snapshots:** `GameState` is read-only; the game loop creates a new instance when the player types or submits (no in-place mutation of state).
- **Containers:** `IReadOnlyList<Row>` for attempts; `string` for `CurrentRowLetters` and `HiddenWord`. Use `List<Row>` (or similar) when building state; expose as `IReadOnlyList` on `GameState`.

Dictionary data (word lists) lives in **assets** under `Data/` (e.g. text/JSON); loading and validation are in C#. The **hidden word** for a session is chosen at session start and held in `GameState.HiddenWord` for the lifetime of that session.

---

## 7. Commits

- **Format:** short subject line (imperative, ~50 chars); optional body for detail. Example: `Add dictionary loader and daily word selection`.
- **No Cursor (or other tool) attribution** in commit messages. Use `git commit -F <file>` (or `-F -` with stdin) so no extra line is appended.
- See workspace rule “No Cursor watermark” for the exact commit command.

---

## 8. Terminology

Use the terms from [GAME_SPECIFICATION.md](GAME_SPECIFICATION.md) § Nomenclature: hidden word, guess/attempt, tile, row, grid, feedback (CorrectPosition / WrongPosition / Absent), dictionary, session.
