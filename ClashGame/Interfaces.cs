using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClashGame
{
    /// <summary>
    /// interface IHealable - интерфейс для объектов, которые могут лечить.
    /// Строка идентификатора "T:ClashGame.IHealable".
    /// </summary>
    interface IHealable
    {
        /// <summary>
        /// void Heal(Warrior target) - метод для лечения воина.
        /// Строка идентификатора "M:ClashGame.IHealable.Heal(ClashGame.Warrior)".
        /// </summary>
        /// <param name="target">Целевой воин для лечения.</param>
        void Heal(Warrior target);
    }

    /// <summary>
    /// interface IClonableUnit - интерфейс для объектов, которые могут клонировать. Прототип
    /// Строка идентификатора "T:ClashGame.IClonableUnit".
    /// </summary>
    interface IClonableUnit
    {
        /// <summary>
        /// Warrior Clone() - метод для клонирования юнита.
        /// Строка идентификатора "M:ClashGame.IClonableUnit.Clone".
        /// </summary>
        /// <returns>Клон юнита.</returns>
        Warrior Clone();
    }

    /// <summary>
    /// interface IRangedUnit - интерфейс для объектов, которые могут рассчитывать дальность атаки.
    /// Строка идентификатора "T:ClashGame.IRangedUnit".
    /// </summary>
    interface IRangedUnit
    {
        /// <summary>
        /// int Range() - метод для получения дальности атаки.
        /// Строка идентификатора "M:ClashGame.IRangedUnit.Range".
        /// </summary>
        /// <returns>Дальность атаки.</returns>
        int Range();

        /// <summary>
        /// double RangedDamage(int index) - метод для получения урона на дальнем расстоянии.
        /// Строка идентификатора "M:ClashGame.IRangedUnit.RangedDamage(System.Int32)".
        /// </summary>
        /// <param name="index">Индекс атакующего.</param>
        /// <returns>Урон на дальнем расстоянии.</returns>
        double RangedDamage(int index);

        /// <summary>
        /// double RangedAttack(List<Warrior> enemies, Warrior target, int attackerIndex) - метод для выполнения дальнобойной атаки.
        /// Строка идентификатора "M:ClashGame.IRangedUnit.RangedAttack(System.Collections.Generic.List{ClashGame.Warrior},ClashGame.Warrior,System.Int32)".
        /// </summary>
        /// <param name="enemies">Список врагов.</param>
        /// <param name="target">Целевой воин для атаки.</param>
        /// <param name="attackerIndex">Индекс атакующего.</param>
        /// <returns>Результат дальнобойной атаки.</returns>
        double RangedAttack(List<Warrior> enemies, Warrior target, int attackerIndex);
    }


    /// <summary>
    /// interface IUnitFactory - интерфейс для юнитов. Паттерн фабрика
    /// Строка идентификатора "T:ClashGame.IUnitFactory".
    /// </summary>
    interface IUnitFactory
    {
        /// <summary>
        /// Warrior CreateLightWarrior(string side) - метод для создания легкого воина.
        /// Строка идентификатора "M:ClashGame.IUnitFactory.CreateLightWarrior(System.String)".
        /// </summary>
        /// <param name="side">Сторона, для которой создается юнит.</param>
        /// <returns>Легкий воин.</returns>
        Warrior CreateLightWarrior(string side);

        /// <summary>
        /// Warrior CreateHeavyWarrior(string side) - метод для создания тяжелого воина.
        /// Строка идентификатора "M:ClashGame.IUnitFactory.CreateHeavyWarrior(System.String)".
        /// </summary>
        /// <param name="side">Сторона, для которой создается юнит.</param>
        /// <returns>Тяжелый воин.</returns>
        Warrior CreateHeavyWarrior(string side);

        /// <summary>
        /// Warrior CreateArcher(string side) - метод для создания лучника.
        /// Строка идентификатора "M:ClashGame.IUnitFactory.CreateArcher(System.String)".
        /// </summary>
        /// <param name="side">Сторона, для которой создается юнит.</param>
        /// <returns>Лучник.</returns>
        Warrior CreateArcher(string side);

        /// <summary>
        /// Warrior CreateHealer(string side) - метод для создания лекаря.
        /// Строка идентификатора "M:ClashGame.IUnitFactory.CreateHealer(System.String)".
        /// </summary>
        /// <param name="side">Сторона, для которой создается юнит.</param>
        /// <returns>Лекарь.</returns>
        Warrior CreateHealer(string side);

        /// <summary>
        /// Warrior CreateWizard(string side) - метод для создания мага.
        /// Строка идентификатора "M:ClashGame.IUnitFactory.CreateWizard(System.String)".
        /// </summary>
        /// <param name="side">Сторона, для которой создается юнит.</param>
        /// <returns>Маг.</returns>
        Warrior CreateWizard(string side);

        /// <summary>
        /// Warrior CreateGulyayGorod(string side) - метод для создания "Гуляй-город".
        /// Строка идентификатора "M:ClashGame.IUnitFactory.CreateGulyayGorod(System.String)".
        /// </summary>
        /// <param name="side">Сторона, для которой создается юнит.</param>
        /// <returns>Гуляй-город.</returns>
        Warrior CreateGulyayGorod(string side);
    }

    /// <summary>
    /// public interface IBattleManager - интерфейс для менеджера битвы. Паттерн прокси
    /// Строка идентификатора "T:ClashGame.IBattleManager".
    /// </summary>
    public interface IBattleManager
    {
        /// <summary>
        /// void Attack(Warrior warrior1, Warrior warrior2, TextBox outputTextBox) - метод для выполнения атаки.
        /// Строка идентификатора "M:ClashGame.IBattleManager.Attack(ClashGame.Warrior,ClashGame.Warrior,System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="warrior1">Атакующий воин.</param>
        /// <param name="warrior2">Защитник воин.</param>
        /// <param name="outputTextBox">TextBox для вывода информации об атаке.</param>
        void Attack(Warrior warrior1, Warrior warrior2, TextBox outputTextBox);

        /// <summary>
        /// void Defence(Warrior warrior1, Warrior warrior2, TextBox outputTextBox) - метод для обработки защиты.
        /// Строка идентификатора "M:ClashGame.IBattleManager.Defence(ClashGame.Warrior,ClashGame.Warrior,System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="warrior1">Атакующий воин.</param>
        /// <param name="warrior2">Защитник воин.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о защите.</param>
        void Defence(Warrior warrior1, Warrior warrior2, TextBox outputTextBox);

        /// <summary>
        /// void IsDead(Warrior warrior, List<Warrior> army) - метод для проверки, мертв ли воин.
        /// Строка идентификатора "M:ClashGame.IBattleManager.IsDead(ClashGame.Warrior,System.Collections.Generic.List{ClashGame.Warrior})".
        /// </summary>
        /// <param name="warrior">Воин для проверки.</param>
        /// <param name="army">Список армии.</param>
        void IsDead(Warrior warrior, List<Warrior> army);

        /// <summary>
        /// void TurnComputer(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox) - метод для выполнения хода компьютера.
        /// Строка идентификатора "M:ClashGame.IBattleManager.TurnComputer(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих юнитов.</param>
        /// <param name="defenders">Список защищающихся юнитов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе.</param>
        void TurnComputer(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox);

        /// <summary>
        /// void WizardTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox) - метод для выполнения хода мага.
        /// Строка идентификатора "M:ClashGame.IBattleManager.WizardTurn(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих юнитов.</param>
        /// <param name="defenders">Список защищающихся юнитов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе мага
        void WizardTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox);

        /// <summary>
        /// void HealerTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox) - метод для выполнения хода лекаря.
        /// Строка идентификатора "M:ClashGame.IBattleManager.HealerTurn(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Forms.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе.</param>
        void HealerTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox);

        /// <summary>
        /// void ImprovedHeavyWarriorTurn(List<Warrior> attackers, Warrior attacker, TextBox outputTextBox) - метод для выполнения хода улучшенного тяжеловооруженного воина.
        /// Строка идентификатора "M:ClashGame.IBattleManager.ImprovedHeavyWarriorTurn(System.Collections.Generic.List{ClashGame.Warrior},ClashGame.Warrior,System.Windows.Forms.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="attacker">Текущий атакующий воин.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе.</param>
        void ImprovedHeavyWarriorTurn(List<Warrior> attackers, Warrior attacker, TextBox outputTextBox);

        /// <summary>
        /// void ArchersTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox) - метод для выполнения хода лучников.
        /// Строка идентификатора "M:ClashGame.IBattleManager.ArchersTurn(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Forms.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе.</param>
        void ArchersTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox);

        /// <summary>
        /// void GulyayGorodTurn(List<Warrior> attackers, TextBox outputTextBox) - метод для выполнения хода гуляй-города.
        /// Строка идентификатора "M:ClashGame.IBattleManager.GulyayGorodTurn(System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Forms.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе.</param>
        void GulyayGorodTurn(List<Warrior> attackers, TextBox outputTextBox);
    }

    /// <summary>
    /// Интерфейс ICommand представляет паттерн команду с возможностью выполнения и отмены. Паттерн команда
    /// Строка идентификатора "T:ClashGame.ICommand".
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Выполняет команду.
        /// Строка идентификатора "M:ClashGame.ICommand.Execute".
        /// </summary>
        void Execute();

        /// <summary>
        /// Отменяет выполнение команды.
        /// Строка идентификатора "M:ClashGame.ICommand.Undo".
        /// </summary>
        void Undo();
    }

    /// <summary>
    /// Интерфейс IBattleStrategy представляет стратегию битвы, включающую выполнение битвы и выбор целей. Паттерн стратегия
    /// Строка идентификатора "T:ClashGame.IBattleStrategy".
    /// </summary>
    public interface IBattleStrategy
    {
        /// <summary>
        /// Выполняет битву между атакующими и защищающимися воинами.
        /// Строка идентификатора "M:ClashGame.IBattleStrategy.ExecuteBattle(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Forms.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе битвы.</param>
        public void ExecuteBattle(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox);

        /// <summary>
        /// Получает воина для лечения.
        /// Строка идентификатора "M:ClashGame.IBattleStrategy.GetWarriorForHeal(System.Collections.Generic.List{ClashGame.Warrior},System.Int32,ClashGame.Healer)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="healerIndex">Индекс лекаря.</param>
        /// <param name="healer">Лекарь.</param>
        /// <returns>Воин, которого следует лечить.</returns>
        public Warrior GetWarriorForHeal(List<Warrior> attackers, int healerIndex, Healer healer);

        /// <summary>
        /// Получает врага для атаки лучника.
        /// Строка идентификатора "M:ClashGame.IBattleStrategy.GetEnemyForArcher(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Int32,ClashGame.Archer)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <param name="archerIndex">Индекс лучника.</param>
        /// <param name="archer">Лучник.</param>
        /// <returns>Враг для атаки.</returns>
        public Warrior GetEnemyForArcher(List<Warrior> attackers, List<Warrior> defenders, int archerIndex, Archer archer);

        /// <summary>
        /// Получает ближайшего легкого воина.
        /// Строка идентификатора "M:ClashGame.IBattleStrategy.GetNearestLightWarrior(System.Collections.Generic.List{ClashGame.Warrior},System.Int32)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="attackersIndex">Индекс атакующего воина.</param>
        /// <returns>Ближайший легкий воин.</returns>
        public Warrior GetNearestLightWarrior(List<Warrior> attackers, int attackersIndex);

        /// <summary>
        /// Проверяет, является ли атакующий воин на передовой линии.
        /// Строка идентификатора "M:ClashGame.IBattleStrategy.IsFrontLine(System.Int32,System.Collections.Generic.List{ClashGame.Warrior})".
        /// </summary>
        /// <param name="attackerIndex">Индекс атакующего воина.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <returns>True, если воин находится на передовой линии, иначе false.</returns>
        public bool IsFrontLine(int attackerIndex, List<Warrior> defenders);

    }
}