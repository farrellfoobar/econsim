using EconSim.logic;
using Godot;

namespace EconSim.render;


public class MerchantRenderer
{
    const float TILE_SIZE_PX = 128;
    private Node rendererParent;
    private Sprite2D sprite;
    public MerchantRenderer(Node rendererParent)
    {
        this.rendererParent = rendererParent;
        sprite = new Sprite2D();
        this.rendererParent.AddChild(sprite);
        sprite.SetTexture( (Texture2D) GD.Load("res://res/entities/troll.png"));
        sprite.SetScale(new Vector2(0.5f, 0.5f));
    }
    
    public void render(Merchant merchant)
    {
        //this seems stupid, maybe just put the sprite on the tile map on another layer
        sprite.SetPosition(new Vector2(25, 22 + TILE_SIZE_PX));
        //sprite.SetPosition(new Vector2(25, 22+1*TILE_SIZE_PX)); //oh god, x,y to cells is ugh idk
        //sprite.SetPosition(new Vector2(merchant.getPosition().X * TILE_SIZE_PX, merchant.getPosition().Y * TILE_SIZE_PX));
    }

    private Vector2 pixelPositionFromHexMapPosition(Vector2I position)
    {
        Vector2 centeredInHex = new Vector2(25, 22);
        
        return new Vector2();
    }
}