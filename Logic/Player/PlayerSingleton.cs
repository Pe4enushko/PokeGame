using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Poke.Logic;

namespace Poke.Logic.Player
{
    public class PlayerSingleton
    {
        public string Name;
        static PlayerSingleton singleton;
        static object locker = new object();
        public Creature PlayerCreature;
        public static PlayerSingleton GetPlayer()
        {
            lock (locker)
            {
                if (singleton == null)
                {
                    singleton = new PlayerSingleton();
                }
                return singleton;
            }
        }

        public void SetCreature(Creature creature)
        {
            PlayerCreature = creature;
        }

        public void AddAttack(Attack attack)
        {
            PlayerCreature.Attacks.Add(attack);
        }

        public void InflictBuff(Buff buff)
        {
            PlayerCreature.Buff(buff);
        }

        public void RemoveBuff(Buff buff)
        {
            PlayerCreature.RemoveBuff(buff);
        }
    }
}