using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClashGame
{
    /// <summary>
    /// CommandManager class that manages the execution and undoing of commands.
    /// Identifier string "T:ClashGame.CommandManager".
    /// </summary>
    public class CommandManager
    {
        /// <summary>
        /// Stack of executed commands.
        /// Identifier string "F:ClashGame.CommandManager._commands".
        /// </summary>
        private readonly Stack<ICommand> _commands = new();

        /// <summary>
        /// Executes a command and adds it to the stack.
        /// Identifier string "M:ClashGame.CommandManager.ExecuteCommand(ClashGame.ICommand)".
        /// </summary>
        /// <param name="command">The command to execute.</param>
        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _commands.Push(command);
        }

        /// <summary>
        /// Undoes the last executed command.
        /// Identifier string "M:ClashGame.CommandManager.Undo".
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
        /// Checks if the last executed command can be undone.
        /// Identifier string "M:ClashGame.CommandManager.CanUndo".
        /// </summary>
        /// <returns>True if there are commands to undo, otherwise false.</returns>
        public bool CanUndo()
        {
            return _commands.Count > 0;
        }
    }

    /// <summary>
    /// HealerTurnCommand class representing the healer's turn command.
    /// Identifier string "T:ClashGame.HealerTurnCommand".
    /// </summary>
    public class HealerTurnCommand : ICommand
    {
        /// <summary>
        /// Battle manager.
        /// Identifier string "F:ClashGame.HealerTurnCommand._battleManager".
        /// </summary>
        private readonly IBattleManager _battleManager;

        /// <summary>
        /// List of attacking warriors.
        /// Identifier string "F:ClashGame.HealerTurnCommand._attackers".
        /// </summary>
        private readonly List<Warrior> _attackers;

        /// <summary>
        /// List of defending warriors.
        /// Identifier string "F:ClashGame.HealerTurnCommand._defenders".
        /// </summary>
        private readonly List<Warrior> _defenders;

        /// <summary>
        /// TextBox for displaying battle information.
        /// Identifier string "F:ClashGame.HealerTurnCommand._outputTextBox".
        /// </summary>
        private readonly TextBox _outputTextBox;

        /// <summary>
        /// List of warriors representing their state before the command execution.
        /// Identifier string "F:ClashGame.HealerTurnCommand._previousState".
        /// </summary>
        private List<Warrior> _previousState = new();

        /// <summary>
        /// Constructor for the HealerTurnCommand class.
        /// Identifier string "M:ClashGame.HealerTurnCommand.#ctor(ClashGame.IBattleManager,System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Forms.TextBox)".
        /// </summary>
        /// <param name="battleManager">Battle manager.</param>
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="defenders">List of defending warriors.</param>
        /// <param name="outputTextBox">TextBox for displaying battle information.</param>
        public HealerTurnCommand(IBattleManager battleManager, List<Warrior> attackers, List<Warrior> defenders,TextBox outputTextBox)
        {
            _battleManager = battleManager;
            _attackers = attackers;
            _defenders = defenders;
            _outputTextBox = outputTextBox;
        }

        /// <summary>
        /// Executes the healer's turn.
        /// Identifier string "M:ClashGame.HealerTurnCommand.Execute".
        /// </summary>
        public void Execute()
        {
            _previousState = _attackers.Select(w => w.Clone()).ToList();
            _battleManager.HealerTurn(_attackers, _defenders, _outputTextBox);
        }

        /// <summary>
        /// Undoes the healer's turn and restores the state of the attacking warriors.
        /// Identifier string "M:ClashGame.HealerTurnCommand.Undo".
        /// </summary>
        public void Undo()
        {
            for (int i = 0; i < _attackers.Count; i++)
                _attackers[i] = _previousState[i];
            _outputTextBox.AppendText("Отмена лечения, восстановлено состояние армии." + Environment.NewLine);
        }
    }

    /// <summary>
    /// WizardTurnCommand class representing the wizard's turn command.
    /// Identifier string "T:ClashGame.WizardTurnCommand".
    /// </summary>
    public class WizardTurnCommand : ICommand
    {
        /// <summary>
        /// Battle manager.
        /// Identifier string "F:ClashGame.WizardTurnCommand._battleManager".
        /// </summary>
        private readonly IBattleManager _battleManager;

        /// <summary>
        /// List of attacking warriors.
        /// Identifier string "F:ClashGame.WizardTurnCommand._attackers".
        /// </summary>
        private readonly List<Warrior> _attackers;

        /// <summary>
        /// List of defending warriors.
        /// Identifier string "F:ClashGame.WizardTurnCommand._defenders".
        /// </summary>
        private readonly List<Warrior> _defenders;

        /// <summary>
        /// TextBox for displaying battle information.
        /// Identifier string "F:ClashGame.WizardTurnCommand._outputTextBox".
        /// </summary>
        private readonly TextBox _outputTextBox;

        /// <summary>
        /// List of warriors representing their state before the command execution.
        /// Identifier string "F:ClashGame.WizardTurnCommand._previousState".
        /// </summary>
        private List<Warrior>? _previousState;

        /// <summary>
        /// Constructor for the WizardTurnCommand class.
        /// Identifier string "M:ClashGame.WizardTurnCommand.#ctor(ClashGame.IBattleManager,System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Forms.TextBox)".
        /// </summary>
        /// <param name="battleManager">Battle manager.</param>
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="defenders">List of defending warriors.</param>
        /// <param name="outputTextBox">TextBox for displaying battle information.</param>
        public WizardTurnCommand(IBattleManager battleManager, List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            _battleManager = battleManager;
            _attackers = attackers;
            _defenders = defenders;
            _outputTextBox = outputTextBox;
        }

        /// <summary>
        /// Executes the wizard's turn.
        /// Identifier string "M:ClashGame.WizardTurnCommand.Execute".
        /// </summary>
        public void Execute()
        {
            _previousState = _attackers.Select(w => w.Clone()).ToList();
            _battleManager.WizardTurn(_attackers, _defenders, _outputTextBox);
        }

        /// <summary>
        /// Undoes the wizard's turn and restores the state of the attacking warriors.
        /// Identifier string "M:ClashGame.WizardTurnCommand.Undo".
        /// </summary>
        public void Undo()
        {
            if (_previousState == null)
            {
                _outputTextBox.AppendText("Не удалось отменить ход: состояние армии не сохранено." + Environment.NewLine);
                return;
            }

            for (int i = 0; i < _attackers.Count-1; i++)
                _attackers[i] = _previousState[i];
            
            if (_attackers.Count == _previousState.Count)
                _attackers[^1] = _previousState[_attackers.Count - 1];
            else
                _attackers.Remove(_attackers[^1]);
       
            _outputTextBox.AppendText("Отмена хода мага, восстановлено состояние армии." + Environment.NewLine);
        }
    }

    /// <summary>
    /// ArcherTurnCommand class representing the archer's turn command.
    /// Identifier string "T:ClashGame.ArcherTurnCommand".
    /// </summary>
    public class ArcherTurnCommand : ICommand
    {
        /// <summary>
        /// Battle manager.
        /// Identifier string "F:ClashGame.ArcherTurnCommand._battleManager".
        /// </summary>
        private readonly IBattleManager _battleManager;

        /// <summary>
        /// List of attacking warriors.
        /// Identifier string "F:ClashGame.ArcherTurnCommand._attackers".
        /// </summary>
        private readonly List<Warrior> _attackers;

        /// <summary>
        /// List of defending warriors.
        /// Identifier string "F:ClashGame.ArcherTurnCommand._defenders".
        /// </summary>
        private readonly List<Warrior> _defenders;

        /// <summary>
        /// TextBox for displaying battle information.
        /// Identifier string "F:ClashGame.ArcherTurnCommand._outputTextBox".
        /// </summary>
        private readonly TextBox _outputTextBox;

        /// <summary>
        /// List of warriors representing their state before the command execution.
        /// Identifier string "F:ClashGame.ArcherTurnCommand._previousState".
        /// </summary>
        private List<Warrior>? _previousDefenderState;

        /// <summary>
        /// Target attack index.
        /// Identifier string "F:ClashGame.ArcherTurnCommand._targetIndex".
        /// </summary>
        private int _targetIndex;

        /// <summary>
        /// Previous health points of the target.
        /// Identifier string "F:ClashGame.ArcherTurnCommand._previousHealthPoints".
        /// </summary>
        private double _previousHealthPoints;

        /// <summary>
        /// Constructor for the ArcherTurnCommand class.
        /// Identifier string "M:ClashGame.ArcherTurnCommand.#ctor(ClashGame.IBattleManager,System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Forms.TextBox)".
        /// </summary>
        /// <param name="battleManager">Battle manager.</param>
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="defenders">List of defending warriors.</param>
        /// <param name="outputTextBox">TextBox for displaying battle information.</param>
        public ArcherTurnCommand(IBattleManager battleManager, List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            _battleManager = battleManager;
            _attackers = attackers;
            _defenders = defenders;
            _outputTextBox = outputTextBox;
        }

        /// <summary>
        /// Executes the archer's turn.
        /// Identifier string "M:ClashGame.ArcherTurnCommand.Execute".
        /// </summary>
        public void Execute()
        {
            _previousDefenderState = _defenders.Select(w => w.Clone()).ToList();
            _targetIndex = new Random().Next(0, _defenders.Count);
            _previousHealthPoints = _defenders[_targetIndex].Healthpoints;
            _battleManager.ArchersTurn(_attackers, _defenders, _outputTextBox);
        }

        /// <summary>
        /// Undoes the archer's turn and restores the state of the defending warriors.
        /// Identifier string "M:ClashGame.ArcherTurnCommand.Undo".
        /// </summary>
        public void Undo()
        {
            if (_previousDefenderState == null)
            {
                _outputTextBox.AppendText("Не удалось отменить ход: состояние армии не сохранено." + Environment.NewLine);
                return;
            }
            for (int i = 0; i < _defenders.Count; i++)
                _defenders[i] = _previousDefenderState[i];
            _outputTextBox.AppendText($"Отмена атаки лучника, восстановлено HP у {_defenders[_targetIndex].Side} {_defenders[_targetIndex]}" + Environment.NewLine);
        }
    }

}
