using System;
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
           if (position.getX() >= width || position.getY() >= height)
           {
               throw new IndexOutOfRangeException(
                   "Tried to assign tile at position: " + position + " in map of " + new Vector2Int(width, height));
           }
           tiles[position].tileType = tileType;
       }
       
       public void addTown(Vector2Int position, Town town)
       {
           setTileType(position, TileType.HAMLET);
       }
       
       public Dictionary<Vector2Int, GameTile> getTiles() { return tiles; }

       public int getHeight(){ return height; }

       public int getWidth(){ return width; }
}