using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;

namespace ClashGame
{
    // Интерфейс для логирования
    public interface ILogger
    {
        void Log(string message);
    }

    // Реализация логгера для записи в файл
    public class FileLogger : ILogger
    {
        private readonly string _filePath;

        public FileLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void Log(string message)
        {
            try
            {
                using (StreamWriter writer = File.AppendText(_filePath))
                {
                    writer.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при записи в файл логов: {ex.Message}");
            }
        }
    }

    // Прокси класс для BattleManager, который добавляет логирование к методам
    public class BattleManagerProxy : BattleManager
    {
        private readonly ILogger _logger;

        public BattleManagerProxy(ILogger logger)
        {
            _logger = logger;
        }

        public override void StartBattle(List<Warrior> firstArmy, List<Warrior> secondArmy, TextBox outputTextBox)
        {
            _logger.Log("Битва началась!");
            base.StartBattle(firstArmy, secondArmy, outputTextBox);
            _logger.Log("Битва завершилась!");
        }

        public override void Turn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            _logger.Log("Ход начат!");
            base.Turn(attackers, defenders, outputTextBox);
            _logger.Log("Ход завершен!");
        }

        public override void Attack(Warrior warrior1, Warrior warrior2, TextBox outputTextBox)
        {
            _logger.Log($"Атака {warrior1.Side} {warrior1} с силой {warrior1.Damage} по {warrior2.Side} {warrior2}");
            base.Attack(warrior1, warrior2, outputTextBox);
        }

        public override void DefencePlease(Warrior warrior1, Warrior warrior2, TextBox outputTextBox)
        {
            _logger.Log($"Получена атака {warrior1.Side} {warrior1} с силой {warrior1.Damage} от {warrior2.Side} {warrior2}");
            base.DefencePlease(warrior1, warrior2, outputTextBox);
        }

        public override void IsDead(Warrior warrior, List<Warrior> army)
        {
            if (warrior.Healthpoints <= 0)
            {
                _logger.Log($"Воин {warrior.Side} {warrior} умер!");
            }

            base.IsDead(warrior, army);
        }
    }
    public class HealerProxy : Healer
    {
        private readonly ILogger _logger;

        public HealerProxy(string side, ILogger logger) : base(side)
        {
            _logger = logger;
        }

        public override void Heal(Warrior target)
        {
            _logger.Log($"Применение лечения воином {Side} {this} воина  {target.Side} {target}");
            _logger.Log($"Теперь у {target.Side} {target} {target.Healthpoints} жизней.");
            base.Heal(target);
        }
    }

    public class WizardProxy : Wizard
    {
        private readonly ILogger _logger;

        public WizardProxy(string side, ILogger logger) : base(side)
        {
            _logger = logger;
        }

        public override Warrior CloneLightWarrior(List<Warrior> warriors)
        {
            _logger.Log($"Клонирование воина {Side} {this}");
            return base.CloneLightWarrior(warriors);
        }
    }

    public class ArcherProxy : Archer
    {
        private readonly ILogger _logger;

        public ArcherProxy(string side, ILogger logger) : base(side)
        {
            _logger = logger;
        }

        public override double RangedAttack(List<Warrior> enemies, int targetIndex, int attackerIndex)
        {
            _logger.Log($"Применение дальней атаки воина {Side} {this} к воину {enemies[targetIndex].Side} {enemies[targetIndex]} ");
            var damage = base.RangedAttack(enemies, targetIndex, attackerIndex);
            _logger.Log($"Урон от дальней атаки: {damage}");
            return damage;
        }
    }
}


