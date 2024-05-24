using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            ToTheEnd.Visibility = Visibility.Visible;
            Turn.IsEnabled = false;
            CanсelTurn.IsEnabled = true;
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
            DrawArmies();
        }

        private void ChooseRedArmy_Click(object sender, RoutedEventArgs e)
        {
            playerArmy = armyManager.GetArmyByColor("Red");
            computerArmy = armyManager.GetArmyByColor("Blue");
            InitializeGame();
            DrawArmies();
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
            DrawArmies();
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
            DrawArmies();
            //// Вызов анимации лечения
            //AnimateHeal(playerArmy[0], playerArmy[1]); // Пример вызова анимации лечения, вы можете выбрать правильного воина
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
            DrawArmies();
            //// Вызов анимации стрельбы
            //AnimateShoot(playerArmy[0], computerArmy[0]); // Пример вызова анимации стрельбы, вы можете выбрать правильного воина
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
                //// Вызов анимации атаки
                //AnimateAttack(playerArmy[0], computerArmy[0]); // Пример вызова анимации атаки, вы можете выбрать правильного воина
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
            DrawArmies();
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
            DrawArmies();
        }

        private void CancelTurn_Click(object sender, RoutedEventArgs e)
        {
            commandManager.Undo();
            RefreshUI();
            DrawArmies();
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
            DrawArmies();
        }

        private void GulyayGorodr_Click(object sender, RoutedEventArgs e)
        {
            var temp = playerArmy.First();
            playerArmy[0] = playerArmy.Last();
            playerArmy[playerArmy.Count() - 1] = temp;

            UseGulyayGorod.IsEnabled = false;

            countTurnsForGulyayGorod = 0;

            battleManagerProxy.TurnComputer(computerArmy, playerArmy, outputTextBox);
            DrawArmies();
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
            MessageBox.Show("Two Rows strategy selected.");
            HideStrategyButtons();
            DrawArmies();
        }

        private void ChooseThreeRows_Click(object sender, RoutedEventArgs e)
        {
            currentStrategy = new ThreeRowStrategy();
            MessageBox.Show("Three Rows strategy selected.");
            HideStrategyButtons();
            DrawArmies();
        }

        private void ChooseWallToWall_Click(object sender, RoutedEventArgs e)
        {
            currentStrategy = new WallToWallStrategy();
            MessageBox.Show("Wall to Wall strategy selected.");
            HideStrategyButtons();
            DrawArmies();
        }

        private void HideStrategyButtons()
        {
            ChooseTwoRows.Visibility = Visibility.Collapsed;
            ChooseThreeRows.Visibility = Visibility.Collapsed;
            ChooseWallToWall.Visibility = Visibility.Collapsed;
            Turn.IsEnabled = true;
        }

        private void DrawArmies()
        {
            battlefieldCanvas.Children.Clear();
            DrawArmy(playerArmy, 100, 50, false);  // Левая армия
            DrawArmy(computerArmy, 100, 300, true); // Правая армия
        }

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
                    // Определяем позицию воина по кратности ряду
                    int row = i % 1;
                    int column = i / 1;

                    double xPos = xOffset + column * 64;
                    double yPos = yOffset + row * 64;

                    DrawWarrior(warrior, xPos, yPos);
                }
            }
        }

        private void DrawArmyInRows(List<Warrior> army, double xOffset, double yOffset, int rows, bool isComputerArmy)
        {
            int warriorsPerRow = (int)Math.Ceiling((double)army.Count / rows);

            for (int i = 0; i < army.Count; i++)
            {
                var warrior = army[i];
                // Определяем позицию воина по кратности ряду
                int row = i % rows;
                int column = i / rows;

                double xPos = xOffset + row * 64;
                double yPos = yOffset + column * 64; 

                DrawWarrior(warrior, xPos, yPos);
            }
        }


        private void DrawWarrior(Warrior warrior, double x, double y)
        {
            Image warriorImage = new Image
            {
                Width = 64,
                Height = 64,
                Source = GetWarriorImage(warrior),
                Tag = warrior // Сохранение ссылки на воина
            };

            Canvas.SetLeft(warriorImage, x);
            Canvas.SetTop(warriorImage, y);
            battlefieldCanvas.Children.Add(warriorImage);

            // Пример анимации при добавлении воина
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
            warriorImage.BeginAnimation(UIElement.OpacityProperty, fadeIn);
        }

        private ImageSource GetWarriorImage(Warrior warrior)
        {
            // Замените пути на реальные пути к вашим изображениям
            if (warrior is Wizard)
            {
                return new BitmapImage(new Uri("J:\\Warriors\\Wizard.png"));
            }
            else if (warrior is Healer)
            {
                return new BitmapImage(new Uri("J:\\Warriors\\Healer.png"));
            }
            else if (warrior is Archer)
            {
                return new BitmapImage(new Uri("J:\\Warriors\\Archer.png"));
            }
            else if (warrior is ImprovedHeavyWarrior)
            {
                return new BitmapImage(new Uri("J:\\Warriors\\ImprovedHeavyWarrior.png"));
            }
            else if (warrior is LightWarrior)
            {
                return new BitmapImage(new Uri("J:\\Warriors\\LightWarrior.png"));
            }
            else if (warrior is HeavyWarrior)
            {
                return new BitmapImage(new Uri("J:\\Warriors\\HeavyWarrior.png"));
            }
            return new BitmapImage(new Uri("J:\\Warriors\\default.png"));
        }

        //private void AnimateAttack(Warrior attacker, Warrior target)
        //{
        //    var attackerImage = FindWarriorImage(attacker);
        //    var targetImage = FindWarriorImage(target);

        //    if (attackerImage == null || targetImage == null) return;

        //    var attackAnimation = new DoubleAnimation
        //    {
        //        From = Canvas.GetLeft(attackerImage),
        //        To = Canvas.GetLeft(targetImage),
        //        Duration = TimeSpan.FromSeconds(0.5),
        //        AutoReverse = true
        //    };

        //    attackerImage.BeginAnimation(Canvas.LeftProperty, attackAnimation);
        //}

        //private void AnimateShoot(Warrior archer, Warrior target)
        //{
        //    var archerImage = FindWarriorImage(archer);
        //    var targetImage = FindWarriorImage(target);

        //    if (archerImage == null || targetImage == null) return;

        //    var shootAnimation = new DoubleAnimation
        //    {
        //        From = Canvas.GetLeft(archerImage),
        //        To = Canvas.GetLeft(targetImage),
        //        Duration = TimeSpan.FromSeconds(0.5),
        //        AutoReverse = true
        //    };

        //    archerImage.BeginAnimation(Canvas.LeftProperty, shootAnimation);
        //}

        //private void AnimateHeal(Warrior healer, Warrior target)
        //{
        //    var targetImage = FindWarriorImage(target);

        //    if (targetImage == null) return;

        //    var healAnimation = new DoubleAnimation
        //    {
        //        From = 0,
        //        To = 1,
        //        Duration = TimeSpan.FromSeconds(0.5),
        //        AutoReverse = true
        //    };

        //    targetImage.BeginAnimation(UIElement.OpacityProperty, healAnimation);
        //}
    }
}
