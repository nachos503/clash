using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClashGame
{
    //чтобы лечить
    interface IHealable
    {
        void Heal(Warrior target);
    }
    //чтобы клонировать - протитип
    interface IClonableUnit
    {
        Warrior Clone();
    }
    //чтобы расчитывать дальность
    interface IRangedUnit
    {
        int Range();
        double RangedDamage(int index);
        double RangedAttack(List<Warrior> enemies, Warrior target, int attackerIndex);
    }
    //чтобы чапкин отъебался - фабрика
    interface IUnitFactory
    {
        Warrior CreateLightWarrior(string side);
        Warrior CreateHeavyWarrior(string side);
        Warrior CreateArcher(string side);
        Warrior CreateHealer(string side);
        Warrior CreateWizard(string side);
        Warrior CreateGulyayGorod(string side);
    }
    //чтобы чапкин отъебался 2 - прокси
    public interface IBattleManager
    {
        void Attack(Warrior warrior1, Warrior warrior2, TextBox outputTextBox);
        void Defence(Warrior warrior1, Warrior warrior2, TextBox outputTextBox);
        void IsDead(Warrior warrior, List<Warrior> army);
        void TurnComputer(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox);
        void WizardTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox);
        void HealerTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox);
        void ImprovedHeavyWarriorTurn(List<Warrior> attackers, Warrior attacker, TextBox outputTextBox);
        void ArchersTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox);
        void GulyayGorodTurn(List<Warrior> attackers, TextBox outputTextBox);
    }

    //чтобы чапкин отъебался 3 - команд
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
    //чтоб чапкин сдох дот нет - стратегия
    public interface IBattleStrategy
    {
        public void ExecuteBattle(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox);
        public Warrior GetWarriorForHeal(List<Warrior> attackers, int healerIndex, Healer healer);
        public Warrior GetEnemyForArcher(List<Warrior> attackers, List<Warrior> defenders, int archerIndex, Archer archer);
        public Warrior GetNearestLightWarrior(List<Warrior> attackers, int attackersIndex);
        public bool IsFrontLine(int attackerIndex, List<Warrior> defenders);

    }
}