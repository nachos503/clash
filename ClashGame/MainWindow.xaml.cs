using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ClashGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameManager gameManager = GameManager.Instance;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            gameManager.StartGame(outputTextBox);
        }
    }

    abstract class Warrior
    {
        public double Healthpoints { get; set; }
        public double Damage { get; set; }
        public double Defence { get; set; }
        public double Dodge { get; set; }
        public int Cost { get; set; }

        public string Side { get; set; }

        protected Warrior()
        {
        }
    }

    class LightWarrior : Warrior
    {
        public LightWarrior(string side) : base()
        {
            Healthpoints = 100;
            Damage = 15;
            Defence = 10;
            Dodge = 10;
            Cost = 10;
            Side = side;
        }

        public override string ToString()
        {
            return "LightWarrior";
        }
    }

    class HeavyWarrior : Warrior
    {
        public HeavyWarrior(string side) : base()
        {
            Healthpoints = 250;
            Damage = 20;
            Defence = 30;
            Dodge = 5;
            Cost = 25;
            Side = side;
        }

        public override string ToString()
        {
            return "HeavyWarrior";
        }
    }

    class Archer : Warrior, IRangedUnit
    {

        public string attackerSide { get; set; }
        public string defenderSide { get; set; }
        public Archer(string side) : base()
        {
            Healthpoints = 75;
            Damage = 15;
            Defence = 5;
            Dodge = 25;
            Cost = 30;
            Side = side;
        }

        public int Range()
        {
            // Расчет дальности атаки арчера
            return 3;
        }

        public double RangedDamage(int index)
        {
            if (index == 0)
            {
                // Ближний бой
               return  15;
            }
            else
            {
                //Дальный бой
                // Расчет урона от атаки арчера
                return 20;
            }
        }

        public double RangedAttack(List<Warrior> enemies, int targetIndex, int attackerIndex)
        {
            var enemy = enemies[targetIndex];
            int distance = Math.Abs(attackerIndex - targetIndex);

            if (distance == 0) // Ближний бой
            {
                if (attackerSide != defenderSide) // Проверяем, находятся ли воины на разных сторонах
                {
                    enemy.Healthpoints -= RangedDamage(attackerIndex);
                    return RangedDamage(attackerIndex);
                }
                else
                {
                    // Воины находятся на одной стороне, ближняя атака невозможна
                    return 0;
                }
            }
            else // Дальний бой
            {
                enemy.Healthpoints -= RangedDamage(attackerIndex);
                return RangedDamage(attackerIndex);
            }
        }
    }

    interface IRangedUnit
    {
        int Range();
        double RangedDamage(int index); // Добавлен индекс атакующего лучника
        double RangedAttack(List<Warrior> enemies, int targetIndex, int attackerIndex); // Добавлен индекс атакующего лучника
    }

    class ArmyManager
    {
        public int MaxCost { get; set; }
        int maxCost = 100;
        private TextBox outputTextBox;

        public ArmyManager(TextBox textBox)
        {
            outputTextBox = textBox;
        }

        public List<Warrior> CreateArmy(List<Warrior> warriorList, string side)
        {
            LightWarrior lightWarrior = new LightWarrior(side);
            HeavyWarrior heavyWarrior = new HeavyWarrior(side);
            Archer archer = new Archer(side); // Создаем арчера
            Random rand = new Random();

            int costSum = 0;
            while (costSum < maxCost)
            {
                if (rand.Next(0, 3) == 0 && costSum + archer.Cost <= maxCost)
                {
                    warriorList.Add(archer);
                    costSum += archer.Cost;
                }
                else if (rand.Next(0, 2) == 0 && costSum + lightWarrior.Cost <= maxCost)
                {
                    warriorList.Add(new LightWarrior(side));
                    costSum += lightWarrior.Cost;
                }
                else if (costSum + heavyWarrior.Cost <= maxCost)
                {
                    warriorList.Add(new HeavyWarrior(side));
                    costSum += heavyWarrior.Cost;
                }
                else
                {
                    break;
                }

                outputTextBox.AppendText(costSum.ToString() + Environment.NewLine); // Добавление информации в TextBox
            }

            foreach (var x in warriorList)
            {
                outputTextBox.AppendText(x.ToString() + Environment.NewLine); // Добавление информации в TextBox
            }

            return warriorList;
        }
    }

    sealed class GameManager
    {
        private static GameManager? _instance;
        public static GameManager Instance
        {
            get
            {
                _instance ??= new GameManager();
                return _instance;
            }
        }

        public void StartGame(TextBox outputTextBox)
        {
            ArmyManager armyManager = new ArmyManager(outputTextBox);
            BattleManager battleManager = new BattleManager();

            List<Warrior> firstArmy = new List<Warrior>();
            List<Warrior> secondArmy = new List<Warrior>();

            armyManager.CreateArmy(firstArmy, "Blue");
            armyManager.CreateArmy(secondArmy, "Red");

            battleManager.StartBattle(firstArmy, secondArmy, outputTextBox);
        }
    }

    class BattleManager
    {
        public void StartBattle(List<Warrior> firstArmy, List<Warrior> secondArmy, TextBox outputTextBox)
        {
            while (firstArmy.Count != 0)
            {
                Turn(firstArmy, secondArmy, outputTextBox);

                if (secondArmy.Count != 0)
                {
                    Turn(secondArmy, firstArmy, outputTextBox);
                }
                else
                {
                    outputTextBox.AppendText("Первые победили!!" + Environment.NewLine);
                    break;
                }
            }
            outputTextBox.AppendText("Вторые победили!!" + Environment.NewLine);
        }

        public void Turn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Warrior attacker = attackers[0];
            Warrior defender = defenders[0];

            Attack(attacker, defender, outputTextBox);

            // После обычной атаки, ищем арчера в оставшемся списке воинов
            foreach (var warrior in attackers)
            {
                if (warrior is Archer)
                {
                    Archer archer = (Archer)warrior;
                    var targetIndex = new Random().Next(0, defenders.Count); // Выбор случайного защищающегося воина
                    var target = defenders[targetIndex]; // Выбранный защищающийся воин

                    // Передача индекса атакующего лучника
                    archer.RangedAttack(defenders, targetIndex, attackers.IndexOf(warrior));
                    outputTextBox.AppendText($"Атака {archer.Side} {archer} с силой {archer.RangedDamage(attackers.IndexOf(warrior))} по {target}" + Environment.NewLine);
                    outputTextBox.AppendText($"HP у  {target.Side} {target} осталось {target.Healthpoints}" + Environment.NewLine);

                    break; // Выходим после того как нашли первого арчера
                }
            }

            IsDead(defender, defenders);
        }

        public void Attack(Warrior warrior1, Warrior warrior2, TextBox outputTextBox)
        {
            outputTextBox.AppendText($"Атака {warrior1.Side} {warrior1} с силой {warrior1.Damage} по {warrior2.Side} {warrior2}" + Environment.NewLine);
            DefencePlease(warrior1, warrior2, outputTextBox);
        }

        public void DefencePlease(Warrior warrior1, Warrior warrior2, TextBox outputTextBox)
        {
            Random random = new Random();

            if (random.Next(0, 101) > warrior2.Dodge)
            {
                warrior2.Healthpoints -= warrior1.Damage;
                outputTextBox.AppendText($"HP у {warrior2.Side} {warrior2} осталось {warrior2.Healthpoints}" + Environment.NewLine);
            }

            else
            {
                outputTextBox.AppendText("Dodge cool" + Environment.NewLine);
            }
        }

        public void IsDead(Warrior warrior, List<Warrior> army)
        {
            if (warrior.Healthpoints <= 0)
            {
                army.Remove(warrior);
            }
        }
    }
}
