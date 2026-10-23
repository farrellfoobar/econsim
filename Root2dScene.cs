using System;
using System.Collections.Generic;
using EconSim.data;
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
	private int turnsInSecond = TurnAndTimeManager.BASE_TURNS_IN_SECOND;
	
	private Simulation simulation;
	private TurnAndTimeManager turnAndTimeManager = new TurnAndTimeManager();
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
			simulation = new Simulation(GameMapFromGodotMap.getMap(
				GetNode<TileMapLayer>("GroundTileMapLayer"), 
				GetNode<TileMapLayer>("TownTileMapLayer"), 
				GetNode<TileMapLayer>("RiverTileMapLayer"), 
				GetNode<TileMapLayer>("RiverDirectionTileMapLayer"))
			);
			
			renderer = new SimulationRenderer(this, simulation);
		}

		GetNode<Button>("FreeCamera/UI CanvasLayer/Time Control UI/Pause Button").Pressed += () => changeGameSpeed(0);
		GetNode<Button>("FreeCamera/UI CanvasLayer/Time Control UI/Play Button").Pressed += () => changeGameSpeed(1);
		GetNode<Button>("FreeCamera/UI CanvasLayer/Time Control UI/Fast Forward Button").Pressed += () => changeGameSpeed(2);
	}

	void changeGameSpeed(int speed)
	{
		turnsInSecond = speed;
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
		if (turnsInSecond == 0)
			return false;
		
		int ticksPerTurn = GODOT_PHYSICS_TICKS_PER_SECOND / turnsInSecond;

		bool turnHappens = currentTick % ticksPerTurn == 0;
		return turnHappens;
	}
}
