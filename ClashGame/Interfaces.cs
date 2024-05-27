using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClashGame
{
    public interface IBattleStrategy
    {
        public void ExecuteBattle(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox);
        public Warrior GetWarriorHeal(List<Warrior> attackers, int healerIndex, Healer healer);
        public bool IsFrontLine(int attackerIndex, List<Warrior> defenders);

    }
    interface IHealable
    {
        void Heal(Warrior target);
    }

    interface IClonableUnit
    {
        Warrior Clone();
    }

    interface IRangedUnit
    {
        int Range();
        double RangedDamage(int index);
        double RangedAttack(List<Warrior> enemies, int targetIndex, int attackerIndex);
    }

    interface IUnitFactory
    {
        Warrior CreateLightWarrior(string side);
        Warrior CreateHeavyWarrior(string side);
        Warrior CreateArcher(string side);
        Warrior CreateHealer(string side);
        Warrior CreateWizard(string side);
        Warrior CreateGulyayGorod(string side);
    }

    public interface ICommand
    {
        void Execute();
        void Undo();
    }

    public interface IBattleManager
    {
        void TurnComputer(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox);
        void WizardTurn(List<Warrior> attackers, TextBox outputTextBox);
        void HealerTurn(List<Warrior> attackers, TextBox outputTextBox);
        void HeavyWarriorUpgradeTurn(List<Warrior> attackers, Warrior attacker, TextBox outputTextBox);
        void ArchersTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox);
        void Attack(Warrior warrior1, Warrior warrior2, TextBox outputTextBox);
        void DefencePlease(Warrior warrior1, Warrior warrior2, TextBox outputTextBox);
        void IsDead(Warrior warrior, List<Warrior> army);
        void GulyayGorodTurn(List<Warrior> attackers, TextBox outputTextBox);
    }
}