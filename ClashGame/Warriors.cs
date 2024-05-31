using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashGame
{
    /// <summary>
    /// Абстрактный класс Warrior, представляющий воина.
    /// Строка идентификатора "T:ClashGame.Warrior".
    /// </summary>
    public abstract class Warrior : IClonableUnit
    {
        /// <summary>
        /// Здоровье воина.
        /// Строка идентификатора "P:ClashGame.Warrior.Healthpoints".
        /// </summary>
        public double Healthpoints { get; set; }

        /// <summary>
        /// Урон воина.
        /// Строка идентификатора "P:ClashGame.Warrior.Damage".
        /// </summary>
        public double Damage { get; set; }

        /// <summary>
        /// Защита воина.
        /// Строка идентификатора "P:ClashGame.Warrior.Defence".
        /// </summary>
        public double Defence { get; set; }

        /// <summary>
        /// Уклонение воина.
        /// Строка идентификатора "P:ClashGame.Warrior.Dodge".
        /// </summary>
        public double Dodge { get; set; }

        /// <summary>
        /// Стоимость воина.
        /// Строка идентификатора "P:ClashGame.Warrior.Cost".
        /// </summary>
        public int Cost { get; set; }

        /// <summary>
        /// Сторона воина.
        /// Строка идентификатора "P:ClashGame.Warrior.Side".
        /// </summary>
        public string Side { get; set; }

        /// <summary>
        /// Конструктор по умолчанию для класса Warrior.
        /// Строка идентификатора "M:ClashGame.Warrior.#ctor".
        /// </summary>
        protected Warrior()
        {
        }

        // <summary>
        /// Реализация метода Clone в базовом классе Warrior.
        /// Возвращает клон текущего воина.
        /// Строка идентификатора "M:ClashGame.Warrior.Clone".
        /// </summary>
        /// <returns>Клон текущего воина.</returns>
        public virtual Warrior Clone() => (Warrior)this.MemberwiseClone();
    }

    /// <summary>
    /// Класс LightWarrior, представляющий легкого воина.
    /// Строка идентификатора "T:ClashGame.LightWarrior".
    /// </summary>
    class LightWarrior : Warrior
    {
        /// <summary>
        /// Конструктор для класса LightWarrior.
        /// Строка идентификатора "M:ClashGame.LightWarrior.#ctor(System.String)".
        /// </summary>
        /// <param name="side">Сторона воина.</param>
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

    /// <summary>
    /// Класс HeavyWarrior, представляющий тяжеловооруженного воина.
    /// Строка идентификатора "T:ClashGame.HeavyWarrior".
    /// </summary>
    class HeavyWarrior : Warrior
    {
        /// <summary>
        /// Конструктор для класса HeavyWarrior.
        /// Строка идентификатора "M:ClashGame.HeavyWarrior.#ctor(System.String)".
        /// </summary>
        /// <param name="side">Сторона воина.</param>
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

    /// <summary>
    /// Класс ImprovedHeavyWarrior, представляющий улучшенного тяжеловооруженного воина.
    /// Строка идентификатора "T:ClashGame.ImprovedHeavyWarrior".
    /// </summary>
    class ImprovedHeavyWarrior : Warrior
    {

        private HeavyWarrior heavyWarrior;
        private bool isUpgraded;

        /// <summary>
        /// Максимальные очки здоровья улучшенного тяжеловооруженного воина.
        /// Строка идентификатора "P:ClashGame.ImprovedHeavyWarrior.MaxHealthpoints".
        /// </summary>
        public double MaxHealthpoints { get; }

        /// <summary>
        /// Конструктор для класса ImprovedHeavyWarrior.
        /// Строка идентификатора "M:ClashGame.ImprovedHeavyWarrior.#ctor(ClashGame.HeavyWarrior)".
        /// </summary>
        /// <param name="heavyWarrior">Исходный тяжеловооруженный воин.</param>
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
    }

    /// <summary>
    /// Класс Archer, представляющий лучника.
    /// Строка идентификатора "T:ClashGame.Archer".
    /// </summary>
    public class Archer : Warrior, IRangedUnit
    {
        /// <summary>
        /// Сторона атакующего.
        /// Строка идентификатора "P:ClashGame.Archer.attackerSide".
        /// </summary>
        public string attackerSide { get; set; }


        /// <summary>
        /// Сторона защищающегося.
        /// Строка идентификатора "P:ClashGame.Archer.defenderSide".
        /// </summary>
        public string defenderSide { get; set; }

        /// <summary>
        /// Конструктор для класса Archer.
        /// Строка идентификатора "M:ClashGame.Archer.#ctor(System.String)".
        /// </summary>
        /// <param name="side">Сторона воина.</param>
        public Archer(string side) : base()
        {
            Healthpoints = 75;
            Damage = 15;
            Defence = 5;
            Dodge = 25;
            Cost = 30;
            Side = side;
        }

        /// <summary>
        /// Расчет дальности атаки арчера.
        /// Строка идентификатора "M:ClashGame.Archer.Range".
        /// </summary>
        /// <returns>Дальность атаки.</returns>
        public int Range() => 3;


        /// <summary>
        /// Расчет урона от атаки арчера.
        /// Строка идентификатора "M:ClashGame.Archer.RangedDamage(System.Int32)".
        /// </summary>
        /// <param name="index">Индекс атаки (ближний или дальний бой).</param>
        /// <returns>Урон от атаки.</returns>
        public double RangedDamage(int index)
        {
            // Ближний бой
            if (index == 0) return 15;
            //Дальний бой
            // Расчет урона от атаки арчера
            else return 20;
            
        }

        /// <summary>
        /// Атака лучника.
        /// Строка идентификатора "M:ClashGame.Archer.RangedAttack(System.Collections.Generic.List{ClashGame.Warrior},ClashGame.Warrior,System.Int32)".
        /// </summary>
        /// <param name="enemies">Список врагов.</param>
        /// <param name="target">Цель атаки.</param>
        /// <param name="attackerIndex">Индекс атакующего.</param>
        /// <returns>Урон, нанесенный цели.</returns>
        virtual public double RangedAttack(List<Warrior> enemies, Warrior target, int attackerIndex)
        {
           target.Healthpoints -= RangedDamage(attackerIndex);
           return RangedDamage(attackerIndex);
        }
    }

    /// <summary>
    /// Класс Healer, представляющий лекаря.
    /// Строка идентификатора "T:ClashGame.Healer".
    /// </summary>
    public class Healer : Warrior, IHealable
    {

        /// <summary>
        /// Конструктор для класса Healer.
        /// Строка идентификатора "M:ClashGame.Healer.#ctor(System.String)".
        /// </summary>
        /// <param name="side">Сторона воина.</param>
        public Healer(string side) : base()
        {
            Healthpoints = 50;
            Damage = 5;
            Defence = 5;
            Dodge = 10;
            Cost = 20;
            Side = side;
        }

        /// <summary>
        /// Лечение цели.
        /// Строка идентификатора "M:ClashGame.Healer.Heal(ClashGame.Warrior)".
        /// </summary>
        /// <param name="target">Цель для лечения.</param>
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

    /// <summary>
    /// Класс Wizard, представляющий мага.
    /// Строка идентификатора "T:ClashGame.Wizard".
    /// </summary>
    public class Wizard : Warrior
    {
        /// <summary>
        /// Конструктор для класса Wizard.
        /// Строка идентификатора "M:ClashGame.Wizard.#ctor(System.String)".
        /// </summary>
        /// <param name="side">Сторона воина.</param>
        public Wizard(string side) : base()
        {
            Healthpoints = 50;
            Damage = 10;
            Defence = 5;
            Dodge = 20;
            Cost = 25;
            Side = side;
        }

        /// <summary>
        /// Клонирование легкого воина.
        /// Строка идентификатора "M:ClashGame.Wizard.CloneLightWarrior(ClashGame.Warrior)".
        /// </summary>
        /// <param name="warrior">Воин для клонирования.</param>
        /// <returns>Клон легкого воина или null, если воин не является легким воином.</returns>
        virtual public Warrior CloneLightWarrior(Warrior warrior)
        {
            // Клонируем LightWarrior
            if (warrior is LightWarrior) return warrior.Clone();
            else return null;
        }
    }

    /// <summary>
    /// Класс GulyayGorod, представляющий гуляй-город.
    /// Строка идентификатора "T:ClashGame.GulyayGorod".
    /// </summary>
    public class GulyayGorod : Warrior
    {
        /// <summary>
        /// Конструктор для класса GulyayGorod.
        /// Строка идентификатора "M:ClashGame.GulyayGorod.#ctor(System.String)".
        /// </summary>
        /// <param name="side">Сторона воина.</param>
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
