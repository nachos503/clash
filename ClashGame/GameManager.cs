using System.Collections.Generic;
using System.Windows.Controls;

namespace ClashGame
{
    /// <summary>
    /// Класс GameManager, представляющий менеджер игры.
    /// Используется паттерн синглтон.
    /// Строка идентификатора "T:ClashGame.GameManager".
    /// </summary>
    sealed class GameManager
    {
        // синглтон сраный
        /// <summary>
        /// Единственный экземпляр класса GameManager.
        /// Строка идентификатора "F:ClashGame.GameManager._instance".
        /// </summary>
        private static GameManager? _instance;

        /// <summary>
        /// Свойство для получения единственного экземпляра класса GameManager.
        /// Строка идентификатора "P:ClashGame.GameManager.Instance".
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
        /// Приватный конструктор для класса GameManager.
        /// Строка идентификатора "M:ClashGame.GameManager.#ctor".
        /// </summary>
        private GameManager()
        {
        }

    }
}
