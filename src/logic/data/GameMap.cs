using System;
using System.Collections.Generic;
using System.Linq; 
using EconSim.data;

namespace EconSim.logic;


public class GameMap
{
    public static readonly Dictionary<Direction, Vector2Int> ODD_COL_DIRECTION_AS_VECTOR = new Dictionary<Direction, Vector2Int>
    {
        { Direction.North, new Vector2Int(0, -1) },
        { Direction.South, new Vector2Int(0, +1) },
        
        { Direction.NorthWest, new Vector2Int(-1, 0) },
        { Direction.NorthEast, new Vector2Int(+1, 0) },
        { Direction.SouthWest, new Vector2Int(-1, +1) },
        { Direction.SouthEast, new Vector2Int(1, 1) },
    };
    
    public static readonly Dictionary<Direction, Vector2Int> EVEN_COL_DIRECTION_AS_VECTOR = new Dictionary<Direction, Vector2Int>
    {
        { Direction.North, new Vector2Int(0, -1) },
        { Direction.South, new Vector2Int(0, +1) },
        
        { Direction.NorthWest, new Vector2Int(-1, -1) },
        { Direction.NorthEast, new Vector2Int(+1, -1) },
        { Direction.SouthWest, new Vector2Int(-1, 0) },
        { Direction.SouthEast, new Vector2Int(1, 0) },
    };
    
   private Dictionary<Vector2Int, GameTile> tiles = new Dictionary<Vector2Int, GameTile>();
   private Dictionary<Vector2Int, Town> towns = new Dictionary<Vector2Int, Town>();
   
   public void AddTile(GameTile tile)
   {
       tiles.Add(tile.GetPosition(), tile);
       if (tile.GetStructureType().Equals(StructureType.Hamlet)) {
           towns.Add(tile.GetPosition(), null);
       }
   }
   
   public void AssignTowns(List<Town> list)
   {
       if (list.Count != towns.Count)
           throw new Exception("Town count from map and town count from simulation are different!");

       int i = 0;
       foreach (Vector2Int position in towns.Keys) {
           list[i].SetPosition(position);
           towns[position] = list[i];
           i++;
       }
   }

   public Dictionary<Direction, GameTile> GetNeighborTiles(GameTile tile)
   {
       Dictionary<Direction, Vector2Int> possibleNeighborPositions = GetPossibleNeighborPositions(tile.GetPosition());
       
       Dictionary<Direction, GameTile> neighbors = new Dictionary<Direction, GameTile>();
       foreach (KeyValuePair<Direction, Vector2Int> dir in possibleNeighborPositions) {
           if (tiles.ContainsKey(dir.Value)) {
               neighbors.Add(dir.Key, tiles[dir.Value]);
           }
       }
       
       return neighbors;
   }
   
   public static Dictionary<Direction, Vector2Int> GetPossibleNeighborPositions(Vector2Int position)
   {
       Dictionary<Direction, Vector2Int> relativeNeighborPositions;
       if (position.GetX() % 2 == 0) {
           relativeNeighborPositions = EVEN_COL_DIRECTION_AS_VECTOR;
       }
       else {
           relativeNeighborPositions = ODD_COL_DIRECTION_AS_VECTOR;
       }
       
       Dictionary<Direction, Vector2Int> possibleNeighborPositions = new Dictionary<Direction, Vector2Int>
       {
            { Direction.North, Vector2Int.Sum(position, relativeNeighborPositions[Direction.North]) },
            { Direction.South, Vector2Int.Sum(position, relativeNeighborPositions[Direction.South]) },
            { Direction.NorthWest, Vector2Int.Sum(position, relativeNeighborPositions[Direction.NorthWest]) },
            { Direction.NorthEast, Vector2Int.Sum(position, relativeNeighborPositions[Direction.NorthEast]) },
            { Direction.SouthWest, Vector2Int.Sum(position, relativeNeighborPositions[Direction.SouthWest]) },
            { Direction.SouthEast, Vector2Int.Sum(position, relativeNeighborPositions[Direction.SouthEast]) },
       };
       
       return possibleNeighborPositions;
   }

   public Town GetTownAt(Vector2Int position) {
       return towns[position]; //this is kinda intentionally unsafe because eventually Merchant should know if/what town it is in
   }

   public Vector2Int GetTownPosition(Town town)
   {
       if(!towns.ContainsValue(town))
           throw new ArgumentException("Tried to find a town that doesnt exist. That shouldnt happen");
       
       return towns.First(towns => towns.Value.Equals(town)).Key;
   }
   
   public GameTile GetTileAt(Vector2Int pos)
   {
       return tiles[pos];
   }
   
   public List<GameTile> GetTiles() {
       return tiles.Values.ToList();
   }

   public int GetTilePathfindingWeight(Vector2Int position)
   {
       return tiles[position].GetWagonPathfindingWeight();
   }

   public List<Town> GetTowns() { return towns.Values.ToList(); }
   
}