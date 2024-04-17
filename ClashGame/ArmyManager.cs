using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClashGame
{
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
}
