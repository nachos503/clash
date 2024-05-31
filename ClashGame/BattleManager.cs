using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ClashGame;

namespace ClashGame
{
    /// <summary>
    /// public class BattleManager - класс для управления атаками.
    /// Реализует интерфейс IBattleManager.
    /// Строка идентификатора "T:ClashGame.BattleManager".
    /// </summary>
    public class BattleManager : IBattleManager
    {
        /// <summary>
        /// public IBattleStrategy _strategy; - стратегия ведения боя.
        /// Строка идентификатора "F:ClashGame.BattleManager._strategy".
        /// </summary>
        public IBattleStrategy _strategy;

        /// <summary>
        /// public BattleManager(IBattleStrategy strategy) - конструктор класса BattleManager.
        /// Инициализирует экземпляр класса BattleManager с заданной стратегией.
        /// Строка идентификатора "M:ClashGame.BattleManager.BattleManager(ClashGame.IBattleStrategy)".
        /// </summary>
        /// <param name="strategy">Стратегия ведения боя.</param>
        public BattleManager(IBattleStrategy strategy)
        {
            this._strategy = strategy ?? throw new ArgumentNullException(nameof(strategy), "Strategy cannot be null");
        }

        /// <summary>
        /// public void SetStrategy(IBattleStrategy strategy) - метод для установки стратегии ведения боя.
        /// Строка идентификатора "M:ClashGame.BattleManager.SetStrategy(ClashGame.IBattleStrategy)".
        /// </summary>
        /// <param name="strategy">Новая стратегия ведения боя.</param>
        public void SetStrategy(IBattleStrategy strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }


        /// <summary>
        /// public int flagGulyayGorodBlue - флаг для отслеживания шагов "Гуляй-город" для синей стороны.
        /// Строка идентификатора "F:ClashGame.BattleManager.flagGulyayGorodBlue".
        /// </summary>
        public int flagGulyayGorodBlue = 0;

        /// <summary>
        /// public int flagGulyayGorodRed - флаг для отслеживания шагов "Гуляй-город" для красной стороны.
        /// Строка идентификатора "F:ClashGame.BattleManager.flagGulyayGorodRed".
        /// </summary>
        public int flagGulyayGorodRed = 0;

        /// <summary>
        /// public bool triggerGulyayGorodBlue - триггер для отслеживания активации "Гуляй-город" для синей стороны.
        /// Строка идентификатора "F:ClashGame.BattleManager.triggerGulyayGorodBlue".
        /// </summary>
        public bool triggerGulyayGorodBlue = true;

        /// <summary>
        /// public bool triggerGulyayGorodRed - триггер для отслеживания активации "Гуляй-город" для красной стороны.
        /// Строка идентификатора "F:ClashGame.BattleManager.triggerGulyayGorodRed".
        /// </summary>
        public bool triggerGulyayGorodRed = true;

        /// <summary>
        /// virtual public void Attack(Warrior attacker, Warrior defender, TextBox outputTextBox) - метод для выполнения атаки.
        /// Выполняет атаку одного воина на другого и выводит информацию об атаке в TextBox.
        /// Строка идентификатора "M:ClashGame.BattleManager.Attack(ClashGame.Warrior,ClashGame.Warrior,System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attacker">Атакующий воин.</param>
        /// <param name="defender">Защищающийся воин.</param>
        /// <param name="outputTextBox">TextBox для вывода информации об атаке.</param>
        virtual public void Attack(Warrior attacker, Warrior defender, TextBox outputTextBox)
        {
            outputTextBox.AppendText($"Атака {attacker.Side} {attacker} с силой {attacker.Damage} по {defender.Side} {defender}" + Environment.NewLine);
            Defence(attacker, defender, outputTextBox);
        }

        /// <summary>
        /// virtual public void Defence(Warrior attacker, Warrior defender, TextBox outputTextBox) - метод для выполнения защиты.
        /// Проверяет, удалось ли защищающемуся уклониться от атаки, и уменьшает его очки здоровья в случае неудачи.
        /// Строка идентификатора "M:ClashGame.BattleManager.Defence(ClashGame.Warrior,ClashGame.Warrior,System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attacker">Атакующий воин.</param>
        /// <param name="defender">Защищающийся воин.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о защите.</param>
        virtual public void Defence(Warrior attacker, Warrior defender, TextBox outputTextBox)
        {
            Random random = new Random();

            if (random.Next(0, 101) > defender.Dodge)
            {
                defender.Healthpoints -= attacker.Damage;
                outputTextBox.AppendText($"HP у {defender.Side} {defender} осталось {defender.Healthpoints}" + Environment.NewLine);
            }

            else
                outputTextBox.AppendText("Dodge cool" + Environment.NewLine);
        }


        /// <summary>
        /// virtual public void IsDead(Warrior warrior, List<Warrior> army) - метод для проверки, мертв ли воин.
        /// Удаляет воина из списка армии, если его очки здоровья меньше или равны нулю.
        /// Строка идентификатора "M:ClashGame.BattleManager.IsDead(ClashGame.Warrior,System.Collections.Generic.List{ClashGame.Warrior})".
        /// </summary>
        /// <param name="warrior">Проверяемый воин.</param>
        /// <param name="army">Армия, к которой принадлежит воин.</param>
        virtual public void IsDead(Warrior warrior, List<Warrior> army)
        {
            if (warrior.Healthpoints <= 0) army.Remove(warrior);
        }

        /// <summary>
        /// virtual public void TurnComputer(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox) - метод для выполнения хода компьютера.
        /// Выполняет ходы различных типов юнитов и применяет стратегию боя.
        /// Строка идентификатора "M:ClashGame.BattleManager.TurnComputer(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих юнитов.</param>
        /// <param name="defenders">Список защищающихся юнитов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе.</param>
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
            ImprovedHeavyWarriorTurn(attackers, attacker, outputTextBox);
            //Контрольная атака за ход
            _strategy.ExecuteBattle(attackers, defenders, outputTextBox);

            IsDead(defender, defenders);

            // После обычной атаки, ищем арчера в оставшемся списке воинов
            ArchersTurn(attackers, defenders, outputTextBox);

            IsDead(defender, defenders);

            //Контрольная проверка на живых соперников
            IsDead(defender, defenders);
        }

        /// <summary>
        /// public void WizardTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox) - метод для выполнения хода волшебника.
        /// Волшебник может клонировать лёгкого воина, если он не на первой линии.
        /// Строка идентификатора "M:ClashGame.BattleManager.WizardTurn(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих юнитов.</param>
        /// <param name="defenders">Список защищающихся юнитов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе волшебника.</param>
        public void WizardTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Wizard wizard = null;
            int wizardIndex = -1;
            //ищем мага
            for (int i = 0; i < attackers.Count; i++)
                if (attackers[i] is Wizard)
                {
                    wizard = new Wizard(attackers[i].Side);
                    wizardIndex = i;
                    //если не на первой линии то может клонировать
                    if (!_strategy.IsFrontLine(wizardIndex, defenders))
                    {
                       if (new Random().Next(0, 5) == 0)
                       {    // Попытка клонирования LightWarrior
                            Warrior clonedWarrior = _strategy.GetNearestLightWarrior(attackers, wizardIndex);
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


        /// <summary>
        /// public void HealerTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox) - метод для выполнения хода лекаря.
        /// Обрабатывает ход лекаря, включая лечение случайного союзного юнита.
        /// Строка идентификатора "M:ClashGame.BattleManager.HealerTurn(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих юнитов.</param>
        /// <param name="defenders">Список защищающихся юнитов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе лекаря.</param>
        public void HealerTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Healer healer = null;
            int healerIndex = -1;
            //ищем среди всех лекаря
            for (int i = 0; i < attackers.Count; i++)
                if (attackers[i] is Healer)
                {
                    healer = new Healer(attackers[i].Side);
                    healerIndex = i;
                    //если не на первой линии
                    if (!_strategy.IsFrontLine(healerIndex, defenders))
                    {
                        // Проверка на выполнение условий для лечения
                        if (new Random().Next(0, 10) == 0)
                        {
                            // Вызов лечения у случайного союзника
                            Warrior targetAlly = _strategy.GetWarriorForHeal(attackers, healerIndex, healer);
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

        /// <summary>
        /// public void UpgradeHeavyWarrior(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox) - метод для улучшения тяжелого воина.
        /// Обрабатывает улучшение тяжелого воина до улучшенного тяжелого воина, если рядом есть легкий воин.
        /// Строка идентификатора "M:ClashGame.BattleManager.UpgradeHeavyWarrior(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих юнитов.</param>
        /// <param name="defenders">Список защищающихся юнитов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации об улучшении.</param>
        public void UpgradeHeavyWarrior(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            for (int i = 0; i < attackers.Count; i++)
                if (attackers[i] is HeavyWarrior)
                {
                    //но только если не на первой линии
                    if (!_strategy.IsFrontLine(i, defenders))
                    {
                        Warrior nearestLightWarrior = _strategy.GetNearestLightWarrior(attackers, i);
                        if (nearestLightWarrior != null)
                        {
                            ImprovedHeavyWarrior improvedHeavyWarrior = new ImprovedHeavyWarrior((HeavyWarrior)attackers[i]);
                            attackers[i] = improvedHeavyWarrior;
                            outputTextBox.AppendText($"Улучшение у ImprovedHeavyWarrior прошло, рядом есть LightWarrior." + Environment.NewLine);
                        }
                    }
                }
        }

        /// <summary>
        /// public void ImprovedHeavyWarriorTurn(List<Warrior> attackers, Warrior attacker, TextBox outputTextBox) - метод для выполнения хода улучшенного тяжелого воина.
        /// Проверяет наличие рядом легкого воина и здоровье улучшенного тяжелого воина, чтобы отменить или сохранить улучшение.
        /// Строка идентификатора "M:ClashGame.BattleManager.ImprovedHeavyWarriorTurn(System.Collections.Generic.List{ClashGame.Warrior},ClashGame.Warrior,System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих юнитов.</param>
        /// <param name="attacker">Атакующий юнит.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе улучшенного тяжелого воина.</param>
        public void ImprovedHeavyWarriorTurn(List<Warrior> attackers, Warrior attacker, TextBox outputTextBox)
        {
            ImprovedHeavyWarrior improvedHeavyWarrior = null;
            int improvedHeavyWarriorIndex = -1;
            for (int i = 0; i < attackers.Count; i++)
                if (attackers[i] is ImprovedHeavyWarrior)
                {
                    improvedHeavyWarrior = (ImprovedHeavyWarrior)attackers[i];
                    improvedHeavyWarriorIndex = i;
                    break;
                }

            // Проверяем, есть ли рядом с ImprovedHeavyWarrior LightWarrior
            if (improvedHeavyWarrior != null)
            {
                // Проверяем наличие LightWarrior на расстоянии 1 от ImprovedHeavyWarrior
                Warrior lightWarriorNearby = _strategy.GetNearestLightWarrior(attackers, improvedHeavyWarriorIndex);

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


        /// <summary>
        /// public void ArchersTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox) - метод для выполнения хода лучников.
        /// Ищет лучника среди атакующих и инициирует его атаку на защитников, если лучник не находится на первой линии.
        /// Строка идентификатора "M:ClashGame.BattleManager.ArchersTurn(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих юнитов.</param>
        /// <param name="defenders">Список защищающихся юнитов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе лучников.</param>
        public void ArchersTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Archer archer = null;
            int archerIndex = -1;
            //ищем среди всех
            for (int i = 0; i < attackers.Count; i++)
                if (attackers[i] is Archer)
                {
                    archer = new Archer(attackers[i].Side);
                    archerIndex = i;
                    //если не первая линия
                    if (!_strategy.IsFrontLine(archerIndex, defenders))
                    {
                        var target = _strategy.GetEnemyForArcher(attackers, defenders, archerIndex, archer); // Выбранный защищающийся воин

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

        /// <summary>
        /// public void GulyayGorodTurn(List<Warrior> attackers, TextBox outputTextBox) - метод для выполнения хода "Гуляй-город".
        /// Перемещает "Гуляй-город" на первую позицию среди атакующих юнитов.
        /// Строка идентификатора "M:ClashGame.BattleManager.GulyayGorodTurn(System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих юнитов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе "Гуляй-город".</param>
        public void GulyayGorodTurn(List<Warrior> attackers, TextBox outputTextBox)
        {
            //ставим вперед
            var temp = attackers.First();
            attackers[0] = attackers.Last();
            attackers[attackers.Count() - 1] = temp;

            outputTextBox.AppendText($"Активирован гуляй город у {attackers[0].Side}" + Environment.NewLine);
        }


        /// <summary>
        /// public void SetGulyayGorodCount(int count, string side) - метод для установки количества ходов "Гуляй-город".
        /// Устанавливает количество ходов для "Гуляй-город" в зависимости от стороны.
        /// Строка идентификатора "M:ClashGame.BattleManager.SetGulyayGorodCount(System.Int32,System.String)".
        /// </summary>
        /// <param name="count">Количество ходов.</param>
        /// <param name="side">Сторона (Blue или Red).</param>
        public void SetGulyayGorodCount(int count, string side)
        {
            if (side == "Blue")
                flagGulyayGorodBlue = count;
            else
                flagGulyayGorodRed = count;
        }

        /// <summary>
        /// public void CheckGulyayGorod(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox) - метод для проверки состояния "Гуляй-город".
        /// Проверяет и управляет состоянием "Гуляй-город" для обеих сторон, устанавливая или убирая его в зависимости от условий.
        /// Строка идентификатора "M:ClashGame.BattleManager.CheckGulyayGorod(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих юнитов.</param>
        /// <param name="defenders">Список защищающихся юнитов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о состоянии "Гуляй-город".</param>
        public void CheckGulyayGorod(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Random random = new Random();
            //проверка что стены не было
            if (flagGulyayGorodBlue < 7)
            {
                //проверка что пользователь вызвал стену но не доиграл ее
                if (attackers[0] is GulyayGorod && triggerGulyayGorodBlue && attackers[0].Side == "Blue")
                    triggerGulyayGorodBlue = false;

                //попытка поставить стену в первый раз компьютером
                if (defenders.Count() > 0)
                {
                    if (triggerGulyayGorodBlue && random.Next(0, 5) == 0 && attackers[0].Side == "Blue" && defenders[0] is not GulyayGorod)
                    {
                        GulyayGorodTurn(attackers, outputTextBox);
                        triggerGulyayGorodBlue = false;
                        outputTextBox.AppendText("СТЕНА ПОЯВИЛАСЬ" + Environment.NewLine);
                    }
                }

                //счетчик ходов когда стена стоит
                if (!triggerGulyayGorodBlue) flagGulyayGorodBlue++;

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
                    triggerGulyayGorodRed = false;

                if (triggerGulyayGorodRed && random.Next(0, 5) == 0 && attackers[0].Side == "Red" && defenders[0] is not GulyayGorod)
                {
                    GulyayGorodTurn(attackers, outputTextBox);
                    triggerGulyayGorodRed = false;
                    outputTextBox.AppendText("СТЕНА ПОЯВИЛАСЬ" + Environment.NewLine);
                }

                if (!triggerGulyayGorodRed)  flagGulyayGorodRed++;

                if (flagGulyayGorodRed == 7)
                {
                    attackers.Remove(attackers[0]);
                    outputTextBox.AppendText("СТЕНА УБРАЛАСЬ Красные" + Environment.NewLine);
                }
            }
        }
    }
}
