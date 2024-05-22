using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ClashGame
{
    public partial class MainWindow : Window
    {
        private readonly ArmyManager armyManager;
        private readonly GameManager gameManager;
        private BattleManagerProxy battleManagerProxy;
        private CommandManager commandManager;
        private IBattleStrategy currentStrategy;

        private List<Warrior> playerArmy;
        private List<Warrior> computerArmy;

        private bool playerTurn = true;

        private bool wizardUsed = false;
        private bool healerUsed = false;
        private bool archerUsed = false;

        private int countTurnsForGulyayGorod;

        public MainWindow()
        {
            InitializeComponent();
            gameManager = GameManager.Instance;
            armyManager = new ArmyManager(outputTextBox, new ArmyUnitFactory());
            battleManagerProxy = new BattleManagerProxy("1.txt", new TwoRowStrategy()); // Default strategy
            commandManager = new CommandManager();
            InitializeUI();
        }

        private void InitializeUI()
        {
            ChooseBlueArmy.Visibility = Visibility.Collapsed;
            ChooseRedArmy.Visibility = Visibility.Collapsed;
            ToTheEnd.Visibility = Visibility.Collapsed;
            Turn.IsEnabled = false;
            CanсelTurn.IsEnabled = true;
            DisableAbilityButtons();
            // Hide strategy buttons initially
            ChooseTwoRows.Visibility = Visibility.Hidden;
            ChooseThreeRows.Visibility = Visibility.Hidden;
            ChooseWallToWall.Visibility = Visibility.Hidden;
            ChooseStrategy.Visibility = Visibility.Hidden;
        }

        private void DisableAbilityButtons()
        {
            UseWizard.IsEnabled = false;
            UseHealer.IsEnabled = false;
            UseArcher.IsEnabled = false;
            CanсelTurn.IsEnabled = false;
        }

        private void CreateArmy_Click(object sender, RoutedEventArgs e)
        {
            CreateArmyClick(outputTextBox);
            DisableAbilityButtons();
            ChooseBlueArmy.Visibility = Visibility.Visible;
            ChooseRedArmy.Visibility = Visibility.Visible;
        }

        private void ChooseBlueArmy_Click(object sender, RoutedEventArgs e)
        {
            playerArmy = armyManager.GetArmyByColor("Blue");
            computerArmy = armyManager.GetArmyByColor("Red");
            InitializeGame();
        }

        private void ChooseRedArmy_Click(object sender, RoutedEventArgs e)
        {
            playerArmy = armyManager.GetArmyByColor("Red");
            computerArmy = armyManager.GetArmyByColor("Blue");
            InitializeGame();
        }

        private void InitializeGame()
        {
            ChooseBlueArmy.Visibility = Visibility.Collapsed;
            ChooseRedArmy.Visibility = Visibility.Collapsed;
            ChooseStrategy.Visibility = Visibility.Visible;
            MessageBox.Show("Выбрана армия: " + (playerArmy == armyManager.GetArmyByColor("Blue") ? "Синяя" : "Красная") + ". Выберите стратегию!");
        }

        private void RefreshUI()
        {
            UseWizard.IsEnabled = playerTurn && CheckForWizard(playerArmy) && !wizardUsed;
            UseHealer.IsEnabled = playerTurn && CheckForHealer(playerArmy) && !healerUsed;
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

            var command = new WizardTurnCommand(battleManagerProxy, playerArmy, outputTextBox);
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

            var command = new HealerTurnCommand(battleManagerProxy, playerArmy, outputTextBox);
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

            RefreshUI();
            Turn.IsEnabled = false;
            CanсelTurn.IsEnabled = true;
        }
        private void EndTurn_Click(object sender, RoutedEventArgs e)
        {
            if (playerArmy.Count > 0 && computerArmy.Count > 0)
            {
                currentStrategy.ExecuteBattle(playerArmy, computerArmy, outputTextBox);
                battleManagerProxy.IsDead(computerArmy[0], computerArmy);
                CheckGameOver();
            }

            playerTurn = false; // Завершение хода игрока
            ComputerTurn();

            if (playerArmy[0] is GulyayGorod)
            {
                battleManagerProxy.SetGulyayGorodCount(countTurnsForGulyayGorod, playerArmy[0].Side); // Передача значения
                if (countTurnsForGulyayGorod == 7)
                {
                    playerArmy.Remove(playerArmy[0]);
                }
            }

            DisableAbilityButtons(); // Деактивировать кнопки способностей
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

        private bool CheckForWizard(List<Warrior> army)
        {
            return army.Any(warrior => warrior is Wizard);
        }

        private bool CheckForHealer(List<Warrior> army)
        {
            return army.Any(warrior => warrior is Healer);
        }

        private bool CheckForArcher(List<Warrior> army)
        {
            return army.Any(warrior => warrior is Archer);
        }

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
        }

        private void ChooseTwoRows_Click(object sender, RoutedEventArgs e)
        {
            currentStrategy = new TwoRowStrategy();
            currentStrategy.ArrangeArmy(playerArmy);
            currentStrategy.ArrangeArmy(computerArmy);
            MessageBox.Show("Two Rows strategy selected.");
            HideStrategyButtons();
        }

        private void ChooseThreeRows_Click(object sender, RoutedEventArgs e)
        {
            currentStrategy = new ThreeRowStrategy();
            currentStrategy.ArrangeArmy(playerArmy);
            currentStrategy.ArrangeArmy(computerArmy);
            MessageBox.Show("Three Rows strategy selected.");
            HideStrategyButtons();
        }

        private void ChooseWallToWall_Click(object sender, RoutedEventArgs e)
        {
            currentStrategy = new WallToWallStrategy();
            MessageBox.Show("Wall to Wall strategy selected.");
            HideStrategyButtons();
        }

        private void HideStrategyButtons()
        {
            ChooseTwoRows.Visibility = Visibility.Collapsed;
            ChooseThreeRows.Visibility = Visibility.Collapsed;
            ChooseWallToWall.Visibility = Visibility.Collapsed;
            Turn.IsEnabled = true;
        }
    }
}
