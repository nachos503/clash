using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashGame
{
    /// <summary>
    /// class ArmyUnitFactory - класс для создания различных типов юнитов.Реализация интерфейса (абстрактной фабрики)
    /// Реализует интерфейс IUnitFactory.
    /// Строка идентификатора "T:ClashGame.ArmyUnitFactory".
    /// </summary>
    class ArmyUnitFactory : IUnitFactory
    {
        /// <summary>
        /// public Warrior CreateLightWarrior(string side) - метод для создания лёгкого воина.
        /// Создаёт и возвращает экземпляр класса LightWarrior для заданной стороны.
        /// Строка идентификатора "M:ClashGame.ArmyUnitFactory.CreateLightWarrior(System.String)".
        /// </summary>
        /// <param name="side">Сторона, для которой создаётся юнит.</param>
        /// <returns>Экземпляр класса LightWarrior.</returns>
        public Warrior CreateLightWarrior(string side)
        {
            return new LightWarrior(side);
        }

        /// <summary>
        /// public Warrior CreateHeavyWarrior(string side) - метод для создания тяжёлого воина.
        /// Создаёт и возвращает экземпляр класса HeavyWarrior для заданной стороны.
        /// Строка идентификатора "M:ClashGame.ArmyUnitFactory.CreateHeavyWarrior(System.String)".
        /// </summary>
        /// <param name="side">Сторона, для которой создаётся юнит.</param>
        /// <returns>Экземпляр класса HeavyWarrior.</returns>
        public Warrior CreateHeavyWarrior(string side)
        {
            return new HeavyWarrior(side);
        }

        /// <summary>
        /// public Warrior CreateArcher(string side) - метод для создания лучника.
        /// Создаёт и возвращает экземпляр класса Archer для заданной стороны.
        /// Строка идентификатора "M:ClashGame.ArmyUnitFactory.CreateArcher(System.String)".
        /// </summary>
        /// <param name="side">Сторона, для которой создаётся юнит.</param>
        /// <returns>Экземпляр класса Archer.</returns>
        public Warrior CreateArcher(string side)
        {
            return new Archer(side);
        }

        /// <summary>
        /// public Warrior CreateHealer(string side) - метод для создания лекаря.
        /// Создаёт и возвращает экземпляр класса Healer для заданной стороны.
        /// Строка идентификатора "M:ClashGame.ArmyUnitFactory.CreateHealer(System.String)".
        /// </summary>
        /// <param name="side">Сторона, для которой создаётся юнит.</param>
        /// <returns>Экземпляр класса Healer.</returns>
        public Warrior CreateHealer(string side)
        {
            return new Healer(side);
        }

        /// <summary>
        /// public Warrior CreateWizard(string side) - метод для создания мага.
        /// Создаёт и возвращает экземпляр класса Wizard для заданной стороны.
        /// Строка идентификатора "M:ClashGame.ArmyUnitFactory.CreateWizard(System.String)".
        /// </summary>
        /// <param name="side">Сторона, для которой создаётся юнит.</param>
        /// <returns>Экземпляр класса Wizard.</returns>
        public Warrior CreateWizard(string side)
        {
            return new Wizard(side);
        }

        /// <summary>
        /// public Warrior CreateGulyayGorod(string side) - метод для создания юнита "Гуляй-город".
        /// Создаёт и возвращает экземпляр класса GulyayGorod для заданной стороны.
        /// Строка идентификатора "M:ClashGame.ArmyUnitFactory.CreateGulyayGorod(System.String)".
        /// </summary>
        /// <param name="side">Сторона, для которой создаётся юнит.</param>
        /// <returns>Экземпляр класса GulyayGorod.</returns>
        public Warrior CreateGulyayGorod(string side)
        {
            return new GulyayGorod(side);
        }
    }
}
