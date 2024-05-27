﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace ClashGame
{
    public class BattleManagerProxy : IBattleManager
    {
        BattleManager _battleManager;
        private readonly string _filePath;


        public BattleManagerProxy(BattleManager battleManager, string filePath)
        {
            _filePath = filePath;
            _battleManager = battleManager;
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
            CheckGulyayGorod(attackers, defenders, outputTextBox);
            Log("Ход завершен!");
        }

        public void WizardTurn(List<Warrior> attackers, TextBox outputTextBox)
        {
            Log($"Клонирование воина {attackers[0].Side} {this}");
            _battleManager.WizardTurn(attackers, outputTextBox);
        }

        public void HealerTurn(List<Warrior> attackers, TextBox outputTextBox)
        {
            Log($"Применение лечения воином {attackers[0].Side} {this}");
            _battleManager.HealerTurn(attackers, outputTextBox);
        }

        public void HeavyWarriorUpgradeTurn(List<Warrior> attackers, Warrior attacker, TextBox outputTextBox)
        {
            Log($"Улучшение воина {attackers[0].Side} {this}");
            _battleManager.HeavyWarriorUpgradeTurn(attackers, attacker, outputTextBox);
        }

        public void ArchersTurn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Log($"Атака лучниками");
            _battleManager.ArchersTurn(attackers, defenders, outputTextBox);
        }

        public void Attack(Warrior warrior1, Warrior warrior2, TextBox outputTextBox)
        {
            Log($"Атака {warrior1.Side} {warrior1} с силой {warrior1.Damage} по {warrior2.Side} {warrior2}");
            _battleManager.Attack(warrior1, warrior2, outputTextBox);
        }

        public void DefencePlease(Warrior warrior1, Warrior warrior2, TextBox outputTextBox)
        {
            Log($"Получена атака {warrior1.Side} {warrior1} с силой {warrior1.Damage} от {warrior2.Side} {warrior2}");
            _battleManager.DefencePlease(warrior1, warrior2, outputTextBox);
        }

        public void IsDead(Warrior warrior, List<Warrior> army)
        {
            if (warrior.Healthpoints <= 0)
            {
                Log($"Воин {warrior.Side} {warrior} умер!");
            }

            _battleManager.IsDead(warrior, army);
        }

        public void GulyayGorodTurn(List<Warrior> attackers, TextBox outputTextBox)
        {
            Log($"Активирован гуляй город у {attackers[0].Side}");
            _battleManager.GulyayGorodTurn(attackers, outputTextBox);
        }

        public void CheckGulyayGorod(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Log($"Попытка поставить гуляй город {attackers[0].Side}");
            _battleManager.CheckGulyayGorod(attackers, defenders, outputTextBox);
        }


    }
}