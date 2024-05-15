using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace ClashGame
{
    sealed class ArmyManager
    {
        private readonly TextBox outputTextBox;
        private readonly IUnitFactory unitFactory;
        private readonly Dictionary<string, List<Warrior>> armies;

        public ArmyManager(TextBox outputTextBox, IUnitFactory unitFactory)
        {
            this.outputTextBox = outputTextBox;
            this.unitFactory = unitFactory;
            armies = new Dictionary<string, List<Warrior>>();
        }

        public List<Warrior> CreateArmy(List<Warrior> warriorList, string side)
        {
            Random rand = new Random();
            int maxCost = 100;
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

            warriorList.Add(unitFactory.CreateGulyayGorod(side));

            for (int i = 0; i < warriorList.Count; i++)
            {
                if (warriorList[i] is HeavyWarrior)
                {
                    if ((i > 0 && warriorList[i - 1] is LightWarrior) || (i < warriorList.Count - 1 && warriorList[i + 1] is LightWarrior))
                    {
                        ImprovedHeavyWarrior improvedHeavyWarrior = new ImprovedHeavyWarrior((HeavyWarrior)warriorList[i]);
                        if (improvedHeavyWarrior.ShouldCancelUpgrade(warriorList))
                        {
                            warriorList[i] = improvedHeavyWarrior.GetBaseHeavyWarrior();
                        }
                        else
                        {
                            warriorList[i] = improvedHeavyWarrior;
                        }
                    }
                }
                outputTextBox.AppendText(warriorList[i].ToString() + Environment.NewLine); // Добавление информации в TextBox
            }

            armies[side] = warriorList;
            return warriorList;
        }

        public List<Warrior> GetArmyByColor(string side)
        {
            return armies.ContainsKey(side) ? armies[side] : new List<Warrior>();
        }
    }
}
