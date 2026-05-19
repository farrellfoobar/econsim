using EconSim.logic;
using Godot;
namespace EconSim.render;


public class MapRenderer
{
    readonly Vector2I atlasCoords = new Vector2I(0, 0); // idk wtf this is
    
    private TileMapLayer terrainMapLayer;
    private TileMapLayer structureMapLayer;
    private GameMap map;
    
    public MapRenderer(Node parent, GameMap map)
    {
        this.map = map;
        terrainMapLayer = new TileMapLayer();
        structureMapLayer = new TileMapLayer();
        parent.AddChild(terrainMapLayer);
        parent.AddChild(structureMapLayer);
        
        terrainMapLayer.SetTileSet( (TileSet) GD.Load("res://res/tiles/tileset.tres"));
        structureMapLayer.SetTileSet( (TileSet) GD.Load("res://res/tiles/tileset.tres"));
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
        terrainMapLayer.SetCell(tilePositionAsGodotType, (int) gameTile.GetTileType(), atlasCoords);

        if (gameTile.GetStructureType() != StructureType.None) {
            structureMapLayer.SetCell(tilePositionAsGodotType, (int) gameTile.GetStructureType(), atlasCoords);
        }
    }
}