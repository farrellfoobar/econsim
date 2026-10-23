using EconSim.logic;
using EconSim.player;
using Godot;

namespace EconSim.render;


public class SimulationRenderer
{
    private Node parent;
    private Simulation simulation;
    
    private MerchantRenderer merchantRenderer;
    
    private FreeCamera freeCamera;
    
    public SimulationRenderer(Node2D root2dScene, Simulation simulation)
    {
        this.parent = root2dScene;
        this.simulation = simulation;
        freeCamera = new FreeCamera(root2dScene);
        
        merchantRenderer = new MerchantRenderer(parent, this.simulation.GetGameMap());
    }
    
    public void RenderSimulation()
    {
        merchantRenderer.Render(simulation.GetMerchants());
    }
}