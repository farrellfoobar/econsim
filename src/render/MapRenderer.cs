using System.Collections.Generic;
using EconSim.data;
using EconSim.logic;
using Godot;
namespace EconSim.render;


public class MapRenderer
{
    readonly Vector2I ATLAS_COORDS = new Vector2I(0, 0); // idk wtf this is
    
    private TileMapLayer tileMapLayer;
    private GameMap map;
    
    public MapRenderer(Node parent, GameMap map)
    {
        this.map = map;
        tileMapLayer = new TileMapLayer();
        parent.AddChild(tileMapLayer);
        
        tileMapLayer.SetTileSet( (TileSet) GD.Load("res://res/tiles/tileset.tres"));
        tileMapLayer.SetScale(new Vector2(0.5f, 0.5f)); //FIX THIS!!!!
    }

    public void renderMap()
    {
        foreach (KeyValuePair<Vector2Int, GameTile> tile in map.getTiles())
        {
            renderTile(tile.Value);
        }
    }

    private void renderTile(GameTile gameTile)
    {
        tileMapLayer.SetCell(new Vector2I(gameTile.position.getX(), gameTile.position.getY()), (int) gameTile.tileType, ATLAS_COORDS);
    }
}