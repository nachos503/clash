using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashGame
{
    interface IHealable
    {
        void Heal(Warrior target);
    }

    interface IClonableUnit
    {
        Warrior Clone();
    }

    interface IRangedUnit
    {
        int Range();
        double RangedDamage(int index);
        double RangedAttack(List<Warrior> enemies, int targetIndex, int attackerIndex);
    }

    interface IUnitFactory
    {
        Warrior CreateLightWarrior(string side);
        Warrior CreateHeavyWarrior(string side);
        Warrior CreateArcher(string side);
        Warrior CreateHealer(string side);
        Warrior CreateWizard(string side);
        Warrior CreateGulyayGorod(string side);
    }
}