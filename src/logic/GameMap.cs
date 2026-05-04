using System.Collections.Generic; //for `Dictionary<>`. Can I scope this down somehow? 
using EconSim.data;
using EconSim.render;

namespace EconSim;

public class GameMap
{
       private int width;
       private int height;
       private Dictionary<Vector2Int, GameTile> tiles = new Dictionary<Vector2Int, GameTile>();
       
       private MapRenderer mapRenderer;
       
       public GameMap(int width, int height, MapRenderer mapRenderer)
       {
            this.mapRenderer = mapRenderer;
            this.width = width;
            this.height = height;

            for (int x = 0; x < width; x++)
            {
                 for (int y = 0; y < height; y++)
                 {
                     GameTile tile = new GameTile(TileType.GRASS, new Vector2Int(x, y));
                     tiles.Add(tile.position, tile);
                     mapRenderer.RenderTile(tile);
                 }   
            }
       }

       public void setTileType(Vector2Int position, TileType tileType)
       {
           tiles[position].tileType = tileType;
           mapRenderer.RenderTile(tiles[position]);
       }
}