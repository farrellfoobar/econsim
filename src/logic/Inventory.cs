namespace EconSim.logic;

public class Inventory
{
    public void addItem(ItemType grain, int p1) {
        throw new System.NotImplementedException();
    }

    public double getItemCount(ItemType item) {
        throw new System.NotImplementedException();
    }
}

public enum ItemType
{
    NONE,
    GRAIN,
    WOOD,
    FISH,
}