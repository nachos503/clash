using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;

namespace ClashGame
{
    /// <summary>
    /// MainWindow class representing the main window of the application.
    /// Identifier string "T:ClashGame.MainWindow".
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Army manager.
        /// Identifier string "F:ClashGame.MainWindow.armyManager".
        /// </summary>
        private readonly ArmyManager armyManager;

        /// <summary>
        /// BattleManager proxy object.
        /// Identifier string "F:ClashGame.MainWindow.battleManagerProxy".
        /// </summary>
        private BattleManagerProxy battleManagerProxy;

        /// <summary>
        /// Command manager.
        /// Identifier string "F:ClashGame.MainWindow.commandManager".
        /// </summary>
        private CommandManager commandManager;

        /// <summary>
        /// Battle manager.
        /// Identifier string "F:ClashGame.MainWindow.battleManager".
        /// </summary>
        private BattleManager battleManager;

        /// <summary>
        /// Current battle strategy.
        /// Identifier string "F:ClashGame.MainWindow.currentStrategy".
        /// </summary>
        private IBattleStrategy currentStrategy;

        /// <summary>
        /// Player's army.
        /// Identifier string "F:ClashGame.MainWindow.playerArmy".
        /// </summary>
        private List<Warrior> playerArmy;

        /// <summary>
        /// Computer's army.
        /// Identifier string "F:ClashGame.MainWindow.computerArmy".
        /// </summary>
        private List<Warrior> computerArmy;

        /// <summary>
        /// Flag indicating that it is currently the player's turn.
        /// Identifier string "F:ClashGame.MainWindow.playerTurn".
        /// </summary>
        private bool playerTurn = true;

        /// <summary>
        /// Flag indicating that the wizard has been used.
        /// Identifier string "F:ClashGame.MainWindow.wizardUsed".
        /// </summary>
        private bool wizardUsed = false;

        /// <summary>
        /// Flag indicating that the healer has been used.
        /// Identifier string "F:ClashGame.MainWindow.healerUsed".
        /// </summary>
        private bool healerUsed = false;

        /// <summary>
        /// Flag indicating that the archer has been used.
        /// Identifier string "F:ClashGame.MainWindow.archerUsed".
        /// </summary>
        private bool archerUsed = false;

        /// <summary>
        /// Flag indicating that the game is over.
        /// Identifier string "F:ClashGame.MainWindow.isGameOverFlag".
        /// </summary>

        private bool isGameOverFlag = false;

        /// <summary>
        /// Player army color.
        /// Identifier string "F:ClashGame.MainWindow.playerColor".
        /// </summary>
        private Color playerColor = Colors.Blue;

        /// <summary>
        /// Computer army color.
        /// Identifier string "F:ClashGame.MainWindow.computerColor".
        /// </summary>
        private Color computerColor = Colors.Red;

        /// <summary>
        /// Constructor for the MainWindow class.
        /// Identifier string "M:ClashGame.MainWindow.#ctor".
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            armyManager = new ArmyManager(outputTextBox, new ArmyUnitFactory());
            commandManager = new CommandManager();
            InitializeUI();
        }

        /// <summary>
        /// Initializes the user interface.
        /// Identifier string "M:ClashGame.MainWindow.InitializeUI".
        /// </summary>
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
            StartTurn.Visibility = Visibility.Hidden;
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
        /// <summary>
        /// Hides ability buttons temporarily.
        /// Identifier string "M:ClashGame.MainWindow.DisableAbilityButtons".
        /// </summary>
        private void DisableAbilityButtons()
        {
            UseWizard.IsEnabled = false;
            UseHealer.IsEnabled = false;
            UseArcher.IsEnabled = false;
            CanсelTurn.IsEnabled = false;
            UseGulyayGorod.IsEnabled = false;
        }

        /// <summary>
        /// Event handler for the Create Army button click event.
        /// Identifier string "M:ClashGame.MainWindow.CreateArmy_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void CreateArmy_Click(object sender, RoutedEventArgs e)
        {
            CreateArmies(outputTextBox);

            DisableAbilityButtons();
            ChooseBlueArmy.Visibility = Visibility.Visible;
            ChooseRedArmy.Visibility = Visibility.Visible;
            ChooseBlueArmy.IsEnabled = true;
            ChooseRedArmy.IsEnabled = true;
        }

        /// <summary>
        /// Creates the player armies.
        /// Identifier string "M:ClashGame.MainWindow.CreateArmies(System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="outputTextBox">TextBox for displaying information.</param>
        public void CreateArmies(TextBox outputTextBox)
        {
            outputTextBox.Clear();
            armyManager.CreateArmy(new List<Warrior>(), "Blue");
            armyManager.CreateArmy(new List<Warrior>(), "Red");
            wizardUsed = false;
            healerUsed = false;
            archerUsed = false;
        }

        /// <summary>
        /// Sets the army colors based on the player's choice.
        /// Identifier string "M:ClashGame.MainWindow.SetArmyColors(System.String)".
        /// </summary>
        /// <param name="playerArmyColor">Player's army color.</param>
        private void SetArmyColors(string playerArmyColor)
        {
            if (playerArmyColor == "Blue")
            {
                playerColor = Colors.Blue;
                computerColor = Colors.Red;
            }
            else if (playerArmyColor == "Red")
            {
                playerColor = Colors.Red;
                computerColor = Colors.Blue;
            }
        }

        /// <summary>
        /// Event handler for the Choose Blue Army button click event.
        /// Identifier string "M:ClashGame.MainWindow.ChooseBlueArmy_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void ChooseBlueArmy_Click(object sender, RoutedEventArgs e)
        {
            playerArmy = armyManager.GetArmyByColor("Blue");
            computerArmy = armyManager.GetArmyByColor("Red");
            SetArmyColors("Blue");
            InitializeGame();
            ChooseRedArmy.Visibility = Visibility.Collapsed;
            ChooseBlueArmy.IsEnabled = false;
            CreateArmy.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Event handler for the Choose Red Army button click event.
        /// Identifier string "M:ClashGame.MainWindow.ChooseRedArmy_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void ChooseRedArmy_Click(object sender, RoutedEventArgs e)
        {
            playerArmy = armyManager.GetArmyByColor("Red");
            computerArmy = armyManager.GetArmyByColor("Blue");
            SetArmyColors("Red");
            InitializeGame();
            ChooseBlueArmy.Visibility = Visibility.Collapsed;
            ChooseRedArmy.IsEnabled = false;
            CreateArmy.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Proceeds to strategy selection.
        /// Identifier string "M:ClashGame.MainWindow.InitializeGame".
        /// </summary>
        private void InitializeGame()
        {
            ChooseStrategy.Visibility = Visibility.Visible;
            MessageBox.Show("Выбрана армия: " + (playerArmy == armyManager.GetArmyByColor("Blue") ? "Синяя" : "Красная") + ". Выберите стратегию!");
        }
        #endregion

        #region ChooseStrategyClicks

        /// <summary>
        /// Event handler for the Choose Strategy button click event.
        /// Identifier string "M:ClashGame.MainWindow.ChooseStrategy_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void ChooseStrategy_Click(object sender, RoutedEventArgs e)
        {
            ChooseTwoRows.Visibility = Visibility.Visible;
            ChooseThreeRows.Visibility = Visibility.Visible;
            ChooseWallToWall.Visibility = Visibility.Visible;
            ChooseTwoRows.IsEnabled = true;
            ChooseThreeRows.IsEnabled = true;
            ChooseWallToWall.IsEnabled = true;
        }

        /// <summary>
        /// Event handler for the Choose Two Rows Strategy button click event.
        /// Identifier string "M:ClashGame.MainWindow.ChooseTwoRows_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments.</param>
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

            DrawArmies();
        }

        /// <summary>
        /// Event handler for the Choose Three Rows Strategy button click event.
        /// Identifier string "M:ClashGame.MainWindow.ChooseThreeRows_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments.</param>
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

            DrawArmies();
        }

        /// <summary>
        /// Event handler for the Choose Wall to Wall Strategy button click event.
        /// Identifier string "M:ClashGame.MainWindow.ChooseWallToWall_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments.</param>
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

            DrawArmies();
        }

        /// <summary>
        /// Shows the buttons for continuing the game.
        /// Identifier string "M:ClashGame.MainWindow.ShowPlayButtons".
        /// </summary>
        private void ShowPlayButtons()
        {
            UseWizard.Visibility = Visibility.Visible;
            UseHealer.Visibility = Visibility.Visible;
            UseArcher.Visibility = Visibility.Visible;
            CanсelTurn.Visibility = Visibility.Visible;
            StartTurn.Visibility = Visibility.Visible;
            ToTheEnd.Visibility = Visibility.Visible;
            UseGulyayGorod.Visibility = Visibility.Visible;
            EndTurn.Visibility = Visibility.Visible;
            UseGulyayGorod.IsEnabled = true;
            EndTurn.IsEnabled = false;
            StartTurn.IsEnabled = true;
            ToTheEnd.IsEnabled = true;
        }
        #endregion

        #region SpecialAbilitiesClicks

        /// <summary>
        /// Updates the state of user interface elements.
        /// Identifier string "M:ClashGame.MainWindow.RefreshUI".
        /// </summary>
        private void RefreshUI()
        {
            UseWizard.IsEnabled = playerTurn && CheckForWizard(playerArmy, computerArmy) && !wizardUsed;
            UseHealer.IsEnabled = playerTurn && CheckForHealer(playerArmy, computerArmy) && !healerUsed;
            UseArcher.IsEnabled = playerTurn && CheckForArcher(playerArmy, computerArmy) && !archerUsed;
            CanсelTurn.IsEnabled = commandManager.CanUndo();
        }

        /// <summary>
        /// Event handler for the click event of the wizard ability button.
        /// Identifier string "M:ClashGame.MainWindow.UseWizard_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
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
            DrawArmies();
        }

        /// <summary>
        /// Event handler for the click event of the healer ability button.
        /// Identifier string "M:ClashGame.MainWindow.UseHealer_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
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
            DrawArmies();
        }

        /// <summary>
        /// Event handler for the click event of the archer ability button.
        /// Identifier string "M:ClashGame.MainWindow.UseArcher_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
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
            DrawArmies();
        }

        /// <summary>
        /// Event handler for the click event of the Gulyay-Gorod ability button.
        /// Identifier string "M:ClashGame.MainWindow.GulyayGorod_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void GulyayGorodr_Click(object sender, RoutedEventArgs e)
        {
            var temp = playerArmy.First();
            playerArmy[0] = playerArmy.Last();
            playerArmy[playerArmy.Count() - 1] = temp;

            UseGulyayGorod.IsEnabled = false;

            battleManagerProxy.TurnComputer(computerArmy, playerArmy, outputTextBox);
            DrawArmies();
        }
        #endregion

        #region CheckForSpecialAbilities

        /// <summary>
        /// Checks for the presence of a wizard in the army who is not on the front line.
        /// Identifier string "M:ClashGame.MainWindow.CheckForWizard(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior})".
        /// </summary>
        /// <param name="army">The army to check.</param>
        /// <param name="defenders">The list of defending warriors.</param>
        /// <returns>True if there is a wizard not on the front line, otherwise false.</returns>
        private bool CheckForWizard(List<Warrior> army, List<Warrior> defenders)
        {
            // If there is at least one
            bool hasWizard = army.Any(warrior => warrior is Wizard);
            // Go through the entire list and check for the front line
            if (hasWizard)
                for (int i = 0; i < army.Count; i++)
                    if (!currentStrategy.IsFrontLine(i, defenders) && army[i] is Wizard)
                        return true;

            return false;
        }

        /// <summary>
        /// Checks for the presence of a healer in the army who is not on the front line.
        /// Identifier: "M:ClashGame.MainWindow.CheckForHealer(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior})".
        /// </summary>
        /// <param name="army">The army to be checked.</param>
        /// <param name="defenders">The list of defending warriors.</param>
        /// <returns>True if there is a healer not on the front line, otherwise false.</returns>
        private bool CheckForHealer(List<Warrior> army, List<Warrior> defenders)
        {
            bool hasHealer = army.Any(warrior => warrior is Healer);

            if (hasHealer)
                for (int i = 0; i < army.Count; i++)
                    if (!currentStrategy.IsFrontLine(i, defenders) && army[i] is Healer)
                        return true;

            return false;
        }

        /// <summary>
        /// Checks for the presence of an archer in the army who is not on the front line.
        /// Identifier: "M:ClashGame.MainWindow.CheckForArcher(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior})".
        /// </summary>
        /// <param name="army">The army to be checked.</param>
        /// <param name="defenders">The list of defending warriors.</param>
        /// <returns>True if there is an archer not on the front line, otherwise false.</returns>
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

        /// <summary>
        /// Event handler for the start turn button click event.
        /// Identifier: "M:ClashGame.MainWindow.StartTurn_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void StartTurn_Click(object sender, RoutedEventArgs e)
        {
            wizardUsed = false;
            healerUsed = false;
            archerUsed = false;
            playerTurn = true;
            EndTurn.IsEnabled = true;

            RefreshUI();
            StartTurn.IsEnabled = false;
            CanсelTurn.IsEnabled = true;
        }

        /// <summary>
        /// Event handler for the end turn button click event.
        /// Identifier: "M:ClashGame.MainWindow.EndTurn_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void EndTurn_Click(object sender, RoutedEventArgs e)
        {
            if (playerArmy.Count > 0 && computerArmy.Count > 0)
            {
                if (playerArmy[0] is not GulyayGorod || computerArmy[0] is not GulyayGorod)
                    currentStrategy.ExecuteBattle(playerArmy, computerArmy, outputTextBox);
                battleManagerProxy.IsDead(computerArmy[0], computerArmy);
                CheckGameOver();
            }

            playerTurn = false; // End of player's turn
            ComputerTurn();

            DisableAbilityButtons(); // Deactivate ability buttons
            EndTurn.IsEnabled = false;
            DrawArmies();
        }

        /// <summary>
        /// Performs the computer's turn after the player's turn ends.
        /// Method identifier: "M:ClashGame.MainWindow.ComputerTurn".
        /// </summary>
        private void ComputerTurn()
        {
            if (computerArmy.Count > 0 && playerArmy.Count > 0)
            {
                battleManagerProxy.TurnComputer(computerArmy, playerArmy, outputTextBox);
                battleManagerProxy.IsDead(computerArmy[0], computerArmy);
                CheckGameOver();
            }
            playerTurn = true; 
            StartTurn.IsEnabled = true; 
            CanсelTurn.IsEnabled = true;
            DisableAbilityButtons(); 
            DrawArmies();
        }

        /// <summary>
        /// Event handler for the cancel turn button click event.
        /// Method identifier: "M:ClashGame.MainWindow.CancelTurn_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void CancelTurn_Click(object sender, RoutedEventArgs e)
        {
            commandManager.Undo();
            RefreshUI();
            CanсelTurn.IsEnabled = false;
            DrawArmies();
        }

        /// <summary>
        /// Event handler for the continue playing until the end button click event.
        /// Method identifier: "M:ClashGame.MainWindow.ToTheEnd_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
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
            DrawArmies();
        }

        /// <summary>
        /// Event handler for the restart game button click event.
        /// Method identifier: "M:ClashGame.MainWindow.StartAgain_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
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

            StartTurn.Visibility = Visibility.Hidden;

            CanсelTurn.Visibility = Visibility.Hidden;

            EndTurn.Visibility = Visibility.Hidden;
            DisableAbilityButtons();

            // Hide strategy buttons initially
            ChooseTwoRows.Visibility = Visibility.Hidden;
            ChooseThreeRows.Visibility = Visibility.Hidden;
            ChooseWallToWall.Visibility = Visibility.Hidden;

            ChooseStrategy.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Checks if the game is over.
        /// Method identifier: "M:ClashGame.MainWindow.CheckGameOver".
        /// </summary>
        private void CheckGameOver()
        {

            if (playerArmy.Count == 0 || (playerArmy.Count == 1 && playerArmy[0] is GulyayGorod && !isGameOverFlag))
            {
                MessageBox.Show("Компьютер победил!");
                StartTurn.IsEnabled = false;
                CanсelTurn.IsEnabled = false;
                UseWizard.IsEnabled = false;
                UseHealer.IsEnabled = false;
                UseArcher.IsEnabled = false;
                isGameOverFlag = true;
            }
            else if (computerArmy.Count == 0 || (computerArmy.Count == 1 && computerArmy[0] is GulyayGorod && !isGameOverFlag))
            {
                MessageBox.Show("Вы победили!");
                StartTurn.IsEnabled = false;
                CanсelTurn.IsEnabled = false;
                UseWizard.IsEnabled = false;
                UseHealer.IsEnabled = false;
                UseArcher.IsEnabled = false;
                isGameOverFlag = true;
            }
        }
        #endregion

        #region Graphics&Animation

        /// <summary>
        /// Draws the armies on the battlefield.
        /// Method identifier: "M:ClashGame.MainWindow.DrawArmies".
        /// </summary>
        private void DrawArmies()
        {
            battlefieldCanvas.Children.Clear();
            DrawArmy(playerArmy, 350, 200, false);  
            DrawArmy(computerArmy, 700, 200, true); 
        }

        /// <summary>
        /// Draws an army on the battlefield.
        /// Method identifier: "M:ClashGame.MainWindow.DrawArmy(System.Collections.Generic.List{ClashGame.Warrior},System.Double,System.Double,System.Boolean)".
        /// </summary>
        /// <param name="army">The list of warriors in the army.</param>
        /// <param name="xOffset">The offset along the X-axis.</param>
        /// <param name="yOffset">The offset along the Y-axis.</param>
        /// <param name="isComputerArmy">Indicates whether the army is the computer's army.</param>
        private void DrawArmy(List<Warrior> army, double xOffset, double yOffset, bool isComputerArmy)
        {
            if (currentStrategy is TwoRowStrategy)
            {
                DrawArmyInRows(army, xOffset, yOffset, 2, isComputerArmy);
            }
            else if (currentStrategy is ThreeRowStrategy)
            {
                DrawArmyInRows(army, xOffset, yOffset, 3, isComputerArmy);
            }
            else if (currentStrategy is WallToWallStrategy)
            {
                for (int i = 0; i < army.Count; i++)
                {
                    var warrior = army[i];
                    int row = i;
                    int column = 1;

                    double xPos = isComputerArmy ? xOffset + column * 64 : xOffset - column * 64;
                    double yPos = yOffset + row * 64;

                    DrawWarrior(warrior, xPos, yPos, isComputerArmy);
                }
            }
        }

        /// <summary>
        /// Draws an army in multiple rows.
        /// Method identifier: "M:ClashGame.MainWindow.DrawArmyInRows(System.Collections.Generic.List{ClashGame.Warrior},System.Double,System.Double,System.Int32,System.Boolean)".
        /// </summary>
        /// <param name="army">The list of warriors in the army.</param>
        /// <param name="xOffset">The offset along the X-axis.</param>
        /// <param name="yOffset">The offset along the Y-axis.</param>
        /// <param name="rows">The number of rows.</param>
        /// <param name="isComputerArmy">Indicates whether the army is the computer's army.</param>
        private void DrawArmyInRows(List<Warrior> army, double xOffset, double yOffset, int rows, bool isComputerArmy)
        {
            int warriorsPerRow = (int)Math.Ceiling((double)army.Count / rows);

            for (int i = 0; i < army.Count; i++)
            {
                var warrior = army[i];
                int row = i % rows;
                int column = i / rows;

                double xPos = isComputerArmy ? xOffset + column * 64 : xOffset - column * 64;
                double yPos = yOffset + row * 64;

                DrawWarrior(warrior, xPos, yPos, isComputerArmy);
            }
        }


        /// <summary>
        /// Changes the color tint of an image.
        /// Method identifier: "M:ClashGame.MainWindow.ChangeImageColor(System.Windows.Media.ImageSource,System.Windows.Media.Color)".
        /// </summary>
        /// <param name="originalImage">The original image.</param>
        /// <param name="targetColor">The target color.</param>
        /// <returns>The new image with the changed color tint.</returns>
        private BitmapSource ChangeImageColor(ImageSource originalImage, Color targetColor)
        {
            if (originalImage is BitmapSource bitmapSource)
            {
                var formatConvertedBitmap = new FormatConvertedBitmap();
                formatConvertedBitmap.BeginInit();
                formatConvertedBitmap.Source = bitmapSource;
                formatConvertedBitmap.DestinationFormat = PixelFormats.Bgra32;
                formatConvertedBitmap.EndInit();

                int stride = (int)formatConvertedBitmap.PixelWidth * (formatConvertedBitmap.Format.BitsPerPixel / 8);
                byte[] pixelData = new byte[(int)formatConvertedBitmap.PixelHeight * stride];
                formatConvertedBitmap.CopyPixels(pixelData, stride, 0);

                double blendFactor = 0.5;// The blending coefficient, where 0.5 means 50% of the original color and 50% of the target color.

                for (int i = 0; i < pixelData.Length; i += 4)
                {
                    byte originalBlue = pixelData[i];
                    byte originalGreen = pixelData[i + 1];
                    byte originalRed = pixelData[i + 2];

                    pixelData[i] = (byte)(originalBlue * (1 - blendFactor) + targetColor.B * blendFactor);
                    pixelData[i + 1] = (byte)(originalGreen * (1 - blendFactor) + targetColor.G * blendFactor);
                    pixelData[i + 2] = (byte)(originalRed * (1 - blendFactor) + targetColor.R * blendFactor);
                }

                var newBitmap = BitmapSource.Create(
                    formatConvertedBitmap.PixelWidth,
                    formatConvertedBitmap.PixelHeight,
                    formatConvertedBitmap.DpiX,
                    formatConvertedBitmap.DpiY,
                    formatConvertedBitmap.Format,
                    formatConvertedBitmap.Palette,
                    pixelData,
                    stride
                );

                return newBitmap;
            }

            throw new InvalidOperationException("The provided ImageSource is not a BitmapSource.");
        }

        /// <summary>
        /// Draws a warrior on the battlefield.
        /// </summary>
        /// <param name="warrior">The warrior to draw.</param>
        /// <param name="x">The position on the X-axis.</param>
        /// <param name="y">The position on the Y-axis.</param>
        /// <param name="isComputerArmy">Specifies whether the army is the computer's army.</param>
        private void DrawWarrior(Warrior warrior, double x, double y, bool isComputerArmy)
        {
            Image warriorImage = new Image
            {
                Width = 64,
                Height = 64,
                Source = ChangeImageColor(GetWarriorImage(warrior), isComputerArmy ? computerColor : playerColor),
                RenderTransformOrigin = new Point(0.5, 0.5),
                Tag = warrior 
            };

            if (isComputerArmy)
            {
                warriorImage.RenderTransform = new ScaleTransform(-1, 1); 
            }

            Canvas.SetLeft(warriorImage, x);
            Canvas.SetTop(warriorImage, y);
            battlefieldCanvas.Children.Add(warriorImage);

            
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
            warriorImage.BeginAnimation(UIElement.OpacityProperty, fadeIn);
        }

        /// <summary>
        /// Retrieves the image for a warrior.
        /// </summary>
        /// <param name="warrior">The warrior to retrieve the image for.</param>
        /// <returns>The warrior's image.</returns>
        private ImageSource GetWarriorImage(Warrior warrior)
        {
            if (warrior is Wizard)
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Wizard.png");
                return new BitmapImage(new Uri(imagePath, UriKind.Absolute));
            }
            else if (warrior is Healer)
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Healer.png");
                return new BitmapImage(new Uri(imagePath, UriKind.Absolute));
            }
            else if (warrior is Archer)
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archer.png");
                return new BitmapImage(new Uri(imagePath, UriKind.Absolute));
            }
            else if (warrior is ImprovedHeavyWarrior)
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImprovedHeavyWarrior.png");
                return new BitmapImage(new Uri(imagePath, UriKind.Absolute));
            }
            else if (warrior is LightWarrior)
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LightWarrior.png");
                return new BitmapImage(new Uri(imagePath, UriKind.Absolute));
            }
            else if (warrior is HeavyWarrior)
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HeavyWarrior.png");
                return new BitmapImage(new Uri(imagePath, UriKind.Absolute));
            }

            string defaultImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "default.png");
            return new BitmapImage(new Uri(defaultImagePath, UriKind.Absolute));
        }

        #endregion
    }
}