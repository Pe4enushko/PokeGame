using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poke.Logic
{
    public class Attack
    {
        public Attack(string title, float power, int accuracy, float penetration, int charges, Buff[] Buffs = null) 
        {
            this.Title = title;
            this.Power = power;
            this.Accuracy = accuracy;
            this.ArmorPenetration = penetration;
            this.chargesLeft = charges;
            this.Buffs = Buffs;
        }
        public string Title {get;set;}
        public float Power {get;set;}
        public float ArmorPenetration {get;set;}
        public int Accuracy {get;set;}
        public int chargesLeft {get;set;}
        /// Can be really null
        public Buff[] Buffs {get;set;}
    }
}