# Dictionary and word list

## Format

- **One word per line**, UTF-8 encoding.
- Only lines whose trimmed length equals the game word length (see `GameConstants.WordLength`, default 5) are used.
- Empty lines and lines of other lengths are ignored. Duplicates (case-insensitive) are deduplicated.

## Source (open source, not hardcoded)

The game loads the word list from an **open-source URL** by default so the list is not hardcoded in the project:

- **Default URL:** `GameConstants.DefaultWordListUrl`  
  Currently: `https://raw.githubusercontent.com/shawon-majid/WordleCracker/main/fiveLetterWords.txt` (English, 5-letter words; repository may be replaced with another open-source list).
- At runtime, the game uses `WordListLoader.LoadFromUrlCoroutine` to fetch this URL and parse the text. The list is kept in memory for validation and for deterministic daily word selection.
- **Fallback:** If the network is unavailable or the URL fails, load a local asset (e.g. `Data/words-fallback.txt`) with `WordListLoader.LoadFromText` so the game still runs offline.

## Local fallback file

- **Path:** `Assets/Data/words-fallback.txt`
- Same format: one word per line, UTF-8. Used when the default URL cannot be loaded (offline or error).

## Loading (runtime and editor); encoding

All loading uses **UTF-8** so accents (e.g. á, ñ) are correct.

- **Runtime — URL:** `LoadFromUrlCoroutine(url, onSuccess, onError)` — response is interpreted as UTF-8.
- **Runtime — fallback:** `LoadFromTextAsset(TextAsset)` for an asset (e.g. in Resources) or `LoadFromText(string)` if you already have the text.
- **Editor or local file:** `LoadFromFile(path)` — reads the file with `System.Text.Encoding.UTF8`; use for validation in editor or when the path is valid (e.g. `Application.persistentDataPath`).
- **Raw bytes:** `LoadFromBytes(byte[])` — decodes with UTF-8, then parses one word per line.

## Usage

- **Validation:** `WordListLoader.IsValidWord(word)` returns true only if the word is in the loaded list and has the correct length.
- **Daily word:** Use the loaded list plus a deterministic index from the current date (see 3.5) so the same word is chosen for everyone on the same day.
