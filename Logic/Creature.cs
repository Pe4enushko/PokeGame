using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Poke.Logic
{
    public class Creature : IOpponent
    {
        #region Stats
        int _health;
        int _atk;
        int _def;
        int _spAtk;
        int _spDef;
        int _speed;
        int _evasion;
        public string Name {get;}
        public int id {get;}
        public int Health {get => _health + GetBuffedStat();
        set 
        {
            if((value + GetBuffedStat()) <= 0)
            {
                Died.Invoke(this, new EventArgs());
            }
            else
            {
                _health = value;
            }
        }}
        public int ATK {get => _atk + GetBuffedStat();
        set
        {
            _atk = value;
        }}
        public int DEF {get => _def + GetBuffedStat();
        set
        {
            _def = value;
        }}
        public int SpAtk {get => _spAtk + GetBuffedStat();
        set
        {
            _spAtk = value;
        }}
        public int SpDef {get => _spDef + GetBuffedStat();
        set
        {
            _spDef = value;
        }}
        public int Speed {get => _speed + GetBuffedStat();
        set
        {
            _speed = value;
        }}
        public int Evasion {get => _evasion + GetBuffedStat();
        set
        {
            _evasion = value;
        }}
        // Additional accuracy
        public int AdditionalAccuracy {get =>  GetBuffedStat();}
        #endregion
        public event EventHandler Died;
        public bool isDead = false;
        public string ImageFileTitle;
        public List<Attack> Attacks;
        List<Buff> Buffs;
        public Creature(int id,
                        int Health,
                        int ATK,
                        int DEF,
                        int SpAtk,
                        int SpDef,
                        int Speed,
                        int Evasion,
                        List<Attack> Attacks,
                        string ImageFileTitle,
                        string Name)
        {
            this.Name = Name;
            this.id = id;
            this._health = Health;
            this._atk = ATK;
            this._def = DEF;
            this._spAtk = SpAtk;
            this._spDef = SpDef;
            this._speed = Speed;
            this._evasion = Evasion;
            this.Attacks = Attacks;
            this.ImageFileTitle = ImageFileTitle;
            Died += Die;
            Buffs = new List<Buff>();
        }

        int GetBuffedStat([CallerMemberName] string StatName = null)
        {
            int addition = 0;
            if(!string.IsNullOrEmpty(StatName))
            {
                foreach (var item in Buffs?.FindAll(a => a.BuffStatName == StatName))
                {
                    addition += item.Value;
                }
            }
            return addition;
        }
        public void Buff(Attack atk) {
            if(atk.Buffs != null)
                Buffs.AddRange(atk.Buffs);                
        }
        public void Buff(Buff buff) {
            Buffs.Add(buff);                
        }
        public void RemoveBuff(Buff buff)
        {
            Buffs.Remove(buff);
        }

        /// <return>Did attack missed</return>
        public void TakeAttack(Attack atk,Creature opponent, out float dmg) {
            var koef = (atk.ArmorPenetration + opponent.ATK)/(DEF);
            dmg = (float)Math.Truncate(atk.Power * koef/4);
            Health -= (int)dmg;
            // recieving buff/debuff of attack
            if(atk.Buffs.Length > 0 && atk.Buffs[0].Target == Targets.opponent)
                Buff(atk);
        }
        private void Die(object sender, EventArgs e)
        {
            this.isDead = true;
        }
    }
}