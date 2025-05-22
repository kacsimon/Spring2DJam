using UnityEngine;
using BaseUtilities;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GameManager : SingletonBase<GameManager>
{
    public Tilemap farmTilemap;
    public Tilemap vegetationTilemap;
    public Tilemap interactableTilemap;

    [SerializeField] float witheringSpreadIntensity;
    [SerializeField] float bunnySpawnIntensity;
    [SerializeField] TileBase ruinedTile;

    //[HideInInspector] public List<Vector3Int> reservedCarrotList = new List<Vector3Int>();
    [HideInInspector] public List<Vector3Int> carrotPositionList = new List<Vector3Int>();
    [HideInInspector] public List<Vector3Int> ogCarrotPositionList = new List<Vector3Int>();
    [HideInInspector] public List<Vector3Int> witheringPositionList = new List<Vector3Int>();

    public void SetFieldRuined(Vector3Int tilePosition)
    {
        interactableTilemap.SetTile(tilePosition, ruinedTile);
    }
}