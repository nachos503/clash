using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace ClashGame
{
    /// <summary>
    /// Класс BattleManagerProxy, представляющий прокси для управления битвой с логированием.
    /// Строка идентификатора "T:ClashGame.BattleManagerProxy".
    /// </summary>
    public class BattleManagerProxy : IBattleManager
    {
        /// <summary>
        /// Экземпляр BattleManager.
        /// Строка идентификатора "F:ClashGame.BattleManagerProxy._battleManager".
        /// </summary>
        BattleManager _battleManager;

        /// <summary>
        /// Путь к файлу логов.
        /// Строка идентификатора "F:ClashGame.BattleManagerProxy._filePath".
        /// </summary>
        private readonly string _filePath;

        /// <summary>
        /// Конструктор для класса BattleManagerProxy.
        /// Строка идентификатора "M:ClashGame.BattleManagerProxy.#ctor(ClashGame.BattleManager,System.String)".
        /// </summary>
        /// <param name="battleManager">Экземпляр BattleManager.</param>
        /// <param name="filePath">Путь к файлу логов.</param>
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
        /// Функция для записи сообщения в файл логов.
        /// Строка идентификатора "M:ClashGame.BattleManagerProxy.Log(System.String)".
        /// </summary>
        /// <param name="message">Сообщение для логирования.</param>
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
        /// Выполняет ход компьютера, логирует действия.
        /// Строка идентификатора "M:ClashGame.BattleManagerProxy.TurnComputer(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе битвы.</param>
        public void TurnComputer(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Log("Ход начат!");

            WizardTurn(attackers, defenders, outputTextBox);
            if (defenders.Count > 0)  IsDead(defenders[0], defenders);
            HealerTurn(attackers, defenders,outputTextBox);
            ImprovedHeavyWarriorTurn(attackers, attackers[0], outputTextBox);
            if (attackers[0] is not GulyayGorod || defenders[0] is not GulyayGorod)
                _battleManager._strategy.ExecuteBattle(attackers, defenders, outputTextBox);
            if (defenders.Count > 0) IsDead(defenders[0], defenders);
            ArchersTurn(attackers, defenders, outputTextBox);
            if (defenders.Count > 0) IsDead(defenders[0], defenders);
            CheckGulyayGorod(attackers, defenders, outputTextBox);

            Log("Ход завершен!");
        }

        /// <summary>
        /// Выполняет ход мага, логирует действие.
        /// Строка идентификатора "M:ClashGame.BattleManagerProxy.WizardTurn(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе битвы.</param>
        public void WizardTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Log($"Клонирование воина {attackers[0].Side} {this}");
            _battleManager.WizardTurn(attackers, defenders, outputTextBox);
        }

        /// <summary>
        /// Выполняет ход лекаря, логирует действие.
        /// Строка идентификатора "M:ClashGame.BattleManagerProxy.HealerTurn(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе битвы.</param>
        public void HealerTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Log($"Применение лечения воином {attackers[0].Side} {this}");
            _battleManager.HealerTurn(attackers, defenders, outputTextBox);
        }

        /// <summary>
        /// Выполняет улучшение тяжелого воина, логирует действие.
        /// Строка идентификатора "M:ClashGame.BattleManagerProxy.ImprovedHeavyWarriorTurn(System.Collections.Generic.List{ClashGame.Warrior},ClashGame.Warrior,System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="attacker">Улучшаемый воин.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе битвы.</param>
        public void ImprovedHeavyWarriorTurn(List<Warrior> attackers, Warrior attacker, TextBox outputTextBox)
        {
            Log($"Улучшение воина {attackers[0].Side} {this}");
            _battleManager.ImprovedHeavyWarriorTurn(attackers, attacker, outputTextBox);
        }

        /// <summary>
        /// Выполняет атаку лучников, логирует действие.
        /// Строка идентификатора "M:ClashGame.BattleManagerProxy.ArchersTurn(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе битвы.</param>
        public void ArchersTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Log($"Атака лучниками");
            _battleManager.ArchersTurn(attackers, defenders, outputTextBox);
        }

        /// <summary>
        /// Выполняет атаку, логирует действие.
        /// Строка идентификатора "M:ClashGame.BattleManagerProxy.Attack(ClashGame.Warrior,ClashGame.Warrior,System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="warrior1">Воин, который атакует.</param>
        /// <param name="warrior2">Воин, который защищается.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе битвы.</param>
        public void Attack(Warrior warrior1, Warrior warrior2, TextBox outputTextBox)
        {
            Log($"Атака {warrior1.Side} {warrior1} с силой {warrior1.Damage} по {warrior2.Side} {warrior2}");
            _battleManager.Attack(warrior1, warrior2, outputTextBox);
        }

        /// <summary>
        /// Выполняет защиту, логирует действие.
        /// Строка идентификатора "M:ClashGame.BattleManagerProxy.Defence(ClashGame.Warrior,ClashGame.Warrior,System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="warrior1">Воин, который защищается.</param>
        /// <param name="warrior2">Воин, который атакует.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе битвы.</param>
        public void Defence(Warrior warrior1, Warrior warrior2, TextBox outputTextBox)
        {
            Log($"Получена атака {warrior1.Side} {warrior1} с силой {warrior1.Damage} от {warrior2.Side} {warrior2}");
            _battleManager.Defence(warrior1, warrior2, outputTextBox);
        }

        /// <summary>
        /// Проверяет, мертв ли воин, логирует действие.
        /// Строка идентификатора "M:ClashGame.BattleManagerProxy.IsDead(ClashGame.Warrior,System.Collections.Generic.List{ClashGame.Warrior})".
        /// </summary>
        /// <param name="warrior">Воин для проверки.</param>
        /// <param name="army">Список армии.</param>
        public void IsDead(Warrior warrior, List<Warrior> army)
        {
            if (warrior.Healthpoints <= 0) Log($"Воин {warrior.Side} {warrior} умер!");

            _battleManager.IsDead(warrior, army);
        }

        /// <summary>
        /// Выполняет ход Гуляй-города, логирует действие.
        /// Строка идентификатора "M:ClashGame.BattleManagerProxy.GulyayGorodTurn(System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе битвы.</param>
        public void GulyayGorodTurn(List<Warrior> attackers, TextBox outputTextBox)
        {
            Log($"Активирован гуляй город у {attackers[0].Side}");
            _battleManager.GulyayGorodTurn(attackers, outputTextBox);
        }

        /// <summary>
        /// Проверяет возможность использования Гуляй-города, логирует действие.
        /// Строка идентификатора "M:ClashGame.BattleManagerProxy.CheckGulyayGorod(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе битвы.</param>
        public void CheckGulyayGorod(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Log($"Попытка поставить гуляй город {attackers[0].Side}");
            _battleManager.CheckGulyayGorod(attackers, defenders, outputTextBox);
        }
    }
}