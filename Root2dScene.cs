using EconSim.logic;
using EconSim.render;
using EconSim.tst;
using Godot;

namespace EconSim;

public partial class Root2dScene : Node2D
{
	private bool isRunningTests = false;
	
	private Simulation simulation;
	private SimulationRenderer renderer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		isRunningTests = "true".Equals(System.Environment.GetEnvironmentVariable("--test"));
		if (isRunningTests) {
			try {
				TestExecutor.Main();
			}
			finally {
				GetTree().Quit();
			}
		} else {
			simulation = new Simulation();
			renderer = new SimulationRenderer(this, simulation);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!isRunningTests) {
			simulation.DoTurn();
			renderer.RenderSimulation();
			OS.DelayMsec(1000);
		}
	}
}
