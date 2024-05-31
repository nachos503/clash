using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClashGame
{
    /// <summary>
    /// interface IHealable - interface for objects that can be treated.
    /// ID string "T:ClashGame.IHealable".
    /// </summary>
    interface IHealable
    {
        /// <summary>
        /// void Heal(Warrior target) is a method for treating a warrior.
        /// ID string "M:ClashGame.IHealable.Heal(ClashGame.Warrior)".
        /// </summary>
        /// <param name="target">Target warrior for treatment.</param>
        void Heal(Warrior target);
    }

    /// <summary>
    /// interface IClonableUnit - interface for objects that can clone. Prototype
    /// Identifier string "T:ClashGame.IClonableUnit".
    /// </summary>
    interface IClonableUnit
    {
        ///<summary>
        /// Warrior Clone() is a method for cloning a unit.
        /// ID string "M:ClashGame.IClonableUnit.Clone".
        /// </summary>
        /// <returns>A clone of the unit.</returns>
        Warrior Clone();
    }

    /// <summary>
    /// interface IRangedUnit is an interface for objects that can calculate the attack range.
    /// ID string "T:ClashGame.IRangedUnit".
    /// </summary>
    interface IRangedUnit
    {
        /// <summary>
        /// int Range() is a method for getting the attack range.
        /// ID string "M:ClashGame.IRangedUnit.Range".
        /// </summary>
        /// <returns>Attack range.</returns>
        int Range();

        /// <summary>
        /// double Ranged Damage(int index) is a method for taking long-range damage.
        /// ID string "M:ClashGame.IRangedUnit.RangedDamage(System.Int32)".
        /// </summary>
        /// <param name="index">The attacker's index.</param>
        /// <returns>Long-range damage.</returns>
        double RangedDamage(int index);

        /// <summary>
        /// double Ranged Attack(Last<Warrior> enemies, Warrior target, int attacker Index) is a method for performing a long-range attack.
        /// ID string "M:ClashGame.IRangedUnit.RangedAttack(System.Collections.Generic.List{ClashGame.Warrior},ClashGame.Warrior,System.Int32)".
        /// </summary>
        /// <param name="enemies">List of enemies.</param>
        /// <param name="target">Target warrior to attack.</param>
        /// <param name="attackerIndex">The attacker's index.</param>
        /// <returns>The result of a long-range attack.</returns>
        double RangedAttack(List<Warrior> enemies, Warrior target, int attackerIndex);
    }


    /// <summary>
    /// interface Junitfactory - interface for units. Pattern factory
    /// ID string "T:ClashGame.IUnitFactory".
    /// </summary
    interface IUnitFactory
    {
        /// <summary>
        /// Warrior CreateLightWarrior(string side) - method for creating a light warrior.
        /// Identifier string: "M:ClashGame.IUnitFactory.CreateLightWarrior(System.String)".
        /// </summary>
        /// <param name="side">The side for which the unit is created.</param>
        /// <returns>The light warrior.</returns>
        Warrior CreateLightWarrior(string side);

        /// <summary>
        /// Warrior CreateHeavyWarrior(string side) - method for creating a heavy warrior.
        /// Identifier string: "M:ClashGame.IUnitFactory.CreateHeavyWarrior(System.String)".
        /// </summary>
        /// <param name="side">The side for which the unit is created.</param>
        /// <returns>The heavy warrior.</returns>
        Warrior CreateHeavyWarrior(string side);

        /// <summary>
        /// Warrior CreateArcher(string side) - method for creating an archer.
        /// Identifier string: "M:ClashGame.IUnitFactory.CreateArcher(System.String)".
        /// </summary>
        /// <param name="side">The side for which the unit is created.</param>
        /// <returns>The archer.</returns>
        Warrior CreateArcher(string side);

        /// <summary>
        /// Warrior CreateHealer(string side) - method for creating a healer.
        /// Identifier string: "M:ClashGame.IUnitFactory.CreateHealer(System.String)".
        /// </summary>
        /// <param name="side">The side for which the unit is created.</param>
        /// <returns>The healer.</returns>
        Warrior CreateHealer(string side);

        /// <summary>
        /// Warrior CreateWizard(string side) - method for creating a wizard.
        /// Identifier string: "M:ClashGame.IUnitFactory.CreateWizard(System.String)".
        /// </summary>
        /// <param name="side">The side for which the unit is created.</param>
        /// <returns>The wizard.</returns>
        Warrior CreateWizard(string side);

        /// <summary>
        /// Warrior CreateGulyayGorod(string side) - method for creating a "Gulyay-gorod".
        /// Identifier string: "M:ClashGame.IUnitFactory.CreateGulyayGorod(System.String)".
        /// </summary>
        /// <param name="side">The side for which the unit is created.</param>
        /// <returns>The "Gulyay-gorod".</returns>
        Warrior CreateGulyayGorod(string side);
    }

    /// <summary>
    /// public interface IBattleManager - interface for the battle manager. Proxy pattern.
    /// Identifier string: "T:ClashGame.IBattleManager".
    /// </summary>
    public interface IBattleManager
    {
        /// <summary>
        /// void Attack(Warrior warrior1, Warrior warrior2, TextBox outputTextBox) - method to execute an attack.
        /// Identifier string: "M:ClashGame.IBattleManager.Attack(ClashGame.Warrior,ClashGame.Warrior,System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="warrior1">Attacking warrior.</param>
        /// <param name="warrior2">Defending warrior.</param>
        /// <param name="outputTextBox">TextBox for displaying attack information.</param>
        void Attack(Warrior warrior1, Warrior warrior2, TextBox outputTextBox);

        /// <summary>
        /// void Defence(Warrior warrior1, Warrior warrior2, TextBox outputTextBox) - method for handling defense.
        /// Identifier string: "M:ClashGame.IBattleManager.Defence(ClashGame.Warrior,ClashGame.Warrior,System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="warrior1">Attacking warrior.</param>
        /// <param name="warrior2">Defending warrior.</param>
        /// <param name="outputTextBox">TextBox for displaying defense information.</param>
        void Defence(Warrior warrior1, Warrior warrior2, TextBox outputTextBox);

        /// <summary>
        /// void IsDead(Warrior warrior, List<Warrior> army) - method to check if a warrior is dead.
        /// Identifier string: "M:ClashGame.IBattleManager.IsDead(ClashGame.Warrior,System.Collections.Generic.List{ClashGame.Warrior})".
        /// </summary>
        /// <param name="warrior">Warrior to check.</param>
        /// <param name="army">List of warriors.</param>
        void IsDead(Warrior warrior, List<Warrior> army);

        /// <summary>
        /// void TurnComputer(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox) - method to execute the computer's turn.
        /// Identifier string: "M:ClashGame.IBattleManager.TurnComputer(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">List of attacking units.</param>
        /// <param name="defenders">List of defending units.</param>
        /// <param name="outputTextBox">TextBox for displaying turn information.</param>
        void TurnComputer(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox);

        /// <summary>
        /// void WizardTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox) - method to execute the wizard's turn.
        /// Identifier string: "M:ClashGame.IBattleManager.WizardTurn(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">List of attacking units.</param>
        /// <param name="defenders">List of defending units.</param>
        /// <param name="outputTextBox">TextBox for displaying wizard's turn information.</param>
        void WizardTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox);

        /// <summary>
        /// void HealerTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox) - method to execute the healer's turn.
        /// Identifier string: "M:ClashGame.IBattleManager.HealerTurn(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Forms.TextBox)".
        /// </summary>
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="defenders">List of defending warriors.</param>
        /// <param name="outputTextBox">TextBox for displaying turn information.</param>
        void HealerTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox);

        /// <summary>
        /// void ImprovedHeavyWarriorTurn(List<Warrior> attackers, Warrior attacker, TextBox outputTextBox) - method to execute the improved heavy warrior's turn.
        /// Identifier string: "M:ClashGame.IBattleManager.ImprovedHeavyWarriorTurn(System.Collections.Generic.List{ClashGame.Warrior},ClashGame.Warrior,System.Windows.Forms.TextBox)".
        /// </summary>
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="attacker">Current attacking warrior.</param>
        /// <param name="outputTextBox">TextBox for displaying turn information.</param>
        void ImprovedHeavyWarriorTurn(List<Warrior> attackers, Warrior attacker, TextBox outputTextBox);

        /// <summary>
        /// void ArchersTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox) - method to execute the archers' turn.
        /// Identifier string: "M:ClashGame.IBattleManager.ArchersTurn(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Forms.TextBox)".
        /// </summary>
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="defenders">List of defending warriors.</param>
        /// <param name="outputTextBox">TextBox for displaying turn information.</param>
        void ArchersTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox);

        /// <summary>
        /// void GulyayGorodTurn(List<Warrior> attackers, TextBox outputTextBox) - method to execute the Gulyay-Gorod's turn.
        /// Identifier string: "M:ClashGame.IBattleManager.GulyayGorodTurn(System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Forms.TextBox)".
        /// </summary>
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="outputTextBox">TextBox for displaying turn information.</param>
        void GulyayGorodTurn(List<Warrior> attackers, TextBox outputTextBox);
    }

    /// <summary>
    /// The ICommand interface represents the command pattern with the ability to execute and undo. Command Pattern
    /// Identifier string: "T:ClashGame.ICommand".
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes the command.
        /// Identifier string: "M:ClashGame.ICommand.Execute".
        /// </summary>
        void Execute();

        /// <summary>
        /// Undoes the execution of the command.
        /// Identifier string: "M:ClashGame.ICommand.Undo".
        /// </summary>
        void Undo();
    }

    /// <summary>
    /// The IBattleStrategy interface represents the battle strategy, including battle execution and target selection. Strategy Pattern
    /// Identifier string: "T:ClashGame.IBattleStrategy".
    /// </summary>
    public interface IBattleStrategy
    {
        /// <summary>
        /// Executes the battle between attacking and defending warriors.
        /// Identifier: "M:ClashGame.IBattleStrategy.ExecuteBattle(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Forms.TextBox)".
        /// </summary>
        /// <param name="attackers">The list of attacking warriors.</param>
        /// <param name="defenders">The list of defending warriors.</param>
        /// <param name="outputTextBox">The TextBox for displaying battle information.</param>
        public void ExecuteBattle(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox);

        /// <summary>
        /// Gets a warrior for healing.
        /// Identifier: "M:ClashGame.IBattleStrategy.GetWarriorForHeal(System.Collections.Generic.List{ClashGame.Warrior},System.Int32,ClashGame.Healer)".
        /// </summary>
        /// <param name="attackers">The list of attacking warriors.</param>
        /// <param name="healerIndex">The index of the healer.</param>
        /// <param name="healer">The healer.</param>
        /// <returns>The warrior to heal.</returns>
        public Warrior GetWarriorForHeal(List<Warrior> attackers, int healerIndex, Healer healer);

        /// <summary>
        /// Gets an enemy for archer attack.
        /// Identifier: "M:ClashGame.IBattleStrategy.GetEnemyForArcher(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Int32,ClashGame.Archer)".
        /// </summary>
        /// <param name="attackers">The list of attacking warriors.</param>
        /// <param name="defenders">The list of defending warriors.</param>
        /// <param name="archerIndex">The index of the archer.</param>
        /// <param name="archer">The archer.</param>
        /// <returns>The enemy for attack.</returns>
        public Warrior GetEnemyForArcher(List<Warrior> attackers, List<Warrior> defenders, int archerIndex, Archer archer);

        /// <summary>
        /// Gets the nearest light warrior.
        /// Identifier: "M:ClashGame.IBattleStrategy.GetNearestLightWarrior(System.Collections.Generic.List{ClashGame.Warrior},System.Int32)".
        /// </summary>
        /// <param name="attackers">The list of attacking warriors.</param>
        /// <param name="attackersIndex">The index of the attacking warrior.</param>
        /// <returns>The nearest light warrior.</returns>
        public Warrior GetNearestLightWarrior(List<Warrior> attackers, int attackersIndex);

        /// <summary>
        /// Checks if the attacking warrior is on the front line.
        /// Identifier: "M:ClashGame.IBattleStrategy.IsFrontLine(System.Int32,System.Collections.Generic.List{ClashGame.Warrior})".
        /// </summary>
        /// <param name="attackerIndex">The index of the attacking warrior.</param>
        /// <param name="defenders">The list of defending warriors.</param>
        /// <returns>True if the warrior is on the front line, otherwise false.</returns>
        public bool IsFrontLine(int attackerIndex, List<Warrior> defenders);

    }
}