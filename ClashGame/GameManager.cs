using System.Collections.Generic;
using System.Windows.Controls;

namespace ClashGame
{
    /// <summary>
    /// The GameManager class representing the game manager.
    /// Implements the Singleton pattern.
    /// Identifier string: "T:ClashGame.GameManager".
    /// </summary>
    sealed class GameManager
    {
        /// <summary>
        /// The single instance of the GameManager class.
        /// Identifier string: "F:ClashGame.GameManager._instance".
        /// </summary>
        private static GameManager? _instance;

        /// <summary>
        /// Property to get the single instance of the GameManager class.
        /// Identifier string: "P:ClashGame.GameManager.Instance".
        /// </summary>
        public static GameManager Instance
        {
            get
            {
                _instance ??= new GameManager();
                return _instance;
            }
        }

        /// <summary>
        /// Private constructor for the GameManager class.
        /// Identifier string: "M:ClashGame.GameManager.#ctor".
        /// </summary>
        private GameManager()
        {
        }

    }
}
