using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace ClashGame
{
    public class BattleManagerProxy : IBattleManager
    {
        BattleManager battleManager = new BattleManager();
        private IBattleStrategy strategy;
        private readonly string _filePath;
        public int flagGulyayGorodBlue = 0;
        public int flagGulyayGorodRed = 0;
        public bool triggerGulyayGorodBlue = true;
        public bool triggerGulyayGorodRed = true;

        public BattleManagerProxy(string filePath, IBattleStrategy strategy)
        {
            _filePath = filePath;
            this.strategy = strategy ?? throw new ArgumentNullException(nameof(strategy), "Strategy cannot be null");

            using (StreamWriter writer = File.AppendText(_filePath))
            {
                writer.WriteLine($"{DateTime.Now}: Starting logging...");
            }
        }

        //получает из окна сколько пользователь успел сделать ходов пока стена стоит
        public void SetGulyayGorodCount(int count, string side)
        {
            if (side == "Blue")
                flagGulyayGorodBlue = count;
            else
                flagGulyayGorodRed = count;
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
            strategy.ExecuteBattle(attackers, defenders, outputTextBox);
            ArchersTurn(attackers, defenders, outputTextBox);
            IsDead(defenders[0], defenders);

            Random rand = new Random();
            //проверка что стены не было
            if (flagGulyayGorodBlue < 7)
            {
                //проверка что пользователь вызвал стену но не доиграл ее
                if (attackers[0] is GulyayGorod && triggerGulyayGorodBlue && attackers[0].Side == "Blue")
                {
                    triggerGulyayGorodBlue = false;
                }

                //попытка поставить стену в первый раз компьютером
                if (triggerGulyayGorodBlue && rand.Next(0, 5) == 0 && attackers[0].Side == "Blue" && defenders[0] is not GulyayGorod)
                {
                    GulyayGorodTurn(attackers, outputTextBox);
                    triggerGulyayGorodBlue = false;
                    outputTextBox.AppendText("СТЕНА ПОЯВИЛАСЬ" + Environment.NewLine);
                }

                //счетчик ходов когда стена стоит
                if (!triggerGulyayGorodBlue)
                {
                    flagGulyayGorodBlue++;
                }

                //убираем стену когда ходы все
                if (flagGulyayGorodBlue == 7)
                {
                    attackers.Remove(attackers[0]);
                    outputTextBox.AppendText("СТЕНА УБРАЛАСЬ Синие" + Environment.NewLine);
                }
            }

            if (flagGulyayGorodRed < 7)
            {
                if (attackers[0] is GulyayGorod && triggerGulyayGorodRed && attackers[0].Side == "Red")
                {
                    triggerGulyayGorodRed = false;
                }

                if (triggerGulyayGorodRed && rand.Next(0, 5) == 0 && attackers[0].Side == "Red" && defenders[0] is not GulyayGorod)
                {
                    GulyayGorodTurn(attackers, outputTextBox);
                    triggerGulyayGorodRed = false;
                    outputTextBox.AppendText("СТЕНА ПОЯВИЛАСЬ" + Environment.NewLine);
                }

                if (!triggerGulyayGorodRed)
                {
                    flagGulyayGorodRed++;
                }


                if (flagGulyayGorodRed == 7)
                {
                    attackers.Remove(attackers[0]);
                    outputTextBox.AppendText("СТЕНА УБРАЛАСЬ Красные" + Environment.NewLine);
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
}


