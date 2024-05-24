using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
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
            int rows = Math.Min(2, Math.Min(attackers.Count, defenders.Count));
            for (int i = 0; i < rows; i++)
            {
                battleManager.Attack(attackers[i], defenders[i], outputTextBox);
            }
        }

       public Warrior GetWarriorHeal(List<Warrior>  attackers, int healerIndex, Healer healer)
       {
            Warrior warriorForHeal = null;
            var minHealth = double.MaxValue;
            if (healerIndex == 0 || healerIndex == 1)
            {
                return null;
            }
            if (attackers[healerIndex+1].Healthpoints < minHealth && healerIndex % 2 == 0 && attackers[healerIndex + 1] is not LightWarrior)
            {
               minHealth = attackers[healerIndex + 1].Healthpoints;
               warriorForHeal = attackers[healerIndex+1];
            }
            if (attackers[healerIndex - 1].Healthpoints < minHealth && healerIndex % 2 != 0 && attackers[healerIndex - 1] is not LightWarrior)
            {
                minHealth = attackers[healerIndex - 1].Healthpoints;
                warriorForHeal = attackers[healerIndex - 1];
            }
            if (attackers[healerIndex-2].Healthpoints < minHealth && attackers[healerIndex - 2] is not LightWarrior)
            {
                minHealth = attackers[healerIndex -2].Healthpoints;
                warriorForHeal = attackers[healerIndex -2];
            }
            if (attackers[healerIndex + 2].Healthpoints < minHealth && attackers[healerIndex + 2] is not LightWarrior)
            {
                minHealth = attackers[healerIndex + 2].Healthpoints;
                warriorForHeal = attackers[healerIndex + 2];
            }
            healer.Heal(warriorForHeal);
            return warriorForHeal;
        }

        public bool IsFrontLine(int attackerIndex)
        {
            if (attackerIndex == 0 || attackerIndex == 1)
            {
                return true;
            }
            else return false;
        }
    }

    public class ThreeRowStrategy : IBattleStrategy
    {
        private BattleManager battleManager = new BattleManager();

        public void ExecuteBattle(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            int rows = Math.Min(3, Math.Min(attackers.Count, defenders.Count));
            for (int i = 0; i < rows; i++)
            {
                battleManager.Attack(attackers[i], defenders[i], outputTextBox);
            }
        }

        public Warrior GetWarriorHeal(List<Warrior> attackers, int healerIndex, Healer healer)
        {
            Warrior warriorForHeal = null;
            var minHealth = double.MaxValue;
            if (attackers[healerIndex + 1].Healthpoints < minHealth && healerIndex % 3 != 2 && attackers[healerIndex + 1] is not LightWarrior)
            {
                minHealth = attackers[healerIndex + 1].Healthpoints;
                warriorForHeal = attackers[healerIndex + 1];
            }
            if (attackers[healerIndex - 1].Healthpoints < minHealth && healerIndex % 3 != 0 && attackers[healerIndex - 1] is not LightWarrior)
            {
                minHealth = attackers[healerIndex - 1].Healthpoints;
                warriorForHeal = attackers[healerIndex - 1];
            }
            if (attackers[healerIndex - 3].Healthpoints < minHealth && attackers[healerIndex - 3] is not LightWarrior)
            {
                minHealth = attackers[healerIndex - 3].Healthpoints;
                warriorForHeal = attackers[healerIndex - 3];
            }
            if (attackers[healerIndex + 3].Healthpoints < minHealth && attackers[healerIndex + 3] is not LightWarrior)
            {
                minHealth = attackers[healerIndex + 3].Healthpoints;
                warriorForHeal = attackers[healerIndex + 3];
            }
            healer.Heal(warriorForHeal);
            return warriorForHeal;
        }

        public bool IsFrontLine(int attackerIndex)
        {
            if (attackerIndex == 0 || attackerIndex == 1 || attackerIndex == 2)
            {
                return true;
            }
            else return false;
        }
    }

    public class WallToWallStrategy : IBattleStrategy
    {
        private BattleManager battleManager = new BattleManager();

        public void ExecuteBattle(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            int rows = Math.Min(attackers.Count, defenders.Count);
            for (int i = 0; i < rows; i++)
            {
                battleManager.Attack(attackers[i], defenders[i], outputTextBox);
            }
        }

        public Warrior GetWarriorHeal(List<Warrior> attackers, int healerIndex, Healer healer)
        {
            Warrior warriorForHeal = null;
            var minHealth = double.MaxValue;
            if (attackers[healerIndex + 1].Healthpoints < minHealth && attackers[healerIndex + 1] is not LightWarrior)
            {
                minHealth = attackers[healerIndex + 1].Healthpoints;
                warriorForHeal = attackers[healerIndex + 1];
            }
            if (attackers[healerIndex - 1].Healthpoints < minHealth && attackers[healerIndex - 1] is not LightWarrior)
            {
                minHealth = attackers[healerIndex - 1].Healthpoints;
                warriorForHeal = attackers[healerIndex - 1];
            }

            healer.Heal(warriorForHeal);
            return warriorForHeal;
        }

        public bool IsFrontLine(int attackerIndex)
        {
            return false;
        }
    }
}
