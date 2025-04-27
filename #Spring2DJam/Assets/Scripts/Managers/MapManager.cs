using UnityEngine;
using UnityEngine.Tilemaps;
using BaseUtilities;
using System.Collections.Generic;

public class MapManager : SingletonBase<MapManager>
{
    [SerializeField] List<TileData> tileDataList;

    Dictionary<TileBase, TileData> dataFromTilesDictionary;

    protected override void Awake()
    {
        base.Awake();
        dataFromTilesDictionary = new Dictionary<TileBase, TileData>();
        foreach (var tileData in tileDataList)
        {
            foreach (var tile in tileData.tileArray)
            {
                dataFromTilesDictionary.Add(tile, tileData);
            }
        }
    }
    public TileData GetTileDataFromMap(Tilemap tilemap, Vector3Int tilePosition)
    {
        TileBase tile = tilemap.GetTile(tilePosition);

        if (tile == null) return null;
        else return dataFromTilesDictionary[tile];
    }
}