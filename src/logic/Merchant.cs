using EconSim.data;
using EconSim.render;

namespace EconSim;

public class Merchant
{
    private Vector2Int position;
    private MerchantRenderer renderer;

    public Merchant(Vector2Int position, MerchantRenderer renderer)
    {
        this.position = position;
        this.renderer = renderer;
        this.renderer.render(this);
    }
    
    public void DoTurn()
    {
        //todo
        // if we move renderer.render();
    }

    public Vector2Int getPosition()
    {
        return position;
    }
}