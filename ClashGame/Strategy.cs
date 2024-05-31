using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClashGame
{
    /// <summary>
    /// Класс DefaultStratagy, представляющий стандартную стратегию битвы.
    /// Строка идентификатора "T:ClashGame.DefaultStratagy".
    /// </summary>
    public class DefaultStratagy : IBattleStrategy
    {
        /// <summary>
        /// Выполняет битву между атакующими и защищающимися воинами.
        /// Строка идентификатора "M:ClashGame.DefaultStratagy.ExecuteBattle(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Forms.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе битвы.</param>
        public void ExecuteBattle(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {

        }

        /// <summary>
        /// Получает врага для атаки лучника.
        /// Строка идентификатора "M:ClashGame.DefaultStratagy.GetEnemyForArcher(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Int32,ClashGame.Archer)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <param name="archerIndex">Индекс лучника.</param>
        /// <param name="archer">Лучник.</param>
        /// <returns>Враг для атаки.</returns>
        public Warrior GetEnemyForArcher(List<Warrior> attackers, List<Warrior> defenders, int archerIndex, Archer archer)
        {
            return null;
        }

        /// <summary>
        /// Получает ближайшего легкого воина.
        /// Строка идентификатора "M:ClashGame.DefaultStratagy.GetNearestLightWarrior(System.Collections.Generic.List{ClashGame.Warrior},System.Int32)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="wizardIndex">Индекс мага.</param>
        /// <returns>Ближайший легкий воин.</returns>
        public Warrior GetNearestLightWarrior(List<Warrior> attackers, int wizardIndex)
        {
            return null;
        }

        /// <summary>
        /// Получает воина для лечения.
        /// Строка идентификатора "M:ClashGame.DefaultStratagy.GetWarriorForHeal(System.Collections.Generic.List{ClashGame.Warrior},System.Int32,ClashGame.Healer)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="healerIndex">Индекс лекаря.</param>
        /// <param name="healer">Лекарь.</param>
        /// <returns>Воин, которого следует лечить.</returns>
        public Warrior GetWarriorForHeal(List<Warrior> attackers, int healerIndex, Healer healer)
        {
            return null;
        }

        /// <summary>
        /// Проверяет, является ли атакующий воин на передовой линии.
        /// Строка идентификатора "M:ClashGame.DefaultStratagy.IsFrontLine(System.Int32,System.Collections.Generic.List{ClashGame.Warrior})".
        /// </summary>
        /// <param name="attackerIndex">Индекс атакующего воина.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <returns>True, если воин находится на передовой линии, иначе false.</returns>
        public bool IsFrontLine(int attackerIndex, List<Warrior> defenders)
        {
            return false;
        }
    }

    /// <summary>
    /// Класс TwoRowStrategy, представляющий стратегию боя с двумя рядами.
    /// Строка идентификатора "T:ClashGame.TwoRowStrategy".
    /// </summary>
    public class TwoRowStrategy : IBattleStrategy
    {
        /// <summary>
        /// Прокси-объект BattleManager.
        /// Строка идентификатора "F:ClashGame.TwoRowStrategy._battleManagerProxy".
        /// </summary>
        BattleManagerProxy _battleManagerProxy;

        /// <summary>
        /// Конструктор для класса TwoRowStrategy.
        /// Строка идентификатора "M:ClashGame.TwoRowStrategy.#ctor(ClashGame.BattleManagerProxy)".
        /// </summary>
        /// <param name="battleManagerProxy">Прокси-объект BattleManager.</param>
        public TwoRowStrategy(BattleManagerProxy battleManagerProxy)
        {
            _battleManagerProxy = battleManagerProxy;
        }

        /// <summary>
        /// Выполняет битву между атакующими и защищающимися воинами.
        /// Строка идентификатора "M:ClashGame.TwoRowStrategy.ExecuteBattle(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Forms.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе битвы.</param>
        public void ExecuteBattle(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            int rows = Math.Min(2, Math.Min(attackers.Count, defenders.Count));
            for (int i = 0; i < rows; i++)
            {
                if (defenders.Count > i)
                {
                    if (attackers[i] is not GulyayGorod && defenders[i] is not GulyayGorod)
                        _battleManagerProxy.Attack(attackers[i], defenders[i], outputTextBox);
                    _battleManagerProxy.IsDead(defenders[i], defenders);
                }
            }
        }

        /// <summary>
        /// Получает врага для атаки лучника.
        /// Строка идентификатора "M:ClashGame.TwoRowStrategy.GetEnemyForArcher(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Int32,ClashGame.Archer)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <param name="archerIndex">Индекс лучника.</param>
        /// <param name="archer">Лучник.</param>
        /// <returns>Враг для атаки.</returns>
        public Warrior GetEnemyForArcher(List<Warrior> attackers, List<Warrior> defenders, int archerIndex, Archer archer)
        {
            var range = archer.Range();
            //идем с позиции лучника до начала его армии
            for (int i = archerIndex; i > 1; i -= 2)
                range--;
            //идем с начала армии противника пока не кончится его дальность
            
            if (range > 0)
                for (int i = 0; i < defenders.Count; i += 2)
                {
                    range--;
                    if (range == 0)
                    {
                        //проверка в каком ряду стоит лучник (стреляет только вперед)
                        if (archerIndex % 2 == 0) return defenders[i];
                        else if (defenders.Count > i+1)  return defenders[i + 1];
                    }
                }

            return null;
        }

        /// <summary>
        /// Получает ближайшего легкого воина.
        /// Строка идентификатора "M:ClashGame.TwoRowStrategy.GetNearestLightWarrior(System.Collections.Generic.List{ClashGame.Warrior},System.Int32)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="attackerIndex">Индекс атакующего воина.</param>
        /// <returns>Ближайший легкий воин.</returns>а
        public Warrior GetNearestLightWarrior (List<Warrior> attackers, int attackerIndex)
        {
            bool isUsed = false;
            Warrior nearestLightWarrior = null;
            //смотрим вверх вних направо налево
            if (attackerIndex != attackers.Count() - 1)
                if (attackerIndex % 2 == 0 && attackers[attackerIndex + 1] is LightWarrior)
                {
                    nearestLightWarrior = attackers[attackerIndex + 1];
                    isUsed = true;
                }
            if (attackerIndex < attackers.Count() - 2)
                if (attackers[attackerIndex + 2] is  LightWarrior && nearestLightWarrior == null && isUsed == false)
                {
                    nearestLightWarrior = attackers[attackerIndex + 2];
                    isUsed = true;
                }
            if (attackerIndex >= 1)
                if (attackerIndex % 2 != 0 && attackers[attackerIndex - 1] is LightWarrior && nearestLightWarrior == null && isUsed == false)
                {
                    nearestLightWarrior = attackers[attackerIndex - 1];
                    isUsed = true;
                }
            if (attackerIndex >= 2)
                if (attackers[attackerIndex - 2] is LightWarrior && nearestLightWarrior == null && isUsed == false)
                {
                    nearestLightWarrior = attackers[attackerIndex - 2];
                    isUsed = true;
                }

            return nearestLightWarrior;
        }

        /// <summary>
        /// Получает воина для лечения.
        /// Строка идентификатора "M:ClashGame.TwoRowStrategy.GetWarriorForHeal(System.Collections.Generic.List{ClashGame.Warrior},System.Int32,ClashGame.Healer)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="healerIndex">Индекс лекаря.</param>
        /// <param name="healer">Лекарь.</param>
        /// <returns>Воин, которого следует лечить.</returns>
        public Warrior GetWarriorForHeal(List<Warrior> attackers, int healerIndex, Healer healer)
        {
            Warrior warriorForHeal = null;
            var minHealth = double.MaxValue;
            //смотрим вверх вниз вправо влево
            if (healerIndex + 1 < attackers.Count())
                if (attackers[healerIndex + 1].Healthpoints < minHealth && healerIndex % 2 == 0 && attackers[healerIndex + 1] is not LightWarrior)
                {
                     minHealth = attackers[healerIndex + 1].Healthpoints;
                     warriorForHeal = attackers[healerIndex + 1];
                }
            if (healerIndex + 2 < attackers.Count())
                if (attackers[healerIndex + 2].Healthpoints < minHealth && attackers[healerIndex + 2] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex + 2].Healthpoints;
                    warriorForHeal = attackers[healerIndex + 2];
                }
            if (healerIndex >= 1)
                if (attackers[healerIndex - 1].Healthpoints < minHealth && healerIndex % 2 != 0 && attackers[healerIndex - 1] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex - 1].Healthpoints;
                    warriorForHeal = attackers[healerIndex - 1];
                }
            if (healerIndex >= 2)
                if (attackers[healerIndex - 2].Healthpoints < minHealth && attackers[healerIndex - 2] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex - 2].Healthpoints;
                    warriorForHeal = attackers[healerIndex - 2];
                }
            
            return warriorForHeal;
        }

        /// <summary>
        /// Проверяет, является ли атакующий воин на передовой линии.
        /// Строка идентификатора "M:ClashGame.TwoRowStrategy.IsFrontLine(System.Int32,System.Collections.Generic.List{ClashGame.Warrior})".
        /// </summary>
        /// <param name="attackerIndex">Индекс атакующего воина.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <returns>True, если воин находится на передовой линии, иначе false.</returns>
        public bool IsFrontLine(int attackerIndex, List<Warrior> defenders)
        {
            if (attackerIndex == 0)  return true;
            else if (defenders.Count() > 0)
            {
                if (attackerIndex == 1) return true;
                else  return false;
            }
            else return false;
        }
    }

    /// <summary>
    /// Класс ThreeRowStrategy, представляющий стратегию боя с тремя рядами.
    /// Строка идентификатора "T:ClashGame.ThreeRowStrategy".
    /// </summary>
    public class ThreeRowStrategy : IBattleStrategy
    {
        /// <summary>
        /// Прокси-объект BattleManager.
        /// Строка идентификатора "F:ClashGame.ThreeRowStrategy._battleManagerProxy".
        /// </summary>
        BattleManagerProxy _battleManagerProxy;

        /// <summary>
        /// Конструктор для класса ThreeRowStrategy.
        /// Строка идентификатора "M:ClashGame.ThreeRowStrategy.#ctor(ClashGame.BattleManagerProxy)".
        /// </summary>
        /// <param name="battleManagerProxy">Прокси-объект BattleManager.</param>
        public ThreeRowStrategy(BattleManagerProxy battleManagerProxy)
        {
            _battleManagerProxy = battleManagerProxy;
        }

        /// <summary>
        /// Выполняет битву между атакующими и защищающимися воинами.
        /// Строка идентификатора "M:ClashGame.ThreeRowStrategy.ExecuteBattle(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Forms.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе битвы.</param>
        public void ExecuteBattle(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            int rows = Math.Min(3, Math.Min(attackers.Count, defenders.Count));
            for (int i = 0; i < rows; i++)
                if (defenders.Count > i)
                {
                    if (attackers[i] is not GulyayGorod && defenders[i] is not GulyayGorod)
                        _battleManagerProxy.Attack(attackers[i], defenders[i], outputTextBox);
                    _battleManagerProxy.IsDead(defenders[i], defenders);
                }
        }

        /// <summary>
        /// Получает врага для атаки лучника.
        /// Строка идентификатора "M:ClashGame.ThreeRowStrategy.GetEnemyForArcher(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Int32,ClashGame.Archer)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <param name="archerIndex">Индекс лучника.</param>
        /// <param name="archer">Лучник.</param>
        /// <returns>Враг для атаки.</returns>
        public Warrior GetEnemyForArcher(List<Warrior> attackers, List<Warrior> defenders, int archerIndex, Archer archer)
        {
            var range = archer.Range();
            for (int i = archerIndex; i > 1; i -= 3)
                range--;

            if (range > 0)
                for (int i = 0; i < defenders.Count; i += 3)
                {
                    range--;
                    if (range == 0)
                    {
                        if (archerIndex % 3 == 0) return defenders[i];
                        else if (defenders.Count > i + 1 && archerIndex % 3 == 1) return defenders[i + 1];
                        else if (defenders.Count > i + 2) return defenders[i + 2];
                    }
                }

            return null;
        }

        /// <summary>
        /// Получает ближайшего легкого воина.
        /// Строка идентификатора "M:ClashGame.ThreeRowStrategy.GetNearestLightWarrior(System.Collections.Generic.List{ClashGame.Warrior},System.Int32)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="attackersIndex">Индекс атакующего воина.</param>
        /// <returns>Ближайший легкий воин.</returns>
        public Warrior GetNearestLightWarrior(List<Warrior> attackers, int attackersIndex)
        {
            bool isUsed = false;
            Warrior nearestLightWarrior = null;
            if (attackersIndex != attackers.Count() - 1 )
                if (attackersIndex % 2 == 0 && attackers[attackersIndex + 1] is LightWarrior && isUsed == false)
                {
                    nearestLightWarrior = attackers[attackersIndex + 1];
                    isUsed = true;
                }
            if (attackersIndex < attackers.Count() - 3)
                if (attackers[attackersIndex + 3] is LightWarrior && nearestLightWarrior == null && isUsed == false)
                {
                    nearestLightWarrior = attackers[attackersIndex + 3];
                    isUsed = true;
                }
            if (attackersIndex >= 1)
                if (attackersIndex % 3 != 0 && attackers[attackersIndex - 1] is LightWarrior && nearestLightWarrior == null && isUsed == false)
                {
                    nearestLightWarrior = attackers[attackersIndex - 1];
                    isUsed = true;
                }
            if (attackersIndex >= 3)
                if (attackers[attackersIndex - 3] is LightWarrior && nearestLightWarrior == null && isUsed == false)
                {
                    nearestLightWarrior = attackers[attackersIndex - 3];
                    isUsed = true;
                }
                
            return nearestLightWarrior;
        }

        /// <summary>
        /// Получает воина для лечения.
        /// Строка идентификатора "M:ClashGame.ThreeRowStrategy.GetWarriorForHeal(System.Collections.Generic.List{ClashGame.Warrior},System.Int32,ClashGame.Healer)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="healerIndex">Индекс лекаря.</param>
        /// <param name="healer">Лекарь.</param>
        /// <returns>Воин, которого следует лечить.</returns>
        public Warrior GetWarriorForHeal(List<Warrior> attackers, int healerIndex, Healer healer)
        {
            Warrior warriorForHeal = null;
            var minHealth = double.MaxValue;

            if (healerIndex + 1 < attackers.Count())
                if (attackers[healerIndex + 1].Healthpoints < minHealth && healerIndex % 2 == 0 && attackers[healerIndex + 1] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex + 1].Healthpoints;
                    warriorForHeal = attackers[healerIndex + 1];
                }
            if (healerIndex + 3 < attackers.Count())
                if (attackers[healerIndex + 3].Healthpoints < minHealth && attackers[healerIndex + 3] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex + 3].Healthpoints;
                    warriorForHeal = attackers[healerIndex + 3];
                }
            if (healerIndex >= 1)
                if (attackers[healerIndex - 1].Healthpoints < minHealth && healerIndex % 3 != 0 && attackers[healerIndex - 1] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex - 1].Healthpoints;
                    warriorForHeal = attackers[healerIndex - 1];
                }
            if (healerIndex >= 3)
                if (attackers[healerIndex - 3].Healthpoints < minHealth && attackers[healerIndex - 3] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex - 3].Healthpoints;
                    warriorForHeal = attackers[healerIndex - 3];
                }

            return warriorForHeal;
        }

        /// <summary>
        /// Проверяет, является ли атакующий воин на передовой линии.
        /// Строка идентификатора "M:ClashGame.ThreeRowStrategy.IsFrontLine(System.Int32,System.Collections.Generic.List{ClashGame.Warrior})".
        /// </summary>
        /// <param name="attackerIndex">Индекс атакующего воина.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <returns>True, если воин находится на передовой линии, иначе false.</returns>
        public bool IsFrontLine(int attackerIndex, List<Warrior> defenders)
        {
            if (attackerIndex == 0)  return true;
            if (defenders.Count() > 0)
                if (attackerIndex == 1) return true;
                
            if (defenders.Count() > 1)
            { 
                if (attackerIndex == 2)  return true;
                else  return false;
            }
            else return false;
        }
    }

    /// <summary>
    /// Класс WallToWallStrategy, представляющий стратегию боя "стенка на стенку".
    /// Строка идентификатора "T:ClashGame.WallToWallStrategy".
    /// </summary>
    public class WallToWallStrategy : IBattleStrategy
    {
        /// <summary>
        /// Прокси-объект BattleManager.
        /// Строка идентификатора "F:ClashGame.WallToWallStrategy._battleManagerProxy".
        /// </summary>
        BattleManagerProxy _battleManagerProxy;

        /// <summary>
        /// Конструктор для класса WallToWallStrategy.
        /// Строка идентификатора "M:ClashGame.WallToWallStrategy.#ctor(ClashGame.BattleManagerProxy)".
        /// </summary>
        /// <param name="battleManagerProxy">Прокси-объект BattleManager.</param>
        public WallToWallStrategy(BattleManagerProxy battleManagerProxy)
        {
            _battleManagerProxy = battleManagerProxy;
        }

        /// <summary>
        /// Выполняет битву между атакующими и защищающимися воинами.
        /// Строка идентификатора "M:ClashGame.WallToWallStrategy.ExecuteBattle(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Windows.Forms.TextBox)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <param name="outputTextBox">TextBox для вывода информации о ходе битвы.</param>
        public void ExecuteBattle(List<Warrior> attackers, List<Warrior> defenders, TextBox outputTextBox)
        {
            int rows = Math.Min(attackers.Count, defenders.Count);
            for (int i = 0; i < rows; i++)
            {
                if (defenders.Count > i)
                {
                        if (attackers[i] is not GulyayGorod && defenders[i] is not GulyayGorod)
                        _battleManagerProxy.Attack(attackers[i], defenders[i], outputTextBox);
                        _battleManagerProxy.IsDead(defenders[i], defenders);
                }
            }
        }

        /// <summary>
        /// Получает врага для атаки лучника.
        /// Строка идентификатора "M:ClashGame.WallToWallStrategy.GetEnemyForArcher(System.Collections.Generic.List{ClashGame.Warrior},System.Collections.Generic.List{ClashGame.Warrior},System.Int32,ClashGame.Archer)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <param name="archerIndex">Индекс лучника.</param>
        /// <param name="archer">Лучник.</param>
        /// <returns>Враг для атаки.</returns>
        public Warrior GetEnemyForArcher(List<Warrior> attackers, List<Warrior> defenders, int archerIndex, Archer archer)
        {
            if (defenders.Count() > 0)
                return defenders.Count() - 1 < archerIndex - 3 ? null : defenders[defenders.Count() - 1];
            return null;
        }

        /// <summary>
        /// Получает ближайшего легкого воина.
        /// Строка идентификатора "M:ClashGame.WallToWallStrategy.GetNearestLightWarrior(System.Collections.Generic.List{ClashGame.Warrior},System.Int32)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="wizardIndex">Индекс мага.</param>
        /// <returns>Ближайший легкий воин.</returns>
        public Warrior GetNearestLightWarrior(List<Warrior> attackers, int wizardIndex)
        {
            bool isUsed = false;
            Warrior nearestLightWarrior = null;
            if (wizardIndex != attackers.Count() - 1 && wizardIndex != attackers.Count() - 2)
                if (attackers[wizardIndex + 1] is LightWarrior && isUsed == false)
                    nearestLightWarrior = attackers[wizardIndex + 1];
                
            if (wizardIndex > 0)
                if (attackers[wizardIndex - 1] is LightWarrior && nearestLightWarrior == null && isUsed == false)
                    nearestLightWarrior = attackers[wizardIndex - 1];
                
            return nearestLightWarrior;
        }

        /// <summary>
        /// Получает воина для лечения.
        /// Строка идентификатора "M:ClashGame.WallToWallStrategy.GetWarriorForHeal(System.Collections.Generic.List{ClashGame.Warrior},System.Int32,ClashGame.Healer)".
        /// </summary>
        /// <param name="attackers">Список атакующих воинов.</param>
        /// <param name="healerIndex">Индекс лекаря.</param>
        /// <param name="healer">Лекарь.</param>
        /// <returns>Воин, которого следует лечить.</returns>
        public Warrior GetWarriorForHeal(List<Warrior> attackers, int healerIndex, Healer healer)
        {
            Warrior warriorForHeal = null;
            var minHealth = double.MaxValue;
            if (healerIndex != attackers.Count() - 1 && healerIndex != attackers.Count() - 2)
                if (attackers[healerIndex + 1].Healthpoints < minHealth && attackers[healerIndex + 1] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex + 1].Healthpoints;
                    warriorForHeal = attackers[healerIndex + 1];
                }
            if (healerIndex >= 1)
                if (attackers[healerIndex - 1].Healthpoints < minHealth && attackers[healerIndex - 1] is not LightWarrior)
                {
                    minHealth = attackers[healerIndex - 1].Healthpoints;
                    warriorForHeal = attackers[healerIndex - 1];
                }
          
            return warriorForHeal;
        }

        /// <summary>
        /// Проверяет, является ли атакующий воин на передовой линии.
        /// Строка идентификатора "M:ClashGame.WallToWallStrategy.IsFrontLine(System.Int32,System.Collections.Generic.List{ClashGame.Warrior})".
        /// </summary>
        /// <param name="attackerIndex">Индекс атакующего воина.</param>
        /// <param name="defenders">Список защищающихся воинов.</param>
        /// <returns>True, если воин находится на передовой линии, иначе false.</returns>
        public bool IsFrontLine(int attackerIndex, List<Warrior> defenders)
        {
            if (defenders.Count() - 1 > attackerIndex) return true;
            else return false;
        }
    }
}
