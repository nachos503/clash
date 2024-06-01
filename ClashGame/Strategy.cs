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
    ///<summary>
    /// /// The Default Strategy class, which represents the standard battle strategy.
    /// ID string "T:ClashGame.DefaultStratagy".
    /// </summary>
    public class DefaultStratagy : IBattleStrategy
    {
        /// <summary>
        /// Performs a battle between attacking and defending warriors.
        /// ID string "M:ClashGame.DefaultStratagy.ExecuteBattle(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Forms.TextBox)".
        /// </summary>
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="defenders">List of defending warriors.</param>
        /// <param name="outputTextBox">TextBox for displaying information about the progress of the battle.</param>
        public void ExecuteBattle(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {

        }

        /// <summary>
        /// Gets an enemy to attack the archer.
        /// ID string "M:ClashGame.DefaultStratagy.GetEnemyForArcher(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Int32,ClashGame.Archer)".
        /// </summary>
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="defenders">List of defending warriors.</param>
        /// <param name="archer Index">The archer's index.</param>
        /// <param name="archer">Archer.</param>
        /// <returns>An enemy to attack.</returns>
        public Warrior GetEnemyForArcher(List<Warrior> attackers, List<Warrior> defenders, int archerIndex, Archer archer)
        {
            return null!;
        }

        /// <summary>
        /// Gets the nearest light warrior.
        /// ID string "M:ClashGame.The default strategy.Get the nearest light source(System.Collections.Generic.List{ClashGame.Warrior},System.Int32)".
        /// </short description>
        /// <parameter name="intruders">Spy attacking warriors.</param>
        /// <parameter name="wizardIndex">The magician's index.</parameter>
        /// <returns>The best lightweight user.</returns>
        public Warrior GetNearestLightWarrior(List<Warrior> attackers, int wizardIndex)
        {
            return null!;
        }

        /// <summary>
        /// Receives a warrior for treatment.
        /// ID string "M:ClashGame.DefaultStratagy.GetWarriorForHeal(System.Collections.Generic.List{ClashGame.Warrior},System.Int32,ClashGame.Healer)".
        /// </summary>
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="header Index">The index of the healer.</param>
        /// <param name="healer">The healer.</param>
        /// <returns>A warrior who should be treated.</returns>
        public Warrior GetWarriorForHeal(List<Warrior> attackers, int healerIndex, Healer healer)
        {
            return null!;
        }

        /// <summary>
        /// Checks if the attacking warrior is on the front line.
        /// ID string "M:ClashGame.DefaultStratagy.IsFrontLine(System.Int32,System.Collections.Generic.List{ClashGame.Warrior})".
        /// </summary>
        /// <param name="attackerIndex">The index of the attacking warrior.</param>
        /// <param name="defenders">List of defending warriors.</param>
        /// <returns>True if the warrior is on the front line, otherwise false.</returns>
        public bool IsFrontLine(int attackerIndex, List<Warrior> defenders)
        {
            return false;
        }
    }

    /// <summary>
    /// The twowstrategy class, which represents a two-row combat strategy.
    /// ID string "T:ClashGame.TwoRowStrategy".
    /// </summary>
    public class TwoRowStrategy : IBattleStrategy
    {
        /// <summary>
        /// The BattleManager proxy object.
        /// Search bar "F:ClashGame.Two main strategies._battleManagerProxy".
        /// </short description>
        readonly BattleManagerProxy _battleManagerProxy;

        /// <summary>
        /// Constructor for the TwoRowStrategy class.
        /// Identifier: "M:ClashGame.TwoRowStrategy.#ctor(ClashGame.BattleManagerProxy)".
        /// </summary>
        /// <param name="battleManagerProxy">The BattleManager proxy object.</param>
        public TwoRowStrategy(BattleManagerProxy battleManagerProxy)
        {
            _battleManagerProxy = battleManagerProxy;
        }

        /// <summary>
        /// Performs a battle between attacking and defending warriors.
        /// ID string "M:ClashGame.TwoRowStrategy.ExecuteBattle(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Forms.TextBox)".
        /// </summary>
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="defenders">List of defending warriors.</param>
        /// <param name="outputTextBox">TextBox for displaying information about the progress of the battle.</param>
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

        /// <summary>
        /// Gets an enemy to attack the archer.
        /// ID string "M:ClashGame.TwoRowStrategy.GetEnemyForArcher(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Int32,ClashGame.Archer)".
        /// </summary>
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="defenders">List of defending warriors.</param>
        /// <param name="archer Index">The archer's index.</param>
        /// <param name="archer">Archer.</param>
        /// <returns>An enemy to attack.</returns>
        public Warrior GetEnemyForArcher(List<Warrior> attackers, List<Warrior> defenders, int archerIndex, Archer archer)
        {
            var range = archer.Range();
            //we go from the archer's position to the beginning of his army
            for (int i = archerIndex; i > 1; i -= 2)
                range--;
            // we go from the beginning of the enemy's army until its range ends

            if (range > 0)
                for (int i = 0; i < defenders.Count; i += 2)
                {
                    range--;
                    if (range == 0)
                    {
                        //checking which row the archer is in (shoots only forward)
                        if (archerIndex % 2 == 0) return defenders[i];
                        else if (defenders.Count > i+1)  return defenders[i + 1];
                    }
                }

            return null!;
        }

        /// <summary>
        /// Gets the nearest light warrior.
        /// Identifier: "M:ClashGame.TwoRowStrategy.GetNearestLightWarrior(System.Collections.Generic.List{ClashGame.Warrior},System.Int32)".
        /// </summary>
        /// <param name="attackers">The list of attacking warriors.</param>
        /// <param name="attackerIndex">The index of the attacking warrior.</param>
        /// <returns>The nearest light warrior.</returns>
        public Warrior GetNearestLightWarrior (List<Warrior> attackers, int attackerIndex)
        {
            bool isUsed = false;
            Warrior? nearestLightWarrior = null;
            //// looking up down right to left
            if (attackerIndex != attackers.Count - 1)
                if (attackerIndex % 2 == 0 && attackers[attackerIndex + 1] is LightWarrior)
                {
                    nearestLightWarrior = attackers[attackerIndex + 1];
                    isUsed = true;
                }
            if (attackerIndex < attackers.Count - 2)
                if (attackers[attackerIndex + 2] is  LightWarrior && nearestLightWarrior == null && isUsed == false)
                {
                    nearestLightWarrior = attackers[attackerIndex + 2];
                    isUsed = true;
                }
            if (attackerIndex >= 1)
                if (attackerIndex % 2 != 0 && attackers[attackerIndex - 1] is LightWarrior && nearestLightWarrior == null && isUsed == false)
                {
                    nearestLightWarrior = attackers[attackerIndex - 1];
                    isUsed = true;
                }
            if (attackerIndex >= 2)
                if (attackers[attackerIndex - 2] is LightWarrior && nearestLightWarrior == null && isUsed == false)
                {
                    nearestLightWarrior = attackers[attackerIndex - 2];
                    isUsed = true;
                }

            return nearestLightWarrior!;
        }

        /// <summary>
        /// Gets the warrior for healing.
        /// Identifier: "M:ClashGame.TwoRowStrategy.GetWarriorForHeal(System.Collections.Generic.List{ClashGame.Warrior},System.Int32,ClashGame.Healer)".
        /// </summary>
        /// <param name="attackers">The list of attacking warriors.</param>
        /// <param name="healerIndex">The index of the healer.</param>
        /// <param name="healer">The healer.</param>
        /// <returns>The warrior to be healed.</returns>
        public Warrior GetWarriorForHeal(List<Warrior> attackers, int healerIndex, Healer healer)
        {
            Warrior? warriorForHeal = null;
            var minHealth = double.MaxValue;
            
            if (healerIndex + 1 < attackers.Count)
                if (attackers[healerIndex + 1].Healthpoints < minHealth && healerIndex % 2 == 0 && attackers[healerIndex + 1] is not LightWarrior)
                {
                     minHealth = attackers[healerIndex + 1].Healthpoints;
                     warriorForHeal = attackers[healerIndex + 1];
                }
            if (healerIndex + 2 < attackers.Count)
                if (attackers[healerIndex + 2].Healthpoints < minHealth && attackers[healerIndex + 2] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex + 2].Healthpoints;
                    warriorForHeal = attackers[healerIndex + 2];
                }
            if (healerIndex >= 1)
                if (attackers[healerIndex - 1].Healthpoints < minHealth && healerIndex % 2 != 0 && attackers[healerIndex - 1] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex - 1].Healthpoints;
                    warriorForHeal = attackers[healerIndex - 1];
                }
            if (healerIndex >= 2)
                if (attackers[healerIndex - 2].Healthpoints < minHealth && attackers[healerIndex - 2] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex - 2].Healthpoints;
                    warriorForHeal = attackers[healerIndex - 2];
                }
            
            return warriorForHeal!;
        }

        /// <summary>
        /// Checks if the attacking warrior is on the front line.
        /// Identifier: "M:ClashGame.TwoRowStrategy.IsFrontLine(System.Int32,System.Collections.Generic.List{ClashGame.Warrior})".
        /// </summary>
        /// <param name="attackerIndex">The index of the attacking warrior.</param>
        /// <param name="defenders">The list of defending warriors.</param>
        /// <returns>True if the warrior is on the front line; otherwise, false.</returns>
        public bool IsFrontLine(int attackerIndex, List<Warrior> defenders)
        {
            if (attackerIndex == 0)  return true;
            else if (defenders.Count > 0)
            {
                if (attackerIndex == 1) return true;
                else  return false;
            }
            else return false;
        }
    }

    /// <summary>
    /// The ThreeRowStrategy class, which represents a three-row combat strategy.
    /// ID string "T:ClashGame.ThreeRowStrategy".
    /// </summary>
    public class ThreeRowStrategy : IBattleStrategy
    {
        /// <summary>
        /// Proxy object for BattleManager.
        /// Identifier: "F:ClashGame.ThreeRowStrategy._battleManagerProxy".
        /// </summary>
        readonly BattleManagerProxy _battleManagerProxy;

        /// <summary>
        /// Constructor for the ThreeRowStrategy class.
        /// Identifier: "M:ClashGame.ThreeRowStrategy.#ctor(ClashGame.BattleManagerProxy)".
        /// </summary>
        /// <param name="battleManagerProxy">Proxy object for BattleManager.</param>
        public ThreeRowStrategy(BattleManagerProxy battleManagerProxy)
        {
            _battleManagerProxy = battleManagerProxy;
        }

        /// <summary>
        /// Performs a battle between attacking and defending warriors.
        /// ID string "M:ClashGame.ThreeRowStrategy.ExecuteBattle(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Forms.TextBox)".
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="defenders">List of defending warriors.</param>
        /// <param name="outputTextBox">TextBox for displaying information about the progress of the battle.</param>
        /// </summary>
        public void ExecuteBattle(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            int rows = Math.Min(3, Math.Min(attackers.Count, defenders.Count));
            for (int i = 0; i < rows; i++)
                if (defenders.Count > i)
                {
                    if (attackers[i] is not GulyayGorod && defenders[i] is not GulyayGorod)
                        _battleManagerProxy.Attack(attackers[i], defenders[i], outputTextBox);
                    _battleManagerProxy.IsDead(defenders[i], defenders);
                }
        }

        /// <summary>
        /// Gets an enemy to attack the archer.
        /// ID string "M:ClashGame.ThreeRowStrategy.GetEnemyForArcher(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Int32,ClashGame.Archer)".
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="defenders">List of defending warriors.</param>
        /// <param name="archer Index">The archer's index.</param>
        /// <param name="archer">Archer.</param>
        /// <returns>An enemy to attack.</returns>
        /// </summary>
        public Warrior GetEnemyForArcher(List<Warrior> attackers, List<Warrior> defenders, int archerIndex, Archer archer)
        {
            var range = archer.Range();
            for (int i = archerIndex; i > 1; i -= 3)
                range--;

            if (range > 0)
                for (int i = 0; i < defenders.Count; i += 3)
                {
                    range--;
                    if (range == 0)
                    {
                        if (archerIndex % 3 == 0) return defenders[i];
                        else if (defenders.Count > i + 1 && archerIndex % 3 == 1) return defenders[i + 1];
                        else if (defenders.Count > i + 2) return defenders[i + 2];
                    }
                }

            return null!;
        }

        /// <summary>
        /// Retrieves the nearest light warrior.
        /// Identifier: "M:ClashGame.ThreeRowStrategy.GetNearestLightWarrior(System.Collections.Generic.List{ClashGame.Warrior},System.Int32)".
        /// </summary>
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="attackersIndex">Index of the attacking warrior.</param>
        /// <returns>The nearest light warrior.</returns>
        public Warrior GetNearestLightWarrior(List<Warrior> attackers, int attackersIndex)
        {
            bool isUsed = false;
            Warrior? nearestLightWarrior = null;
            if (attackersIndex != attackers.Count - 1 )
                if (attackersIndex % 2 == 0 && attackers[attackersIndex + 1] is LightWarrior && isUsed == false)
                {
                    nearestLightWarrior = attackers[attackersIndex + 1];
                    isUsed = true;
                }
            if (attackersIndex < attackers.Count - 3)
                if (attackers[attackersIndex + 3] is LightWarrior && nearestLightWarrior == null && isUsed == false)
                {
                    nearestLightWarrior = attackers[attackersIndex + 3];
                    isUsed = true;
                }
            if (attackersIndex >= 1)
                if (attackersIndex % 3 != 0 && attackers[attackersIndex - 1] is LightWarrior && nearestLightWarrior == null && isUsed == false)
                {
                    nearestLightWarrior = attackers[attackersIndex - 1];
                    isUsed = true;
                }
            if (attackersIndex >= 3)
                if (attackers[attackersIndex - 3] is LightWarrior && nearestLightWarrior == null && isUsed == false)
                {
                    nearestLightWarrior = attackers[attackersIndex - 3];
                    isUsed = true;
                }
                
            return nearestLightWarrior!;
        }

        /// <summary>
        /// Retrieves a warrior for healing.
        /// Identifier: "M:ClashGame.ThreeRowStrategy.GetWarriorForHeal(System.Collections.Generic.List{ClashGame.Warrior},System.Int32,ClashGame.Healer)".
        /// </summary>
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="healerIndex">Index of the healer.</param>
        /// <param name="healer">The healer.</param>
        /// <returns>The warrior to be healed.</returns>

        public Warrior GetWarriorForHeal(List<Warrior> attackers, int healerIndex, Healer healer)
        {
            Warrior? warriorForHeal = null;
            var minHealth = double.MaxValue;

            if (healerIndex + 1 < attackers.Count)
                if (attackers[healerIndex + 1].Healthpoints < minHealth && healerIndex % 2 == 0 && attackers[healerIndex + 1] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex + 1].Healthpoints;
                    warriorForHeal = attackers[healerIndex + 1];
                }
            if (healerIndex + 3 < attackers.Count)
                if (attackers[healerIndex + 3].Healthpoints < minHealth && attackers[healerIndex + 3] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex + 3].Healthpoints;
                    warriorForHeal = attackers[healerIndex + 3];
                }
            if (healerIndex >= 1)
                if (attackers[healerIndex - 1].Healthpoints < minHealth && healerIndex % 3 != 0 && attackers[healerIndex - 1] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex - 1].Healthpoints;
                    warriorForHeal = attackers[healerIndex - 1];
                }
            if (healerIndex >= 3)
                if (attackers[healerIndex - 3].Healthpoints < minHealth && attackers[healerIndex - 3] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex - 3].Healthpoints;
                    warriorForHeal = attackers[healerIndex - 3];
                }

            return warriorForHeal!;
        }

        /// <summary>
        /// Checks if the attacking warrior is on the front line.
        /// Identifier: "M:ClashGame.ThreeRowStrategy.IsFrontLine(System.Int32,System.Collections.Generic.List{ClashGame.Warrior})".
        /// </summary>
        /// <param name="attackerIndex">Index of the attacking warrior.</param>
        /// <param name="defenders">List of defending warriors.</param>
        /// <returns>True if the warrior is on the front line, otherwise false.</returns>

        public bool IsFrontLine(int attackerIndex, List<Warrior> defenders)
        {
            if (attackerIndex == 0)  return true;
            if (defenders.Count > 0)
                if (attackerIndex == 1) return true;
                
            if (defenders.Count > 1)
            { 
                if (attackerIndex == 2)  return true;
                else  return false;
            }
            else return false;
        }
    }

    /// <summary>
    /// The WallToWallStrategy class representing the "wall-to-wall" battle strategy.
    /// Identifier: "T:ClashGame.WallToWallStrategy".
    /// </summary>

    public class WallToWallStrategy : IBattleStrategy
    {
        /// <summary>
        /// Proxy object for BattleManager.
        /// Identifier: "F:ClashGame.WallToWallStrategy._battleManagerProxy".
        /// </summary>
        readonly BattleManagerProxy _battleManagerProxy;

        /// <summary>
        /// Constructor for the WallToWallStrategy class.
        /// Identifier: "M:ClashGame.WallToWallStrategy.#ctor(ClashGame.BattleManagerProxy)".
        /// </summary>
        /// <param name="battleManagerProxy">Proxy object for BattleManager.</param>
        public WallToWallStrategy(BattleManagerProxy battleManagerProxy)
        {
            _battleManagerProxy = battleManagerProxy;
        }

        /// <summary>
        /// Executes the battle between attacking and defending warriors.
        /// Identifier: "M:ClashGame.WallToWallStrategy.ExecuteBattle(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Forms.TextBox)".
        /// </summary>
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="defenders">List of defending warriors.</param>
        /// <param name="outputTextBox">TextBox for displaying battle information.</param>
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

        /// <summary>
        /// Retrieves the enemy for the archer to attack.
        /// Identifier: "M:ClashGame.WallToWallStrategy.GetEnemyForArcher(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Int32,ClashGame.Archer)".
        /// </summary>
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="defenders">List of defending warriors.</param>
        /// <param name="archerIndex">Index of the archer.</param>
        /// <param name="archer">The archer.</param>
        /// <returns>The enemy to attack.</returns>
        public Warrior GetEnemyForArcher(List<Warrior> attackers, List<Warrior> defenders, int archerIndex, Archer archer)
        {
            if (defenders.Count == 0)
                return null!;

            if (defenders.Count - 1 < archerIndex - 3)
                return null!;
            else
                return defenders[^1];
        }
        /// <summary>
        /// Retrieves the nearest light warrior.
        /// Identifier: "M:ClashGame.WallToWallStrategy.GetNearestLightWarrior(System.Collections.Generic.List{ClashGame.Warrior},System.Int32)".
        /// </summary>
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="wizardIndex">Index of the wizard.</param>
        /// <returns>The nearest light warrior.</returns>
        public Warrior GetNearestLightWarrior(List<Warrior> attackers, int wizardIndex)
        {
            bool isUsed = false;
            Warrior? nearestLightWarrior = null;
            if (wizardIndex != attackers.Count - 1 && wizardIndex != attackers.Count - 2)
                if (attackers[wizardIndex + 1] is LightWarrior && isUsed == false)
                    nearestLightWarrior = attackers[wizardIndex + 1];
                
            if (wizardIndex > 0)
                if (attackers[wizardIndex - 1] is LightWarrior && nearestLightWarrior == null && isUsed == false)
                    nearestLightWarrior = attackers[wizardIndex - 1];
                
            return nearestLightWarrior!;
        }

        /// <summary>
        /// Retrieves a warrior for healing.
        /// Identifier: "M:ClashGame.WallToWallStrategy.GetWarriorForHeal(System.Collections.Generic.List{ClashGame.Warrior},System.Int32,ClashGame.Healer)".
        /// </summary>
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="healerIndex">Index of the healer.</param>
        /// <param name="healer">The healer.</param>
        /// <returns>The warrior to heal.</returns>
        public Warrior GetWarriorForHeal(List<Warrior> attackers, int healerIndex, Healer healer)
        {
            Warrior? warriorForHeal = null;
            var minHealth = double.MaxValue;
            if (healerIndex != attackers.Count - 1 && healerIndex != attackers.Count - 2)
                if (attackers[healerIndex + 1].Healthpoints < minHealth && attackers[healerIndex + 1] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex + 1].Healthpoints;
                    warriorForHeal = attackers[healerIndex + 1];
                }
            if (healerIndex >= 1)
                if (attackers[healerIndex - 1].Healthpoints < minHealth && attackers[healerIndex - 1] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex - 1].Healthpoints;
                    warriorForHeal = attackers[healerIndex - 1];
                }
          
            return warriorForHeal!;
        }

        /// <summary>
        /// Checks if the attacking warrior is on the front line.
        /// Identifier: "M:ClashGame.WallToWallStrategy.IsFrontLine(System.Int32,System.Collections.Generic.List{ClashGame.Warrior})".
        /// </summary>
        /// <param name="attackerIndex">The index of the attacking warrior.</param>
        /// <param name="defenders">The list of defending warriors.</param>
        /// <returns>True if the warrior is on the front line, otherwise false.</returns>
        public bool IsFrontLine(int attackerIndex, List<Warrior> defenders)
        {
            if (defenders.Count - 1 > attackerIndex) return true;
            else return false;
        }
    }
}
