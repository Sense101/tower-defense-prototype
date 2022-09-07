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
    private Dictionary<Vector2Int, TileInfo> _tiles = new Dictionary<Vector2Int, TileInfo>();

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
                _tiles.Add((Vector2Int)tilePos, new TileInfo(mapTile.path, mapTile.placeable));
            }
        }
    }

    public Vector2Int WorldToMapSpace(Vector2 worldSpace)
    {
        return (Vector2Int)_grid.WorldToCell(worldSpace);
    }

    public Vector2 MapToWorldSpace(Vector2Int mapSpace)
    {
        Vector3Int cellSpace = (Vector3Int)mapSpace;
        return _grid.CellToWorld(cellSpace) + (_grid.cellSize / 2);
    }

    /// <summary>
    /// tries to get the tile info from the dictionary, and returns null if there is none
    /// </summary>
    public TileInfo TryGetTile(Vector2Int mapSpace)
    {
        if (_tiles.TryGetValue(mapSpace, out TileInfo tileInfo))
        {
            return tileInfo;
        }
        Debug.LogWarning("Failed to get tile at: " + mapSpace);

        return null;
    }
    /// <summary>
    /// tries to get the tile info from the dictionary, and returns null if there is none
    /// </summary>
    public TileInfo TryGetTileWorldSpace(Vector2 worldSpace)
    {
        return TryGetTile(WorldToMapSpace(worldSpace));
    }

    /// <summary>
    /// checks if a turret can be placed at a given tile, in map space
    /// </summary>
    public bool IsTilePlaceable(Vector2Int mapSpace)
    {
        TileInfo tileInfo = TryGetTile(mapSpace);

        if (tileInfo != null && tileInfo.Placeable && !tileInfo.Turret)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// checks if a turret can be placed at a given tile, in world space
    /// </summary>
    public bool IsTilePlaceableWorldSpace(Vector2Int worldSpace)
    {
        Vector2Int mapSpace = WorldToMapSpace(worldSpace);
        return IsTilePlaceable(mapSpace);
    }


    public void SetTurret(Vector2Int mapSpace, Turret turret)
    {
        _tiles[mapSpace].Turret = turret;
    }
    public void SetTurretWorldSpace(Vector2Int worldSpace, Turret turret)
    {
        SetTurret(WorldToMapSpace(worldSpace), turret);
    }

    public Turret GetTurret(Vector2Int mapSpace)
    {
        return TryGetTile(mapSpace)?.Turret;
    }
    public Turret GetTurretWorldSpace(Vector2 worldSpace)
    {
        return GetTurret(WorldToMapSpace(worldSpace));
    }
}
