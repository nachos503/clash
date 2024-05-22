using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ClashGame;

namespace ClashGame
{
    public class BattleManager : IBattleManager
    {
        private IBattleStrategy _battleStrategy;
        public void SetStrategy(IBattleStrategy strategy)
        {
            _battleStrategy = strategy;
        }

        virtual public void TurnComputer(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Warrior attacker = attackers[0];
            Warrior defender = defenders[0];

            // Проверка наличия мага в списке атакующих
            WizardTurn(attackers, outputTextBox);

            // Проверка на наличие лекаря в списке атакующих и его позиции
            HealerTurn(attackers, outputTextBox);

            // Проверка на наличие ImprovedHeavyWarrior в списке атакующих и его позиции
            HeavyWarriorUpgradeTurn(attackers, attacker, outputTextBox);

            //Контрольная атака за ход
            Attack(attacker, defender, outputTextBox);

            // После обычной атаки, ищем арчера в оставшемся списке воинов
            ArchersTurn(attackers, defenders, outputTextBox);

            //Контрольная проверка на живых соперников
            IsDead(defender, defenders);
        }

        public void WizardTurn(List<Warrior> attackers, TextBox outputTextBox)
        {

            Wizard wizard = null;

            foreach (var attacker1 in attackers)
            {
                if (attacker1 is Wizard)
                {
                    wizard = new Wizard(attacker1.Side);
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
                else
                {
                    outputTextBox.AppendText($"Маг из команды {wizard.Side} не смог склонировать LigthWarrior: " + Environment.NewLine);
                    outputTextBox.AppendText("LightWarrior отсутствует, либо не повезло." + Environment.NewLine);
                }
            }
        }

        public void HealerTurn(List<Warrior> attackers, TextBox outputTextBox)
        {
            Healer healer = null;
            int healerIndex = -1;
            for (int i = 0; i < attackers.Count; i++)
            {
                if (attackers[i] is Healer)
                {
                    healer = new Healer(attackers[i].Side);
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
                else
                {
                    outputTextBox.AppendText($"Лекарь из команды {healer.Side} никого не вылечил" + Environment.NewLine);
                    outputTextBox.AppendText("Не прокнуло." + Environment.NewLine);
                }
            }
        }

        public void HeavyWarriorUpgradeTurn(List<Warrior> attackers, Warrior attacker, TextBox outputTextBox)
        {
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
        }

        public void ArchersTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            foreach (var warrior in attackers)
            {
                if (warrior is Archer)
                {
                    //ArcherProxy archer = new ArcherProxy(warrior.Side, fileLogger);
                    var targetIndex = new Random().Next(0, defenders.Count-1); // Выбор случайного защищающегося воина
                    var target = defenders[targetIndex]; // Выбранный защищающийся воин

                    Archer archer = new Archer(warrior.Side);
                    // Передача индекса атакующего лучника
                    archer.RangedAttack(defenders, targetIndex, attackers.IndexOf(warrior));
                    outputTextBox.AppendText($"Атака {archer.Side} {archer} с силой {archer.RangedDamage(attackers.IndexOf(warrior))} по {target}" + Environment.NewLine);
                    outputTextBox.AppendText($"HP у  {target.Side} {target} осталось {target.Healthpoints}" + Environment.NewLine);

                    break; // Выходим после того как нашли первого арчера
                }
            }
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

        public void GulyayGorodTurn(List<Warrior> attackers, TextBox outputTextBox)
        {
            var temp = attackers.First();
            attackers[0] = attackers.Last();
            attackers[attackers.Count() - 1] = temp;

            outputTextBox.AppendText($"Активирован гуляй город у {attackers[0].Side}" + Environment.NewLine);
        }
    }
}
