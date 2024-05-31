using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClashGame
{
    /// <summary>
    /// Класс CommandManager, управляющий выполнением и отменой команд.
    /// Строка идентификатора "T:ClashGame.CommandManager".
    /// </summary>
    public class CommandManager
    {
        /// <summary>
        /// Стек выполненных команд.
        /// Строка идентификатора "F:ClashGame.CommandManager._commands".
        /// </summary>
        private Stack<ICommand> _commands = new Stack<ICommand>();

        /// <summary>
        /// Выполняет команду и добавляет её в стек.
        /// Строка идентификатора "M:ClashGame.CommandManager.ExecuteCommand(ClashGame.ICommand)".
        /// </summary>
        /// <param name="command">Команда для выполнения.</param>
        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _commands.Push(command);
        }

        /// <summary>
        /// Отменяет последнюю выполненную команду.
        /// Строка идентификатора "M:ClashGame.CommandManager.Undo".
        /// </summary>
        public void Undo()
        {
            if (_commands.Count > 0)
            {
                ICommand command = _commands.Pop();
                command.Undo();
            }
        }

        /// <summary>
        /// Проверяет, можно ли отменить последнюю выполненную команду.
        /// Строка идентификатора "M:ClashGame.CommandManager.CanUndo".
        /// </summary>
        /// <returns>True, если есть команды для отмены, иначе false.</returns>
        public bool CanUndo()
        {
            return _commands.Count > 0;
        }
    }

    /// <summary>
    /// Класс HealerTurnCommand, представляющий команду хода лекаря.
    /// Строка идентификатора "T:ClashGame.HealerTurnCommand".
    /// </summary>
    public class HealerTurnCommand : ICommand
    {
        /// <summary>
        /// Управляющий битвой.
        /// Строка идентификатора "F:ClashGame.HealerTurnCommand._battleManager".
        /// </summary>
        private IBattleManager _battleManager;

        /// <summary>
        /// Список атакующих воинов.
        /// Строка идентификатора "F:ClashGame.HealerTurnCommand._attackers".
        /// </summary>
        private List<Warrior> _attackers;

        /// <summary>
        /// Список защищающихся воинов.
        /// Строка идентификатора "F:ClashGame.HealerTurnCommand._defenders".
        /// </summary>
        private List<Warrior> _defenders;

        /// <summary>
        /// TextBox для вывода информации о ходе битвы.
        /// Строка идентификатора "F:ClashGame.HealerTurnCommand._outputTextBox".
        /// </summary>
        private TextBox _outputTextBox;

        /// <summary>
        /// Список воинов, представляющий их состояние до выполнения команды.
        /// Строка идентификатора "F:ClashGame.HealerTurnCommand._previousState".
        /// </summary>
        private List<Warrior> _previousState;

        /// <summary>
        /// Конструктор для класса HealerTurnCommand.
        /// Строка идентификатора "M:ClashGame.HealerTurnCommand.#ctor(ClashGame.IBattleManager,System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Forms.TextBox)".
        /// </summary>
        /// <param name="battleManager">Управляющий битвой.</param>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе битвы.</param>
        public HealerTurnCommand(IBattleManager battleManager, List<Warrior> attackers, List<Warrior> defenders,TextBox outputTextBox)
        {
            _battleManager = battleManager;
            _attackers = attackers;
            _defenders = defenders;
            _outputTextBox = outputTextBox;
        }

        /// <summary>
        /// Выполняет ход лекаря.
        /// Строка идентификатора "M:ClashGame.HealerTurnCommand.Execute".
        /// </summary>
        public void Execute()
        {
            _previousState = _attackers.Select(w => w.Clone()).ToList();
            _battleManager.HealerTurn(_attackers, _defenders, _outputTextBox);
        }

        /// <summary>
        /// Отменяет ход лекаря и восстанавливает состояние атакующих воинов.
        /// Строка идентификатора "M:ClashGame.HealerTurnCommand.Undo".
        /// </summary>
        public void Undo()
        {
            for (int i = 0; i < _attackers.Count; i++)
                _attackers[i] = _previousState[i];
            _outputTextBox.AppendText("Отмена лечения, восстановлено состояние армии." + Environment.NewLine);
        }
    }

    /// <summary>
    /// Класс WizardTurnCommand, представляющий команду хода мага.
    /// Строка идентификатора "T:ClashGame.WizardTurnCommand".
    /// </summary>
    public class WizardTurnCommand : ICommand
    {
        /// <summary>
        /// Управляющий битвой.
        /// Строка идентификатора "F:ClashGame.WizardTurnCommand._battleManager".
        /// </summary>
        private IBattleManager _battleManager;

        /// <summary>
        /// Список атакующих воинов.
        /// Строка идентификатора "F:ClashGame.WizardTurnCommand._attackers".
        /// </summary>
        private List<Warrior> _attackers;

        /// <summary>
        /// Список защищающихся воинов.
        /// Строка идентификатора "F:ClashGame.WizardTurnCommand._defenders".
        /// </summary>
        private List<Warrior> _defenders;

        /// <summary>
        /// TextBox для вывода информации о ходе битвы.
        /// Строка идентификатора "F:ClashGame.WizardTurnCommand._outputTextBox".
        /// </summary>
        private TextBox _outputTextBox;

        /// <summary>
        /// Список воинов, представляющий их состояние до выполнения команды.
        /// Строка идентификатора "F:ClashGame.WizardTurnCommand._previousState".
        /// </summary>
        private List<Warrior> _previousState;

        /// <summary>
        /// Конструктор для класса WizardTurnCommand.
        /// Строка идентификатора "M:ClashGame.WizardTurnCommand.#ctor(ClashGame.IBattleManager,System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Forms.TextBox)".
        /// </summary>
        /// <param name="battleManager">Управляющий битвой.</param>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе битвы.</param>
        public WizardTurnCommand(IBattleManager battleManager, List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            _battleManager = battleManager;
            _attackers = attackers;
            _defenders = defenders;
            _outputTextBox = outputTextBox;
        }

        /// <summary>
        /// Выполняет ход мага.
        /// Строка идентификатора "M:ClashGame.WizardTurnCommand.Execute".
        /// </summary>
        public void Execute()
        {
            _previousState = _attackers.Select(w => w.Clone()).ToList();
            _battleManager.WizardTurn(_attackers, _defenders, _outputTextBox);
        }

        /// <summary>
        /// Отменяет ход мага и восстанавливает состояние атакующих воинов.
        /// Строка идентификатора "M:ClashGame.WizardTurnCommand.Undo".
        /// </summary>
        public void Undo()
        {
            for (int i = 0; i < _attackers.Count-1; i++)
                _attackers[i] = _previousState[i];
            
            if (_attackers.Count == _previousState.Count)
                _attackers[_attackers.Count - 1] = _previousState[_attackers.Count - 1];
            else
                _attackers.Remove(_attackers[_attackers.Count-1]);
       
            _outputTextBox.AppendText("Отмена хода мага, восстановлено состояние армии." + Environment.NewLine);
        }
    }

    /// <summary>
    /// Класс ArcherTurnCommand, представляющий команду хода лучника.
    /// Строка идентификатора "T:ClashGame.ArcherTurnCommand".
    /// </summary>
    public class ArcherTurnCommand : ICommand
    {
        /// <summary>
        /// Управляющий битвой.
        /// Строка идентификатора "F:ClashGame.ArcherTurnCommand._battleManager".
        /// </summary>
        private IBattleManager _battleManager;

        /// <summary>
        /// Список атакующих воинов.
        /// Строка идентификатора "F:ClashGame.ArcherTurnCommand._attackers".
        /// </summary>
        private List<Warrior> _attackers;

        /// <summary>
        /// Список защищающихся воинов.
        /// Строка идентификатора "F:ClashGame.ArcherTurnCommand._defenders".
        /// </summary>
        private List<Warrior> _defenders;

        /// <summary>
        /// TextBox для вывода информации о ходе битвы.
        /// Строка идентификатора "F:ClashGame.ArcherTurnCommand._outputTextBox".
        /// </summary>
        private TextBox _outputTextBox;

        /// <summary>
        /// Список воинов, представляющий их состояние до выполнения команды.
        /// Строка идентификатора "F:ClashGame.ArcherTurnCommand._previousState".
        /// </summary>
        private List<Warrior> _previousDefenderState;

        /// <summary>
        /// Индекс цели атаки.
        /// Строка идентификатора "F:ClashGame.ArcherTurnCommand._targetIndex".
        /// </summary>
        private int _targetIndex;

        /// <summary>
        /// Предыдущее количество очков здоровья цели.
        /// Строка идентификатора "F:ClashGame.ArcherTurnCommand._previousHealthPoints".
        /// </summary>
        private double _previousHealthPoints;

        /// <summary>
        /// Конструктор для класса ArcherTurnCommand.
        /// Строка идентификатора "M:ClashGame.ArcherTurnCommand.#ctor(ClashGame.IBattleManager,System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Forms.TextBox)".
        /// </summary>
        /// <param name="battleManager">Управляющий битвой.</param>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе битвы.</param>
        public ArcherTurnCommand(IBattleManager battleManager, List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            _battleManager = battleManager;
            _attackers = attackers;
            _defenders = defenders;
            _outputTextBox = outputTextBox;
        }

        /// <summary>
        /// Выполняет ход лучника.
        /// Строка идентификатора "M:ClashGame.ArcherTurnCommand.Execute".
        /// </summary>
        public void Execute()
        {
            _previousDefenderState = _defenders.Select(w => w.Clone()).ToList();
            _targetIndex = new Random().Next(0, _defenders.Count);
            _previousHealthPoints = _defenders[_targetIndex].Healthpoints;
            _battleManager.ArchersTurn(_attackers, _defenders, _outputTextBox);
        }

        /// <summary>
        /// Отменяет ход лучника и восстанавливает состояние защищающихся воинов.
        /// Строка идентификатора "M:ClashGame.ArcherTurnCommand.Undo".
        /// </summary>
        public void Undo()
        {
            for (int i = 0; i < _defenders.Count; i++)
                _defenders[i] = _previousDefenderState[i];
            _outputTextBox.AppendText($"Отмена атаки лучника, восстановлено HP у {_defenders[_targetIndex].Side} {_defenders[_targetIndex]}" + Environment.NewLine);
        }
    }

}
