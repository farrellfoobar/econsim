using EconSim.logic;
using Godot;

namespace EconSim.render;


public class SimulationRenderer
{
    private Node parent;
    private Simulation simulation;
    
    private MapRenderer mapRenderer;
    private MerchantRenderer merchantRenderer;
    
    public SimulationRenderer(Node2D root2dScene, Simulation simulation)
    {
        this.parent = root2dScene;
        this.simulation = simulation;
        
        mapRenderer = new MapRenderer(parent, this.simulation.getGameMap());
        merchantRenderer = new MerchantRenderer(parent, this.simulation.getGameMap());
    }
    
    public void renderSimulation()
    {
        mapRenderer.renderMap();
        merchantRenderer.render(simulation.getMerchants());
    }
}