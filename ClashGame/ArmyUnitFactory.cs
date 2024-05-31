using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashGame
{
    /// <summary>
    /// class ArmyUnitFactory - a class for creating various types of units. Implementation of the interface (abstract factory).
    /// Implements the IUnitFactory interface.
    /// Identifier string: "T:ClashGame.ArmyUnitFactory".
    /// </summary>
    class ArmyUnitFactory : IUnitFactory
    {
        /// <summary>
        /// public Warrior CreateLightWarrior(string side) - method for creating a light warrior.
        /// Creates and returns an instance of the LightWarrior class for the specified side.
        /// Identifier string: "M:ClashGame.ArmyUnitFactory.CreateLightWarrior(System.String)".
        /// </summary>
        /// <param name="side">The side for which the unit is created.</param>
        /// <returns>An instance of the LightWarrior class.</returns>
        public Warrior CreateLightWarrior(string side)
        {
            return new LightWarrior(side);
        }

        /// <summary>
        /// public Warrior CreateHeavyWarrior(string side) - method for creating a heavy warrior.
        /// Creates and returns an instance of the HeavyWarrior class for the specified side.
        /// Identifier string: "M:ClashGame.ArmyUnitFactory.CreateHeavyWarrior(System.String)".
        /// </summary>
        /// <param name="side">The side for which the unit is created.</param>
        /// <returns>An instance of the HeavyWarrior class.</returns>
        public Warrior CreateHeavyWarrior(string side)
        {
            return new HeavyWarrior(side);
        }

        /// <summary>
        /// public Warrior CreateArcher(string side) - method for creating an archer.
        /// Creates and returns an instance of the Archer class for the specified side.
        /// Identifier string: "M:ClashGame.ArmyUnitFactory.CreateArcher(System.String)".
        /// </summary>
        /// <param name="side">The side for which the unit is created.</param>
        /// <returns>An instance of the Archer class.</returns>
        public Warrior CreateArcher(string side)
        {
            return new Archer(side);
        }

        /// <summary>
        /// public Warrior CreateHealer(string side) - method for creating a healer.
        /// Creates and returns an instance of the Healer class for the specified side.
        /// Identifier string: "M:ClashGame.ArmyUnitFactory.CreateHealer(System.String)".
        /// </summary>
        /// <param name="side">The side for which the unit is created.</param>
        /// <returns>An instance of the Healer class.</returns>
        public Warrior CreateHealer(string side)
        {
            return new Healer(side);
        }

        /// <summary>
        /// public Warrior CreateWizard(string side) - method for creating a wizard.
        /// Creates and returns an instance of the Wizard class for the specified side.
        /// Identifier string: "M:ClashGame.ArmyUnitFactory.CreateWizard(System.String)".
        /// </summary>
        /// <param name="side">The side for which the unit is created.</param>
        /// <returns>An instance of the Wizard class.</returns>
        public Warrior CreateWizard(string side)
        {
            return new Wizard(side);
        }

        /// <summary>
        /// public Warrior CreateGulyayGorod(string side) - method for creating a "Gulyay-gorod" unit.
        /// Creates and returns an instance of the GulyayGorod class for the specified side.
        /// Identifier string: "M:ClashGame.ArmyUnitFactory.CreateGulyayGorod(System.String)".
        /// </summary>
        /// <param name="side">The side for which the unit is created.</param>
        /// <returns>An instance of the GulyayGorod class.</returns>
        public Warrior CreateGulyayGorod(string side)
        {
            return new GulyayGorod(side);
        }
    }
}
