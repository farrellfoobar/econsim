using EconSim.logic;
using EconSim.render;
using Godot;

namespace EconSim;

public partial class Root2dScene : Node2D
{
	private Simulation simulation;
	private SimulationRenderer renderer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		simulation = new Simulation();
		renderer = new SimulationRenderer(this, simulation);
		
		GD.Print("for my sanity");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		simulation.doTurn();
		renderer.renderSimulation();
		OS.DelayMsec(1000);
	}
}