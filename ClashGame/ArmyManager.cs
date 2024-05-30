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
        //конструктор
        public ArmyManager(TextBox outputTextBox, IUnitFactory unitFactory)
        {
            this.outputTextBox = outputTextBox;
            this.unitFactory = unitFactory;
            armies = new Dictionary<string, List<Warrior>>();
        }
        //создание армии
        public List<Warrior> CreateArmy(List<Warrior> warriorList, string side)
        {
            Random random = new Random();
            int maxArmyCost = 100;
            int currentArmyCostSum = 0;
            //пока еще есть на какую сумму набираь - рандомно выбираем воина
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

                outputTextBox.AppendText(currentArmyCostSum.ToString() + Environment.NewLine); // Добавление информации в TextBox
            }

            for (int i = 0; i < warriorList.Count; i++)
                outputTextBox.AppendText(warriorList[i].ToString() + Environment.NewLine); // Добавление информации в TextBox

            //добавление в конце списка стену
            warriorList.Add(unitFactory.CreateGulyayGorod(side));

            armies[side] = warriorList;
            return warriorList;
        }
        //получение слиста армии по ее цвету
        public List<Warrior> GetArmyByColor(string side) => armies.ContainsKey(side) ? armies[side] : new List<Warrior>();
    }
}
