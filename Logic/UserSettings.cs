using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poke.Logic
{
    public partial class UserSettings
    {
        //squad = IDs of creatures
        int[] _squad {get;}
        public Creature[] Basement {get;set;}
        public Creature[] Squad {get 
        {
            Creature[] sq = new Creature[_squad.Length];
            for (int i = 0; i < _squad.Length; i++)
            {
                sq[i] = Basement?.First(a => a.id == _squad[i]);
            }
            return sq;
        }}
    }
}