using EconSim.logic;
using EconSim.player;
using Godot;

namespace EconSim.render;


public class SimulationRenderer
{
    private Node parent;
    private Simulation simulation;
    
    private MapRenderer mapRenderer;
    private MerchantRenderer merchantRenderer;
    
    private FreeCamera freeCamera;
    
    public SimulationRenderer(Node2D root2dScene, Simulation simulation)
    {
        this.parent = root2dScene;
        this.simulation = simulation;
        freeCamera = new FreeCamera(root2dScene);
        
        mapRenderer = new MapRenderer(parent, this.simulation.GetGameMap());
        merchantRenderer = new MerchantRenderer(parent, this.simulation.GetGameMap());
    }
    
    public void RenderSimulation()
    {
        mapRenderer.RenderMap();
        merchantRenderer.Render(simulation.GetMerchants());
    }
}