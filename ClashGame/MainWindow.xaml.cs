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
    /// Класс MainWindow, представляющий главное окно приложения.
    /// Строка идентификатора "T:ClashGame.MainWindow".
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Управляющий армией.
        /// Строка идентификатора "F:ClashGame.MainWindow.armyManager".
        /// </summary>
        private readonly ArmyManager armyManager;

        /// <summary>
        /// Прокси-объект BattleManager.
        /// Строка идентификатора "F:ClashGame.MainWindow.battleManagerProxy".
        /// </summary>
        private BattleManagerProxy battleManagerProxy;

        /// <summary>
        /// Управляющий командами.
        /// Строка идентификатора "F:ClashGame.MainWindow.commandManager".
        /// </summary>
        private CommandManager commandManager;

        /// <summary>
        /// Управляющий битвой.
        /// Строка идентификатора "F:ClashGame.MainWindow.battleManager".
        /// </summary>
        private BattleManager battleManager;

        /// <summary>
        /// Текущая стратегия битвы.
        /// Строка идентификатора "F:ClashGame.MainWindow.currentStrategy".
        /// </summary>
        private IBattleStrategy currentStrategy;

        /// <summary>
        /// Армия игрока.
        /// Строка идентификатора "F:ClashGame.MainWindow.playerArmy".
        /// </summary>
        private List<Warrior> playerArmy;

        /// <summary>
        /// Армия компьютера.
        /// Строка идентификатора "F:ClashGame.MainWindow.computerArmy".
        /// </summary>
        private List<Warrior> computerArmy;

        /// <summary>
        /// Флаг, указывающий, что сейчас ход игрока.
        /// Строка идентификатора "F:ClashGame.MainWindow.playerTurn".
        /// </summary>
        private bool playerTurn = true;

        /// <summary>
        /// Флаг, указывающий, что маг использован.
        /// Строка идентификатора "F:ClashGame.MainWindow.wizardUsed".
        /// </summary>
        private bool wizardUsed = false;

        /// <summary>
        /// Флаг, указывающий, что лекарь использован.
        /// Строка идентификатора "F:ClashGame.MainWindow.healerUsed".
        /// </summary>
        private bool healerUsed = false;

        /// <summary>
        /// Флаг, указывающий, что лучник использован.
        /// Строка идентификатора "F:ClashGame.MainWindow.archerUsed".
        /// </summary>
        private bool archerUsed = false;

        /// <summary>
        /// Флаг, указывающий, что игра окончена.
        /// Строка идентификатора "F:ClashGame.MainWindow.isGameOverFlag".
        /// </summary>

        private bool isGameOverFlag = false;

        /// <summary>
        /// Цвет армии игрока.
        /// Строка идентификатора "F:ClashGame.MainWindow.playerColor".
        /// </summary>
        private Color playerColor = Colors.Blue;

        /// <summary>
        /// Цвет армии компьютера.
        /// Строка идентификатора "F:ClashGame.MainWindow.computerColor".
        /// </summary>
        private Color computerColor = Colors.Red;

        /// <summary>
        /// Конструктор для класса MainWindow.
        /// Строка идентификатора "M:ClashGame.MainWindow.#ctor".
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            armyManager = new ArmyManager(outputTextBox, new ArmyUnitFactory());
            commandManager = new CommandManager();
            InitializeUI();
        }

        /// <summary>
        /// Инициализирует пользовательский интерфейс.
        /// Строка идентификатора "M:ClashGame.MainWindow.InitializeUI".
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
        /// Скрывает кнопки способностей на время.
        /// Строка идентификатора "M:ClashGame.MainWindow.DisableAbilityButtons".
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
        /// Обработчик события нажатия кнопки создания армии.
        /// Строка идентификатора "M:ClashGame.MainWindow.CreateArmy_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void CreateArmy_Click(object sender, RoutedEventArgs e)
        {
            CreateArmies(outputTextBox);

            DisableAbilityButtons();
            ChooseBlueArmy.Visibility = Visibility.Visible;
            ChooseRedArmy.Visibility = Visibility.Visible;
            ChooseBlueArmy.IsEnabled = true;
            ChooseRedArmy.IsEnabled = true;
        }

        //// <summary>
        /// Создает армии игроков.
        /// Строка идентификатора "M:ClashGame.MainWindow.CreateArmies(System.Windows.Controls.TextBox)".
        /// </summary>
        /// <param name="outputTextBox">TextBox для вывода информации.</param>
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
        /// Устанавливает цвета армий в зависимости от выбора игрока.
        /// Строка идентификатора "M:ClashGame.MainWindow.SetArmyColors(System.String)".
        /// </summary>
        /// <param name="playerArmyColor">Цвет армии игрока.</param>
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
        /// Обработчик события нажатия кнопки выбора синей армии.
        /// Строка идентификатора "M:ClashGame.MainWindow.ChooseBlueArmy_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
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
        /// Обработчик события нажатия кнопки выбора красной армии.
        /// Строка идентификатора "M:ClashGame.MainWindow.ChooseRedArmy_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
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
        /// Переход к выбору стратегии.
        /// Строка идентификатора "M:ClashGame.MainWindow.InitializeGame".
        /// </summary>
        private void InitializeGame()
        {
            ChooseStrategy.Visibility = Visibility.Visible;
            MessageBox.Show("Выбрана армия: " + (playerArmy == armyManager.GetArmyByColor("Blue") ? "Синяя" : "Красная") + ". Выберите стратегию!");
        }
        #endregion

        #region ChooseStrategyClicks
       
        /// <summary>
        /// Обработчик события нажатия кнопки выбора стратегии.
        /// Строка идентификатора "M:ClashGame.MainWindow.ChooseStrategy_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
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
        /// Обработчик события нажатия кнопки выбора стратегии "две линии".
        /// Строка идентификатора "M:ClashGame.MainWindow.ChooseTwoRows_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
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
        /// Обработчик события нажатия кнопки выбора стратегии "три линии".
        /// Строка идентификатора "M:ClashGame.MainWindow.ChooseThreeRows_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
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
        /// Обработчик события нажатия кнопки выбора стратегии "стенка на стенку".
        /// Строка идентификатора "M:ClashGame.MainWindow.ChooseWallToWall_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
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
        /// Показывает кнопки для продолжения игры.
        /// Строка идентификатора "M:ClashGame.MainWindow.ShowPlayButtons".
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
        /// Обновляет состояние элементов пользовательского интерфейса.
        /// Строка идентификатора "M:ClashGame.MainWindow.RefreshUI".
        /// </summary>
        private void RefreshUI()
        {
            UseWizard.IsEnabled = playerTurn && CheckForWizard(playerArmy, computerArmy) && !wizardUsed;
            UseHealer.IsEnabled = playerTurn && CheckForHealer(playerArmy, computerArmy) && !healerUsed;
            UseArcher.IsEnabled = playerTurn && CheckForArcher(playerArmy, computerArmy) && !archerUsed;
            CanсelTurn.IsEnabled = commandManager.CanUndo();
        }

        /// <summary>
        /// Обработчик события нажатия кнопки применения способности мага.
        /// Строка идентификатора "M:ClashGame.MainWindow.UseWizard_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
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
        /// Обработчик события нажатия кнопки применения способности лекаря.
        /// Строка идентификатора "M:ClashGame.MainWindow.UseHealer_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
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
        /// Обработчик события нажатия кнопки применения способности лучника.
        /// Строка идентификатора "M:ClashGame.MainWindow.UseArcher_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
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
        /// Обработчик события нажатия кнопки применения способности Гуляй-города.
        /// Строка идентификатора "M:ClashGame.MainWindow.GulyayGorod_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
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
        /// Проверяет наличие мага в армии, который не находится на передовой линии.
        /// Строка идентификатора "M:ClashGame.MainWindow.CheckForWizard(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior})".
        /// </summary>
        /// <param name="army">Армия, которую нужно проверить.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <returns>True, если есть маг не на передовой линии, иначе false.</returns>
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

        /// <summary>
        /// Проверяет наличие лекаря в армии, который не находится на передовой линии.
        /// Строка идентификатора "M:ClashGame.MainWindow.CheckForHealer(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior})".
        /// </summary>
        /// <param name="army">Армия, которую нужно проверить.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <returns>True, если есть лекарь не на передовой линии, иначе false.</returns>
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
        /// Проверяет наличие лучника в армии, который не находится на передовой линии.
        /// Строка идентификатора "M:ClashGame.MainWindow.CheckForArcher(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior})".
        /// </summary>
        /// <param name="army">Армия, которую нужно проверить.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <returns>True, если есть лучник не на передовой линии, иначе false.</returns>
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
        /// Обработчик события нажатия кнопки начала хода.
        /// Строка идентификатора "M:ClashGame.MainWindow.StartTurn_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
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
        /// Обработчик события нажатия кнопки окончания хода.
        /// Строка идентификатора "M:ClashGame.MainWindow.EndTurn_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
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
            DrawArmies();
        }

        /// <summary>
        /// Выполняет ход компьютера после окончания хода игрока.
        /// Строка идентификатора "M:ClashGame.MainWindow.ComputerTurn".
        /// </summary>
        private void ComputerTurn()
        {
            if (computerArmy.Count > 0 && playerArmy.Count > 0)
            {
                battleManagerProxy.TurnComputer(computerArmy, playerArmy, outputTextBox);
                battleManagerProxy.IsDead(computerArmy[0], computerArmy);
                CheckGameOver();
            }
            playerTurn = true; // Подготовка к началу нового хода игрока
            StartTurn.IsEnabled = true; // Позволяем начать новый ход
            CanсelTurn.IsEnabled = true;
            DisableAbilityButtons(); // Деактивировать кнопки до явного начала хода
            DrawArmies();
        }

        /// <summary>
        /// Обработчик события нажатия кнопки отмены хода.
        /// Строка идентификатора "M:ClashGame.MainWindow.CancelTurn_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void CancelTurn_Click(object sender, RoutedEventArgs e)
        {
            commandManager.Undo();
            RefreshUI();
            CanсelTurn.IsEnabled = false;
            DrawArmies();
        }

        /// <summary>
        /// Обработчик события нажатия кнопки доиграть игру до конца.
        /// Строка идентификатора "M:ClashGame.MainWindow.ToTheEnd_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
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
        /// Обработчик события нажатия кнопки начать игру заново.
        /// Строка идентификатора "M:ClashGame.MainWindow.StartAgain_Click(System.Object,System.Windows.RoutedEventArgs)".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
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
        /// Проверяет, завершена ли игра.
        /// Строка идентификатора "M:ClashGame.MainWindow.CheckGameOver".
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
        /// Рисует армии на поле битвы.
        /// Строка идентификатора "M:ClashGame.MainWindow.DrawArmies".
        /// </summary>
        private void DrawArmies()
        {
            battlefieldCanvas.Children.Clear();
            DrawArmy(playerArmy, 350, 200, false);  // Левая армия
            DrawArmy(computerArmy, 700, 200, true); // Правая армия
        }

        /// <summary>
        /// Рисует армию на поле битвы.
        /// Строка идентификатора "M:ClashGame.MainWindow.DrawArmy(System.Collections.Generic.List{ClashGame.Warrior},System.Double,System.Double,System.Boolean)".
        /// </summary>
        /// <param name="army">Список воинов армии.</param>
        /// <param name="xOffset">Смещение по оси X.</param>
        /// <param name="yOffset">Смещение по оси Y.</param>
        /// <param name="isComputerArmy">Указывает, является ли армия армией компьютера.</param>
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
        /// Рисует армию в несколько рядов.
        /// Строка идентификатора "M:ClashGame.MainWindow.DrawArmyInRows(System.Collections.Generic.List{ClashGame.Warrior},System.Double,System.Double,System.Int32,System.Boolean)".
        /// </summary>
        /// <param name="army">Список воинов армии.</param>
        /// <param name="xOffset">Смещение по оси X.</param>
        /// <param name="yOffset">Смещение по оси Y.</param>
        /// <param name="rows">Количество рядов.</param>
        /// <param name="isComputerArmy">Указывает, является ли армия армией компьютера.</param>
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
        /// Изменяет оттенок изображения.
        /// Строка идентификатора "M:ClashGame.MainWindow.ChangeImageColor(System.Windows.Media.ImageSource,System.Windows.Media.Color)".
        /// </summary>
        /// <param name="originalImage">Исходное изображение.</param>
        /// <param name="targetColor">Целевой цвет.</param>
        /// <returns>Новое изображение с измененным оттенком.</returns>
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

                double blendFactor = 0.5; // Коэффициент смешивания, 0.5 означает 50% исходного цвета и 50% целевого цвета

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
        /// Рисует воина на поле битвы.
        /// Строка идентификатора "M:ClashGame.MainWindow.DrawWarrior(ClashGame.Warrior,System.Double,System.Double,System.Boolean)".
        /// </summary>
        /// <param name="warrior">Воин, которого нужно нарисовать.</param>
        /// <param name="x">Позиция по оси X.</param>
        /// <param name="y">Позиция по оси Y.</param>
        /// <param name="isComputerArmy">Указывает, является ли армия армией компьютера.</param>
        private void DrawWarrior(Warrior warrior, double x, double y, bool isComputerArmy)
        {
            Image warriorImage = new Image
            {
                Width = 64,
                Height = 64,
                Source = ChangeImageColor(GetWarriorImage(warrior), isComputerArmy ? computerColor : playerColor),
                RenderTransformOrigin = new Point(0.5, 0.5),
                Tag = warrior // Сохранение ссылки на воина
            };

            if (isComputerArmy)
            {
                warriorImage.RenderTransform = new ScaleTransform(-1, 1); // Зеркальное отображение для армии компьютера
            }

            Canvas.SetLeft(warriorImage, x);
            Canvas.SetTop(warriorImage, y);
            battlefieldCanvas.Children.Add(warriorImage);

            // Пример анимации при добавлении воина
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
            warriorImage.BeginAnimation(UIElement.OpacityProperty, fadeIn);
        }

        /// <summary>
        /// Получает изображение для воина.
        /// Строка идентификатора "M:ClashGame.MainWindow.GetWarriorImage(ClashGame.Warrior)".
        /// </summary>
        /// <param name="warrior">Воин, для которого нужно получить изображение.</param>
        /// <returns>Изображение воина.</returns>
        private ImageSource GetWarriorImage(Warrior warrior)
        {
            // Замените пути на реальные пути к вашим изображениям
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