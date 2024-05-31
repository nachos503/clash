using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace ClashGame
{
    /// <summary>
    /// BattleManagerProxy class representing a proxy for battle management with logging.
    /// Identifier string "T:ClashGame.BattleManagerProxy".
    /// </summary>
    public class BattleManagerProxy : IBattleManager
    {
        /// <summary>
        /// Instance of BattleManager.
        /// Identifier string "F:ClashGame.BattleManagerProxy._battleManager".
        /// </summary>
        BattleManager _battleManager;

        /// <summary>
        /// Path to the log file.
        /// Identifier string "F:ClashGame.BattleManagerProxy._filePath".
        /// </summary>
        private readonly string _filePath;

        /// <summary>
        /// Constructor for the BattleManagerProxy class.
        /// Identifier string "M:ClashGame.BattleManagerProxy.#ctor(ClashGame.BattleManager,System.String)".
        /// </summary>
        /// <param name="battleManager">Instance of BattleManager.</param>
        /// <param name="filePath">Path to the log file.</param>
        public BattleManagerProxy(BattleManager battleManager, string filePath)
        {
            _filePath = filePath;
            _battleManager = battleManager;
            using (StreamWriter writer = File.AppendText(_filePath))
            {
                writer.WriteLine($"{DateTime.Now}: Starting logging...");
            }
        }

        /// <summary>
        /// Function to write a message to the log file.
        /// Identifier string "M:ClashGame.BattleManagerProxy.Log(System.String)".
        /// </summary>
        /// <param name="message">Message to log.</param>
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

        /// <summary>
        /// Performs the computer's turn, logs the actions.
        /// Identifier string "M:ClashGame.BattleManagerProxy.TurnComputer(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="defenders">List of defending warriors.</param>
        /// <param name="outputTextBox">TextBox for displaying battle information.</param>
        public void TurnComputer(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Log("Ход начат!");

            WizardTurn(attackers, defenders, outputTextBox);
            if (defenders.Count > 0)  IsDead(defenders[0], defenders);
            HealerTurn(attackers, defenders,outputTextBox);
            ImprovedHeavyWarriorTurn(attackers, attackers[0], outputTextBox);
            if (attackers[0] is not GulyayGorod && defenders[0] is not GulyayGorod)
                _battleManager._strategy.ExecuteBattle(attackers, defenders, outputTextBox);
            if (defenders.Count > 0) IsDead(defenders[0], defenders);
            ArchersTurn(attackers, defenders, outputTextBox);
            if (defenders.Count > 0) IsDead(defenders[0], defenders);
            CheckGulyayGorod(attackers, defenders, outputTextBox);

            Log("Ход завершен!");
        }

        /// <summary>
        /// Performs a wizard's turn, logs the action.
        /// Identifier string "M:ClashGame.BattleManagerProxy.WizardTurn(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="defenders">List of defending warriors.</param>
        /// <param name="outputTextBox">TextBox for displaying battle information.</param>
        public void WizardTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Log($"Клонирование воина {attackers[0].Side} {this}");
            _battleManager.WizardTurn(attackers, defenders, outputTextBox);
        }

        /// <summary>
        /// Performs a healer's turn, logs the action.
        /// Identifier string "M:ClashGame.BattleManagerProxy.HealerTurn(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="defenders">List of defending warriors.</param>
        /// <param name="outputTextBox">TextBox for displaying battle information.</param>
        public void HealerTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Log($"Применение лечения воином {attackers[0].Side} {this}");
            _battleManager.HealerTurn(attackers, defenders, outputTextBox);
        }

        /// <summary>
        /// Performs an improvement on a heavy warrior, logs the action.
        /// Identifier string "M:ClashGame.BattleManagerProxy.ImprovedHeavyWarriorTurn(System.Collections.Generic.List{ClashGame.Warrior},ClashGame.Warrior,System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="attacker">The warrior to be improved.</param>
        /// <param name="outputTextBox">TextBox for displaying battle information.</param>
        public void ImprovedHeavyWarriorTurn(List<Warrior> attackers, Warrior attacker, TextBox outputTextBox)
        {
            Log($"Улучшение воина {attackers[0].Side} {this}");
            _battleManager.ImprovedHeavyWarriorTurn(attackers, attacker, outputTextBox);
        }

        /// <summary>
        /// Performs an archer attack, logs the action.
        /// Identifier string "M:ClashGame.BattleManagerProxy.ArchersTurn(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="defenders">List of defending warriors.</param>
        /// <param name="outputTextBox">TextBox for displaying battle information.</param>

        public void ArchersTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Log($"Атака лучниками");
            _battleManager.ArchersTurn(attackers, defenders, outputTextBox);
        }

        /// <summary>
        /// Performs an attack, logs the action.
        /// Identifier string "M:ClashGame.BattleManagerProxy.Attack(ClashGame.Warrior,ClashGame.Warrior,System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="warrior1">The warrior who is attacking.</param>
        /// <param name="warrior2">The warrior who is defending.</param>
        /// <param name="outputTextBox">TextBox for displaying battle information.</param>
        public void Attack(Warrior warrior1, Warrior warrior2, TextBox outputTextBox)
        {
            Log($"Атака {warrior1.Side} {warrior1} с силой {warrior1.Damage} по {warrior2.Side} {warrior2}");
            _battleManager.Attack(warrior1, warrior2, outputTextBox);
        }

        /// <summary>
        /// Performs defense, logs the action.
        /// Identifier string "M:ClashGame.BattleManagerProxy.Defence(ClashGame.Warrior,ClashGame.Warrior,System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="warrior1">The warrior who is defending.</param>
        /// <param name="warrior2">The warrior who is attacking.</param>
        /// <param name="outputTextBox">TextBox for displaying battle information.</param>
        public void Defence(Warrior warrior1, Warrior warrior2, TextBox outputTextBox)
        {
            Log($"Получена атака {warrior1.Side} {warrior1} с силой {warrior1.Damage} от {warrior2.Side} {warrior2}");
            _battleManager.Defence(warrior1, warrior2, outputTextBox);
        }

        /// <summary>
        /// Checks if a warrior is dead, logs the action.
        /// Identifier string "M:ClashGame.BattleManagerProxy.IsDead(ClashGame.Warrior,System.Collections.Generic.List{ClashGame.Warrior})".
        /// </summary>
        /// <param name="warrior">Warrior to check.</param>
        /// <param name="army">List of the army.</param>
        public void IsDead(Warrior warrior, List<Warrior> army)
        {
            if (warrior.Healthpoints <= 0) Log($"Воин {warrior.Side} {warrior} умер!");

            _battleManager.IsDead(warrior, army);
        }

        /// <summary>
        /// Executes the Gulyay-Gorod turn, logs the action.
        /// Identifier string "M:ClashGame.BattleManagerProxy.GulyayGorodTurn(System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="outputTextBox">TextBox for displaying battle information.</param>
        public void GulyayGorodTurn(List<Warrior> attackers, TextBox outputTextBox)
        {
            Log($"Активирован гуляй город у {attackers[0].Side}");
            _battleManager.GulyayGorodTurn(attackers, outputTextBox);
        }

        /// <summary>
        /// Checks the possibility of using Gulyay-Gorod, logs the action.
        /// Identifier string "M:ClashGame.BattleManagerProxy.CheckGulyayGorod(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">List of attacking warriors.</param>
        /// <param name="defenders">List of defending warriors.</param>
        /// <param name="outputTextBox">TextBox for displaying battle information.</param>
        public void CheckGulyayGorod(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Log($"Попытка поставить гуляй город {attackers[0].Side}");
            _battleManager.CheckGulyayGorod(attackers, defenders, outputTextBox);
        }
    }
}