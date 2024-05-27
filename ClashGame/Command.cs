using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClashGame
{
    public class CommandManager
    {
        private Stack<ICommand> _commands = new Stack<ICommand>();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _commands.Push(command);
        }

        public void Undo()
        {
            if (_commands.Count > 0)
            {
                ICommand command = _commands.Pop();
                command.Undo();
            }
        }

        public bool CanUndo()
        {
            return _commands.Count > 0;
        }
    }

    public class AttackCommand : ICommand
    {
        private IBattleManager _battleManager;
        private Warrior _attacker;
        private Warrior _defender;
        private TextBox _outputTextBox;
        private double _previousHealthPoints;

        public AttackCommand(IBattleManager battleManager, Warrior attacker, Warrior defender, TextBox outputTextBox)
        {
            _battleManager = battleManager;
            _attacker = attacker;
            _defender = defender;
            _outputTextBox = outputTextBox;
        }

        public void Execute()
        {
            _previousHealthPoints = _defender.Healthpoints;
            _battleManager.Attack(_attacker, _defender, _outputTextBox);
        }

        public void Undo()
        {
            _defender.Healthpoints = _previousHealthPoints;
            _outputTextBox.AppendText($"Отмена атаки, восстановлено HP у {_defender.Side} {_defender}" + Environment.NewLine);
        }
    }

    public class HealerTurnCommand : ICommand
    {
        private IBattleManager _battleManager;
        private List<Warrior> _attackers;
        private List<Warrior> _defenders;
        private TextBox _outputTextBox;
        private List<Warrior> _previousState;

        public HealerTurnCommand(IBattleManager battleManager, List<Warrior> attackers, List<Warrior> defenders,TextBox outputTextBox)
        {
            _battleManager = battleManager;
            _attackers = attackers;
            _defenders = defenders;
            _outputTextBox = outputTextBox;
        }

        public void Execute()
        {
            _previousState = _attackers.Select(w => w.Clone()).ToList();
            _battleManager.HealerTurn(_attackers, _defenders, _outputTextBox);
        }

        public void Undo()
        {
            for (int i = 0; i < _attackers.Count; i++)
            {
                _attackers[i] = _previousState[i];
            }
            _outputTextBox.AppendText("Отмена лечения, восстановлено состояние армии." + Environment.NewLine);
        }
    }

    public class WizardTurnCommand : ICommand
    {
        private IBattleManager _battleManager;
        private List<Warrior> _attackers;
        private List<Warrior> _defenders;
        private TextBox _outputTextBox;
        private List<Warrior> _previousState;

        public WizardTurnCommand(IBattleManager battleManager, List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            _battleManager = battleManager;
            _attackers = attackers;
            _defenders = defenders;
            _outputTextBox = outputTextBox;
        }

        public void Execute()
        {
            _previousState = _attackers.Select(w => w.Clone()).ToList();
            _battleManager.WizardTurn(_attackers, _defenders, _outputTextBox);
        }

        public void Undo()
        {
            for (int i = 0; i < _attackers.Count-1; i++)
            {
                    _attackers[i] = _previousState[i];
            }
            
            if (_attackers.Count == _previousState.Count)
                _attackers[_attackers.Count - 1] = _previousState[_attackers.Count - 1];
            else
                _attackers.Remove(_attackers[_attackers.Count-1]);
       
            _outputTextBox.AppendText("Отмена хода мага, восстановлено состояние армии." + Environment.NewLine);
        }
    }

    public class ArcherTurnCommand : ICommand
    {
        private IBattleManager _battleManager;
        private List<Warrior> _attackers;
        private List<Warrior> _defenders;
        private TextBox _outputTextBox;
        private List<Warrior> _previousDefenderState;
        private int _targetIndex;
        private double _previousHealthPoints;

        public ArcherTurnCommand(IBattleManager battleManager, List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            _battleManager = battleManager;
            _attackers = attackers;
            _defenders = defenders;
            _outputTextBox = outputTextBox;
        }

        public void Execute()
        {
            _previousDefenderState = _defenders.Select(w => w.Clone()).ToList();
            _targetIndex = new Random().Next(0, _defenders.Count);
            _previousHealthPoints = _defenders[_targetIndex].Healthpoints;
            _battleManager.ArchersTurn(_attackers, _defenders, _outputTextBox);
        }

        public void Undo()
        {
            for (int i = 0; i < _defenders.Count; i++)
            {
                _defenders[i] = _previousDefenderState[i];
            }
            _outputTextBox.AppendText($"Отмена атаки лучника, восстановлено HP у {_defenders[_targetIndex].Side} {_defenders[_targetIndex]}" + Environment.NewLine);
        }
    }

}
