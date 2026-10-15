using EconSim.logic;
using EconSim.render;
using EconSim.tst;
using Godot;

namespace EconSim;

public partial class Root2dScene : Node2D
{
	private bool isRunningTests = false;
	
	public const int GODOT_PHYSICS_TICKS_PER_SECOND = 60;
	private int current_tick = 0;
	
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
			renderer.RenderSimulation();
		}
	}
	
	public override void _PhysicsProcess(double delta)
	{
		current_tick++;
		if(current_tick == GODOT_PHYSICS_TICKS_PER_SECOND)
			current_tick = 0;
		
		if (doesTurnHappenThisTick(current_tick)) {
			simulation.DoTurn();
		}
	}

	private bool doesTurnHappenThisTick(int currentTick)
	{
		int ticksPerTurn = GODOT_PHYSICS_TICKS_PER_SECOND / TurnAndTimeManager.TurnsInSecond;

		bool turnHappens = currentTick % ticksPerTurn == 0;
		return turnHappens;
	}
}
