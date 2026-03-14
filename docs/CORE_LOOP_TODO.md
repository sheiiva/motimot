# Core loop — TODO list

Checklist for the **Core loop** phase (see [GAME_SPECIFICATION.md](GAME_SPECIFICATION.md) §4.2). Track progress by checking off items.

---

## 1. Input

- [x] **1.1** Capture letter input (keyboard or on-screen): add typed letter to current row, up to `GameConstants.WordLength` characters. → `SessionController.AddLetter(char)`.
- [x] **1.2** Capture backspace: remove last letter from current row. → `SessionController.Backspace()`.
- [x] **1.3** Capture submit: trigger validation and feedback when the row is complete (length = WordLength). → `SessionController.Submit()`; returns `SubmitResult` (Accepted, IgnoredGameOver, RowIncomplete, InvalidWord).
- [x] **1.4** Ignore or block input when the session is over (Phase is Won or Lost). → `AddLetter`, `Backspace`, `Submit` all check `Phase != InProgress` and return early.

---

## 2. Validation

- [x] **2.1** Before accepting a submit: ensure current row has exactly `GameConstants.WordLength` letters. → In `SessionController.Submit()` (RowIncomplete).
- [x] **2.2** Call `WordListLoader.IsValidWord`; if invalid, reject submission and optionally signal "not a valid word" (no state change). → In `SessionController.Submit()` (InvalidWord).
- [x] **2.3** Only when valid: proceed to feedback logic and state update. → In `SessionController.Submit()`.

---

## 3. Feedback logic

- [x] **3.1** For a submitted guess (string) and hidden word, compute per-letter `LetterFeedback` (CorrectPosition / WrongPosition / Absent). → `FeedbackComputer.Compute`.
- [x] **3.2** Handle repeated letters: each occurrence in the hidden word is "consumed" at most once; green (correct position) takes priority over yellow (wrong position). → In `FeedbackComputer.Compute` (two-pass).
- [x] **3.3** Build a `Row` (array of `Tile`) from the guess and computed feedback. → `FeedbackComputer.Compute` returns `Tile[]`; `SessionController.Submit` builds `Row`.

---

## 4. State updates

- [x] **4.1** Add the new `Row` to attempts; clear current row for the next guess. → In `SessionController.Submit()` (newAttempts, currentRowLetters = "").
- [x] **4.2** If guess equals hidden word: set Phase to Won. → In `SessionController.Submit()`.
- [x] **4.3** If guess is wrong and attempts count reached `GameConstants.MaxAttempts`: set Phase to Lost. → In `SessionController.Submit()`.
- [x] **4.4** Create a new `GameState` snapshot (immutable) and make it the current state for the session. → In `SessionController.Submit()`.

---

## 5. Wiring and session controller

- [x] **5.1** Implement or designate a session controller that holds: `WordListLoader`, current `GameState`, hidden word for the session. → `SessionController`.
- [ ] **5.2** On session start: load word list (or use cached), get daily word, create initial GameState (empty attempts, empty current row, Phase.InProgress).
- [ ] **5.3** Connect input events (letter, backspace, submit) to the controller; controller performs validation, feedback, and state updates.
- [ ] **5.4** Expose the current GameState (or events) so the presentation layer can react (grid, keyboard, win/lose UI).

---

*Next phase: **Presentation** (grid, tiles, virtual keyboard, animations).*
