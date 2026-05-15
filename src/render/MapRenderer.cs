using System.Collections.Generic;
using EconSim.data;
using EconSim.logic;
using Godot;
namespace EconSim.render;


public class MapRenderer
{
    readonly Vector2I atlasCoords = new Vector2I(0, 0); // idk wtf this is
    
    private TileMapLayer tileMapLayer;
    private GameMap map;
    
    public MapRenderer(Node parent, GameMap map)
    {
        this.map = map;
        tileMapLayer = new TileMapLayer();
        parent.AddChild(tileMapLayer);
        
        tileMapLayer.SetTileSet( (TileSet) GD.Load("res://res/tiles/tileset.tres"));
        
        //todo: remove setscale once we add camera movement. Only set to half scale to fit in screen
        tileMapLayer.SetScale(new Vector2(0.5f, 0.5f));
    }

    public void RenderMap()
    {
        foreach (GameTile tile in map.GetTiles())
        {
            renderTile(tile);
        }
    }

    private void renderTile(GameTile gameTile)
    {
        Vector2I tilePositionAsGodotType = new Vector2I(gameTile.GetPosition().GetX(), gameTile.GetPosition().GetY());
        tileMapLayer.SetCell(tilePositionAsGodotType, (int) gameTile.GetTileType(), atlasCoords);
    }
}