using System;
using UnityEngine.Tilemaps;

namespace UnityEngine
{
    [Serializable]
    [CreateAssetMenu(menuName = "Tiles/MapTile")]
    public class MapTile : TileBase
    {
        public Sprite sprite;
        public bool path = false;
        public bool placeable = false;

        public override void GetTileData(Vector3Int position, ITilemap tileMap, ref TileData tileData)
        {
            tileData.sprite = sprite;
            tileData.colliderType = Tile.ColliderType.None;
        }
    }
}

