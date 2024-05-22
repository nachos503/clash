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
        public void ArrangeArmy(List<Warrior> army)
        {
            List<Warrior>[] twoRowArmy = new List<Warrior>[2];
            twoRowArmy[0] = new List<Warrior>();
            twoRowArmy[1] = new List<Warrior>();

            for (int i = 0; i < army.Count; i++)
            {
                twoRowArmy[i % 2].Add(army[i]);
            }

            army.Clear();
            army.AddRange(twoRowArmy[0]);
            army.AddRange(twoRowArmy[1]);
        }

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
        public void ArrangeArmy(List<Warrior> army)
        {
            List<Warrior>[] threeRowArmy = new List<Warrior>[3];
            threeRowArmy[0] = new List<Warrior>();
            threeRowArmy[1] = new List<Warrior>();
            threeRowArmy[2] = new List<Warrior>();

            for (int i = 0; i < army.Count; i++)
            {
                threeRowArmy[i % 3].Add(army[i]);
            }

            army.Clear();
            army.AddRange(threeRowArmy[0]);
            army.AddRange(threeRowArmy[1]);
            army.AddRange(threeRowArmy[2]);
        }

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
        public void ArrangeArmy(List<Warrior> army)
        {
            // No special arrangement needed for wall-to-wall battle
        }

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
