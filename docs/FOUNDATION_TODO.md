# Foundation — TODO list

Checklist for the **Foundation** phase (see [GAME_SPECIFICATION.md](GAME_SPECIFICATION.md) §4.1). Track progress by checking off items.

---

## 1. Project setup

- [x] **1.1** Define folder structure under `Assets/` for the game (e.g. `Scripts/`, `Scripts/Runtime/`, `Scripts/Editor/`, `Data/`, `Prefabs/`, `Scenes/` — adjust to your convention). → `Scripts/Runtime`, `Scripts/Editor`, `Data`, `Prefabs`; see `Assets/README.md`.
- [x] **1.2** Add a main game scene (or designate `SampleScene` as the game scene) and document which scene is the entry point. → `Assets/Scenes/Main.unity` is the default scene (index 0 in Build Settings); see `Assets/README.md` § Entry point.
- [x] **1.3** Define coding style and assembly layout: namespaces, naming (PascalCase for public types, etc.), and whether to use one or more `.asmdef` for game code. → See `docs/CODING_STYLE_AND_ASSEMBLIES.md`; Runtime `Motimot.asmdef`, Editor `Motimot.Editor.asmdef`; `.cursor/rules/project-standards.mdc` (alwaysApply) for the agent.
- [x] **1.4** Document where game logic lives: which folders for core game state, UI, data (dictionary), and any editor tools. → See `Assets/README.md` § Where game logic lives.
- [x] **1.5** (Optional) Add a minimal README or `Assets/.../README` describing the project layout for future contributors. → Root [README.md](../README.md) points to project and key docs; [Motimot/Assets/README.md](../Motimot/Assets/README.md) is the layout reference for contributors.

---

## 2. Data model

- [x] **2.1** Define constants: word length (e.g. 5), max attempts (e.g. 6), and any mode-specific values. → `GameConstants.cs`: `WordLength = 5`, `MaxAttempts = 6`.
- [x] **2.2** Define a **letter feedback** enum or struct (e.g. `CorrectPosition`, `WrongPosition`, `Absent` / green, yellow, gray). → `LetterFeedback.cs`: enum `CorrectPosition`, `WrongPosition`, `Absent`.
- [x] **2.3** Define a **tile** representation: letter + feedback state (and optionally position in row). → `Tile.cs`: readonly struct with `Letter`, `Feedback`, `IndexInRow`.
- [x] **2.4** Define a **row** representation: ordered list of tiles for one guess. → `Row.cs`: sealed class with `Tiles` (IReadOnlyList&lt;Tile&gt;), constructor from `Tile[]` of length WordLength.
- [x] **2.5** Define **game state**: hidden word, list of submitted attempts (rows with feedback), current row (in progress), win/lose/in-progress. → `GamePhase.cs`: enum `InProgress`, `Won`, `Lost`. `GameState.cs`: sealed class with `HiddenWord`, `Attempts` (IReadOnlyList&lt;Row&gt;), `CurrentRowLetters`, `Phase`, `AttemptsCount`.
- [ ] **2.6** Decide how the hidden word and attempts are stored at runtime (e.g. plain C# types, ScriptableObject, or data containers) and document the choice.

---

## 3. Dictionary

- [ ] **3.1** Obtain or create a word list: 5-letter words for standard mode (language: e.g. Spanish or English); store as a text/JSON/asset file under `Data/` (or chosen folder).
- [ ] **3.2** Define the file format (one word per line, JSON array, or other) and document it.
- [ ] **3.3** Implement **loading** the dictionary at runtime (or in editor for validation); handle encoding (e.g. UTF-8 for accents).
- [ ] **3.4** Implement **validation**: `IsValidWord(string word)` returning true only if the word is in the dictionary and has the correct length.
- [ ] **3.5** Implement **daily word selection**: `GetDailyWord(DateOnly date)` or equivalent, deterministic (e.g. hash of date → index into word list); document the algorithm so the same word is chosen for the same day everywhere.
- [ ] **3.6** (Optional) Add a small set of test words and a way to override the daily word for testing (e.g. debug menu or test scene).

---

## 4. Verification

- [ ] **4.1** Run through the list: project structure exists, data types compile, dictionary loads and validates, daily word is deterministic for a given date.
- [ ] **4.2** Update this document: check off completed items and add short notes if something was done differently (e.g. folder names, namespace).

---

*Next phase after Foundation: **Core loop** (input, validation, feedback logic, win/lose).*
