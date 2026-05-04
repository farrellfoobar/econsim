using Godot;
using System;

public partial class Root2dScene : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TileSet tileSet = (TileSet) GD.Load("res://res/tiles/tileset.tres");
		TileMapLayer tileMapLayer = new TileMapLayer();
		this.AddChild(tileMapLayer);
		tileMapLayer.SetTileSet(tileSet);
		
		tileMapLayer.SetCell(new Vector2I(0,0), 1, new Vector2I(0,0), 0);
		tileMapLayer.SetCell(new Vector2I(1,0), 1, new Vector2I(0,0), 0);
		tileMapLayer.SetCell(new Vector2I(2,0), 1, new Vector2I(0,0), 0);
		tileMapLayer.SetCell(new Vector2I(3,0), 1, new Vector2I(0,0), 0);
		tileMapLayer.SetCell(new Vector2I(4,0), 1, new Vector2I(0,0), 0);
		
		GD.Print("for my sanity");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
