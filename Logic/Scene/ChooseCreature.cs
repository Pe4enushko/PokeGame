using Godot;
using Poke.Logic;
using Poke.Logic.DB;
using Poke.Logic.Player;
using Poke.Logic.Scene;
using System;

public class ChooseCreature : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var scroller = GetNode<ScrollContainer>("Cards");
        VBoxContainer grid = scroller.GetNode<VBoxContainer>("VBox");
        grid.Alignment = BoxContainer.AlignMode.Begin;
        var h = 0;
        foreach (Creature item in DBWorker.GetAllCreaturesWithoutAttacks())
        {
            TextureButton btn = new TextureButton();

            var textr = new AtlasTexture();
            var texture = new ImageTexture();
            texture.Load("res://Images/Pokes/" + item.ImageFileTitle);

            textr.Atlas = texture;
            textr.Region = new Rect2(0,0,600,570);
            btn.TextureNormal = textr;

            btn.Expand = true;
            btn.StretchMode = TextureButton.StretchModeEnum.KeepAspectCentered;
            btn.SizeFlagsVertical = (int)TextureButton.SizeFlags.ExpandFill;
            btn.MouseDefaultCursorShape = Control.CursorShape.PointingHand;

            // Заранее готовим параметры для передачи на кнопку
            var arr = new Godot.Collections.Array();
            arr.Add(item.id);

            btn.Connect("pressed",this, nameof(btnPressed),arr);
            grid.AddChild(btn);
            h += 300;
        }
        GD.Print(h);
        grid.RectMinSize = new Vector2(grid.RectSize.x,h);
    }
    void btnPressed(int creatureId)
    {
        var creature = DBWorker.GetCreature(creatureId);
        GD.Print("pressed " + creature.Name);
        PlayerSingleton.GetPlayer().SetCreature(creature);
        GetTree().ChangeScene("res://Scenes/Arena.tscn");
        
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
