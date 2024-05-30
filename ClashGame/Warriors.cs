using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashGame
{
    public abstract class Warrior : IClonableUnit
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

    public class Archer : Warrior, IRangedUnit
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

        virtual public double RangedAttack(List<Warrior> enemies, Warrior target, int attackerIndex)
        {
                target.Healthpoints -= RangedDamage(attackerIndex);
                return RangedDamage(attackerIndex);
        }
    }

    // Класс лекаря
    public class Healer : Warrior, IHealable
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

        virtual public void Heal(Warrior target)
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

   public class Wizard : Warrior
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

        virtual public Warrior CloneLightWarrior(Warrior warrior)
        {
            if (warrior is LightWarrior)
            {
                // Клонируем LightWarrior
                return warrior.Clone();
            }
            else
                return null;
        }
    }

    public class GulyayGorod : Warrior
    {
        public GulyayGorod(string side) : base()
        {
            Healthpoints = 500;
            Damage = 0;
            Defence = 0;
            Dodge = 0;
            Cost = 0;
            Side = side;
        }
    }
}
