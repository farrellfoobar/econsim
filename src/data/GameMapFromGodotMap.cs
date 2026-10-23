using System;
using System.Collections.Generic;
using EconSim.logic;
using Godot;
using Godot.Collections;

namespace EconSim.data;

public class GameMapFromGodotMap
{
    
    public static GameMap getMap(TileMapLayer groundLayer, TileMapLayer townTileMapLayer, TileMapLayer riverTileMapLayer, TileMapLayer riverDirectionsTileMapLayer)
    {
        GameMap map = new GameMap();

        Array<Vector2I> cells = groundLayer.GetUsedCells();
        Array<Vector2I> riverCells = riverTileMapLayer.GetUsedCells();
        Array<Vector2I> townCells = townTileMapLayer.GetUsedCells();
        
        foreach (Vector2I cell in cells) {
            TileType tileType = atlasCoordToTileType(groundLayer.GetCellAtlasCoords(cell));
            GameTile tile;
            
            if (riverCells.Contains(cell)) {
                Direction direction = atlasCoordToRiverDirection(riverDirectionsTileMapLayer.GetCellAtlasCoords(cell));
                tile = new RiverTile(tileType, new Vector2Int(cell), direction);
            }
            else {
                if (townCells.Contains(cell)) {
                    tile = new GameTile(tileType, new Vector2Int(cell), StructureType.Hamlet);
                }
                else {
                    tile = new GameTile(tileType, new Vector2Int(cell));
                }
            }

            map.AddTile(tile);
        }
        
        return map;
    }
    
    private static TileType atlasCoordToTileType(Vector2I atlasCoords)
    {
        if (atlasCoords.Equals(new Vector2I(-1, -1))) {
            return TileType.None;
        }
        
        int cardinal = atlasCoords.Y * 3 + atlasCoords.X;
        return (TileType)cardinal;
    }
    
    private static Direction atlasCoordToRiverDirection(Vector2I atlasCoords)
    {
        if (atlasCoords.Equals(new Vector2I(-1, -1))) {
            throw new ArgumentException("River tile has no direction!" );
        }
        
        int cardinal = atlasCoords.Y * 3 + atlasCoords.X;
        
        Direction direction = Direction.None;
        
        if(cardinal == 0)
            direction = Direction.North;
        if(cardinal == 1)
            direction = Direction.NorthEast;
        if(cardinal == 2)
            direction = Direction.SouthEast;
        if(cardinal == 3)
            direction = Direction.South;
        if(cardinal == 4)
            direction = Direction.SouthWest;
        if(cardinal == 5)
            direction = Direction.SouthWest;
        
        return direction;
    }
}