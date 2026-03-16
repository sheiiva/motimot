using UnityEngine;
using UnityEngine.InputSystem;

namespace Motimot
{
    /// <summary>
    /// Connects keyboard input to SessionController (5.3). Add to a GameObject; set Controller at runtime (e.g. from bootstrap).
    /// Uses Input System package. Letters A–Z, Backspace, Return/Enter.
    /// </summary>
    public sealed class KeyboardInputBridge : MonoBehaviour
    {
        /// <summary>Controller to receive input. Set at runtime (e.g. from GameBootstrap after session start).</summary>
        public SessionController Controller { get; set; }

        private void Update()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            SessionController ctrl = Controller;
            if (ctrl == null) return;

            if (keyboard.backspaceKey.wasPressedThisFrame)
            {
                ctrl.Backspace();
                return;
            }

            if (keyboard.enterKey.wasPressedThisFrame || keyboard.numpadEnterKey.wasPressedThisFrame)
            {
                ctrl.Submit();
                return;
            }

            for (int i = 0; i < 26; i++)
            {
                var key = (Key)((int)Key.A + i);
                if (keyboard[key].wasPressedThisFrame)
                {
                    ctrl.AddLetter((char)('a' + i));
                    return;
                }
            }
        }
    }
}
