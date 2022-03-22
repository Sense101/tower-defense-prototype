using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// stores all info about the map
/// </summary>
public class Map : Singleton<Map>
{
    Grid _grid;
    Tilemap _mainTilemap;

    // holds all the information about the map tiles
    private readonly Dictionary<Vector2Int, TileInfo> tiles = new Dictionary<Vector2Int, TileInfo>();

    private void OnEnable()
    {
        _grid = GetComponent<Grid>();
        _mainTilemap = GetComponentInChildren<Tilemap>();
        InitializeTileInfo();
    }

    // finds all the map tiles and adds them to the dictionary
    private void InitializeTileInfo()
    {
        foreach (Vector3Int tilePos in _mainTilemap.cellBounds.allPositionsWithin)
        {
            TileBase tile = _mainTilemap.GetTile(tilePos);
            if (tile is MapTile)
            {
                var mapTile = tile as MapTile;
                tiles.Add((Vector2Int)tilePos, new TileInfo(mapTile.path, mapTile.placeable));
            }
        }
    }

    public Vector2Int WorldToMapSpace(Vector2 worldSpace)
    {
        var x = _grid.WorldToCell(worldSpace).x;
        var y = _grid.WorldToCell(worldSpace).y;
        return new Vector2Int(x, y);
    }
    public Vector2 MapToWorldSpace(Vector2Int mapSpace)
    {
        Vector3Int cellSpace = (Vector3Int)mapSpace;
        return _grid.CellToWorld(cellSpace);
    }
    /// <summary>
    /// tries to get the tile info from the dictionary, and returns null if there is none
    /// </summary>
    public TileInfo TryGetTile(Vector2Int mapSpace)
    {
        if (tiles.TryGetValue(mapSpace, out TileInfo tileInfo))
        {
            return tileInfo;
        }
        Debug.LogWarning("Failed to get tile at: " + mapSpace);

        return null;
    }
    /// <summary>
    /// tries to get the tile info from the dictionary, and returns null if there is none
    /// </summary>
    public TileInfo TryGetTileWorldSpace(Vector2Int worldSpace)
    {
        var mapSpace = WorldToMapSpace(worldSpace);

        if (tiles.TryGetValue(mapSpace, out TileInfo tileInfo))
        {
            return tileInfo;
        }
        Debug.LogWarning("Failed to get tile at: " + mapSpace);

        return null;
    }


    public void SetTile(Vector2Int mapSpace, Turret turret)
    {
        tiles[mapSpace].turret = turret;
    }


}
