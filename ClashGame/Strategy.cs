using System;
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
    //деволтная стратегия, чтобы не ломался конструктор
    public class DefoltStratagy : IBattleStrategy
    {
        public void ExecuteBattle(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {

        }
        public Warrior GetEnemyWarrior(List<Warrior> attackers, List<Warrior> defenders, int archerIndex, Archer archer)
        {
            return null;
        }
        public Warrior CloneWarrior(List<Warrior> attackers, int wizardIndex, Wizard wizard)
        {
            return null;
        }
        public Warrior GetWarriorHeal(List<Warrior> attackers, int healerIndex, Healer healer)
        {
            return null;
        }
        public bool IsFrontLine(int attackerIndex, List<Warrior> defenders)
        {
            return false;
        }
    }
    public class TwoRowStrategy : IBattleStrategy
    {
        BattleManagerProxy _battleManagerProxy;

        public TwoRowStrategy(BattleManagerProxy battleManagerProxy)
        {
            _battleManagerProxy = battleManagerProxy;
        }

        public void ExecuteBattle(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            int rows = Math.Min(2, Math.Min(attackers.Count, defenders.Count));
            for (int i = 0; i < rows; i++)
            {
                if (attackers[i] is not GulyayGorod && defenders[i] is not GulyayGorod)
                    _battleManagerProxy.Attack(attackers[i], defenders[i], outputTextBox);
            }
        }

        public Warrior GetEnemyWarrior(List<Warrior> attackers, List<Warrior> defenders, int archerIndex, Archer archer)
        {
            return null;
        }

        public Warrior CloneWarrior (List<Warrior> attackers, int wizardIndex, Wizard wizard)
        {
            bool isCloned = false;
            Warrior warriorForClone = null;
            if (wizardIndex != attackers.Count() - 1)
            {
                if (wizardIndex % 2 == 0 && attackers[wizardIndex + 1] is LightWarrior)
                {
                    warriorForClone = attackers[wizardIndex + 1];
                    isCloned = true;
                }
            }
            if (wizardIndex < attackers.Count() - 2)
            { 
                if (attackers[wizardIndex + 2] is  LightWarrior && warriorForClone == null && isCloned == false)
                {
                    warriorForClone = attackers[wizardIndex + 2];
                    isCloned = true;
                }
            }
            if (wizardIndex % 2 != 0 && attackers[wizardIndex - 1] is LightWarrior && warriorForClone == null && isCloned == false)
            {
                warriorForClone = attackers[wizardIndex - 1];
                isCloned = true;
            }
            if (attackers[wizardIndex - 2] is LightWarrior && warriorForClone == null && isCloned == false)
            {
                warriorForClone = attackers[wizardIndex - 2];
                isCloned = true;
            }
            if (warriorForClone != null)
                wizard.CloneLightWarrior(warriorForClone);
            return warriorForClone;
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
        BattleManagerProxy _battleManagerProxy;

        public ThreeRowStrategy(BattleManagerProxy battleManagerProxy)
        {
            _battleManagerProxy = battleManagerProxy;
        }

        public void ExecuteBattle(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            int rows = Math.Min(3, Math.Min(attackers.Count, defenders.Count));
            for (int i = 0; i < rows; i++)
            {
                if (attackers[i] is not GulyayGorod && defenders[i] is not GulyayGorod)
                    _battleManagerProxy.Attack(attackers[i], defenders[i], outputTextBox);
            }
        }

        public Warrior GetEnemyWarrior(List<Warrior> attackers, List<Warrior> defenders, int archerIndex, Archer archer)
        {
            return null;
        }

        public Warrior CloneWarrior(List<Warrior> attackers, int wizardIndex, Wizard wizard)
        {
            bool isCloned = false;
            Warrior warriorForClone = null;
            if (wizardIndex != attackers.Count() - 1 )
            {
                if (wizardIndex % 2 == 0 && attackers[wizardIndex + 1] is LightWarrior && isCloned == false)
                {
                    warriorForClone = attackers[wizardIndex + 1];
                    isCloned = true;
                }
            }
            if (wizardIndex < attackers.Count() - 3)
            {

                if (attackers[wizardIndex + 3] is LightWarrior && warriorForClone == null && isCloned == false)
                {
                    warriorForClone = attackers[wizardIndex + 3];
                    isCloned = true;
                }
            }

            if (wizardIndex % 3 != 0 && attackers[wizardIndex - 1] is LightWarrior && warriorForClone == null && isCloned == false)
            {
                warriorForClone = attackers[wizardIndex - 1];
                isCloned = true;
            }
            if (attackers[wizardIndex - 3] is LightWarrior && warriorForClone == null && isCloned == false)
            {
                warriorForClone = attackers[wizardIndex - 3];
                isCloned = true;
            }
            if (warriorForClone != null)
                wizard.CloneLightWarrior(warriorForClone);
            return warriorForClone;
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

            }
            if (defenders.Count() > 1)
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
        BattleManagerProxy _battleManagerProxy;

        public WallToWallStrategy(BattleManagerProxy battleManagerProxy)
        {
            _battleManagerProxy = battleManagerProxy;
        }

        public void ExecuteBattle(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            int rows = Math.Min(attackers.Count, defenders.Count);
            for (int i = 0; i < rows; i++)
            {
                if (attackers[i] is not GulyayGorod && defenders[i] is not GulyayGorod)
                _battleManagerProxy.Attack(attackers[i], defenders[i], outputTextBox);
            }
        }

        public Warrior GetEnemyWarrior(List<Warrior> attackers, List<Warrior> defenders, int archerIndex, Archer archer)
        {
            return null;
        }

        public Warrior CloneWarrior(List<Warrior> attackers, int wizardIndex, Wizard wizard)
        {
            bool isCloned = false;
            Warrior warriorForClone = null;
            if (wizardIndex != attackers.Count() - 1 && wizardIndex != attackers.Count() - 2)
            {
                if (attackers[wizardIndex + 1] is LightWarrior && isCloned == false)
                {
                    warriorForClone = attackers[wizardIndex + 1];
                }
            }
            if (attackers[wizardIndex - 1] is LightWarrior && warriorForClone == null && isCloned == false)
            {
                warriorForClone = attackers[wizardIndex - 1];
            }
            if (warriorForClone != null)
                wizard.CloneLightWarrior(warriorForClone);
            return warriorForClone;
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
