using System.Collections.Generic; //for `Dictionary<>`. Can I scope this down somehow? 
using EconSim.data;

namespace EconSim.logic;


public class GameMap
{
       private int width;
       private int height;
       private Dictionary<Vector2Int, GameTile> tiles = new Dictionary<Vector2Int, GameTile>();
       
       public GameMap(int width, int height)
       {
            this.width = width;
            this.height = height;

            for (int x = 0; x < width; x++)
            {
                 for (int y = 0; y < height; y++)
                 {
                     GameTile tile = new GameTile(TileType.GRASS, new Vector2Int(x, y));
                     tiles.Add(tile.position, tile);
                 }   
            }
       }

       public void setTileType(Vector2Int position, TileType tileType)
       {
           tiles[position].tileType = tileType;
       }
       
       public Dictionary<Vector2Int, GameTile> getTiles() { return tiles; }
}