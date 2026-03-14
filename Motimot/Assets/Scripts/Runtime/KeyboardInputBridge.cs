using UnityEngine;

namespace Motimot
{
    /// <summary>
    /// Connects keyboard input to SessionController (5.3). Add to a GameObject; set Controller at runtime (e.g. from bootstrap).
    /// Uses legacy Input. Letters A–Z, Backspace, Return/Enter.
    /// </summary>
    public sealed class KeyboardInputBridge : MonoBehaviour
    {
        /// <summary>Controller to receive input. Set at runtime (e.g. from GameBootstrap after session start).</summary>
        public SessionController Controller { get; set; }

        private void Update()
        {
            SessionController ctrl = Controller;
            if (ctrl == null)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                ctrl.Backspace();
                return;
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                ctrl.Submit();
                return;
            }

            string input = Input.inputString;
            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            foreach (char ch in input)
            {
                if (ch >= 'a' && ch <= 'z')
                {
                    ctrl.AddLetter(ch);
                }
                else if (ch >= 'A' && ch <= 'Z')
                {
                    ctrl.AddLetter(char.ToLowerInvariant(ch));
                }
            }
        }
    }
}
