# Game specification — Word-of-the-day

Context, goals, and implementation steps for the game. This document sets expectations and nomenclature for the product.

---

## 1. Context

### 1.1 Reference

The game is inspired by **La palabra del día** ([lapalabradeldia.com](https://lapalabradeldia.com/)): a daily word-guessing game in Spanish. We build our own implementation with the same core loop and optional variants.

### 1.2 Core concept

- **One hidden word per day** (or per session), same for all players.
- **Limited attempts** to guess the word (e.g. 6 tries).
- **Fixed word length** in the base mode (e.g. 5 letters).
- **Feedback per letter** after each guess: correct position, wrong position, or not in the word.
- **Valid words only**: each guess must be in the game’s dictionary.

### 1.3 Reference mechanics (summary)

| Element | Description |
|--------|-------------|
| **Objective** | Guess the hidden word within the allowed attempts. |
| **Word length** | 5 letters in the standard mode. |
| **Attempts** | 6 guesses. |
| **Dictionary** | Spanish (or target language) word list; accents (e.g. á, é, ñ) supported. |
| **Feedback** | **Green**: letter correct and in the right position. **Yellow**: letter in the word but wrong position. **Gray**: letter not in the word. |
| **Repeated letters** | Each occurrence is evaluated separately; correct position (green) has priority over wrong position (yellow). |
| **Daily reset** | Same word for everyone on a given day; resets at a fixed time (e.g. midnight). |

### 1.4 Optional modes (reference)

- **Tildes / Accents**: Words with accents; word length can vary (e.g. 5–7 letters); accented and non-accented letters are distinct.
- **Timed (Contrarreloj)**: Maximum number of words guessed in a fixed time (e.g. 5 minutes).
- **Custom / Create**: User-defined word length (e.g. 3–11 letters); optional sharing with others.
- **Thematic**: e.g. science, countries — same rules, restricted vocabulary.

Our implementation may support a subset of these; the core experience is the standard 5-letter, 6-attempt, daily word.

---

## 2. Goals

### 2.1 Product goals

- **Deliver a complete, playable copy** of the word-of-the-day game (standard mode as minimum).
- **Clear, consistent nomenclature** so all documentation and code share the same terms (word, attempt, feedback, tile, keyboard, etc.).
- **Single-player, local-first** experience: daily word (or session word), no mandatory backend.
- **Extensible design** so extra modes (timed, custom word length, themes) can be added later without rewriting core logic.

### 2.2 User experience goals

- **Immediate readability**: grid of letters, clear feedback colors (green / yellow / gray or equivalent).
- **Input**: type or tap letters; support backspace and submit; optional on-screen keyboard that reflects feedback.
- **Feedback**: after each guess, show per-letter state and update the on-screen keyboard if present.
- **End states**: win (word guessed), lose (attempts exhausted), with option to reveal the word and optionally share or replay.

### 2.3 Technical expectations

- **Unity** as the runtime (target platforms TBD: e.g. standalone, WebGL).
- **Deterministic daily word** (e.g. seed from date) so the same word is used for a given day for all players without a server.
- **Word list and validation**: built-in dictionary; only valid words accepted as guesses.
- **No dependency on the reference site**: our own assets, logic, and data.

---

## 3. Nomenclature (shared vocabulary)

Use these terms consistently in docs, code, and UI (or map UI strings from them).

| Term | Meaning |
|------|--------|
| **Hidden word / target word** | The word the player must guess (one per day or per session). |
| **Guess / attempt** | One submitted word (same length as the hidden word). |
| **Tile** | One cell in the grid: a letter plus its feedback state (empty, correct, wrong position, absent). |
| **Row** | One full guess: a horizontal line of tiles. |
| **Grid** | All rows (e.g. 6 rows × 5 columns for standard mode). |
| **Feedback (per letter)** | Correct position (green), wrong position (yellow), absent (gray). |
| **Keyboard (virtual)** | On-screen key set; keys can be updated with feedback from previous guesses. |
| **Dictionary** | Set of valid guessable words (and optionally the set of possible hidden words). |
| **Daily word** | The single hidden word for a given calendar day (or session). |
| **Session** | One play from first guess to win/lose; may be “today’s daily” or a custom/themed round. |

---

## 4. Steps to achieve the game

High-level phases to go from zero to a shippable standard mode. Order can be adjusted; dependencies are called out.

### 4.1 Foundation

- **Project setup**: Unity project structure, scenes, coding style, and where game logic and data live.
- **Data model**: Define how the hidden word, attempts, and per-tile feedback are represented (e.g. word length, attempt count, letter states).
- **Dictionary**: Obtain or create a word list (e.g. 5-letter words); load and validate format; implement “is valid word” and “get daily word for date” (deterministic from date seed).

### 4.2 Core loop

- **Input**: Capture letter input (keyboard and/or on-screen), backspace, submit.
- **Validation**: Only allow submission when the current row spells a valid word from the dictionary.
- **Feedback logic**: For a submitted guess, compute per-letter state (correct position / wrong position / absent) including rules for repeated letters (green over yellow).
- **State updates**: Store attempts and feedback; detect win (exact match) and lose (no success within max attempts).

### 4.3 Presentation

- **Grid**: Display rows and tiles; show letters and feedback colors (green / yellow / gray) after each submit.
- **Animations (optional)**: Flip or reveal tiles, row shake on invalid word, celebration on win.
- **Virtual keyboard**: Render keys; update key colors from feedback of all submitted rows (e.g. show best state per letter).

### 4.4 Session and daily word

- **Daily word selection**: Implement deterministic “word for date” (e.g. hash of date → index into list).
- **Session flow**: Start round (load daily word), play until win/lose, show result and optionally the word; support “new game” or “next day” where applicable.

### 4.5 Polish and optional modes

- **UI/UX**: Menus, how to play, share result (e.g. emoji grid), settings (language, accessibility).
- **Optional modes**: If scope includes tildes, timed, or custom word length, add mode-specific word lists and rules (length, time limit, etc.) without breaking the core nomenclature and loop.

---

## 5. Out of scope for this specification

- Backend or user accounts (scores, history) unless explicitly added later.
- Exact clone of the reference site’s UI or branding.
- Legal or licensing of word lists (to be handled separately).

---

*This document defines context, goals, and steps for the word-of-the-day game. Implementation details (classes, scene layout, packages) are left to the development phase.*
