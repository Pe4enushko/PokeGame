using Godot;
using System.Data.SQLite;
using System.Collections.Generic;
using static System.Environment;
using System;
using Poke.Logic.DB;
using Poke.Logic.Player;

namespace Poke.Logic.Scene
{
	public partial class Arena : Node2D
	{
		public event EventHandler PlayerLost;
		public event EventHandler PlayerWon;
		int _turn = 1;
		int turn {get => _turn; set 
		{
			_turn = value;
			ButtonsContainer.Visible = isYourTurn;
		}}
		bool isYourTurn {get => turn % 2 != 0;}
		Creature _opponent;
		Creature _player;
		Creature Player {get => PlayerSingleton.GetPlayer().PlayerCreature;
		set
		{
			PlayerSingleton.GetPlayer().SetCreature(value);
			GetNode<TextureRect>("LeftCreature").Texture = ImageTexture.CreateFromImage(Image.LoadFromFile("res://Images/Pokes/" + value.ImageFileTitle));
		}}
		Creature Opponent {get => _opponent;
		set
		{
			_opponent = value;
			GD.Print("Opponent image is at " + value.ImageFileTitle);
			GetNode<TextureRect>("RightCreature").Texture = ImageTexture.CreateFromImage(Image.LoadFromFile("res://Images/Pokes/" + value.ImageFileTitle));
		}}
		HBoxContainer ButtonsContainer {get;set;}
		Label LeftHp {get;set;}
		Label RightHp {get;set;}
		
		int _oppId;
		bool _initBySingleton = false;
		public Arena(int OpponentId)
		{
			_oppId = OpponentId;
		}
		public Arena()
		{
			_initBySingleton = true;
		}
		
		public override void _Ready()
		{
			if (_initBySingleton)
				_oppId = PlayerSingleton.GetPlayer().OpponentToFightId;
			GD.Print($"Arena init with opponent {_oppId} and player {Player.id}");
			// хз чо ето, но краши должно пофиксить
			SQLitePCL.Batteries.Init();

			LeftHp = GetNode<Label>("LeftHP");
			RightHp = GetNode<Label>("RightHP");
			GD.Print("Will it crash?");

			Load(_oppId);
			LoadButtons();

			GD.Print("Loaded all things");
			GD.Print("Opponent is here");

			Opponent.Died += (obj,e) => {
				GD.Print("the opponent dude is ded");
				RightHp.Text = "Ded";
				PlayerWon?.Invoke(this, null);
			};

			Player.Died += (obj,e) => {
				GD.Print("U are ded. Not big soup rice"); 
				LeftHp.Text = "Ded";
				PlayerLost?.Invoke(this, null);
			};

			PlayerWon += Win;
			PlayerLost += Lose;
			

			if(Player.Speed != Opponent.Speed)
				turn = Player.Speed > Opponent.Speed ? 1 : 0;
			else
				turn = new Random().Next(0,2);

			if(!isYourTurn)
				OpponentAttack();
		}
		
		
		void Win(object sender, EventArgs e)
		{
			OS.Alert("U won");
			GetTree().ChangeSceneToFile("res://Scenes/Map.tscn");
		}
		void Lose(object sender, EventArgs e)
		{
			OS.Alert("U are ded. Not big soup rice");
			GetTree().ChangeSceneToFile("res://Scenes/Map.tscn");
		}
		
		
		public void RecieveAttack(int attackNumber)
		{
			GD.Print("you recieving hit on turn №" + turn);
			GD.Print("Opponent used " + Opponent.Attacks[attackNumber].Title);
			
			if((!Player.isDead && !Opponent.isDead))
			{ 
				// Противник берёт свой бафф
				if(Opponent.Attacks[attackNumber].Buffs.Length > 0)
						if(Opponent.Attacks[attackNumber].Buffs[0].Target == Targets.self)
							Opponent.Buff(Opponent.Attacks[attackNumber]);
							
				// Мы ловим урон
				Player.TakeAttack(Opponent.Attacks[attackNumber],Opponent, out float dmg);
				GD.Print($"Player recieved {dmg} damage \n");
			}
			
			ButtonsContainer.Visible = true;
		}      
		public void FinishTurn()
		{
			turn += 1;
		}
		public bool PerformRound(int attackNumber)
		{
			GD.Print("you performing hit on turn №" + turn);
			
			if(!Player.isDead && !Opponent.isDead)
			{
				var plrAtk = Player.Attacks;
				var rnd = new Random().Next(0,101);
				
				bool didRecieve = (
					// Если попал
					rnd <= ((plrAtk[attackNumber].Accuracy + Player.AdditionalAccuracy) - Opponent.Evasion)) ||
					// Или если это бафф на себя
					(plrAtk[attackNumber].Buffs.Length > 0 ? plrAtk[attackNumber].Buffs[0].Target == Targets.self : false);
				
				GD.Print("You've used " + Player.Attacks[attackNumber].Title);
				GD.Print("Did opponent recieve that atk? " + didRecieve + " and rnd = " + rnd + " and other shit is accuracy = " + Player.Attacks[attackNumber].Accuracy + " evasion = " + Opponent.Evasion);
				
				ButtonsContainer.Visible = false;
				if(didRecieve)
				{
					// Сначала чекаем баффы на себя
					if(plrAtk[attackNumber].Buffs.Length > 0)
						if(plrAtk[attackNumber].Buffs[0].Target == Targets.self)
							Player.Buff(plrAtk[attackNumber]);
							
					// Противник берёт урон
					Opponent.TakeAttack(Player.Attacks[attackNumber],Player, out float dmg);
					
					GD.Print($"Opponent recieved {dmg} damage");
				}

				FinishTurn();
				// Противник кидает ответочку
				OpponentAttack();
				
				UpdateHealth();
				return didRecieve;
			}
			else
			{
				GD.Print("Fight ended");
				return false;
			}
		}
		bool OpponentAttack()
		{
			var oppAtk = Opponent.Attacks;
			var oppAtkNum = new System.Random().Next(0,Opponent.Attacks.Count);
			var rnd = new Random().Next(0,101);
			
			bool didRecieve = (rnd <= ((oppAtk[oppAtkNum].Accuracy + Opponent.AdditionalAccuracy) - Player.Evasion)) ||
							(oppAtk[oppAtkNum].Buffs.Length > 0 ? oppAtk[oppAtkNum].Buffs[0].Target == Targets.self : false);
			
			GD.Print("Did player recieve that atk? " + didRecieve +
			 " and rnd = " + rnd + 
			 " and other shit is accuracy = " + Opponent.Attacks[oppAtkNum].Accuracy +
			 " evasion = " + Player.Evasion + "\n");
			
			if(didRecieve)
				RecieveAttack(oppAtkNum);

			FinishTurn();

			return didRecieve;
		}
		
		
		#region Loading
		void LoadButtons()
		{
			var list = GetNode<ColorRect>("AttackContainerColor").GetChild<HBoxContainer>(0);
			list.GetChildren().Clear();
			list.AddSpacer(true);
			for (int i = 0; i < Player.Attacks?.Count; i++)
			{
				var btn = new AttackButton() {Size = new Vector2(20,50), Text=$"button №{i}"};
				btn.Name = $"Btn_Atk{i}";
				btn.Text = Player.Attacks[i].Title;
				btn.CustomMinimumSize = new Vector2(125,25);
				btn.SetMeta("AtkNum",i);
				btn.SetMeta("sender",this);
				
				list.AddChild(btn);
				list.AddSpacer(false);
				GD.Print("Added button");
			}
			ButtonsContainer = list;
		}
		/// в sql запросе нужно просто выбрать конкретные атаки типа "SELECT * FROM Creature_Attack WHERE..."
		
		void Load(int opponentId)
		{
			Player = PlayerSingleton.GetPlayer().PlayerCreature;
			LeftHp.Text = "Health: " + Player.Health.ToString();

			Opponent = DBWorker.GetCreature(opponentId);
			RightHp.Text = "Health: " + Opponent.Health.ToString();
		}
		void UpdateHealth()
		{
			if(!Player.isDead && !Opponent.isDead)
			{
				LeftHp.Text = "Health: " + Player.Health.ToString();
				RightHp.Text = "Health: " + Opponent.Health.ToString();
			}
		}
		#endregion
	}
}
