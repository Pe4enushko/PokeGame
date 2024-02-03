using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poke.Logic
{
    public partial class Buff
    {
        public int Value;
        public string BuffStatName;
        public int Attack_Id;
        public Targets Target;

        public Buff(int value, string buffStatName, int attack_Id, Targets target)
        {
            Value = value;
            BuffStatName = buffStatName;
            Attack_Id = attack_Id;
            Target = target;
        }
    }
}