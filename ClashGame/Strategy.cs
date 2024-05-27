﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClashGame
{
    public class TwoRowStrategy : IBattleStrategy
    {
        BattleManager _battleManager;

        public void ExecuteBattle(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            int rows = Math.Min(2, Math.Min(attackers.Count, defenders.Count));
            for (int i = 0; i < rows; i++)
            {
                _battleManager.Attack(attackers[i], defenders[i], outputTextBox);
            }
        }

        public Warrior GetWarriorHeal(List<Warrior> attackers, int healerIndex, Healer healer)
        {
            Warrior warriorForHeal = null;
            var minHealth = double.MaxValue;
            if(healerIndex != attackers.Count() - 1 && healerIndex != attackers.Count() - 2)
            {
                if (attackers[healerIndex + 1].Healthpoints < minHealth && healerIndex % 2 == 0 && attackers[healerIndex + 1] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex + 1].Healthpoints;
                    warriorForHeal = attackers[healerIndex + 1];
                }
                if (attackers[healerIndex + 2].Healthpoints < minHealth && attackers[healerIndex + 2] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex + 2].Healthpoints;
                    warriorForHeal = attackers[healerIndex + 2];
                }

            }
            if (attackers[healerIndex - 1].Healthpoints < minHealth && healerIndex % 2 != 0 && attackers[healerIndex - 1] is not LightWarrior)
            {
                minHealth = attackers[healerIndex - 1].Healthpoints;
                warriorForHeal = attackers[healerIndex - 1];
            }
            if (attackers[healerIndex - 2].Healthpoints < minHealth && attackers[healerIndex - 2] is not LightWarrior)
            {
                minHealth = attackers[healerIndex - 2].Healthpoints;
                warriorForHeal = attackers[healerIndex - 2];
            }
            if (warriorForHeal != null)
                healer.Heal(warriorForHeal);
            return warriorForHeal;
        }

        public bool IsFrontLine(int attackerIndex, List<Warrior> defenders)
        {
            if (attackerIndex == 0)
            {
                return true;
            }
            if (defenders.Count() > 0)
            {
                if (attackerIndex == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else return false;
        }
    }

    public class ThreeRowStrategy : IBattleStrategy
    {
        BattleManager _battleManager;

        public void ExecuteBattle(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            int rows = Math.Min(3, Math.Min(attackers.Count, defenders.Count));
            for (int i = 0; i < rows; i++)
            {
                _battleManager.Attack(attackers[i], defenders[i], outputTextBox);
            }
        }

        public Warrior GetWarriorHeal(List<Warrior> attackers, int healerIndex, Healer healer)
        {
            Warrior warriorForHeal = null;
            var minHealth = double.MaxValue;
            if (healerIndex != attackers.Count() - 1 && healerIndex != attackers.Count() - 2)
            {
                if (attackers[healerIndex + 1].Healthpoints < minHealth && healerIndex % 2 == 0 && attackers[healerIndex + 1] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex + 1].Healthpoints;
                    warriorForHeal = attackers[healerIndex + 1];
                }
                if (attackers[healerIndex + 3].Healthpoints < minHealth && attackers[healerIndex + 3] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex + 3].Healthpoints;
                    warriorForHeal = attackers[healerIndex + 3];
                }

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
            if (warriorForHeal != null)
                healer.Heal(warriorForHeal);
            return warriorForHeal;
        }

        public bool IsFrontLine(int attackerIndex, List<Warrior> defenders)
        {
            if (attackerIndex == 0)
            {
                return true;
            }
            if (defenders.Count() > 0)
            {
                if (attackerIndex == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (defenders.Count() > 1)
                if (attackerIndex == 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            else return false;
        }
    }

    public class WallToWallStrategy : IBattleStrategy
    {
        BattleManager _battleManager;

        public void ExecuteBattle(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            int rows = Math.Min(attackers.Count, defenders.Count);
            for (int i = 0; i < rows; i++)
            {
                _battleManager.Attack(attackers[i], defenders[i], outputTextBox);
            }
        }

        public Warrior GetWarriorHeal(List<Warrior> attackers, int healerIndex, Healer healer)
        {
            Warrior warriorForHeal = null;
            var minHealth = double.MaxValue;
            if (healerIndex != attackers.Count() - 1 && healerIndex != attackers.Count() - 2)
            {
                if (attackers[healerIndex + 1].Healthpoints < minHealth && attackers[healerIndex + 1] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex + 1].Healthpoints;
                    warriorForHeal = attackers[healerIndex + 1];
                }
            }
            if (attackers[healerIndex - 1].Healthpoints < minHealth && attackers[healerIndex - 1] is not LightWarrior)
            {
                minHealth = attackers[healerIndex - 1].Healthpoints;
                warriorForHeal = attackers[healerIndex - 1];
            }
            if (warriorForHeal!=null)
                healer.Heal(warriorForHeal);
            return warriorForHeal;
        }

        public bool IsFrontLine(int attackerIndex, List<Warrior> defenders)
        {
            if (defenders.Count() - 1 > attackerIndex)
                return true;
            else
                return false;
        }
    }
}
