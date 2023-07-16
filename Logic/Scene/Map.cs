using Godot;
using System;
using Poke.Logic;

public class Map : Control
{
	public Tile[] Tiles;
	Tile CurrentTile;
	public Map(Tile[] tiles)
	{
		this.Tiles = tiles;
	}
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}
	void VisitTile(Tile tile)
	{
		
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
