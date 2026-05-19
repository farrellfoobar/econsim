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
                     tiles.Add(tile.GetPosition(), tile);
                 }   
            }
       }
       
       public void AddTown(Town town)
       {
           if(towns.ContainsKey(town.GetPosition()))
               throw new ArgumentException("There is already a town with the same position: " + town);
           
           tiles[town.GetPosition()].SetStructureType(StructureType.Hamlet);
           towns.Add(town.GetPosition(), town);
       }

       public List<GameTile> GetNeighborTiles(GameTile tile)
       {
           Vector2Int position = tile.GetPosition();
           List<Vector2Int> possibleNeighbors = new List<Vector2Int>
           {
                new Vector2Int(position.GetX()+1, position.GetY()),
                new Vector2Int(position.GetX(), position.GetY()+1),
                new Vector2Int(position.GetX()-1, position.GetY()),
                new Vector2Int(position.GetX(), position.GetY()-1),
                new Vector2Int(position.GetX()+1, position.GetY()+1),
                new Vector2Int(position.GetX()-1, position.GetY()-1),
           };
           
           List<GameTile> neighbors = new List<GameTile>();
           foreach (Vector2Int possibleNeighbor in possibleNeighbors) {
               if (tiles.ContainsKey(possibleNeighbor)) {
                   neighbors.Add(tiles[possibleNeighbor]);
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