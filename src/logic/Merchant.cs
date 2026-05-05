using EconSim.data;

namespace EconSim.logic;


public class Merchant
{
    private Vector2Int position;

    public Merchant(Vector2Int position)
    {
        this.position = position;
    }
    
    public void DoTurn()
    {
        //todo
    }

    public Vector2Int getPosition()
    {
        return position;
    }
}