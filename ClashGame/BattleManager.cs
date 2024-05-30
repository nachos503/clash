﻿using System;
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
        public IBattleStrategy _strategy;

        public BattleManager(IBattleStrategy strategy)
        {
            this._strategy = strategy ?? throw new ArgumentNullException(nameof(strategy), "Strategy cannot be null");
        }

        public void SetStrategy(IBattleStrategy strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        public int flagGulyayGorodBlue = 0;
        public int flagGulyayGorodRed = 0;
        public bool triggerGulyayGorodBlue = true;
        public bool triggerGulyayGorodRed = true;

        virtual public void TurnComputer(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {

            Warrior attacker = attackers[0];
            Warrior defender = defenders[0];

            // Проверка наличия мага в списке атакующих
            WizardTurn(attackers, defenders,outputTextBox);
            IsDead(defender, defenders);

            // Проверка на наличие лекаря в списке атакующих и его позиции
            HealerTurn(attackers, defenders, outputTextBox);

            IsDead(defender, defenders);

            // Проверка на наличие ImprovedHeavyWarrior в списке атакующих и его позиции
            HeavyWarriorUpgradeTurn(attackers, attacker, outputTextBox);
            //Контрольная атака за ход
            _strategy.ExecuteBattle(attackers, defenders, outputTextBox);

            IsDead(defender, defenders);

            // После обычной атаки, ищем арчера в оставшемся списке воинов
            ArchersTurn(attackers, defenders, outputTextBox);

            IsDead(defender, defenders);

            //Контрольная проверка на живых соперников
            IsDead(defender, defenders);
        }

        public void WizardTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {

            Wizard wizard = null;
            int wizardIndex = -1;

            for (int i = 0; i < attackers.Count; i++)
            {
                if (attackers[i] is Wizard)
                {
                    wizard = new Wizard(attackers[i].Side);
                    wizardIndex = i;

                    if (!_strategy.IsFrontLine(wizardIndex, defenders))
                    {
                       if (new Random().Next(0, 5) == 0)
                       {    // Попытка клонирования LightWarrior
                                Warrior clonedWarrior = _strategy.GetWarriorClone(attackers, wizardIndex);
                                if (clonedWarrior != null)
                                {
                                    wizard.CloneLightWarrior(clonedWarrior);
                                    attackers.Insert(1, clonedWarrior); // Вставляем клонированного LightWarrior перед магом (на вторую позицию)
                                    outputTextBox.AppendText($"Маг из команды {wizard.Side} клонировал LightWarrior с {clonedWarrior.Healthpoints} HP" + Environment.NewLine);
                                }
                                else
                                {
                                    outputTextBox.AppendText($"Маг из команды {wizard.Side} не смог склонировать LigthWarrior: " + Environment.NewLine);
                                    outputTextBox.AppendText("LightWarrior отсутствует, либо не повезло." + Environment.NewLine);
                                }
                       }
                       else
                          outputTextBox.AppendText($"Маг из команды {wizard.Side} не смог клонировать LightWarrior" + Environment.NewLine);
                    }
                }
            }
        }

        public void HealerTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Healer healer = null;
            int healerIndex = -1;
            for (int i = 0; i < attackers.Count; i++)
            {
                if (attackers[i] is Healer)
                {
                    healer = new Healer(attackers[i].Side);
                    healerIndex = i;

                    if (!_strategy.IsFrontLine(healerIndex, defenders))
                    {
                        // Проверка на выполнение условий для лечения
                        if (new Random().Next(0, 10) == 0)
                        {
                            // Вызов лечения у случайного союзника
                            Warrior targetAlly = _strategy.GetWarriorHeal(attackers, healerIndex, healer);
                            if (targetAlly != null)
                            {
                                healer.Heal(targetAlly);
                                outputTextBox.AppendText($"Лекарь из команды {healer.Side} вылечил {targetAlly.Side} {targetAlly}" + Environment.NewLine);
                                outputTextBox.AppendText($"Теперь у {targetAlly.Side} {targetAlly} {targetAlly.Healthpoints} HP" + Environment.NewLine);
                            }

                        }
                        else
                        {
                            outputTextBox.AppendText($"Лекарь из команды {healer.Side} никого не вылечил" + Environment.NewLine);
                            outputTextBox.AppendText("Не прокнуло." + Environment.NewLine);
                        }
                    }
                }
            }
        }


        public void UpgradeHeavyWarrior(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {

            for (int i = 0; i < attackers.Count; i++)
            {
                if (attackers[i] is HeavyWarrior)
                {
                    if (!_strategy.IsFrontLine(i, defenders))
                    {
                        Warrior lightWarriorNearby = _strategy.GetWarriorClone(attackers, i);
                        if (lightWarriorNearby != null)
                        {
                            ImprovedHeavyWarrior improvedHeavyWarrior = new ImprovedHeavyWarrior((HeavyWarrior)attackers[i]);
                            attackers[i] = improvedHeavyWarrior;
                            outputTextBox.AppendText($"Улучшение у ImprovedHeavyWarrior прошло, рядом есть LightWarrior." + Environment.NewLine);
                        }
                    }
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
            if (improvedHeavyWarrior != null)
            {
                // Проверяем наличие LightWarrior на расстоянии 1 от ImprovedHeavyWarrior
                Warrior lightWarriorNearby = _strategy.GetWarriorClone(attackers, improvedHeavyWarriorIndex);

                if (lightWarriorNearby == null)
                {
                    // Если рядом с ImprovedHeavyWarrior нет LightWarrior, отменяем улучшение
                    attackers[improvedHeavyWarriorIndex] = new HeavyWarrior(improvedHeavyWarrior.Side)
                    {
                        Healthpoints = improvedHeavyWarrior.Healthpoints // сохраняем текущее здоровье
                    };
                    outputTextBox.AppendText($"Улучшение у ImprovedHeavyWarrior отменено, так как рядом нет LightWarrior." + Environment.NewLine);
                }
            }

            // Проверяем здоровье ImprovedHeavyWarrior
            if (improvedHeavyWarrior != null && improvedHeavyWarrior.Healthpoints < 0.4 * improvedHeavyWarrior.MaxHealthpoints)
            {
                // Если здоровье упало ниже 40%, отменяем улучшение
                attackers[improvedHeavyWarriorIndex] = new HeavyWarrior(improvedHeavyWarrior.Side);
                outputTextBox.AppendText($"Улучшение у ImprovedHeavyWarrior отменено, так как его здоровье упало ниже 40%." + Environment.NewLine);
            }
        }

        public void ArchersTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Archer archer = null;
            int archerIndex = -1;
            for (int i = 0; i < attackers.Count; i++)
            {
                if (attackers[i] is Archer)
                {
                    archer = new Archer(attackers[i].Side);
                    archerIndex = i;

                    if (!_strategy.IsFrontLine(archerIndex, defenders))
                    {
                        var target = _strategy.GetEnemyWarrior(attackers, defenders, archerIndex, archer); // Выбранный защищающийся воин

                        if (target != null && target is not GulyayGorod)
                        {
                            // Передача индекса атакующего лучника
                            archer.RangedAttack(defenders, target, attackers.IndexOf(archer));
                            outputTextBox.AppendText($"Атака {archer.Side} {archer} с силой {archer.RangedDamage(attackers.IndexOf(archer))} по {target}" + Environment.NewLine);
                            outputTextBox.AppendText($"HP у  {target.Side} {target} осталось {target.Healthpoints}" + Environment.NewLine);
                        }

                    }
                }
            }
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


        //получает из окна сколько пользователь успел сделать ходов пока стена стоит
        public void SetGulyayGorodCount(int count, string side)
        {
            if (side == "Blue")
                flagGulyayGorodBlue = count;
            else
                flagGulyayGorodRed = count;
        }

        public void CheckGulyayGorod(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
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
                if (defenders.Count() > 0)
                {
                    if (triggerGulyayGorodBlue && rand.Next(0, 5) == 0 && attackers[0].Side == "Blue" && defenders[0] is not GulyayGorod)
                    {
                        GulyayGorodTurn(attackers, outputTextBox);
                        triggerGulyayGorodBlue = false;
                        outputTextBox.AppendText("СТЕНА ПОЯВИЛАСЬ" + Environment.NewLine);
                    }
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
        }
    }
}
