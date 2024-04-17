// MainWindow.xaml.cs

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ClashGame
{
    public partial class MainWindow : Window
    {
        private readonly ArmyManager armyManager;
        private readonly GameManager gameManager;

        public MainWindow()
        {
            InitializeComponent();
            gameManager = GameManager.Instance;
            armyManager = new ArmyManager(outputTextBox, new ArmyUnitFactory());
        }

        private void CreateArmy_Click(object sender, RoutedEventArgs e)
        {
            outputTextBox.Clear(); // Очищаем текстовое поле перед созданием армии

            List<Warrior> firstArmy = new List<Warrior>();
            List<Warrior> secondArmy = new List<Warrior>();

            firstArmy = armyManager.CreateArmy(firstArmy, "Blue");
            secondArmy = armyManager.CreateArmy(secondArmy, "Red");

            // Выводим список армии в текстовое поле
            outputTextBox.AppendText($"\nАрмия Blue:\n");
            foreach (var warrior in firstArmy)
            {
                outputTextBox.AppendText($"{warrior}\n");
            }

            outputTextBox.AppendText($"\nАрмия Red:\n");
            foreach (var warrior in secondArmy)
            {
                outputTextBox.AppendText($"{warrior}\n");
            }

            // Показываем кнопки "Синяя" и "Красная" для выбора команды
            ChooseBlueArmy.Visibility = Visibility.Visible;
            ChooseRedArmy.Visibility = Visibility.Visible;
        }

        private void ChooseBlueArmy_Click(object sender, RoutedEventArgs e)
        {
            StartGame(outputTextBox, "Blue", "Red");
        }

        private void ChooseRedArmy_Click(object sender, RoutedEventArgs e)
        {
            StartGame(outputTextBox, "Red", "Blue");
        }

        public void StartGame(TextBox outputTextBox, string firstArmyColor, string secondArmyColor)
        {
            outputTextBox.Clear(); // Очищаем текстовое поле перед началом игры
            var firstArmy = armyManager.GetArmyByColor(firstArmyColor);
            var secondArmy = armyManager.GetArmyByColor(secondArmyColor);
            gameManager.StartGame(outputTextBox, firstArmy, secondArmy);

            // Скрываем кнопки "Синяя" и "Красная" после выбора команды
            ChooseBlueArmy.Visibility = Visibility.Collapsed;
            ChooseRedArmy.Visibility = Visibility.Collapsed;
        }
    }
}
