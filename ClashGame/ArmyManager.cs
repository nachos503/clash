using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace ClashGame
{
    sealed class ArmyManager
    {
        /// <summary>
        /// private readonly TextBox outputTextBox; - создаётся переменная outputTextBox типа TextBox для вывода информации.
        /// IUnitFactory unitFactory - создаётся переменная unitFactory типа IUnitFactory для создания юнитов.
        /// Dictionary<string, List<Warrior>> armies - создаётся словарь armies, где ключом является строка (название стороны), а значением - список объектов типа Warrior.
        /// Строка идентификатора "F:ClashGame.ArmyManager.outputTextBox".
        /// Строка идентификатора "F:ClashGame.ArmyManager.unitFactory".
        /// Строка идентификатора "F:ClashGame.ArmyManager.armies".
        /// </summary>
        private readonly TextBox outputTextBox;
        private readonly IUnitFactory unitFactory;
        private readonly Dictionary<string, List<Warrior>> armies;


        /// <summary>
        /// public ArmyManager(TextBox outputTextBox, IUnitFactory unitFactory) - конструктор класса ArmyManager.
        /// Инициализирует экземпляр класса ArmyManager с заданными параметрами.
        /// Строка идентификатора "M:ClashGame.ArmyManager.ArmyManager(System.Windows.Controls.TextBox,ClashGame.IUnitFactory)".
        /// </summary>
        /// <param name="outputTextBox">TextBox для вывода информации.</param>
        /// <param name="unitFactory">IUnitFactory для создания юнитов.</param>
        public ArmyManager(TextBox outputTextBox, IUnitFactory unitFactory)
        {
            this.outputTextBox = outputTextBox;
            this.unitFactory = unitFactory;
            armies = new Dictionary<string, List<Warrior>>();
        }


        /// <summary>
        /// public List<Warrior> CreateArmy(List<Warrior> warriorList, string side) - метод для создания армии.
        /// Создаёт армию из различных юнитов для заданной стороны, случайным образом выбирая юниты, пока не достигнет максимальной стоимости армии.
        /// Строка идентификатора "M:ClashGame.ArmyManager.CreateArmy(System.Collections.Generic.List{ClashGame.Warrior},System.String)".
        /// </summary>
        /// <param name="warriorList">Список, в который добавляются созданные юниты.</param>
        /// <param name="side">Сторона, для которой создаётся армия.</param>
        /// <returns>Список созданных юнитов для заданной стороны.</returns>
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

                outputTextBox.AppendText(currentArmyCostSum.ToString() + Environment.NewLine); // Добавление информации в TextBox
            }

            for (int i = 0; i < warriorList.Count; i++)
                outputTextBox.AppendText(warriorList[i].ToString() + Environment.NewLine); // Добавление информации в TextBox

            //добавление в конце списка стену
            warriorList.Add(unitFactory.CreateGulyayGorod(side));

            armies[side] = warriorList;
            return warriorList;
        }

        /// <summary>
        /// public List<Warrior> GetArmyByColor(string side) - метод для получения списка юнитов армии по её цвету.
        /// Возвращает список юнитов для заданной стороны, если такая армия существует; в противном случае возвращает пустой список.
        /// Строка идентификатора "M:ClashGame.ArmyManager.GetArmyByColor(System.String)".
        /// </summary>
        /// <param name="side">Сторона, для которой нужно получить список юнитов.</param>
        /// <returns>Список юнитов для заданной стороны или пустой список, если армия не найдена.</returns>
        public List<Warrior> GetArmyByColor(string side) => armies.ContainsKey(side) ? armies[side] : new List<Warrior>();
    }
}
