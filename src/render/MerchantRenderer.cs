using EconSim.logic;
using Godot;

namespace EconSim.render;


public class MerchantRenderer
{
    const float TILE_SIZE_PX = 128;
    private const int MERCHANT_TEXTURE_INDEX = 1;
    private TileMapLayer mapLayer;
    
    readonly Vector2I ATLAS_COORDS = new Vector2I(0, 0); // idk wtf this is
    
    public MerchantRenderer(Node rendererParent)
    {
        mapLayer = new TileMapLayer();
        rendererParent.AddChild(mapLayer);
        
        mapLayer.SetTileSet( (TileSet) GD.Load("res://res/tiles/agents.tres"));
        mapLayer.SetScale(new Vector2(0.5f, 0.5f)); //FIX THIS!!!!
    }
    
    public void render(Merchant merchant)
    {
        Vector2I tilePositionAsGodotType = new Vector2I(merchant.getPosition().getX(), merchant.getPosition().getY());
        mapLayer.SetCell(tilePositionAsGodotType, MERCHANT_TEXTURE_INDEX, ATLAS_COORDS);
    }

    private Vector2 pixelPositionFromHexMapPosition(Vector2I position)
    {
        Vector2 centeredInHex = new Vector2(25, 22);
        
        return new Vector2();
    }
}