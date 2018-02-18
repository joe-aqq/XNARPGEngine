using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine.Managers
{
    public static class InputManager
    {
        #region Variables
        private static KeyboardState kbs; //The current state of the keyboard
        private static KeyboardState kbsp; //Th//Character 1
        public static string character1Name = "King"; //Character's name
        public static int character1Health = 30; //Character's current health
        public static int character1HealthMax = 30; //Character's max health
        public static int character1MP = 30; //Character's current magic points
        public static int character1MPMax = 30; //Character's max magic points
        public static int character1EXP = 0; //Character's current EXPeprevious state of the keyboard
        private static Keys keysUse;
        private static Keys keysBack;
        private static Keys keysMenu;
        private static Keys keysEsc;
        private static Keys keysRun;
        #endregion

        /// <summary>
        /// Creates a new InputManager instance.
        /// </summary>
        static InputManager() { }

        /// <summary>
        /// Instiantiates variables for the input manager
        /// </summary>
        public static void Init()
        {
            keysUse = Keys.Z;
            keysBack = Keys.X;
            keysMenu = Keys.C;
            keysEsc = Keys.Escape;
            keysRun = Keys.LeftShift;
        }

        /// <summary>
        /// Sets the previous keyboard state to the current, and updates the keyboard state.
        /// </summary>
        public static void Update()
        {
            kbsp = kbs;
            kbs = Keyboard.GetState();
        }

        /// <summary>
        /// Checks to see if a key has been clicked properly.
        /// </summary>
        /// <param name="key">The key you want to check.</param>
        /// <returns></returns>
        public static bool Clicked(Keys key)
        {
            return kbs.IsKeyDown(key) && !kbsp.IsKeyDown(key);
        }

        /// <summary>
        /// Returns the current state of the keyboard.
        /// </summary>
        public static KeyboardState State
        {
            get { return kbs; }
        }

        /// <summary>
        /// Returns the previous state of the keyboard.
        /// </summary>
        public static KeyboardState StateP
        {
            get { return kbsp; }
        }

        /// <summary>
        /// Keys for input; can be changed directly.
        /// </summary>
        public static Keys UseKey { get { return keysUse; } set { keysUse = value; } }
        public static Keys BackKey { get { return keysBack; } set { keysBack = value; } }
        public static Keys MenuKey { get { return keysMenu; } set { keysMenu = value; } }
        public static Keys EscapeKey { get { return keysEsc; } set { keysEsc = value; } }
        public static Keys RunKey { get { return keysRun; } set { keysRun = value; } }
    }
}
