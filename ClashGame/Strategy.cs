using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClashGame
{
    public class TwoRowStrategy : IBattleStrategy
    {
        private BattleManager battleManager = new BattleManager();
        public void ExecuteBattle(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            if (attackers.Count > 0 && defenders.Count > 0)
            {
                for (int i = 0; i <= 1; i++)
                {
                    battleManager.Attack(attackers[i],defenders[i], outputTextBox);
                }
            }
        }
    }

    public class ThreeRowStrategy : IBattleStrategy
    {
        private BattleManager battleManager = new BattleManager();

        public void ExecuteBattle(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            if (attackers.Count > 0 && defenders.Count > 0)
            {
                for (int i = 0; i <= 2; i++)
                {
                    battleManager.Attack(attackers[i], defenders[i], outputTextBox);
                }
            }
        }
    }

    public class WallToWallStrategy : IBattleStrategy
    {
        private BattleManager battleManager = new BattleManager();

        public void ExecuteBattle(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            if (attackers.Count > 0 && defenders.Count > 0)
            {
                for (int i = 0; i < Math.Min(attackers.Count, defenders.Count); i++)
                {
                    battleManager.Attack(attackers[i], defenders[i], outputTextBox);
                }
            }
        }
    }
}
