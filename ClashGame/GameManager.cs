// GameManager.cs

using System.Collections.Generic;
using System.Windows.Controls;

namespace ClashGame
{
    sealed class GameManager
    {
        private static GameManager? _instance;
        public static GameManager Instance
        {
            get
            {
                _instance ??= new GameManager();
                return _instance;
            }
        }

        private readonly IUnitFactory unitFactory;

        private GameManager()
        {
            unitFactory = new ArmyUnitFactory();
        }

        public void StartGame(TextBox outputTextBox, List<Warrior> firstArmy, List<Warrior> secondArmy)
        {
            BattleManager battleManager = new BattleManager();
            battleManager.StartBattle(firstArmy, secondArmy, outputTextBox);
        }

    }
}
