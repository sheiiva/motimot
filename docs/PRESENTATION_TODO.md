# Presentation — TODO list

Checklist for the **Presentation** phase (see [GAME_SPECIFICATION.md](GAME_SPECIFICATION.md) §4.3). Track progress by checking off items.

---

## 1. Grid

- [x] **1.1** Display the grid: `GameConstants.MaxAttempts` rows × `GameConstants.WordLength` tiles (e.g. 6×5). → `GameGridView`: builds 6×5 UI grid at runtime; add to empty GameObject in Main scene.
- [x] **1.2** Bind grid to `GameState`: show submitted attempts (rows with letters and feedback) and the current row in progress (letters only).
- [x] **1.3** Show letters in each tile (from `Row.Tiles` or `CurrentRowLetters`).
- [ ] **1.4** Apply feedback colors after submit: green (CorrectPosition), yellow (WrongPosition), gray (Absent). Update when `OnStateChanged` fires.

---

## 2. Virtual keyboard

- [ ] **2.1** Render an on-screen keyboard (e.g. QWERTY layout: rows of keys).
- [ ] **2.2** Keys trigger the same input as physical keyboard: letter → `AddLetter`, backspace → `Backspace`, enter → `Submit`.
- [ ] **2.3** Update key colors from feedback: for each letter, show the “best” state from all submitted rows (green > yellow > gray > default). Use `GameState.Attempts` to derive per-letter feedback.

---

## 3. Bootstrap and wiring (scene)

- [ ] **3.1** Bootstrap in Main scene: load word list (URL or fallback), create `SessionController` via `SessionStarter.StartSession`, assign to `KeyboardInputBridge.Controller`.
- [ ] **3.2** Grid (and optional keyboard) subscribe to `SessionController.OnStateChanged` to refresh from `GameState`.

---

## 4. Animations (optional)

- [ ] **4.1** Tile reveal: after submit, animate tiles showing feedback (e.g. flip or color transition).
- [ ] **4.2** Row shake on invalid word: when `Submit()` returns `InvalidWord`, play a short shake or highlight on the current row.
- [ ] **4.3** Celebration on win: when `Phase == Won`, play a simple celebration (e.g. confetti, message, or highlight).

---

*Next phase: **Session and daily word** (4.4) — result screen, reveal word on lose, new game / next day.*
