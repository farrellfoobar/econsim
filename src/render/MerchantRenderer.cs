using System.Collections.Generic;
using EconSim.data;
using EconSim.logic;
using Godot;

namespace EconSim.render;


public class MerchantRenderer
{
    const double TILE_SIZE_PX = 128;
    private const int MERCHANT_TEXTURE_INDEX = 1;
    private const int ERASE_TEXTURE_INDEX = -1;
    private TileMapLayer mapLayer;
    GameMap gameMap;
    
    readonly Vector2I ATLAS_COORDS = new Vector2I(0, 0); // idk wtf this is
    readonly Vector2I ERASE_ATLAS_COORDS = new Vector2I(-1, -1); // idk wtf this is
    
    public MerchantRenderer(Node rendererParent, GameMap gameMap)
    {
        this.gameMap = gameMap;
        mapLayer = new TileMapLayer();
        rendererParent.AddChild(mapLayer);
        
        mapLayer.SetTileSet( (TileSet) GD.Load("res://res/tiles/agents.tres"));
        
        //todo: remove setscale once we add camera movement. Only set to half scale to fit in screen
        mapLayer.SetScale(new Vector2(0.5f, 0.5f)); 
    }
    
    public void render(List<Merchant> merchants)
    {
        clear();
        foreach (Merchant merchant in merchants){
            Vector2I tilePositionAsGodotType = new Vector2I(merchant.getPosition().getX(), merchant.getPosition().getY());
            mapLayer.SetCell(tilePositionAsGodotType, MERCHANT_TEXTURE_INDEX, ATLAS_COORDS);
        }
    }

    private void clear()
    {
        foreach (KeyValuePair<Vector2Int, GameTile> tile in gameMap.getTiles()){
            mapLayer.SetCell(tile.Key.asGodotVector(), ERASE_TEXTURE_INDEX, ERASE_ATLAS_COORDS);
        }
    }
}