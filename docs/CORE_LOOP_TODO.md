# Core loop — TODO list

Checklist for the **Core loop** phase (see [GAME_SPECIFICATION.md](GAME_SPECIFICATION.md) §4.2). Track progress by checking off items.

---

## 1. Input

- [x] **1.1** Capture letter input (keyboard or on-screen): add typed letter to current row, up to `GameConstants.WordLength` characters. → `SessionController.AddLetter(char)`.
- [ ] **1.2** Capture backspace: remove last letter from current row.
- [ ] **1.3** Capture submit: trigger validation and feedback when the row is complete (length = WordLength).
- [ ] **1.4** Ignore or block input when the session is over (Phase is Won or Lost).

---

## 2. Validation

- [ ] **2.1** Before accepting a submit: ensure current row has exactly `GameConstants.WordLength` letters.
- [ ] **2.2** Call `WordListLoader.IsValidWord`; if invalid, reject submission and optionally signal "not a valid word" (no state change).
- [ ] **2.3** Only when valid: proceed to feedback logic and state update.

---

## 3. Feedback logic

- [ ] **3.1** For a submitted guess (string) and hidden word, compute per-letter `LetterFeedback` (CorrectPosition / WrongPosition / Absent).
- [ ] **3.2** Handle repeated letters: each occurrence in the hidden word is "consumed" at most once; green (correct position) takes priority over yellow (wrong position). See game spec nomenclature (repeated letters).
- [ ] **3.3** Build a `Row` (array of `Tile`) from the guess and computed feedback.

---

## 4. State updates

- [ ] **4.1** Add the new `Row` to attempts; clear current row for the next guess.
- [ ] **4.2** If guess equals hidden word: set Phase to Won.
- [ ] **4.3** If guess is wrong and attempts count reached `GameConstants.MaxAttempts`: set Phase to Lost.
- [ ] **4.4** Create a new `GameState` snapshot (immutable) and make it the current state for the session.

---

## 5. Wiring and session controller

- [ ] **5.1** Implement or designate a session controller that holds: `WordListLoader`, current `GameState`, hidden word for the session.
- [ ] **5.2** On session start: load word list (or use cached), get daily word, create initial GameState (empty attempts, empty current row, Phase.InProgress).
- [ ] **5.3** Connect input events (letter, backspace, submit) to the controller; controller performs validation, feedback, and state updates.
- [ ] **5.4** Expose the current GameState (or events) so the presentation layer can react (grid, keyboard, win/lose UI).

---

*Next phase: **Presentation** (grid, tiles, virtual keyboard, animations).*
