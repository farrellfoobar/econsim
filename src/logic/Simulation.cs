using EconSim.data;
using EconSim.render;
using Godot; //!!!!!!!!!!!!!!!!!!!!!!!!!!! this breaks the readme and I need to fix it by... idk, making a
             //SimulationRenderer ?

namespace EconSim;

public class Simulation
{
    private GameMap gameMap;
    private Merchant merchant;

    private MapRenderer mapRenderer;
    private MerchantRenderer merchantRenderer;
    
    public Simulation(Node parent)
    {
        mapRenderer = new MapRenderer(parent);
        merchantRenderer = new MerchantRenderer(parent);
        
        gameMap = new GameMap(27, 13, mapRenderer);
        gameMap.setTileType(new Vector2Int(3, 3), TileType.HAMLET);
        gameMap.setTileType(new Vector2Int(13, 7), TileType.HAMLET);
        gameMap.setTileType(new Vector2Int(20, 9), TileType.HAMLET);
        
        merchant = new Merchant(new Vector2Int(3,3), merchantRenderer);
    }

    public void DoTurn()
    {
        merchant.DoTurn();
    }
}