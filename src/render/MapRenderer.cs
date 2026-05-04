using Godot;

namespace EconSim.render;



public class MapRenderer
{
    readonly Vector2I ATLAS_COORDS = new Vector2I(0, 0); // idk wtf this is
    
    private TileMapLayer tileMapLayer;
    public MapRenderer(Node parent)
    {
        tileMapLayer = new TileMapLayer();
        parent.AddChild(tileMapLayer);
        
        tileMapLayer.SetTileSet( (TileSet) GD.Load("res://res/tiles/tileset.tres"));
        tileMapLayer.SetScale(new Vector2(0.5f, 0.5f));
    }


    public void RenderTile(GameTile gameTile)
    {
        tileMapLayer.SetCell(new Vector2I(gameTile.position.X, gameTile.position.Y), (int) gameTile.tileType, ATLAS_COORDS);
    }
}