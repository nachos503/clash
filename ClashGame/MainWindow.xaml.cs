using System;
using System.Collections.Generic;
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

namespace ClashGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
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

        public void Attack()
        {
            Console.WriteLine($"Атака с силой {Damage}");
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


        LightWarrior lightWarrior = new LightWarrior();
        HeavyWarrior heavyWarrior = new HeavyWarrior();

        Random rand = new Random();
        List<Warrior> warriorList = new List<Warrior>();

        public void CreateArmy()
        {
            int costSum = 0;
            do
            {
                if (rand.Next(0, 2) == 0)
                {
                    warriorList.Add(new LightWarrior());
                    costSum += lightWarrior.Cost;
                }

                if (rand.Next(0, 2) == 1)
                {
                    warriorList.Add(new HeavyWarrior());
                    costSum += heavyWarrior.Cost;
                }

                Console.WriteLine(costSum);
            } while (maxCost > costSum + lightWarrior.Cost && maxCost > costSum + heavyWarrior.Cost);

            foreach (var x in warriorList)
            {
                Console.WriteLine(x.ToString());
            }
        }

    }

    class GameManager
    {

    }

    class BattleManager
    {

    }

    class UIManager
    {

    }



    //class Game
    //{
    //    static void Main()
    //    {
    //        ArmyManager gameManager = new ArmyManager();

    //        gameManager.CreateArmy();
    //    }

    //}
}
