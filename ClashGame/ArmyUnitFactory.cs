using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashGame
{
    //Реализация интерфейса (абстрактной фабрики)
    class ArmyUnitFactory : IUnitFactory
    {
        public Warrior CreateLightWarrior(string side)
        {
            return new LightWarrior(side);
        }


        public Warrior CreateHeavyWarrior(string side)
        {
            return new HeavyWarrior(side);
        }

        public Warrior CreateArcher(string side)
        {
            return new Archer(side);
        }

        public Warrior CreateHealer(string side)
        {
            return new Healer(side);
        }
        public Warrior CreateWizard(string side)
        {
            return new Wizard(side);
        }
    }
}
