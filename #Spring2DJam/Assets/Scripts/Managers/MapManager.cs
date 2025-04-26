using UnityEngine;
using UnityEngine.Tilemaps;
using BaseUtilities;
using System.Collections.Generic;

public class MapManager : SingletonBase<MapManager>
{
    //[SerializeField] Tilemap farmTilemap;
    //[SerializeField] Tilemap vegetationTilemap;
    [SerializeField] List<TileData> tileDataList;

    Dictionary<TileBase, TileData> dataFromTilesDictionary;

    void Start()
    {
        dataFromTilesDictionary = new Dictionary<TileBase, TileData>();
        foreach (var tileData in tileDataList)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTilesDictionary.Add(tile, tileData);
            }
        }
    }
    //public TileData GetTileDataFromFarm(Vector3Int tilePosition)
    //{
    //    TileBase tile = farmTilemap.GetTile(tilePosition);

    //    if (tile == null) return null;
    //    else return dataFromTilesDictionary[tile];
    //}
    public TileData GetTileDataFromMap(Tilemap tilemap, Vector3Int tilePosition)
    {
        TileBase tile = tilemap.GetTile(tilePosition);

        if (tile == null) return null;
        else return dataFromTilesDictionary[tile];
    }
}
