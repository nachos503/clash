using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ClashGame
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            GameManager.Instance.StartGame(outputTextBox);
        }
    }
    // Интерфейс для лечения
    interface IHealable
    {
        void Heal(Warrior target);
    }

    // Интерфейс для клонирования юнитов
    interface IClonableUnit
    {
        Warrior Clone();
    }


    interface IRangedUnit
    {
        int Range();
        double RangedDamage(int index); // Добавлен индекс атакующего лучника
        double RangedAttack(List<Warrior> enemies, int targetIndex, int attackerIndex); // Добавлен индекс атакующего лучника
    }

    //Абстрактная фабрика
    interface IUnitFactory
    {
        Warrior CreateLightWarrior(string side);
        Warrior CreateHeavyWarrior(string side);
        Warrior CreateArcher(string side);
        Warrior CreateHealer(string side);
        Warrior CreateWizard(string side);

    }

    abstract class Warrior : IClonableUnit
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

        // Реализация метода Clone в базовом классе Warrior
        public virtual Warrior Clone()
        {
            // Возвращаем клон текущего воина
            return (Warrior)this.MemberwiseClone();
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

        public Warrior Clone()
        {
            // Создаем клон с текущими показателями
            return new LightWarrior(Side)
            {
                Healthpoints = this.Healthpoints,
                Damage = this.Damage,
                Defence = this.Defence,
                Dodge = this.Dodge,
                Cost = this.Cost
            };
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

    class ImprovedHeavyWarrior : Warrior
    {
        private HeavyWarrior heavyWarrior;
        private bool isUpgraded;
        public double MaxHealthpoints { get; }

        public ImprovedHeavyWarrior(HeavyWarrior heavyWarrior)
        {
            this.heavyWarrior = heavyWarrior;
            Healthpoints = heavyWarrior.Healthpoints;
            MaxHealthpoints = 250;
            Damage = heavyWarrior.Damage + 20; // Увеличиваем атаку на 20
            Defence = heavyWarrior.Defence + 20; // Увеличиваем защиту на 20
            Dodge = heavyWarrior.Dodge;
            Cost = heavyWarrior.Cost;
            Side = heavyWarrior.Side;
            isUpgraded = true;
        }

        public override string ToString()
        {
            return "ImprovedHeavyWarrior";
        }

        // Метод для проверки, должно ли улучшение быть отменено
        public bool ShouldCancelUpgrade(List<Warrior> allies)
        {
            // Проверяем, есть ли рядом LightWarrior
            foreach (var ally in allies)
            {
                if (ally is LightWarrior && ally.Healthpoints > 0)
                    return false;
            }

            // Проверяем, не мало ли у нас HP
            if (Healthpoints < 0.4 * heavyWarrior.Healthpoints)
                return true;

            return false;
        }

        // Метод для получения базового тяжёлого воина
        public HeavyWarrior GetBaseHeavyWarrior()
        {
            return heavyWarrior;
        }

        // Метод для проверки, улучшён ли воин
        public bool IsUpgraded()
        {
            return isUpgraded;
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
                return 15;
            }
            else
            {
                //Дальний бой
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

    // Класс лекаря
    class Healer : Warrior, IHealable
    {
        public Healer(string side) : base()
        {
            Healthpoints = 50;
            Damage = 5;
            Defence = 5;
            Dodge = 10;
            Cost = 20;
            Side = side;
        }

        public void Heal(Warrior target)
        {

            double maxHealableHealthpoints = target.Healthpoints * 0.8;
            double healAmount = 20;

            if (maxHealableHealthpoints > target.Healthpoints)
                if (target.Healthpoints + healAmount > maxHealableHealthpoints)
                    target.Healthpoints = maxHealableHealthpoints; // Восстанавливаем до максимально возможного
                else
                    target.Healthpoints += healAmount; // Восстанавливаем 20 единиц здоровья

        }
    }

    class Wizard : Warrior
    {
        public Wizard(string side) : base()
        {
            Healthpoints = 50;
            Damage = 10;
            Defence = 5;
            Dodge = 20;
            Cost = 25;
            Side = side;
        }

        public Warrior CloneLightWarrior(List<Warrior> warriors)
        {
            if (new Random().Next(0, 2) == 0)
            {
                // Ищем LightWarrior в списке воинов
                foreach (var warrior in warriors)
                {
                    if (warrior is LightWarrior)
                    {
                        // Клонируем LightWarrior
                        return warrior.Clone();
                    }
                }
            }
            return null;
        }

    }
    //Реализация интерфейса (абстрактной фабрики)
    class ArmyUnitFactory : IUnitFactory
    {
        public Warrior CreateLightWarrior(string side)
        {
            return new LightWarrior(side);
        }


        public Warrior CreateHeavyWarrior(string side)
        {
            return new HeavyWarrior(side);
        }

        public Warrior CreateArcher(string side)
        {
            return new Archer(side);
        }

        public Warrior CreateHealer(string side)
        {
            return new Healer(side);
        }
        public Warrior CreateWizard(string side)
        {
            return new Wizard(side);
        }
    }

    class ArmyManager
    {
        private readonly IUnitFactory unitFactory;
        private TextBox outputTextBox;
        private const int maxCost = 100;

        public ArmyManager(TextBox textBox, IUnitFactory factory)
        {
            outputTextBox = textBox;
            unitFactory = factory;
        }

        public List<Warrior> CreateArmy(List<Warrior> warriorList, string side)
        {
            Random rand = new Random();

            int costSum = 0;
            while (costSum < maxCost)
            {
                if (rand.Next(0, 5) == 0 && costSum + unitFactory.CreateWizard(side).Cost <= maxCost)
                {
                    warriorList.Add(unitFactory.CreateWizard(side));
                    costSum += unitFactory.CreateWizard(side).Cost;
                }
                if (rand.Next(0, 4) == 0 && costSum + unitFactory.CreateArcher(side).Cost <= maxCost)
                {
                    warriorList.Add(unitFactory.CreateArcher(side));
                    costSum += unitFactory.CreateArcher(side).Cost;
                }
                else if (rand.Next(0, 3) == 0 && costSum + unitFactory.CreateHealer(side).Cost <= maxCost)
                {
                    warriorList.Add(unitFactory.CreateHealer(side));
                    costSum += unitFactory.CreateHealer(side).Cost;
                }
                else if (rand.Next(0, 2) == 0 && costSum + unitFactory.CreateLightWarrior(side).Cost <= maxCost)
                {
                    warriorList.Add(unitFactory.CreateLightWarrior(side));
                    costSum += unitFactory.CreateLightWarrior(side).Cost;
                }
                else if (costSum + unitFactory.CreateHeavyWarrior(side).Cost <= maxCost)
                {
                    warriorList.Add(unitFactory.CreateHeavyWarrior(side));
                    costSum += unitFactory.CreateHeavyWarrior(side).Cost;
                }
                else
                {
                    break;
                }

                outputTextBox.AppendText(costSum.ToString() + Environment.NewLine); // Добавление информации в TextBox
            }

            for (int i = 0; i < warriorList.Count; i++)
            {
                // Проверяем, является ли текущий воин HeavyWarrior
                if (warriorList[i] is HeavyWarrior)
                {
                    // Проверяем, есть ли рядом LightWarrior
                    if ((i > 0 && warriorList[i - 1] is LightWarrior) || (i < warriorList.Count - 1 && warriorList[i + 1] is LightWarrior))
                    {
                        ImprovedHeavyWarrior improvedHeavyWarrior = new ImprovedHeavyWarrior((HeavyWarrior)warriorList[i]);

                        // Проверяем, должно ли улучшение быть отменено
                        if (improvedHeavyWarrior.ShouldCancelUpgrade(warriorList))
                        {
                            warriorList[i] = improvedHeavyWarrior.GetBaseHeavyWarrior(); // Возвращаем базового тяжёлого воина
                        }
                        else
                        {
                            warriorList[i] = improvedHeavyWarrior; // Заменяем текущего воина на улучшенного
                        }
                    }
                }
                outputTextBox.AppendText(warriorList[i].ToString() + Environment.NewLine); // Добавление информации в TextBox
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

        private readonly IUnitFactory unitFactory;

        private GameManager()
        {
            unitFactory = new ArmyUnitFactory();
        }

        public void StartGame(TextBox outputTextBox)
        {
            ArmyManager armyManager = new ArmyManager(outputTextBox, unitFactory);
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
            if (firstArmy.Count == 0) outputTextBox.AppendText("Вторые победили!!" + Environment.NewLine);
        }

        public void Turn(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            Warrior attacker = attackers[0];
            Warrior defender = defenders[0];

            // Проверка наличия мага в списке атакующих
            Wizard wizard = null;
            foreach (var attacker1 in attackers)
            {
                if (attacker1 is Wizard)
                {
                    wizard = (Wizard)attacker1;
                    break;
                }
            }

            // Попытка клонирования LightWarrior
            if (wizard != null)
            {
                Warrior clonedWarrior = wizard.CloneLightWarrior(attackers);
                if (clonedWarrior != null)
                {
                    attackers.Insert(1, clonedWarrior); // Вставляем клонированного LightWarrior перед магом (на вторую позицию)
                    outputTextBox.AppendText($"Маг из команды {wizard.Side} клонировал LightWarrior с {clonedWarrior.Healthpoints} HP" + Environment.NewLine);
                }
            }

            // Проверка на наличие лекаря в списке атакующих и его позиции
            Healer healer = null;
            int healerIndex = -1;
            for (int i = 0; i < attackers.Count; i++)
            {
                if (attackers[i] is Healer)
                {
                    healer = (Healer)attackers[i];
                    healerIndex = i;
                    break;
                }
            }

            if (healer != null && healerIndex != 0)
            {
                // Проверка на выполнение условий для лечения
                if (new Random().Next(0, 10) == 0)
                {
                    // Вызов лечения у случайного союзника
                    List<Warrior> alliesInRange = GetAlliesInRange(attackers, healerIndex);
                    if (alliesInRange.Count > 0)
                    {
                        int targetIndex = new Random().Next(0, alliesInRange.Count);
                        Warrior targetAlly = alliesInRange[targetIndex];
                        if (!(targetAlly is LightWarrior))
                        {
                            healer.Heal(targetAlly);
                            outputTextBox.AppendText($"Лекарь из команды {healer.Side} вылечил {targetAlly.Side} {targetAlly}" + Environment.NewLine);
                            outputTextBox.AppendText($"Теперь у {targetAlly.Side} {targetAlly} {targetAlly.Healthpoints} HP" + Environment.NewLine);
                        }
                    }
                }
            }

            // Проверка на наличие ImprovedHeavyWarrior в списке атакующих и его позиции
            ImprovedHeavyWarrior improvedHeavyWarrior = null;
            int improvedHeavyWarriorIndex = -1;
            for (int i = 0; i < attackers.Count; i++)
            {
                if (attackers[i] is ImprovedHeavyWarrior)
                {
                    improvedHeavyWarrior = (ImprovedHeavyWarrior)attackers[i];
                    improvedHeavyWarriorIndex = i;
                    break;
                }
            }

            // Проверяем, есть ли рядом с ImprovedHeavyWarrior LightWarrior
            bool lightWarriorNearby = false;
            if (improvedHeavyWarrior != null)
            {
                // Проверяем наличие LightWarrior на расстоянии 1 от ImprovedHeavyWarrior
                for (int i = Math.Max(0, improvedHeavyWarriorIndex - 1); i < Math.Min(attackers.Count, improvedHeavyWarriorIndex + 2); i++)
                {
                    if (attackers[i] is LightWarrior)
                    {
                        lightWarriorNearby = true;
                        break;
                    }
                }

                if (!lightWarriorNearby)
                {
                    // Если рядом с ImprovedHeavyWarrior нет LightWarrior, отменяем улучшение
                    attackers[improvedHeavyWarriorIndex] = new HeavyWarrior(improvedHeavyWarrior.Side)
                    {
                        Healthpoints = improvedHeavyWarrior.Healthpoints // сохраняем текущее здоровье
                    };
                    outputTextBox.AppendText($"Улучшение у ImprovedHeavyWarrior отменено, так как рядом нет LightWarrior." + Environment.NewLine);
                    attacker = attackers[0];
                }
            }

            // Проверяем здоровье ImprovedHeavyWarrior
            if (improvedHeavyWarrior != null && improvedHeavyWarrior.Healthpoints < 0.4 * improvedHeavyWarrior.MaxHealthpoints)
            {
                // Если здоровье упало ниже 40%, отменяем улучшение
                attackers[improvedHeavyWarriorIndex] = new HeavyWarrior(improvedHeavyWarrior.Side);
                outputTextBox.AppendText($"Улучшение у ImprovedHeavyWarrior отменено, так как его здоровье упало ниже 40%." + Environment.NewLine);
                attacker = attackers[0];
            }

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

        private List<Warrior> GetAlliesInRange(List<Warrior> attackers, int healerIndex)
        {
            List<Warrior> alliesInRange = new List<Warrior>();
            for (int i = Math.Max(0, healerIndex - 3); i < Math.Min(attackers.Count, healerIndex + 4); i++)
            {
                if (i != healerIndex && attackers[i].Side == attackers[healerIndex].Side)
                {
                    alliesInRange.Add(attackers[i]);
                }
            }
            return alliesInRange;
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
