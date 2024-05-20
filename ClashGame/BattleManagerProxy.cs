using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace ClashGame
{
    public interface IBattleManager
    {
        void StartBattle(List<Warrior> firstArmy, List<Warrior> secondArmy, TextBox outputTextBox);
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

    public class BattleManagerProxy : IBattleManager
    {
        BattleManager battleManager = new BattleManager();
        private readonly string _filePath;
        public int flagGulyayGorodBlue = 0;
        public int flagGulyayGorodRed = 0;
        public bool triggerGulyayGorodBlue = true;
        public bool triggerGulyayGorodRed = true;

        public BattleManagerProxy(string filePath)
        {
            _filePath = filePath;
            using (StreamWriter writer = File.AppendText(_filePath))
            {
                writer.WriteLine($"{DateTime.Now}: Starting logging...");
            }
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

        public void StartBattle(List<Warrior> firstArmy, List<Warrior> secondArmy, TextBox outputTextBox)
        {
            Log("Битва началась!");
            battleManager.StartBattle(firstArmy, secondArmy, outputTextBox);
            Log("Битва завершилась!");
        }

        public void TurnComputer(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Log("Ход начат!");
            //battleManager.TurnComputer(attackers, defenders, outputTextBox);

            WizardTurn(attackers, outputTextBox);
            HealerTurn(attackers, outputTextBox);
            HeavyWarriorUpgradeTurn(attackers, attackers[0], outputTextBox);
            Attack(attackers[0], defenders[0], outputTextBox);
            ArchersTurn(attackers, defenders, outputTextBox);
            IsDead(defenders[0], defenders);

            //ЗДЕСЬ БОДАТЬСЯ С УСЛОВИЯМИ
            Random rand = new Random();
            if (flagGulyayGorodBlue <= 3)
            {
                if (triggerGulyayGorodBlue && rand.Next(0, 5) == 0 && attackers[0].Side == "Blue" && defenders[0] is not GulyayGorod)
                {
                    GulyayGorodTurn(attackers, outputTextBox);
                    triggerGulyayGorodBlue = false;
                }

                if (!triggerGulyayGorodBlue)
                {
                    flagGulyayGorodBlue++;
                    GulyayGorodTurn(attackers, outputTextBox);
                }

                if (flagGulyayGorodBlue == 3)
                {
                    attackers.Remove(attackers[0]);
                }
            }

            if (flagGulyayGorodRed <= 3)
            {
                if (triggerGulyayGorodRed && rand.Next(0, 5) == 0 && attackers[0].Side == "Red" && defenders[0] is not GulyayGorod)
                {
                    GulyayGorodTurn(attackers, outputTextBox);
                    triggerGulyayGorodRed = false;
                }

                if (!triggerGulyayGorodRed)
                {
                    flagGulyayGorodRed++;
                    GulyayGorodTurn(attackers, outputTextBox);
                }


                if (flagGulyayGorodRed == 3)
                {
                    attackers.Remove(attackers[0]);
                }
            }
            Log("Ход завершен!");
        }

        public void WizardTurn(List<Warrior> attackers, TextBox outputTextBox)
        {
            Log($"Клонирование воина {attackers[0].Side} {this}");
            battleManager.WizardTurn(attackers, outputTextBox);
        }

        public void HealerTurn(List<Warrior> attackers, TextBox outputTextBox)
        {
            Log($"Применение лечения воином {attackers[0].Side} {this}");
            battleManager.HealerTurn(attackers, outputTextBox);
        }

        public void HeavyWarriorUpgradeTurn(List<Warrior> attackers, Warrior attacker, TextBox outputTextBox)
        {
            Log($"Улучшение воина {attackers[0].Side} {this}");
            battleManager.HeavyWarriorUpgradeTurn(attackers, attacker, outputTextBox);
        }

        public void ArchersTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Log($"Атака лучниками");
            battleManager.ArchersTurn(attackers, defenders, outputTextBox);
        }

        public void Attack(Warrior warrior1, Warrior warrior2, TextBox outputTextBox)
        {
            Log($"Атака {warrior1.Side} {warrior1} с силой {warrior1.Damage} по {warrior2.Side} {warrior2}");
            battleManager.Attack(warrior1, warrior2, outputTextBox);
        }
        public void DefencePlease(Warrior warrior1, Warrior warrior2, TextBox outputTextBox)
        {
            Log($"Получена атака {warrior1.Side} {warrior1} с силой {warrior1.Damage} от {warrior2.Side} {warrior2}");
            battleManager.DefencePlease(warrior1, warrior2, outputTextBox);
        }
        public void IsDead(Warrior warrior, List<Warrior> army)
        {
            if (warrior.Healthpoints <= 0)
            {
                Log($"Воин {warrior.Side} {warrior} умер!");
            }

            battleManager.IsDead(warrior, army);
        }

        public void GulyayGorodTurn(List<Warrior> attackers, TextBox outputTextBox)
        {
            Log($"Активирован гуляй город у {attackers[0].Side}");
            battleManager.GulyayGorodTurn(attackers, outputTextBox);
        }
    }






    // Интерфейс для логирования
    //public interface ILogger
    //{
    //    void Log(string message);
    //}

    //// Реализация логгера для записи в файл
    //public class FileLogger : ILogger
    //{
    //    private readonly string _filePath;

    //    public FileLogger(string filePath)
    //    {
    //        _filePath = filePath;
    //    }

    //    public void Log(string message)
    //    {
    //        try
    //        {
    //            using (StreamWriter writer = File.AppendText(_filePath))
    //            {
    //                writer.WriteLine($"{DateTime.Now}: {message}");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"Ошибка при записи в файл логов: {ex.Message}");
    //        }
    //    }
    //}

    //// Прокси класс для BattleManager, который добавляет логирование к методам
    //public class BattleManagerProxy : ILogger
    //{
    //    private readonly ILogger _logger;

    //    public BattleManagerProxy(ILogger logger)
    //    {
    //        _logger = logger;
    //    }


    //    public void Log(string message)
    //    {
    //        try
    //        {
    //            using (StreamWriter writer = File.AppendText(_filePath))
    //            {
    //                writer.WriteLine($"{DateTime.Now}: {message}");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"Ошибка при записи в файл логов: {ex.Message}");
    //        }
    //    }


    //    //public override void StartBattle(List<Warrior> firstArmy, List<Warrior> secondArmy, TextBox outputTextBox)
    //    //{
    //    //    _logger.Log("Битва началась!");
    //    //    base.StartBattle(firstArmy, secondArmy, outputTextBox);
    //    //    _logger.Log("Битва завершилась!");
    //    //}

    //    public override void TurnComputer(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
    //    {
    //        _logger.Log("Ход начат!");
    //        base.TurnComputer(attackers, defenders, outputTextBox);
    //        _logger.Log("Ход завершен!");
    //    }

    //    public override void Attack(Warrior warrior1, Warrior warrior2, TextBox outputTextBox)
    //    {
    //        _logger.Log($"Атака {warrior1.Side} {warrior1} с силой {warrior1.Damage} по {warrior2.Side} {warrior2}");
    //        base.Attack(warrior1, warrior2, outputTextBox);
    //    }

    //    public override void DefencePlease(Warrior warrior1, Warrior warrior2, TextBox outputTextBox)
    //    {
    //        _logger.Log($"Получена атака {warrior1.Side} {warrior1} с силой {warrior1.Damage} от {warrior2.Side} {warrior2}");
    //        base.DefencePlease(warrior1, warrior2, outputTextBox);
    //    }

    //    public override void IsDead(Warrior warrior, List<Warrior> army)
    //    {
    //        if (warrior.Healthpoints <= 0)
    //        {
    //            _logger.Log($"Воин {warrior.Side} {warrior} умер!");
    //        }

    //        base.IsDead(warrior, army);
    //    }
    //}
    //public class HealerProxy : Healer
    //{
    //    private readonly ILogger _logger;

    //    public HealerProxy(string side, ILogger logger) : base(side)
    //    {
    //        _logger = logger;
    //    }

    //    public override void Heal(Warrior target)
    //    {
    //        _logger.Log($"Применение лечения воином {Side} {this} воина  {target.Side} {target}");
    //        _logger.Log($"Теперь у {target.Side} {target} {target.Healthpoints} жизней.");
    //        base.Heal(target);
    //    }
    //}

    //public class WizardProxy : Wizard
    //{
    //    private readonly ILogger _logger;

    //    public WizardProxy(string side, ILogger logger) : base(side)
    //    {
    //        _logger = logger;
    //    }

    //    public override Warrior CloneLightWarrior(List<Warrior> warriors)
    //    {
    //        _logger.Log($"Клонирование воина {Side} {this}");
    //        return base.CloneLightWarrior(warriors);
    //    }
    //}

    //public class ArcherProxy : Archer
    //{
    //    private readonly ILogger _logger;

    //    public ArcherProxy(string side, ILogger logger) : base(side)
    //    {
    //        _logger = logger;
    //    }

    //    public override double RangedAttack(List<Warrior> enemies, int targetIndex, int attackerIndex)
    //    {
    //        _logger.Log($"Применение дальней атаки воина {Side} {this} к воину {enemies[targetIndex].Side} {enemies[targetIndex]} ");
    //        var damage = base.RangedAttack(enemies, targetIndex, attackerIndex);
    //        _logger.Log($"Урон от дальней атаки: {damage}");
    //        return damage;
    //    }
    //}
}


