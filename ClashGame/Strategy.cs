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
    //дефолтная стратегия, чтобы не ломался конструктор
    public class DefaultStratagy : IBattleStrategy
    {
        public void ExecuteBattle(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {

        }
        public Warrior GetEnemyWarrior(List<Warrior> attackers, List<Warrior> defenders, int archerIndex, Archer archer)
        {
            return null;
        }
        public Warrior GetWarriorClone(List<Warrior> attackers, int wizardIndex)
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
                if (defenders.Count > i)
                {
                    if (attackers[i] is not GulyayGorod && defenders[i] is not GulyayGorod)
                        _battleManagerProxy.Attack(attackers[i], defenders[i], outputTextBox);
                    _battleManagerProxy.IsDead(defenders[i], defenders);
                }
            }
        }

        //прикол
        public Warrior GetEnemyWarrior(List<Warrior> attackers, List<Warrior> defenders, int archerIndex, Archer archer)
        {
            var range = archer.Range();
            for (int i = archerIndex; i > 1; i -= 2)
            {
                range--;
            }
            if (range > 0)
            {
                for (int i = 0; i < defenders.Count; i += 2)
                {
                    range--;
                    if (range == 0)
                    {
                        if (archerIndex % 2 == 0)
                        {
                            return defenders[i];
                        }
                        else if (defenders.Count > i+1) 
                        {
                            return defenders[i + 1];
                        }
                    }
                } 
            }
            return null;
        }

        public Warrior GetWarriorClone (List<Warrior> attackers, int wizardIndex)
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
            if (wizardIndex >= 1)
            {
                if (wizardIndex % 2 != 0 && attackers[wizardIndex - 1] is LightWarrior && warriorForClone == null && isCloned == false)
                {
                    warriorForClone = attackers[wizardIndex - 1];
                    isCloned = true;
                }
            }
            if (wizardIndex >= 2)
            {
                if (attackers[wizardIndex - 2] is LightWarrior && warriorForClone == null && isCloned == false)
                {
                    warriorForClone = attackers[wizardIndex - 2];
                    isCloned = true;
                }
            }
            return warriorForClone;
        }
    
        public Warrior GetWarriorHeal(List<Warrior> attackers, int healerIndex, Healer healer)
        {
            Warrior warriorForHeal = null;
            var minHealth = double.MaxValue;

                if (healerIndex + 1 < attackers.Count())
                {
                    if (attackers[healerIndex + 1].Healthpoints < minHealth && healerIndex % 2 == 0 && attackers[healerIndex + 1] is not LightWarrior)
                    {
                        minHealth = attackers[healerIndex + 1].Healthpoints;
                        warriorForHeal = attackers[healerIndex + 1];
                    }
                }
                if (healerIndex + 2 < attackers.Count())
                {
                    if (attackers[healerIndex + 2].Healthpoints < minHealth && attackers[healerIndex + 2] is not LightWarrior)
                    {
                        minHealth = attackers[healerIndex + 2].Healthpoints;
                        warriorForHeal = attackers[healerIndex + 2];
                    }
                }
            if (healerIndex >= 1)
            {
                if (attackers[healerIndex - 1].Healthpoints < minHealth && healerIndex % 2 != 0 && attackers[healerIndex - 1] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex - 1].Healthpoints;
                    warriorForHeal = attackers[healerIndex - 1];
                }
            }
            if (healerIndex >= 2)
            {
                if (attackers[healerIndex - 2].Healthpoints < minHealth && attackers[healerIndex - 2] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex - 2].Healthpoints;
                    warriorForHeal = attackers[healerIndex - 2];
                }
            }
            
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
                if (defenders.Count > i)
                {
                    if (attackers[i] is not GulyayGorod && defenders[i] is not GulyayGorod)
                        _battleManagerProxy.Attack(attackers[i], defenders[i], outputTextBox);
                    _battleManagerProxy.IsDead(defenders[i], defenders);
                }
            }
        }

        public Warrior GetEnemyWarrior(List<Warrior> attackers, List<Warrior> defenders, int archerIndex, Archer archer)
        {
            var range = archer.Range();
            for (int i = archerIndex; i > 1; i -= 3)
            {
                range--;
            }
            if (range > 0)
            {
                for (int i = 0; i < defenders.Count; i += 3)
                {
                    range--;
                    if (range == 0)
                    {
                        if (archerIndex % 3 == 0)
                        {
                            return defenders[i];
                        }
                        else if (defenders.Count > i + 1 && archerIndex % 3 == 1)
                        {
                            return defenders[i + 1];
                        }
                        else if (defenders.Count > i + 2)
                        {
                            return defenders[i + 2];
                        }
                    }
                }
            }
            return null;
        }

        public Warrior GetWarriorClone(List<Warrior> attackers, int wizardIndex)
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

            if (wizardIndex >= 1)
            {
                if (wizardIndex % 3 != 0 && attackers[wizardIndex - 1] is LightWarrior && warriorForClone == null && isCloned == false)
                {
                    warriorForClone = attackers[wizardIndex - 1];
                    isCloned = true;
                }
            }
            if (wizardIndex >= 3)
            {
                if (attackers[wizardIndex - 3] is LightWarrior && warriorForClone == null && isCloned == false)
                {
                    warriorForClone = attackers[wizardIndex - 3];
                    isCloned = true;
                }
            }
                
            return warriorForClone;
        }

        public Warrior GetWarriorHeal(List<Warrior> attackers, int healerIndex, Healer healer)
        {
            Warrior warriorForHeal = null;
            var minHealth = double.MaxValue;

                if (healerIndex + 1 < attackers.Count())
                {
                    if (attackers[healerIndex + 1].Healthpoints < minHealth && healerIndex % 2 == 0 && attackers[healerIndex + 1] is not LightWarrior)
                    {
                        minHealth = attackers[healerIndex + 1].Healthpoints;
                        warriorForHeal = attackers[healerIndex + 1];
                    }
                }
                if (healerIndex + 3 < attackers.Count())
                {
                    if (attackers[healerIndex + 3].Healthpoints < minHealth && attackers[healerIndex + 3] is not LightWarrior)
                    {
                        minHealth = attackers[healerIndex + 3].Healthpoints;
                        warriorForHeal = attackers[healerIndex + 3];
                    }
                }
            if (healerIndex >= 1)
            {
                if (attackers[healerIndex - 1].Healthpoints < minHealth && healerIndex % 3 != 0 && attackers[healerIndex - 1] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex - 1].Healthpoints;
                    warriorForHeal = attackers[healerIndex - 1];
                }
            }
            if (healerIndex >= 3)
            {
                if (attackers[healerIndex - 3].Healthpoints < minHealth && attackers[healerIndex - 3] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex - 3].Healthpoints;
                    warriorForHeal = attackers[healerIndex - 3];
                }
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
            { 
                if (attackerIndex == 2)
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
                if (defenders.Count > i)
                {
                        if (attackers[i] is not GulyayGorod && defenders[i] is not GulyayGorod)
                        _battleManagerProxy.Attack(attackers[i], defenders[i], outputTextBox);
                        _battleManagerProxy.IsDead(defenders[i], defenders);
                    }
            }
        }

        public Warrior GetEnemyWarrior(List<Warrior> attackers, List<Warrior> defenders, int archerIndex, Archer archer)
        {
            if (defenders.Count() > 0)
            {
                return defenders.Count() - 1 < archerIndex - 3 ? null : defenders[defenders.Count() - 1];
            }
            return null;
        }

        public Warrior GetWarriorClone(List<Warrior> attackers, int wizardIndex)
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
            if (wizardIndex > 0)
            {
                if (attackers[wizardIndex - 1] is LightWarrior && warriorForClone == null && isCloned == false)
                {
                    warriorForClone = attackers[wizardIndex - 1];
                }
            }
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
