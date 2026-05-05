using System;

namespace EconSim.data;

/*
 * This class exists to avoid `using Godot;` (for `Vector2I` type) in the EconSim.logic namespace for code cleanliness.
 * See README.md
 */
public class Vector2Int
{
    private int x;
    private int y;
    
    public Vector2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override bool Equals(object that)
    {
        if (!(that is Vector2Int))
            return false;
        
        Vector2Int thatVector = (Vector2Int)that;
        
        return x.Equals(thatVector.getX()) && y.Equals(thatVector.getY());
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x.GetHashCode(), y.GetHashCode());
    }

    public int getX()
    {
        return x;
    }
    
    public int getY()
    {
        return y;
    }

    public override string ToString()
    {
        return "(" + x + "," + y + ")";
    }
}