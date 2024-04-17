using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void StartGame(TextBox outputTextBox)
        {
            ArmyManager armyManager = new ArmyManager(outputTextBox, unitFactory);
            BattleManager battleManager = new BattleManager();

            List<Warrior> firstArmy = new List<Warrior>();
            List<Warrior> secondArmy = new List<Warrior>();

            armyManager.CreateArmy(firstArmy, "Blue");
            armyManager.CreateArmy(secondArmy, "Red");

            battleManager.StartBattle(firstArmy, secondArmy, outputTextBox);
        }
    }
}
