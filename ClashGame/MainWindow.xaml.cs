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
        private IBattleStrategy currentStrategy;

        private List<Warrior> playerArmy;
        private List<Warrior> computerArmy;

        private bool playerTurn = true;
        private bool wizardUsed = false;
        private bool healerUsed = false;
        private bool archerUsed = false;
        private bool isGameOverFlag = false;


        public MainWindow()
        {
            InitializeComponent();
            armyManager = new ArmyManager(outputTextBox, new ArmyUnitFactory());
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

        #region ChooseArmyClicks
        //скрываем кнопки бафом на время 
        private void DisableAbilityButtons()
        {
            UseWizard.IsEnabled = false;
            UseHealer.IsEnabled = false;
            UseArcher.IsEnabled = false;
            CanсelTurn.IsEnabled = false;
            UseGulyayGorod.IsEnabled = false;
        }
        //создание армий - кнопка
        private void CreateArmy_Click(object sender, RoutedEventArgs e)
        {
            CreateArmies(outputTextBox);

            DisableAbilityButtons();
            ChooseBlueArmy.Visibility = Visibility.Visible;
            ChooseRedArmy.Visibility = Visibility.Visible;
            ChooseBlueArmy.IsEnabled = true;
            ChooseRedArmy.IsEnabled = true;
        }
        //создаине армий - метод
        public void CreateArmies(TextBox outputTextBox)
        {
            outputTextBox.Clear();
            armyManager.CreateArmy(new List<Warrior>(), "Blue");
            armyManager.CreateArmy(new List<Warrior>(), "Red");
            wizardUsed = false;
            healerUsed = false;
            archerUsed = false;
        }
        //выбор синих
        private void ChooseBlueArmy_Click(object sender, RoutedEventArgs e)
        {
            playerArmy = armyManager.GetArmyByColor("Blue");
            computerArmy = armyManager.GetArmyByColor("Red");
            InitializeGame();
            ChooseRedArmy.Visibility = Visibility.Collapsed;
            ChooseBlueArmy.IsEnabled = false;
            CreateArmy.Visibility = Visibility.Collapsed;

        }
        //выбор ленина
        private void ChooseRedArmy_Click(object sender, RoutedEventArgs e)
        {
            playerArmy = armyManager.GetArmyByColor("Red");
            computerArmy = armyManager.GetArmyByColor("Blue");
            InitializeGame();
            ChooseBlueArmy.Visibility = Visibility.Collapsed;
            ChooseRedArmy.IsEnabled = false;
            CreateArmy.Visibility = Visibility.Collapsed;
        }
        //переход к выбору стратегии
        private void InitializeGame()
        {
            ChooseStrategy.Visibility = Visibility.Visible;
            MessageBox.Show("Выбрана армия: " + (playerArmy == armyManager.GetArmyByColor("Blue") ? "Синяя" : "Красная") + ". Выберите стратегию!");
        }
        #endregion

        #region ChooseStrategyClicks
        //выбор статегии - кнопка
        private void ChooseStrategy_Click(object sender, RoutedEventArgs e)
        {
            ChooseTwoRows.Visibility = Visibility.Visible;
            ChooseThreeRows.Visibility = Visibility.Visible;
            ChooseWallToWall.Visibility = Visibility.Visible;
            ChooseTwoRows.IsEnabled = true;
            ChooseThreeRows.IsEnabled = true;
            ChooseWallToWall.IsEnabled = true;
        }
        //2 линии
        private void ChooseTwoRows_Click(object sender, RoutedEventArgs e)
        {
            IBattleStrategy initialStrategy = new DefaultStratagy();

            battleManager = new BattleManager(initialStrategy);
            battleManagerProxy = new BattleManagerProxy(battleManager, "1.txt");

            currentStrategy = new TwoRowStrategy(battleManagerProxy);
            battleManager.SetStrategy(currentStrategy);

            MessageBox.Show("Two Rows strategy selected.");

            battleManager.UpgradeHeavyWarrior(playerArmy, computerArmy, outputTextBox);
            battleManager.UpgradeHeavyWarrior(computerArmy, playerArmy, outputTextBox);

            ShowPlayButtons();
            ChooseTwoRows.IsEnabled = false;
            ChooseThreeRows.IsEnabled = true;
            ChooseWallToWall.IsEnabled = true;

            //DrawArmies();
        }
        //3 линии
        private void ChooseThreeRows_Click(object sender, RoutedEventArgs e)
        {
            IBattleStrategy initialStrategy = new DefaultStratagy();

            battleManager = new BattleManager(initialStrategy);
            battleManagerProxy = new BattleManagerProxy(battleManager, "1.txt");

            currentStrategy = new ThreeRowStrategy(battleManagerProxy);
            battleManager.SetStrategy(currentStrategy);

            MessageBox.Show("Three Rows strategy selected.");

            battleManager.UpgradeHeavyWarrior(playerArmy, computerArmy, outputTextBox);
            battleManager.UpgradeHeavyWarrior(computerArmy, playerArmy, outputTextBox);

            ShowPlayButtons();
            ChooseThreeRows.IsEnabled = false;
            ChooseWallToWall.IsEnabled = true;
            ChooseTwoRows.IsEnabled = true;

            //DrawArmies();
        }
        //стенка на стенку
        private void ChooseWallToWall_Click(object sender, RoutedEventArgs e)
        {
            IBattleStrategy initialStrategy = new DefaultStratagy();

            battleManager = new BattleManager(initialStrategy);
            battleManagerProxy = new BattleManagerProxy(battleManager, "1.txt");

            currentStrategy = new WallToWallStrategy(battleManagerProxy);
            battleManager.SetStrategy(currentStrategy);

            MessageBox.Show("Wall to Wall strategy selected.");

            battleManager.UpgradeHeavyWarrior(playerArmy, computerArmy, outputTextBox);
            battleManager.UpgradeHeavyWarrior(computerArmy, playerArmy, outputTextBox);

            ShowPlayButtons();
            ChooseWallToWall.IsEnabled = false;
            ChooseTwoRows.IsEnabled = true;
            ChooseThreeRows.IsEnabled = true;

            //DrawArmies();
        }
        //показывать кнопки для игры дальше
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
        #endregion

        #region SpecialAbilitiesClicks
       //проверка что бафы есть
        private void RefreshUI()
        {
            UseWizard.IsEnabled = playerTurn && CheckForWizard(playerArmy, computerArmy) && !wizardUsed;
            UseHealer.IsEnabled = playerTurn && CheckForHealer(playerArmy, computerArmy) && !healerUsed;
            UseArcher.IsEnabled = playerTurn && CheckForArcher(playerArmy, computerArmy) && !archerUsed;
            CanсelTurn.IsEnabled = commandManager.CanUndo();
        }
        //применение кудесника
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
        //применение медсестры
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
        //асташлепка
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
        //блядская стена
        private void GulyayGorodr_Click(object sender, RoutedEventArgs e)
        {
            var temp = playerArmy.First();
            playerArmy[0] = playerArmy.Last();
            playerArmy[playerArmy.Count() - 1] = temp;

            UseGulyayGorod.IsEnabled = false;

            battleManagerProxy.TurnComputer(computerArmy, playerArmy, outputTextBox);
        }
        #endregion

        #region CheckForSpecialAbilities
        //функции проверки существоваиня бафов
        private bool CheckForWizard(List<Warrior> army, List<Warrior> defenders)
        {
            //если есть хоть один
            bool hasWizard = army.Any(warrior => warrior is Wizard);
            //идем по всему списку и проверяем на первую линию
            if (hasWizard)
                for (int i = 0; i < army.Count; i++)
                    if (!currentStrategy.IsFrontLine(i, defenders) && army[i] is Wizard)
                        return true;

            return false;
        }

        private bool CheckForHealer(List<Warrior> army, List<Warrior> defenders)
        {
            bool hasHealer = army.Any(warrior => warrior is Healer);

            if (hasHealer)
                for (int i = 0; i < army.Count; i++)
                    if (!currentStrategy.IsFrontLine(i, defenders) && army[i] is Healer)
                        return true;

            return false;
        }

        private bool CheckForArcher(List<Warrior> army, List<Warrior> defenders)
        {
            bool hasArcher = army.Any(warrior => warrior is Archer);

            if (hasArcher)
                for (int i = 0; i < army.Count; i++)
                    if (!currentStrategy.IsFrontLine(i, defenders) && army[i] is Archer)
                        return true;

            return false;
        }

        #endregion

        #region AnyTurnClick
        //анопка начала хода - выбор бафов, отмены бафов
        private void StartTurn_Click(object sender, RoutedEventArgs e)
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
        //конец хода - конечная атака
        private void EndTurn_Click(object sender, RoutedEventArgs e)
        {
            if (playerArmy.Count > 0 && computerArmy.Count > 0)
            {
                if (playerArmy[0] is not GulyayGorod || computerArmy[0] is not GulyayGorod)
                    currentStrategy.ExecuteBattle(playerArmy, computerArmy, outputTextBox);
                battleManagerProxy.IsDead(computerArmy[0], computerArmy);
                CheckGameOver();
            }

            playerTurn = false; // Завершение хода игрока
            ComputerTurn();

            DisableAbilityButtons(); // Деактивировать кнопки способностей
            EndTurn.IsEnabled = false;
        }
        //ход компьютера после атаки игрока
        private void ComputerTurn()
        {
            if (computerArmy.Count > 0 && playerArmy.Count > 0)
            {
                battleManagerProxy.TurnComputer(computerArmy, playerArmy, outputTextBox);
                battleManagerProxy.IsDead(computerArmy[0], computerArmy);
                CheckGameOver();
            }
            playerTurn = true; // Подготовка к началу нового хода игрока
            Turn.IsEnabled = true; // Позволяем начать новый ход
            CanсelTurn.IsEnabled = true;
            DisableAbilityButtons(); // Деактивировать кнопки до явного начала хода
        }
        //отмена хода через паттерн команда
        private void CancelTurn_Click(object sender, RoutedEventArgs e)
        {
            commandManager.Undo();
            RefreshUI();
            CanсelTurn.IsEnabled = false;
        }
        //доиграть гру до конца
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
        //начать игру заново
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
        //проверка что армии не сдохли поголовно
        private void CheckGameOver()
        {

            if (playerArmy.Count == 0 || (playerArmy.Count == 1 && playerArmy[0] is GulyayGorod && !isGameOverFlag))
            {
                MessageBox.Show("Компьютер победил!");
                Turn.IsEnabled = false;
                CanсelTurn.IsEnabled = false;
                UseWizard.IsEnabled = false;
                UseHealer.IsEnabled = false;
                UseArcher.IsEnabled = false;
                isGameOverFlag = true;
            }
            else if (computerArmy.Count == 0 || (computerArmy.Count == 1 && computerArmy[0] is GulyayGorod && !isGameOverFlag))
            {
                MessageBox.Show("Вы победили!");
                Turn.IsEnabled = false;
                CanсelTurn.IsEnabled = false;
                UseWizard.IsEnabled = false;
                UseHealer.IsEnabled = false;
                UseArcher.IsEnabled = false;
                isGameOverFlag = true;
            }
        }
        #endregion

    }
}
