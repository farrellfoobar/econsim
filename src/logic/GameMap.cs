using System.Collections.Generic;
using EconSim.render;
using Godot;

namespace EconSim;

public class GameMap
{
       private int width;
       private int height;
       private Dictionary<Vector2I, GameTile> tiles = new Dictionary<Vector2I, GameTile>();
       
       private MapRenderer mapRenderer;
       
       public GameMap(int width, int height, Node rendererParent)
       {
            mapRenderer = new MapRenderer(rendererParent);
            this.width = width;
            this.height = height;

            for (int x = 0; x < width; x++)
            {
                 for (int y = 0; y < height; y++)
                 {
                     GameTile tile = new GameTile(TileType.GRASS, new Vector2I(x, y));
                     tiles.Add(tile.position, tile);
                     mapRenderer.RenderTile(tile);
                 }   
            }
       }

       public void setTileType(Vector2I position, TileType tileType)
       {
           tiles[position].tileType = tileType;
           mapRenderer.RenderTile(tiles[position]);
       }
}