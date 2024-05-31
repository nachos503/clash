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
    /// Represents a class for managing battles.
    /// Implements the IBattleManager interface.
    /// Identifier: "T:ClashGame.BattleManager".
    /// </summary>
    public class BattleManager : IBattleManager
    {
        /// <summary>
        /// The battle strategy used in combat.
        /// Identifier: "F:ClashGame.BattleManager._strategy".
        /// </summary>
        public IBattleStrategy _strategy;

        /// <summary>
        /// Constructor for the BattleManager class.
        /// Initializes an instance of the BattleManager class with the specified strategy.
        /// Identifier: "M:ClashGame.BattleManager.#ctor(ClashGame.IBattleStrategy)".
        /// </summary>
        /// <param name="strategy">The battle strategy.</param>
        public BattleManager(IBattleStrategy strategy)
        {
            this._strategy = strategy ?? throw new ArgumentNullException(nameof(strategy), "Strategy cannot be null");
        }

        /// <summary>
        /// Sets the battle strategy.
        /// Identifier: "M:ClashGame.BattleManager.SetStrategy(ClashGame.IBattleStrategy)".
        /// </summary>
        /// <param name="strategy">The new battle strategy.</param>
        public void SetStrategy(IBattleStrategy strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }


        /// <summary>
        /// Flag to track steps of "Gulyay-Gorod" for the blue side.
        /// Identifier: "F:ClashGame.BattleManager.flagGulyayGorodBlue".
        /// </summary>
        public int flagGulyayGorodBlue = 0;

        /// <summary>
        /// Flag to track steps of "Gulyay-Gorod" for the red side.
        /// Identifier: "F:ClashGame.BattleManager.flagGulyayGorodRed".
        /// </summary>
        public int flagGulyayGorodRed = 0;

        /// <summary>
        /// Trigger to track activation of "Gulyay-Gorod" for the blue side.
        /// Identifier: "F:ClashGame.BattleManager.triggerGulyayGorodBlue".
        /// </summary>
        public bool triggerGulyayGorodBlue = true;

        /// <summary>
        /// Trigger to track activation of "Gulyay-Gorod" for the red side.
        /// Identifier: "F:ClashGame.BattleManager.triggerGulyayGorodRed".
        /// </summary>
        public bool triggerGulyayGorodRed = true;

        /// <summary>
        /// virtual public void Attack(Warrior attacker, Warrior defender, TextBox outputTextBox) - method for executing an attack.
        /// Executes an attack from one warrior to another and displays information about the attack in a TextBox.
        /// Identifier string: "M:ClashGame.BattleManager.Attack(ClashGame.Warrior,ClashGame.Warrior,System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attacker">The attacking warrior.</param>
        /// <param name="defender">The defending warrior.</param>
        /// <param name="outputTextBox">The TextBox for displaying information about the attack.</param>
        virtual public void Attack(Warrior attacker, Warrior defender, TextBox outputTextBox)
        {
            outputTextBox.AppendText($"Атака {attacker.Side} {attacker} с силой {attacker.Damage} по {defender.Side} {defender}" + Environment.NewLine);
            Defence(attacker, defender, outputTextBox);
        }

        /// <summary>
        /// virtual public void Defence(Warrior attacker, Warrior defender, TextBox outputTextBox) - method for executing defense.
        /// Checks if the defending warrior managed to dodge the attack and reduces their health points in case of failure.
        /// Identifier string: "M:ClashGame.BattleManager.Defence(ClashGame.Warrior,ClashGame.Warrior,System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attacker">The attacking warrior.</param>
        /// <param name="defender">The defending warrior.</param>
        /// <param name="outputTextBox">The TextBox for displaying information about the defense.</param>
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
        /// virtual public void IsDead(Warrior warrior, List<Warrior> army) - method for checking if a warrior is dead.
        /// Removes the warrior from the army list if their health points are less than or equal to zero.
        /// Identifier string: "M:ClashGame.BattleManager.IsDead(ClashGame.Warrior,System.Collections.Generic.List{ClashGame.Warrior})".
        /// </summary>
        /// <param name="warrior">The warrior to be checked.</param>
        /// <param name="army">The army to which the warrior belongs.</param>
        virtual public void IsDead(Warrior warrior, List<Warrior> army)
        {
            for (int i =0; i < army.Count(); i++)
            {
                if (army[i].Healthpoints <= 0) army.Remove(army[i]);
            }
        }

        /// <summary>
        /// virtual public void TurnComputer(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox) - method for executing the computer's turn.
        /// Executes actions of various unit types and applies the battle strategy.
        /// Identifier string: "M:ClashGame.BattleManager.TurnComputer(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">The list of attacking units.</param>
        /// <param name="defenders">The list of defending units.</param>
        /// <param name="outputTextBox">TextBox for displaying turn information.</param>
        virtual public void TurnComputer(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {

            Warrior attacker = attackers[0];
            Warrior defender = defenders[0];

            WizardTurn(attackers, defenders,outputTextBox);
            IsDead(defender, defenders);

            HealerTurn(attackers, defenders, outputTextBox);

            IsDead(defender, defenders);

            ImprovedHeavyWarriorTurn(attackers, attacker, outputTextBox);
            _strategy.ExecuteBattle(attackers, defenders, outputTextBox);

            IsDead(defender, defenders);

            ArchersTurn(attackers, defenders, outputTextBox);

            IsDead(defender, defenders);

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
            for (int i = 0; i < attackers.Count; i++)
                if (attackers[i] is Wizard)
                {
                    wizard = new Wizard(attackers[i].Side);
                    wizardIndex = i;
                    if (!_strategy.IsFrontLine(wizardIndex, defenders))
                    {
                       if (new Random().Next(0, 5) == 0)
                       {    
                            Warrior clonedWarrior = _strategy.GetNearestLightWarrior(attackers, wizardIndex);
                            if (clonedWarrior != null)
                            {
                                wizard.CloneLightWarrior(clonedWarrior);
                                attackers.Insert(1, clonedWarrior); 
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
        /// public void HealerTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox) - method for executing healer's turn.
        /// Handles the healer's turn, including healing a random allied unit.
        /// Identifier string: "M:ClashGame.BattleManager.HealerTurn(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">List of attacking units.</param>
        /// <param name="defenders">List of defending units.</param>
        /// <param name="outputTextBox">TextBox for displaying healer's turn information.</param>
        public void HealerTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Healer healer = null;
            int healerIndex = -1;
            for (int i = 0; i < attackers.Count; i++)
                if (attackers[i] is Healer)
                {
                    healer = new Healer(attackers[i].Side);
                    healerIndex = i;
                    if (!_strategy.IsFrontLine(healerIndex, defenders))
                    {
                        if (new Random().Next(0, 10) == 0)
                        {
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
        /// public void UpgradeHeavyWarrior(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox) - method for upgrading heavy warrior.
        /// Handles the upgrade of a heavy warrior to an upgraded heavy warrior if there is a nearby light warrior.
        /// Identifier string: "M:ClashGame.BattleManager.UpgradeHeavyWarrior(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">List of attacking units.</param>
        /// <param name="defenders">List of defending units.</param>
        /// <param name="outputTextBox">TextBox for displaying upgrade information.</param>
        public void UpgradeHeavyWarrior(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            for (int i = 0; i < attackers.Count; i++)
                if (attackers[i] is HeavyWarrior)
                {
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
        /// public void ImprovedHeavyWarriorTurn(List<Warrior> attackers, Warrior attacker, TextBox outputTextBox) - method for executing the turn of an improved heavy warrior.
        /// Checks for the presence of a nearby light warrior and the health of the improved heavy warrior to either cancel or maintain the upgrade.
        /// Identifier string: "M:ClashGame.BattleManager.ImprovedHeavyWarriorTurn(System.Collections.Generic.List{ClashGame.Warrior},ClashGame.Warrior,System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">List of attacking units.</param>
        /// <param name="attacker">Attacking unit.</param>
        /// <param name="outputTextBox">TextBox for displaying information about the turn of the improved heavy warrior.</param>
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

            if (improvedHeavyWarrior != null)
            {
                Warrior lightWarriorNearby = _strategy.GetNearestLightWarrior(attackers, improvedHeavyWarriorIndex);

                if (lightWarriorNearby == null)
                {
                    attackers[improvedHeavyWarriorIndex] = new HeavyWarrior(improvedHeavyWarrior.Side)
                    {
                        Healthpoints = improvedHeavyWarrior.Healthpoints // сохраняем текущее здоровье
                    };
                    outputTextBox.AppendText($"Улучшение у ImprovedHeavyWarrior отменено, так как рядом нет LightWarrior." + Environment.NewLine);
                }
            }

            if (improvedHeavyWarrior != null && improvedHeavyWarrior.Healthpoints < 0.4 * improvedHeavyWarrior.MaxHealthpoints)
            {
                attackers[improvedHeavyWarriorIndex] = new HeavyWarrior(improvedHeavyWarrior.Side);
                outputTextBox.AppendText($"Улучшение у ImprovedHeavyWarrior отменено, так как его здоровье упало ниже 40%." + Environment.NewLine);
            }
        }


        /// <summary>
        /// public void ArchersTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox) - method for executing the turn of archers.
        /// Searches for an archer among the attackers and initiates their attack on defenders if the archer is not on the front line.
        /// Identifier string: "M:ClashGame.BattleManager.ArchersTurn(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">List of attacking units.</param>
        /// <param name="defenders">List of defending units.</param>
        /// <param name="outputTextBox">TextBox for displaying information about the archers' turn.</param>
        public void ArchersTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Archer archer = null;
            int archerIndex = -1;
            for (int i = 0; i < attackers.Count; i++)
                if (attackers[i] is Archer)
                {
                    archer = new Archer(attackers[i].Side);
                    archerIndex = i;
                    if (!_strategy.IsFrontLine(archerIndex, defenders))
                    {
                        var target = _strategy.GetEnemyForArcher(attackers, defenders, archerIndex, archer); 

                        if (target != null && target is not GulyayGorod)
                        {
                            archer.RangedAttack(defenders, target, attackers.IndexOf(archer));
                            outputTextBox.AppendText($"Атака {archer.Side} {archer} с силой {archer.RangedDamage(attackers.IndexOf(archer))} по {target}" + Environment.NewLine);
                            outputTextBox.AppendText($"HP у  {target.Side} {target} осталось {target.Healthpoints}" + Environment.NewLine);
                        }

                    }
                }
        }

        /// <summary>
        /// public void GulyayGorodTurn(List<Warrior> attackers, TextBox outputTextBox) - method for executing the "Gulyay-Gorod" turn.
        /// Moves the "Gulyay-Gorod" to the first position among the attacking units.
        /// Identifier string: "M:ClashGame.BattleManager.GulyayGorodTurn(System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">List of attacking units.</param>
        /// <param name="outputTextBox">TextBox for displaying information about the "Gulyay-Gorod" turn.</param>
        public void GulyayGorodTurn(List<Warrior> attackers, TextBox outputTextBox)
        {
            var temp = attackers.First();
            attackers[0] = attackers.Last();
            attackers[attackers.Count() - 1] = temp;

            outputTextBox.AppendText($"Активирован гуляй город у {attackers[0].Side}" + Environment.NewLine);
        }


        /// <summary>
        /// public void SetGulyayGorodCount(int count, string side) - method for setting the number of "Gulyay-Gorod" turns.
        /// Sets the number of turns for "Gulyay-Gorod" depending on the side.
        /// Identifier string: "M:ClashGame.BattleManager.SetGulyayGorodCount(System.Int32,System.String)".
        /// </summary>
        /// <param name="count">The number of turns.</param>
        /// <param name="side">The side (Blue or Red).</param>
        public void SetGulyayGorodCount(int count, string side)
        {
            if (side == "Blue")
                flagGulyayGorodBlue = count;
            else
                flagGulyayGorodRed = count;
        }

        /// <summary>
        /// public void CheckGulyayGorod(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox) - method for checking the state of "Gulyay-Gorod".
        /// Checks and manages the state of "Gulyay-Gorod" for both sides, setting it up or removing it depending on the conditions.
        /// Identifier string: "M:ClashGame.BattleManager.CheckGulyayGorod(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">The list of attacking units.</param>
        /// <param name="defenders">The list of defending units.</param>
        /// <param name="outputTextBox">TextBox for displaying information about the "Gulyay-Gorod" state.</param>
        public void CheckGulyayGorod(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Random random = new Random();
            if (flagGulyayGorodBlue < 7)
            {
                if (attackers[0] is GulyayGorod && triggerGulyayGorodBlue && attackers[0].Side == "Blue")
                    triggerGulyayGorodBlue = false;

                if (defenders.Count() > 0)
                {
                    if (triggerGulyayGorodBlue && random.Next(0, 5) == 0 && attackers[0].Side == "Blue" && defenders[0] is not GulyayGorod)
                    {
                        GulyayGorodTurn(attackers, outputTextBox);
                        triggerGulyayGorodBlue = false;
                        outputTextBox.AppendText("СТЕНА ПОЯВИЛАСЬ" + Environment.NewLine);
                    }
                }

                if (!triggerGulyayGorodBlue) flagGulyayGorodBlue++;

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
