using System;
using Godot;

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

    public Vector2Int(Vector2 godotVector)
    {
        if (!double.IsInteger(godotVector.X) || !double.IsInteger(godotVector.Y)){
            throw new ArgumentException("Got double values: " + godotVector + " trying to create Vector2Int. ");
        }
        x = (int) godotVector.X;
        y = (int) godotVector.Y;
    }
    
    public override bool Equals(object that)
    {
        if (!(that is Vector2Int))
            return false;
        
        Vector2Int thatVector = (Vector2Int)that;
        
        return x.Equals(thatVector.GetX()) && y.Equals(thatVector.GetY());
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x.GetHashCode(), y.GetHashCode());
    }

    public Vector2I AsGodotVector()
    {
        return new Vector2I(x, y);
    }
    
    public int GetX()
    {
        return x;
    }
    
    public int GetY()
    {
        return y;
    }

    public override string ToString()
    {
        return "(" + x + "," + y + ")";
    }
}