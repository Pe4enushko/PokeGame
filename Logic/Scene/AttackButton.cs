using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace Poke.Logic.Scene
{
    public partial class AttackButton : Button
    {
        public override void _Ready()
        {
        }
        public override void _Pressed()
        {
            int attackNumber = (int)GetMeta("AtkNum");
            Arena sender = (Arena)GetMeta("sender");
            sender.PerformRound(attackNumber);
        }
    }
}