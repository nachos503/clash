﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace ClashGame
{
    public partial class MainWindow : Window
    {
        private readonly ArmyManager armyManager;
        private BattleManagerProxy battleManagerProxy;
        private CommandManager commandManager;
        private BattleManager battleManager;

        private List<Warrior> playerArmy;
        private List<Warrior> computerArmy;
        private bool playerTurn = true;

        private bool wizardUsed = false;
        private bool healerUsed = false;
        private bool archerUsed = false;

        private int countTurnsForGulyayGorod;

        private IBattleStrategy currentStrategy = new TwoRowStrategy();
        public MainWindow()
        {
            InitializeComponent();
            armyManager = new ArmyManager(outputTextBox, new ArmyUnitFactory());
            battleManager = new BattleManager(currentStrategy);
            battleManagerProxy = new BattleManagerProxy(battleManager, "1.txt");

            commandManager = new CommandManager();
            InitializeUI();
        }

        private void InitializeUI()
        {
            ChooseBlueArmy.Visibility = Visibility.Hidden;
            ChooseRedArmy.Visibility = Visibility.Hidden;
            ChooseBlueArmy.Visibility = Visibility.Hidden;
            UseWizard.Visibility = Visibility.Hidden;
            UseHealer.Visibility = Visibility.Hidden;
            UseArcher.Visibility = Visibility.Hidden;
            CanсelTurn.Visibility = Visibility.Hidden;
            ToTheEnd.Visibility = Visibility.Hidden;
            Turn.Visibility = Visibility.Hidden;
            CanсelTurn.Visibility = Visibility.Hidden;
            UseGulyayGorod.Visibility = Visibility.Hidden;
            EndTurn.Visibility = Visibility.Hidden;
            DisableAbilityButtons();

            // Hide strategy buttons initially
            ChooseTwoRows.Visibility = Visibility.Hidden;
            ChooseThreeRows.Visibility = Visibility.Hidden;
            ChooseWallToWall.Visibility = Visibility.Hidden;
            ChooseStrategy.Visibility = Visibility.Hidden;

  
            battlefieldCanvas = new Canvas();
            battlefieldCanvas.Width = 800;
            battlefieldCanvas.Height = 400;
            MainGrid.Children.Add(battlefieldCanvas);
           
        }

        #region Clicks
        private void DisableAbilityButtons()
        {
            UseWizard.IsEnabled = false;
            UseHealer.IsEnabled = false;
            UseArcher.IsEnabled = false;
            CanсelTurn.IsEnabled = false;
            UseGulyayGorod.IsEnabled = false;
        }

        private void CreateArmy_Click(object sender, RoutedEventArgs e)
        {
            CreateArmyClick(outputTextBox);
            DisableAbilityButtons();
            ChooseBlueArmy.Visibility = Visibility.Visible;
            ChooseRedArmy.Visibility = Visibility.Visible;
            ChooseBlueArmy.IsEnabled = true;
            ChooseRedArmy.IsEnabled = true;
        }

        private void ChooseBlueArmy_Click(object sender, RoutedEventArgs e)
        {
            playerArmy = armyManager.GetArmyByColor("Blue");
            computerArmy = armyManager.GetArmyByColor("Red");
            InitializeGame();
            ChooseRedArmy.Visibility = Visibility.Collapsed;
            ChooseBlueArmy.IsEnabled = false;
            CreateArmy.Visibility = Visibility.Collapsed;

        }

        private void ChooseRedArmy_Click(object sender, RoutedEventArgs e)
        {
            playerArmy = armyManager.GetArmyByColor("Red");
            computerArmy = armyManager.GetArmyByColor("Blue");
            InitializeGame();
            ChooseBlueArmy.Visibility = Visibility.Collapsed;
            ChooseRedArmy.IsEnabled = false;
            CreateArmy.Visibility = Visibility.Collapsed;
        }

        private void InitializeGame()
        {
            ChooseStrategy.Visibility = Visibility.Visible;
            MessageBox.Show("Выбрана армия: " + (playerArmy == armyManager.GetArmyByColor("Blue") ? "Синяя" : "Красная") + ". Выберите стратегию!");
        }

        private void RefreshUI()
        {
            UseWizard.IsEnabled = playerTurn && CheckForWizard(playerArmy) && !wizardUsed;
            UseHealer.IsEnabled = playerTurn && CheckForHealer(playerArmy, computerArmy) && !healerUsed;
            UseArcher.IsEnabled = playerTurn && CheckForArcher(playerArmy) && !archerUsed;
            CanсelTurn.IsEnabled = commandManager.CanUndo();
        }

        private void UseWizard_Click(object sender, RoutedEventArgs e)
        {
            if (playerArmy == null || playerArmy.Count == 0 || !playerTurn || wizardUsed)
            {
                MessageBox.Show("Не ваш ход или у вас нет армии или вы уже использовали данную способность!");
                return;
            }

            var command = new WizardTurnCommand(battleManagerProxy, playerArmy, computerArmy, outputTextBox);
            commandManager.ExecuteCommand(command);
            wizardUsed = true;
            UseWizard.IsEnabled = false;
            CanсelTurn.IsEnabled = true;
            RefreshUI();
        }

        private void UseHealer_Click(object sender, RoutedEventArgs e)
        {
            if (playerArmy == null || playerArmy.Count == 0 || !playerTurn || healerUsed)
            {
                MessageBox.Show("Не ваш ход или у вас нет армии или вы уже использовали данную способность!");
                return;
            }

            var command = new HealerTurnCommand(battleManagerProxy, playerArmy, computerArmy, outputTextBox);
            commandManager.ExecuteCommand(command);
            healerUsed = true;
            UseHealer.IsEnabled = false;
            CanсelTurn.IsEnabled = true;
            RefreshUI();
        }

        private void UseArcher_Click(object sender, RoutedEventArgs e)
        {
            if (playerArmy == null || playerArmy.Count == 0 || !playerTurn || archerUsed)
            {
                MessageBox.Show("Не ваш ход или у вас нет армии или вы уже использовали данную способность!");
                return;
            }

            var command = new ArcherTurnCommand(battleManagerProxy, playerArmy, computerArmy, outputTextBox);
            commandManager.ExecuteCommand(command);
            archerUsed = true;
            UseArcher.IsEnabled = false;
            CanсelTurn.IsEnabled = true;
            RefreshUI();
        }

        private void Turn_Click(object sender, RoutedEventArgs e)
        {
            wizardUsed = false;
            healerUsed = false;
            archerUsed = false;
            playerTurn = true;
            EndTurn.IsEnabled = true;

            RefreshUI();
            Turn.IsEnabled = false;
            CanсelTurn.IsEnabled = true;
        }
        private void EndTurn_Click(object sender, RoutedEventArgs e)
        {
            if (playerArmy.Count > 0 && computerArmy.Count > 0)
            {
                battleManagerProxy.Attack(playerArmy[0], computerArmy[0], outputTextBox);
                battleManagerProxy.IsDead(computerArmy[0], computerArmy);
                CheckGameOver();
            }

            playerTurn = false; // Завершение хода игрока
            ComputerTurn();

            if (playerArmy[0] is GulyayGorod)
            {
                battleManager.SetGulyayGorodCount(countTurnsForGulyayGorod, playerArmy[0].Side); // Передача значения
                if (countTurnsForGulyayGorod == 7)
                {
                    playerArmy.Remove(playerArmy[0]);
                }
            }

            DisableAbilityButtons(); // Деактивировать кнопки способностей
            EndTurn.IsEnabled = false;
        }


        private void ComputerTurn()
        {
            if (computerArmy.Count > 0 && playerArmy.Count > 0)
            {
                battleManagerProxy.TurnComputer(computerArmy, playerArmy, outputTextBox);
                CheckGameOver();
            }
            playerTurn = true; // Подготовка к началу нового хода игрока
            Turn.IsEnabled = true; // Позволяем начать новый ход
            CanсelTurn.IsEnabled = true;
            DisableAbilityButtons(); // Деактивировать кнопки до явного начала хода
        }

        private void CancelTurn_Click(object sender, RoutedEventArgs e)
        {
            commandManager.Undo();
            RefreshUI();
            CanсelTurn.IsEnabled = false;
        }

        private void ToTheEnd_Click(object sender, RoutedEventArgs e)
        {
            while (computerArmy.Count > 0 && playerArmy.Count > 0)
            {
                battleManagerProxy.TurnComputer(playerArmy, computerArmy, outputTextBox);
                CheckGameOver();
                if (computerArmy.Count > 0)
                {
                    battleManagerProxy.TurnComputer(computerArmy, playerArmy, outputTextBox);
                    CheckGameOver();
                }
            }
            ToTheEnd.IsEnabled = false;

            UseGulyayGorod.IsEnabled = false;
            EndTurn.IsEnabled = false;
        }

        private void GulyayGorodr_Click(object sender, RoutedEventArgs e)
        {
            var temp = playerArmy.First();
            playerArmy[0] = playerArmy.Last();
            playerArmy[playerArmy.Count() - 1] = temp;

            UseGulyayGorod.IsEnabled = false;

            countTurnsForGulyayGorod = 0;

            battleManagerProxy.TurnComputer(computerArmy, playerArmy, outputTextBox);
        }
        #endregion

        #region Check
        private bool CheckForWizard(List<Warrior> army)
        {
            return army.Any(warrior => warrior is Wizard);
        }

        private bool CheckForHealer(List<Warrior> army, List<Warrior> defenders)
        {
            bool hasHealer = army.Any(warrior => warrior is Healer);

            if (hasHealer)
            {
                for (int i = 0; i < army.Count; i++)
                {
                    if (!currentStrategy.IsFrontLine(i, defenders) && army[i] is Healer)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CheckForArcher(List<Warrior> army)
        {
            return army.Any(warrior => warrior is Archer);
        }

        #endregion

        public void CreateArmyClick(TextBox outputTextBox)
        {
            outputTextBox.Clear();
            armyManager.CreateArmy(new List<Warrior>(), "Blue");
            armyManager.CreateArmy(new List<Warrior>(), "Red");
            wizardUsed = false;
            healerUsed = false;
            archerUsed = false;
        }

        private void CheckGameOver()
        {
            if (playerArmy.Count == 0)
            {
                MessageBox.Show("Компьютер победил!");
                Turn.IsEnabled = false;
                CanсelTurn.IsEnabled = false;
                UseWizard.IsEnabled = false;
                UseHealer.IsEnabled = false;
                UseArcher.IsEnabled = false;
            }
            else if (computerArmy.Count == 0)
            {
                MessageBox.Show("Вы победили!");
                Turn.IsEnabled = false;
                CanсelTurn.IsEnabled = false;
                UseWizard.IsEnabled = false;
                UseHealer.IsEnabled = false;
                UseArcher.IsEnabled = false;
            }
        }

        private void ChooseStrategy_Click(object sender, RoutedEventArgs e)
        {
            ChooseTwoRows.Visibility = Visibility.Visible;
            ChooseThreeRows.Visibility = Visibility.Visible;
            ChooseWallToWall.Visibility = Visibility.Visible;
            ChooseTwoRows.IsEnabled = true;
            ChooseThreeRows.IsEnabled = true;
            ChooseWallToWall.IsEnabled = true;
        }

        private void ChooseTwoRows_Click(object sender, RoutedEventArgs e)
        {
            currentStrategy = new TwoRowStrategy();
            MessageBox.Show("Two Rows strategy selected.");
            ChooseThreeRows.Visibility = Visibility.Collapsed;
            ChooseWallToWall.Visibility = Visibility.Collapsed;
            ChooseStrategy.Visibility = Visibility.Collapsed;
            ShowPlayButtons();
            ChooseTwoRows.IsEnabled = false;
          

            //DrawArmies();
        }

        private void ChooseThreeRows_Click(object sender, RoutedEventArgs e)
        {
            currentStrategy = new ThreeRowStrategy();
            MessageBox.Show("Three Rows strategy selected.");
            ChooseTwoRows.Visibility = Visibility.Collapsed;
            ChooseWallToWall.Visibility = Visibility.Collapsed;
            ChooseStrategy.Visibility = Visibility.Collapsed;
            ShowPlayButtons();
            ChooseThreeRows.IsEnabled = false;
           
            //DrawArmies();
        }

        private void ChooseWallToWall_Click(object sender, RoutedEventArgs e)
        {
            
            currentStrategy = new WallToWallStrategy();
            MessageBox.Show("Wall to Wall strategy selected.");
            ChooseTwoRows.Visibility = Visibility.Collapsed;
            ChooseThreeRows.Visibility = Visibility.Collapsed;
            ChooseStrategy.Visibility = Visibility.Collapsed;
            ShowPlayButtons();
            ChooseWallToWall.IsEnabled = false;
           
            //DrawArmies();
        }

        private void ShowPlayButtons()
        {
            UseWizard.Visibility = Visibility.Visible;
            UseHealer.Visibility = Visibility.Visible;
            UseArcher.Visibility = Visibility.Visible;
            CanсelTurn.Visibility = Visibility.Visible;
            Turn.Visibility = Visibility.Visible;
            ToTheEnd.Visibility = Visibility.Visible;
            UseGulyayGorod.Visibility = Visibility.Visible;
            EndTurn.Visibility = Visibility.Visible;
            UseGulyayGorod.IsEnabled = true;
            EndTurn.IsEnabled = false;
            Turn.IsEnabled = true;
            ToTheEnd.IsEnabled = true;
        }

        private void StartAgain_Click(object sender, RoutedEventArgs e)
        {
            CreateArmy.Visibility = Visibility.Visible;
            ChooseBlueArmy.Visibility = Visibility.Hidden;
            ChooseRedArmy.Visibility = Visibility.Hidden;
  
            UseWizard.Visibility = Visibility.Hidden;
            UseHealer.Visibility = Visibility.Hidden;
            UseArcher.Visibility = Visibility.Hidden;
            UseGulyayGorod.Visibility = Visibility.Hidden;

            CanсelTurn.Visibility = Visibility.Hidden;

            ToTheEnd.Visibility = Visibility.Hidden;

            Turn.Visibility = Visibility.Hidden;

            CanсelTurn.Visibility = Visibility.Hidden;

            EndTurn.Visibility = Visibility.Hidden;
            DisableAbilityButtons();

            // Hide strategy buttons initially
            ChooseTwoRows.Visibility = Visibility.Hidden;
            ChooseThreeRows.Visibility = Visibility.Hidden;
            ChooseWallToWall.Visibility = Visibility.Hidden;

            ChooseStrategy.Visibility = Visibility.Hidden;
        }
    }
}
