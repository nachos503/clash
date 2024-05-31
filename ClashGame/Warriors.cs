using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashGame
{
    /// <summary>
    /// Abstract class Warrior representing a warrior.
    /// Identifier string: "T:ClashGame.Warrior".
    /// </summary>
    public abstract class Warrior : IClonableUnit
    {
        /// <summary>
        /// Warrior's health points.
        /// Identifier string: "P:ClashGame.Warrior.Healthpoints".
        /// </summary>
        public double Healthpoints { get; set; }

        /// <summary>
        /// Warrior's damage.
        /// Identifier string: "P:ClashGame.Warrior.Damage".
        /// </summary>
        public double Damage { get; set; }

        /// <summary>
        /// Warrior's defense.
        /// Identifier string: "P:ClashGame.Warrior.Defence".
        /// </summary>
        public double Defence { get; set; }

        /// <summary>
        /// Warrior's dodge.
        /// Identifier string: "P:ClashGame.Warrior.Dodge".
        /// </summary>
        public double Dodge { get; set; }

        /// <summary>
        /// Warrior's cost.
        /// Identifier string: "P:ClashGame.Warrior.Cost".
        /// </summary>
        public int Cost { get; set; }

        /// <summary>
        /// Warrior's side.
        /// Identifier string: "P:ClashGame.Warrior.Side".
        /// </summary>
        public string Side { get; set; }

        /// <summary>
        /// Default constructor for the Warrior class.
        /// Identifier string: "M:ClashGame.Warrior.#ctor".
        /// </summary>
        protected Warrior()
        {
        }

        /// <summary>
        /// Implementation of the Clone method in the base Warrior class.
        /// Returns a clone of the current warrior.
        /// Identifier string: "M:ClashGame.Warrior.Clone".
        /// </summary>
        /// <returns>A clone of the current warrior.</returns>
        public virtual Warrior Clone() => (Warrior)this.MemberwiseClone();
    }

    /// <summary>
    /// Class LightWarrior representing a light warrior.
    /// Identifier string: "T:ClashGame.LightWarrior".
    /// </summary>
    class LightWarrior : Warrior
    {
        /// <summary>
        /// Constructor for the LightWarrior class.
        /// Identifier string: "M:ClashGame.LightWarrior.#ctor(System.String)".
        /// </summary>
        /// <param name="side">The side of the warrior.</param>
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
    /// The HeavyWarrior class representing a heavily armed warrior.
    /// Identifier string: "T:ClashGame.HeavyWarrior".
    /// </summary>
    class HeavyWarrior : Warrior
    {
        /// <summary>
        /// Constructor for the HeavyWarrior class.
        /// Identifier string: "M:ClashGame.HeavyWarrior.#ctor(System.String)".
        /// <param name="side">The side of the warrior.</param>
        /// </summary>
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
    /// The ImprovedHeavyWarrior class, representing an improved heavy warrior.
    /// Identifier string: "T:ClashGame.ImprovedHeavyWarrior".
    /// </summary>
    class ImprovedHeavyWarrior : Warrior
    {

        private HeavyWarrior heavyWarrior;
        private bool isUpgraded;

        /// <summary>
        /// The maximum health points of the improved heavy warrior.
        /// Identifier string: "P:ClashGame.ImprovedHeavyWarrior.MaxHealthpoints".
        /// </summary>
        public double MaxHealthpoints { get; }

        /// <summary>
        /// Constructor for the ImprovedHeavyWarrior class.
        /// Identifier string: "M:ClashGame.ImprovedHeavyWarrior.#ctor(ClashGame.HeavyWarrior)".
        /// </summary>
        /// <param name="heavyWarrior">The original heavy warrior.</param>
        public ImprovedHeavyWarrior(HeavyWarrior heavyWarrior)
        {
            this.heavyWarrior = heavyWarrior;
            Healthpoints = heavyWarrior.Healthpoints;
            MaxHealthpoints = 250;
            Damage = heavyWarrior.Damage + 20; 
            Defence = heavyWarrior.Defence + 20; 
            Dodge = heavyWarrior.Dodge;
            Cost = heavyWarrior.Cost;
            Side = heavyWarrior.Side;
            isUpgraded = true;
        }
    }

    /// <summary>
    /// The Archer class, representing an archer.
    /// Identifier string: "T:ClashGame.Archer".
    /// </summary>
    public class Archer : Warrior, IRangedUnit
    {
        /// <summary>
        /// The side of the attacker.
        /// Identifier string: "P:ClashGame.Archer.attackerSide".
        /// </summary>
        public string attackerSide { get; set; }

        /// <summary>
        /// The side of the defender.
        /// Identifier string: "P:ClashGame.Archer.defenderSide".
        /// </summary>
        public string defenderSide { get; set; }

        /// <summary>
        /// Constructor for the Archer class.
        /// Identifier string: "M:ClashGame.Archer.#ctor(System.String)".
        /// </summary>
        /// <param name="side">The side of the warrior.</param>
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
        /// Calculates the range of the archer's attack.
        /// Identifier string: "M:ClashGame.Archer.Range".
        /// </summary>
        /// <returns>The range of the attack.</returns>
        public int Range() => 3;

        /// <summary>
        /// Calculates the damage of the archer's attack.
        /// Identifier string: "M:ClashGame.Archer.RangedDamage(System.Int32)".
        /// </summary>
        /// <param name="index">The index of the attack (melee or ranged).</param>
        /// <returns>The damage of the attack.</returns>
        public double RangedDamage(int index)
        {
            // Melee
            if (index == 0) return 15;
            // Ranged
            // Calculates the damage of the archer's ranged attack
            else return 20;
        }

        /// <summary>
        /// Archer's ranged attack.
        /// Identifier string: "M:ClashGame.Archer.RangedAttack(System.Collections.Generic.List{ClashGame.Warrior},ClashGame.Warrior,System.Int32)".
        /// </summary>
        /// <param name="enemies">The list of enemies.</param>
        /// <param name="target">The target of the attack.</param>
        /// <param name="attackerIndex">The index of the attacker.</param>
        /// <returns>The damage dealt to the target.</returns>
        virtual public double RangedAttack(List<Warrior> enemies, Warrior target, int attackerIndex)
        {
           target.Healthpoints -= RangedDamage(attackerIndex);
           return RangedDamage(attackerIndex);
        }
    }

    /// <summary>
    /// The Healer class, representing a healer.
    /// Identifier string: "T:ClashGame.Healer".
    /// </summary>
    public class Healer : Warrior, IHealable
    {

        /// <summary>
        /// Constructor for the Healer class.
        /// ID string "M:ClashGame.Healer.#ctor(System.String)".
        /// </summary>
        /// <param name="side">The warrior's side.</param>
        public Healer(string side) : base()
        {
            Healthpoints = 50;
            Damage = 5;
            Defence = 5;
            Dodge = 10;
            Cost = 20;
            Side = side;
        }

        /// <краткое описание>
        /// Лечение цели.
        /// Строка "Идентификатор ":ClashGame.Целитель.Исцеление(ClashGame.Воин)".
        /// </краткое описание>
        /// <имя параметра="цель">для лечения.</параметр>
        virtual public void Heal(Warrior target)
        {
            double maxHealableHealthpoints = target.Healthpoints * 0.8;
            double healAmount = 20;

            if (maxHealableHealthpoints > target.Healthpoints)
                if (target.Healthpoints + healAmount > maxHealableHealthpoints)
                    target.Healthpoints = maxHealableHealthpoints;  // Restoring to the maximum possible
                else
                    target.Healthpoints += healAmount; // Restoring 20 health units
        }
    }

    /// <summary>
    /// The Wizard class representing the magician.
    /// ID string "T:ClashGame.Wizard".
    /// </summary>
    public class Wizard : Warrior
    {
        /// <summary>
        /// Constructor for the Wizard class.
        /// ID string "M:ClashGame.Wizard.#ctor(System.String)".
        /// </summary>
        /// <param name="side">The warrior's side.</param>
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
        /// Cloning a light warrior.
        /// ID string "M:ClashGame.Wizard.CloneLightWarrior(ClashGame.Warrior)".
        /// </summary>
        /// <param name="warrior">A clone warrior.</param>
        /// <returns>A clone of a light warrior, or null if the warrior is not a light warrior.</returns>
        virtual public Warrior CloneLightWarrior(Warrior warrior)
        {
            // Клонируем LightWarrior
            if (warrior is LightWarrior) return warrior.Clone();
            else return null;
        }
    }

    /// <summary>
    /// GulyayGorod class representing gulyai-gorod.
    /// Identifier string "T:ClashGame.GulyayGorod".
    /// </summary>
    public class GulyayGorod : Warrior
    {
        /// <summary>
        /// Constructor for the GulyayGorod class.
        /// ID string "M:ClashGame.GulyayGorod.#ctor(System.String)".
        /// </summary>
        /// <param name="side">The warrior's side.</param>
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
