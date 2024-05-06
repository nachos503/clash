using System.Collections.Generic;
using System.Windows.Controls;

namespace ClashGame
{
    sealed class GameManager
    {
        //синглтон сраный
        private static GameManager? _instance;
        public static GameManager Instance
        {
            get
            {
                _instance ??= new GameManager();
                return _instance;
            }
        }

        private GameManager()
        {
        }

        public void StartGame(TextBox outputTextBox, List<Warrior> firstArmy, List<Warrior> secondArmy)
        {
            string logFilePath = "battle_logs.txt";
            ILogger fileLogger = new FileLogger(logFilePath);

           // BattleManager battleManager = new BattleManager();
            BattleManagerProxy proxy = new BattleManagerProxy(fileLogger);
            proxy.StartBattle(firstArmy, secondArmy, outputTextBox);
        }

    }
}
