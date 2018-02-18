using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine.Managers
{
    public static class GlobalVariables
    {
        #region Variables
        public static GameState gameState;
        public static Maps currentMap;
        public enum GameState
        {
            gamePlay,
            showingText,
            menuDisplay,
            fightMode,
            cutscene
        }

        public enum Maps
        {
            testMap
        }
        #endregion

        /// <summary>
        /// Create a new GlobalVariables instance.
        /// </summary>
        static GlobalVariables() { }

        /// <summary>
        /// Instantiate global variables.
        /// </summary>
        public static void Init()
        {
            gameState = GameState.gamePlay;
            currentMap = Maps.testMap;
        }
    }
}
