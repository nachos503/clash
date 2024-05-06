using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClashGame
{
    public class BattleManager
    { 
        virtual public void StartBattle(List<Warrior> firstArmy, List<Warrior> secondArmy, TextBox outputTextBox)
        {
            while (firstArmy.Count != 0)
            {
                Turn(firstArmy, secondArmy, outputTextBox);

                if (secondArmy.Count != 0)
                {
                    Turn(secondArmy, firstArmy, outputTextBox);
                }
                else
                {
                    outputTextBox.AppendText("Первые победили!!" + Environment.NewLine);
                    break;
                }
            }
            if (firstArmy.Count == 0) outputTextBox.AppendText("Вторые победили!!" + Environment.NewLine);
        }

        virtual public void Turn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            string logFilePath = "battle_logs.txt";
            ILogger fileLogger = new FileLogger(logFilePath);

            Warrior attacker = attackers[0];
            Warrior defender = defenders[0];

            // Проверка наличия мага в списке атакующих
            WizardProxy wizard = null;
            foreach (var attacker1 in attackers)
            {
                if (attacker1 is Wizard)
                {
                    wizard = new WizardProxy(attacker1.Side, fileLogger);
                    break;
                }
            }

            // Попытка клонирования LightWarrior
            if (wizard != null)
            {
                Warrior clonedWarrior = wizard.CloneLightWarrior(attackers);
                if (clonedWarrior != null)
                {
                    attackers.Insert(1, clonedWarrior); // Вставляем клонированного LightWarrior перед магом (на вторую позицию)
                    outputTextBox.AppendText($"Маг из команды {wizard.Side} клонировал LightWarrior с {clonedWarrior.Healthpoints} HP" + Environment.NewLine);
                }
            }

            // Проверка на наличие лекаря в списке атакующих и его позиции
            HealerProxy healer = null;
            int healerIndex = -1;
            for (int i = 0; i < attackers.Count; i++)
            {
                if (attackers[i] is Healer)
                {
                    healer = new HealerProxy(attackers[i].Side, fileLogger);
                    healerIndex = i;
                    break;
                }
            }

            if (healer != null && healerIndex != 0)
            {
                // Проверка на выполнение условий для лечения
                if (new Random().Next(0, 10) == 0)
                {
                    // Вызов лечения у случайного союзника
                    List<Warrior> alliesInRange = GetAlliesInRange(attackers, healerIndex);
                    if (alliesInRange.Count > 0)
                    {
                        int targetIndex = new Random().Next(0, alliesInRange.Count);
                        Warrior targetAlly = alliesInRange[targetIndex];
                        if (!(targetAlly is LightWarrior))
                        {
                            healer.Heal(targetAlly);
                            outputTextBox.AppendText($"Лекарь из команды {healer.Side} вылечил {targetAlly.Side} {targetAlly}" + Environment.NewLine);
                            outputTextBox.AppendText($"Теперь у {targetAlly.Side} {targetAlly} {targetAlly.Healthpoints} HP" + Environment.NewLine);
                        }
                    }
                }
            }


            // Проверка на наличие ImprovedHeavyWarrior в списке атакующих и его позиции
            ImprovedHeavyWarrior improvedHeavyWarrior = null;
            int improvedHeavyWarriorIndex = -1;
            for (int i = 0; i < attackers.Count; i++)
            {
                if (attackers[i] is ImprovedHeavyWarrior)
                {
                    improvedHeavyWarrior = (ImprovedHeavyWarrior)attackers[i];
                    improvedHeavyWarriorIndex = i;
                    break;
                }
            }

            // Проверяем, есть ли рядом с ImprovedHeavyWarrior LightWarrior
            bool lightWarriorNearby = false;
            if (improvedHeavyWarrior != null)
            {
                // Проверяем наличие LightWarrior на расстоянии 1 от ImprovedHeavyWarrior
                for (int i = Math.Max(0, improvedHeavyWarriorIndex - 1); i < Math.Min(attackers.Count, improvedHeavyWarriorIndex + 2); i++)
                {
                    if (attackers[i] is LightWarrior)
                    {
                        lightWarriorNearby = true;
                        break;
                    }
                }

                if (!lightWarriorNearby)
                {
                    // Если рядом с ImprovedHeavyWarrior нет LightWarrior, отменяем улучшение
                    attackers[improvedHeavyWarriorIndex] = new HeavyWarrior(improvedHeavyWarrior.Side)
                    {
                        Healthpoints = improvedHeavyWarrior.Healthpoints // сохраняем текущее здоровье
                    };
                    outputTextBox.AppendText($"Улучшение у ImprovedHeavyWarrior отменено, так как рядом нет LightWarrior." + Environment.NewLine);
                    attacker = attackers[0];
                }
            }

            // Проверяем здоровье ImprovedHeavyWarrior
            if (improvedHeavyWarrior != null && improvedHeavyWarrior.Healthpoints < 0.4 * improvedHeavyWarrior.MaxHealthpoints)
            {
                // Если здоровье упало ниже 40%, отменяем улучшение
                attackers[improvedHeavyWarriorIndex] = new HeavyWarrior(improvedHeavyWarrior.Side);
                outputTextBox.AppendText($"Улучшение у ImprovedHeavyWarrior отменено, так как его здоровье упало ниже 40%." + Environment.NewLine);
                attacker = attackers[0];
            }

            Attack(attacker, defender, outputTextBox);

            // После обычной атаки, ищем арчера в оставшемся списке воинов
            foreach (var warrior in attackers)
            {
                if (warrior is Archer)
                {
                    ArcherProxy archer = new ArcherProxy(warrior.Side, fileLogger);
                    var targetIndex = new Random().Next(0, defenders.Count); // Выбор случайного защищающегося воина
                    var target = defenders[targetIndex]; // Выбранный защищающийся воин

                    // Передача индекса атакующего лучника
                    archer.RangedAttack(defenders, targetIndex, attackers.IndexOf(warrior));
                    outputTextBox.AppendText($"Атака {archer.Side} {archer} с силой {archer.RangedDamage(attackers.IndexOf(warrior))} по {target}" + Environment.NewLine);
                    outputTextBox.AppendText($"HP у  {target.Side} {target} осталось {target.Healthpoints}" + Environment.NewLine);

                    break; // Выходим после того как нашли первого арчера
                }
            }
            IsDead(defender, defenders);
        }

        private List<Warrior> GetAlliesInRange(List<Warrior> attackers, int healerIndex)
        {
            List<Warrior> alliesInRange = new List<Warrior>();
            for (int i = Math.Max(0, healerIndex - 3); i < Math.Min(attackers.Count, healerIndex + 4); i++)
            {
                if (i != healerIndex && attackers[i].Side == attackers[healerIndex].Side)
                {
                    alliesInRange.Add(attackers[i]);
                }
            }
            return alliesInRange;
        }

        virtual public void Attack(Warrior warrior1, Warrior warrior2, TextBox outputTextBox)
        {
            outputTextBox.AppendText($"Атака {warrior1.Side} {warrior1} с силой {warrior1.Damage} по {warrior2.Side} {warrior2}" + Environment.NewLine);
            DefencePlease(warrior1, warrior2, outputTextBox);
        }

        virtual public void DefencePlease(Warrior warrior1, Warrior warrior2, TextBox outputTextBox)
        {
            Random random = new Random();

            if (random.Next(0, 101) > warrior2.Dodge)
            {
                warrior2.Healthpoints -= warrior1.Damage;
                outputTextBox.AppendText($"HP у {warrior2.Side} {warrior2} осталось {warrior2.Healthpoints}" + Environment.NewLine);
            }

            else
            {
                outputTextBox.AppendText("Dodge cool" + Environment.NewLine);
            }
        }

        virtual public void IsDead(Warrior warrior, List<Warrior> army)
        {
            if (warrior.Healthpoints <= 0)
            {
                army.Remove(warrior);
            }
        }
    }
}
