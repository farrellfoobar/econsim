using System;
using System.Collections.Generic;
using System.Linq; 
using EconSim.data;

namespace EconSim.logic;


public class GameMap
{
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
                     tiles.Add(tile.position, tile);
                 }   
            }
       }

       public void SetTileType(Vector2Int position, TileType tileType)
       {
           if (position.GetX() >= width || position.GetY() >= height)
           {
               throw new IndexOutOfRangeException(
                   "Tried to assign tile at position: " + position + " in map of " + new Vector2Int(width, height));
           }
           tiles[position].tileType = tileType;
       }
       
       public void AddTown(Vector2Int position, Town town)
       {
           SetTileType(position, TileType.Hamlet);
           towns.Add(position, town);
       }

       public Town GetTownAt(Vector2Int position) {
           return towns[position]; //this is kinda intentionally unsafe because eventually Merchant should know if/what town it is in
       }

       public List<Town> GetTowns() { return towns.Values.ToList(); }
       
       public Dictionary<Vector2Int, GameTile> GetTiles() { return tiles; }

       public int GetHeight(){ return height; }

       public int GetWidth(){ return width; }
}