using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace ClashGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ArmyManager armyManager;
        public MainWindow()
        {
            InitializeComponent();

            //armyManager = new ArmyManager(outputTextBox);
        }

        //private void CreateArmy_Click(object sender, RoutedEventArgs e)
        //{
        //    armyManager.CreateArmy();
        //}
    }

    abstract class Warrior
    {
        public double Healthpoints { get; set; }
        public double Damage { get; set; }
        public double Defence { get; set; }
        public double Dodge { get; set; }
        public int Cost { get; set; }

        protected Warrior()
        {
        }


    }
    class LightWarrior : Warrior
    {
        public LightWarrior() : base()
        {
            Healthpoints = 100;
            Damage = 15;
            Defence = 10;
            Dodge = 10;
            Cost = 10;
        }

    }

    class HeavyWarrior : Warrior
    {
        public HeavyWarrior() : base()
        {
            Healthpoints = 250;
            Damage = 20;
            Defence = 30;
            Dodge = 5;
            Cost = 25;
        }

    }

    class ArmyManager
    {
        public int MaxCost { get; set; }
        int maxCost = 100;

        private TextBox outputTextBox;

        //public ArmyManager(TextBox textBox)
        //{
        //    outputTextBox = textBox;
        //}

        public List<Warrior> CreateArmy(List<Warrior> warriorList)
        {
            LightWarrior lightWarrior = new LightWarrior();
            HeavyWarrior heavyWarrior = new HeavyWarrior();

            Random rand = new Random();

            int costSum = 0;
            while (costSum < maxCost)
            {
                if (rand.Next(0, 2) == 0 && costSum + lightWarrior.Cost <= maxCost)
                {
                    warriorList.Add(new LightWarrior());
                    costSum += lightWarrior.Cost;
                }
                else if (costSum + heavyWarrior.Cost <= maxCost)
                {
                    warriorList.Add(new HeavyWarrior());
                    costSum += heavyWarrior.Cost;
                }
                else
                {
                    break; // Если ни легкий, ни тяжелый воин не может быть добавлен, прерываем цикл
                }

                outputTextBox.AppendText(costSum.ToString() + Environment.NewLine); // Добавление информации в TextBox
            } 

            foreach (var x in warriorList)
            {
                outputTextBox.AppendText(x.ToString() + Environment.NewLine); // Добавление информации в TextBox
            }
            //тут ссылка на лист :)
            return warriorList;
        }

    }

    //GameManager делаем singleton, чтобы не запускать в параллель несколько игр
    sealed class GameManager
    {
        private static GameManager? _instance;
        public static GameManager Instance
        {
            get
            {
                _instance = new GameManager();
                return _instance;
            }
        }

        public static GameManager GetInstance() { return null; }

        //запуск игры
        public void StartGame()
        {
            //создать две армии

            ArmyManager armyManager = new ArmyManager();
            BattleManager battleManager = new BattleManager();

            List <Warrior> firstArmy = new List <Warrior>();
            List <Warrior> secondArmy = new List <Warrior>();

            armyManager.CreateArmy(firstArmy);
            armyManager.CreateArmy(secondArmy);

            //выбор стороны
            battleManager.StartBattle(firstArmy, secondArmy);
        }
    }

    class BattleManager
    {
        //метод для пиздиловки
        public void StartBattle(List<Warrior> firstArmy, List<Warrior> secondArmy)
        {
            while (firstArmy.Count != 0)
            {
                Turn(firstArmy, secondArmy);

                if  (secondArmy.Count != 0)
                {
                    Turn(secondArmy, firstArmy);
                }
                else
                {
                    Console.WriteLine("Первые победили!!");
                    break;
                }
            }
            Console.WriteLine("Вторые победили!!");
        }

        //метод реализации атаки
        public void Turn(List<Warrior> firstArmy, List<Warrior> secondArmy)
        {
                Attack(firstArmy[0], secondArmy[0]);
                IsDead(secondArmy[0], secondArmy);         
        }

        public void Attack(Warrior warrior1, Warrior warrior2)
        {
            Console.WriteLine($"Атака с силой {warrior1.Damage}");
            DefencePlease(warrior1, warrior2);
        }

        public void DefencePlease(Warrior warrior1, Warrior warrior2)
        {
            Random random = new Random();

            if (random.Next(0, 101) > warrior2.Dodge)
            {
                warrior2.Healthpoints -= warrior1.Damage;
            }

            else
            {
                Console.WriteLine("Dodge cool");
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

    class UIManager
    {

    }
}
