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

        private List<Warrior> playerArmy;
        private List<Warrior> computerArmy;
        private bool playerTurn = true;

        private bool wizardUsed = false;
        private bool healerUsed = false;
        private bool archerUsed = false;

        public MainWindow()
        {
            InitializeComponent();
            gameManager = GameManager.Instance;
            armyManager = new ArmyManager(outputTextBox, new ArmyUnitFactory());
            battleManagerProxy = new BattleManagerProxy("1.txt");
            InitializeUI();
        }

        private void InitializeUI()
        {
            ChooseBlueArmy.Visibility = Visibility.Collapsed;
            ChooseRedArmy.Visibility = Visibility.Collapsed;
            ToTheEnd.Visibility = Visibility.Collapsed;
            Turn.IsEnabled = false;
            CancelTurn.IsEnabled = false;
            DisableAbilityButtons();
        }

        private void DisableAbilityButtons()
        {
            UseWizard.IsEnabled = false;
            UseHealer.IsEnabled = false;
            UseArcher.IsEnabled = false;
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
            MessageBox.Show("Выбрана армия: " + (playerArmy == armyManager.GetArmyByColor("Blue") ? "Синяя" : "Красная") + ". Ваш ход!");
            Turn.IsEnabled = true;
            ToTheEnd.Visibility = Visibility.Visible;
            ToTheEnd.IsEnabled = true;
        }

        private void RefreshUI()
        {
            // Проверяем, активен ли ход игрока и не были ли использованы способности
            UseWizard.IsEnabled = playerTurn && CheckForWizard(playerArmy) && !wizardUsed;
            UseHealer.IsEnabled = playerTurn && CheckForHealer(playerArmy) && !healerUsed;
            UseArcher.IsEnabled = playerTurn && CheckForArcher(playerArmy) && !archerUsed;
        }

        private void UseWizard_Click(object sender, RoutedEventArgs e)
        {
            if (playerArmy == null || playerArmy.Count == 0 || !playerTurn || wizardUsed)
            {
                MessageBox.Show("Не ваш ход или у вас нет армии или вы уже использовали данную способность!");
                return;
            }

            battleManagerProxy.WizardTurn(playerArmy, outputTextBox);
            wizardUsed = true;
            UseWizard.IsEnabled = false;
            RefreshUI();
        }

        private void UseHealer_Click(object sender, RoutedEventArgs e)
        {
            if (playerArmy == null || playerArmy.Count == 0 || !playerTurn || healerUsed)
            {
                MessageBox.Show("Не ваш ход или у вас нет армии или вы уже использовали данную способность!");
                return;
            }

            battleManagerProxy.HealerTurn(playerArmy, outputTextBox);
            healerUsed = true;
            UseHealer.IsEnabled = false;
            RefreshUI();
        }

        private void UseArcher_Click(object sender, RoutedEventArgs e)
        {
            if (playerArmy == null || playerArmy.Count == 0 || !playerTurn || archerUsed)
            {
                MessageBox.Show("Не ваш ход или у вас нет армии или вы уже использовали данную способность!");
                return;
            }

            battleManagerProxy.ArchersTurn(playerArmy, computerArmy, outputTextBox);
            archerUsed = true;
            UseArcher.IsEnabled = false;
            RefreshUI();
        }

        private void Turn_Click(object sender, RoutedEventArgs e)
        {
            wizardUsed = false; // Сброс состояния использования способностей
            healerUsed = false;
            archerUsed = false;
            playerTurn = true; // Начало хода игрока

            RefreshUI(); // Активировать кнопки, если условия позволяют
            Turn.IsEnabled = false;
            CancelTurn.IsEnabled = true;
        }


        private void CancelTurn_Click(object sender, RoutedEventArgs e)
        {
            if (playerArmy.Count > 0 && computerArmy.Count > 0)
            {
                battleManagerProxy.Attack(playerArmy[0], computerArmy[0], outputTextBox);
                battleManagerProxy.IsDead(computerArmy[0], computerArmy);
                CheckGameOver();
            }

            playerTurn = false; // Завершение хода игрока
            ComputerTurn();

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
            CancelTurn.IsEnabled = false;
            DisableAbilityButtons(); // Деактивировать кнопки до явного начала хода
        }


        private void CheckGameOver()
        {
            if (playerArmy.Count == 0)
            {
                MessageBox.Show("Компьютер победил!"); 
                Turn.IsEnabled = false;
                CancelTurn.IsEnabled = false;
                UseWizard.IsEnabled = false;
                UseHealer.IsEnabled = false;
                UseArcher.IsEnabled = false;
            }
            else if (computerArmy.Count == 0)
            {
                MessageBox.Show("Вы победили!");
                Turn.IsEnabled = false;
                CancelTurn.IsEnabled = false;
                UseWizard.IsEnabled = false;
                UseHealer.IsEnabled = false;
                UseArcher.IsEnabled = false;
            }
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
        }
    }
}