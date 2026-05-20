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
    
   private int width;
   private int height;
   private Dictionary<Vector2Int, GameTile> tiles = new Dictionary<Vector2Int, GameTile>();
   private Dictionary<Vector2Int, Town> towns = new Dictionary<Vector2Int, Town>();
   
   public GameMap(int width, int height)
   {
        this.width = width;
        this.height = height;

        for (int x = 0; x < width; x++)
        {
             for (int y = 0; y < height; y++)
             {
                 GameTile tile = new GameTile(TileType.Grass, new Vector2Int(x, y));
                 tiles.Add(tile.GetPosition(), tile);
             }   
        }
   }

   public void AddRiver(Vector2Int start, List<Direction> path)
   {
       tiles[start] = new RiverTile(start, path[0]);
       Vector2Int pos = start;
       
       foreach (Direction direction in path) {
           Vector2Int next = GetNeighborTiles(tiles[pos])[direction].GetPosition();
           tiles[next] = new RiverTile(next, direction);
           pos = next;
       }
   }
   
   public void AddTown(Town town)
   {
       if(towns.ContainsKey(town.GetPosition()))
           throw new ArgumentException("There is already a town with the same position: " + town);
       
       tiles[town.GetPosition()].SetStructureType(StructureType.Hamlet);
       towns.Add(town.GetPosition(), town);
   }

   public Dictionary<Direction, GameTile> GetNeighborTiles(GameTile tile)
   {
       Vector2Int position = tile.GetPosition();

       Dictionary<Direction, Vector2Int> relativeNeighborPositions;
       if (tile.GetPosition().GetX() % 2 == 0) {
           relativeNeighborPositions = EVEN_COL_DIRECTION_AS_VECTOR;
       }
       else {
           relativeNeighborPositions = ODD_COL_DIRECTION_AS_VECTOR;
       }
       
       Dictionary<Direction, Vector2Int> possibleNeighborPositions = new Dictionary<Direction, Vector2Int>
       {
            { Direction.North, new Vector2Int(Vector2Int.Sum(position, relativeNeighborPositions[Direction.North]).AsGodotVector() ) },
            { Direction.South, new Vector2Int(Vector2Int.Sum(position, relativeNeighborPositions[Direction.South]).AsGodotVector() ) },
            { Direction.NorthWest, new Vector2Int(Vector2Int.Sum(position, relativeNeighborPositions[Direction.NorthWest]).AsGodotVector() ) },
            { Direction.NorthEast, new Vector2Int(Vector2Int.Sum(position, relativeNeighborPositions[Direction.NorthEast]).AsGodotVector() ) },
            { Direction.SouthWest, new Vector2Int(Vector2Int.Sum(position, relativeNeighborPositions[Direction.SouthWest]).AsGodotVector() ) },
            { Direction.SouthEast, new Vector2Int(Vector2Int.Sum(position, relativeNeighborPositions[Direction.SouthEast]).AsGodotVector() ) },
       };

       Dictionary<Direction, GameTile> neighbors = new Dictionary<Direction, GameTile>();
       foreach (KeyValuePair<Direction, Vector2Int> dir in possibleNeighborPositions) {
           if (tiles.ContainsKey(dir.Value)) {
               neighbors.Add(dir.Key, tiles[dir.Value]);
           }
       }
       
       return neighbors;
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
   
   public List<GameTile> GetTiles() {
       return tiles.Values.ToList();
   }

   public int GetTilePathfindingWeight(Vector2Int position)
   {
       return tiles[position].GetPathfindingWeight();
   }

   public List<Town> GetTowns() { return towns.Values.ToList(); }
   public int GetHeight(){ return height; }
   public int GetWidth(){ return width; }
}