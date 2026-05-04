using Godot;
using EconSim;

public partial class Root2dScene : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Simulation simulation = new Simulation(this);
		
		GD.Print("for my sanity");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
