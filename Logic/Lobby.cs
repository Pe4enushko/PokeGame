using Godot;
using Poke.Logic.Player;
using System;

public partial class Lobby : Node2D
{
	int defaultPort = 8040;
	string playerName = "";
	bool isOffline = true;
	TextEdit Txt_ID;
	Label Lb_Text;
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Txt_ID = GetNode<ColorRect>("Container").GetNode<TextEdit>("Txt_ID");
		Lb_Text = GetNode<ColorRect>("Container").GetNode<Label>("Lb_Text");
	}
	public void _on_Txt_ID_text_changed()
	{
		playerName = Txt_ID.Text;
	}
	public void _on_Btn_Host_pressed()
	{
		PlayerSingleton.GetPlayer().Name = playerName;
		GetTree().ChangeSceneToFile("res://Scenes/ChooseCreature.tscn");
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
