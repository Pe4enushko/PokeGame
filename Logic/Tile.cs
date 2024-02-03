using System;
using Godot;
using Poke.Logic.Player;

namespace Poke.Logic
{
    public partial class Tile : ColorRect
    {
        public event EventHandler onVisit;
        public bool Visited = false;
        public EventType EventType;
        /// Every map has several columns to choose
        public int Column;
        public Guid Guid;
        Buff[] Buffs; 
        public Tile(int column,
                    EventType eventType,
                    Buff[] buffs = null)
        {
            this.Column = column;
            this.EventType = eventType;
            this.Buffs = buffs;
        }
        public void Enter()
        {
            Visited = true;
            onVisit?.Invoke(this,null);
        }
        public void Leave()
        {

        }

        void InflictBuffs(Buff[] buffs)
        {
            var player = PlayerSingleton.GetPlayer();
            foreach (var item in buffs)
            {
                player.InflictBuff(item);
            }
        }
        
        void RemoveBuffs(Buff[] buffs)
        {
            var player = PlayerSingleton.GetPlayer();
            foreach (var item in buffs)
            {
                player.RemoveBuff(item);
            }
        }

        
    }
}