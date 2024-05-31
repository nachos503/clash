using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace ClashGame
{
    sealed class ArmyManager
    {
        /// <summary>
        /// private readonly TextBox outputTextBox; - creates a variable outputTextBox of type TextBox for displaying information.
        /// IUnitFactory unitFactory - creates a variable unitFactory of type IUnitFactory for creating units.
        /// Dictionary<string, List<Warrior>> armies - creates a dictionary armies, where the key is a string (the name of the side), and the value is a list of objects of type Warrior.
        /// Identifier string: "F:ClashGame.ArmyManager.outputTextBox".
        /// Identifier string: "F:ClashGame.ArmyManager.unitFactory".
        /// Identifier string: "F:ClashGame.ArmyManager.armies".
        /// </summary>
        private readonly TextBox outputTextBox;
        private readonly IUnitFactory unitFactory;
        private readonly Dictionary<string, List<Warrior>> armies;


        /// <summary>
        /// public ArmyManager(TextBox outputTextBox, IUnitFactory unitFactory) - constructor of the ArmyManager class.
        /// Initializes an instance of the ArmyManager class with the specified parameters.
        /// Identifier string: "M:ClashGame.ArmyManager.ArmyManager(System.Windows.Controls.TextBox,ClashGame.IUnitFactory)".
        /// </summary>
        /// <param name="outputTextBox">TextBox for displaying information.</param>
        /// <param name="unitFactory">IUnitFactory for creating units.</param>
        public ArmyManager(TextBox outputTextBox, IUnitFactory unitFactory)
        {
            this.outputTextBox = outputTextBox;
            this.unitFactory = unitFactory;
            armies = new Dictionary<string, List<Warrior>>();
        }


        /// <summary>
        /// public List<Warrior> CreateArmy(List<Warrior> warriorList, string side) - method for creating an army.
        /// Creates an army of various units for the specified side, randomly selecting units until the maximum army cost is reached.
        /// Identifier string: "M:ClashGame.ArmyManager.CreateArmy(System.Collections.Generic.List{ClashGame.Warrior},System.String)".
        /// </summary>
        /// <param name="warriorList">The list to which the created units are added.</param>
        /// <param name="side">The side for which the army is created.</param>
        /// <returns>The list of created units for the specified side.</returns>
        public List<Warrior> CreateArmy(List<Warrior> warriorList, string side)
        {
            Random random = new Random();
            int maxArmyCost = 100;
            int currentArmyCostSum = 0;
            
            while (currentArmyCostSum < maxArmyCost)
            {
                if (random.Next(0, 5) == 0 && currentArmyCostSum + unitFactory.CreateWizard(side).Cost <= maxArmyCost)
                {
                    warriorList.Add(unitFactory.CreateWizard(side));
                    currentArmyCostSum += unitFactory.CreateWizard(side).Cost;
                }
                if (random.Next(0, 4) == 0 && currentArmyCostSum + unitFactory.CreateArcher(side).Cost <= maxArmyCost)
                {
                    warriorList.Add(unitFactory.CreateArcher(side));
                    currentArmyCostSum += unitFactory.CreateArcher(side).Cost;
                }
                else if (random.Next(0, 3) == 0 && currentArmyCostSum + unitFactory.CreateHealer(side).Cost <= maxArmyCost)
                {
                    warriorList.Add(unitFactory.CreateHealer(side));
                    currentArmyCostSum += unitFactory.CreateHealer(side).Cost;
                }
                else if (random.Next(0, 2) == 0 && currentArmyCostSum + unitFactory.CreateLightWarrior(side).Cost <= maxArmyCost)
                {
                    warriorList.Add(unitFactory.CreateLightWarrior(side));
                    currentArmyCostSum += unitFactory.CreateLightWarrior(side).Cost;
                }
                else if (currentArmyCostSum + unitFactory.CreateHeavyWarrior(side).Cost <= maxArmyCost)
                {
                    warriorList.Add(unitFactory.CreateHeavyWarrior(side));
                    currentArmyCostSum += unitFactory.CreateHeavyWarrior(side).Cost;
                }
                else break;

                outputTextBox.AppendText(currentArmyCostSum.ToString() + Environment.NewLine); 
            }

            for (int i = 0; i < warriorList.Count; i++)
                outputTextBox.AppendText(warriorList[i].ToString() + Environment.NewLine); 

            warriorList.Add(unitFactory.CreateGulyayGorod(side));

            armies[side] = warriorList;
            return warriorList;
        }

        /// <summary>
        /// public List<Warrior> GetArmyByColor(string side) - method for retrieving the list of warriors in the army by its color.
        /// Returns the list of warriors for the specified side if such an army exists; otherwise, returns an empty list.
        /// Identifier string: "M:ClashGame.ArmyManager.GetArmyByColor(System.String)".
        /// </summary>
        /// <param name="side">The side for which to retrieve the list of warriors.</param>
        /// <returns>The list of warriors for the specified side or an empty list if the army is not found.</returns>
        public List<Warrior> GetArmyByColor(string side) => armies.ContainsKey(side) ? armies[side] : new List<Warrior>();
    }
}
