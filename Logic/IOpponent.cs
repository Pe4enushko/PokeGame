using System;

namespace Poke.Logic
{
    public interface IOpponent
    {
        event EventHandler Died;
        void TakeAttack(Attack atk,Creature opponent, out float dmg);

    }
}