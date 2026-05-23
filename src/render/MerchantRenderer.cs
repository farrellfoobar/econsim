using System.Collections.Generic;
using EconSim.logic;
using Godot;

namespace EconSim.render;


public class MerchantRenderer
{
    const double tileSizePx = 128;
    private const int merchantWagonTextureIndex = 2;
    private const int merchantBoatTextureIndex = 0;
    private const int eraseTextureIndex = -1;
    private TileMapLayer mapLayer;
    GameMap gameMap;
    
    readonly Vector2I atlasCoords = new Vector2I(0, 0); // idk wtf this is
    readonly Vector2I eraseAtlasCoords = new Vector2I(-1, -1);
    
    public MerchantRenderer(Node rendererParent, GameMap gameMap)
    {
        this.gameMap = gameMap;
        mapLayer = new TileMapLayer();
        rendererParent.AddChild(mapLayer);
        
        mapLayer.SetTileSet( (TileSet) GD.Load("res://res/tiles/agents.tres"));
    }
    
    public void Render(List<Merchant> merchants)
    {
        clear();
        foreach (Merchant merchant in merchants){
            Vector2I tilePositionAsGodotType = new Vector2I(merchant.GetPosition().GetX(), merchant.GetPosition().GetY());
            int texture = merchant.GetType().Equals(MerchantType.Wagon) ? merchantWagonTextureIndex : merchantBoatTextureIndex;
            mapLayer.SetCell(tilePositionAsGodotType, texture, atlasCoords);
        }
    }

    private void clear()
    {
        foreach (GameTile tile in gameMap.GetTiles()){
            mapLayer.SetCell(tile.GetPosition().AsGodotVector(), eraseTextureIndex, eraseAtlasCoords);
        }
    }
}