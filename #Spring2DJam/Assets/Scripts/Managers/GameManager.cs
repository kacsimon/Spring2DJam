using UnityEngine;
using BaseUtilities;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GameManager : SingletonBase<GameManager>
{
    public Tilemap farmTilemap;
    public Tilemap vegetationTilemap;

    //[HideInInspector] public List<Vector3Int> reservedCarrots = new List<Vector3Int>();
    [HideInInspector] public List<Vector3Int> carrotPositions = new List<Vector3Int>();
    [HideInInspector] public List<Vector3Int> ogCarrotPositions = new List<Vector3Int>();
    [HideInInspector] public List<Vector3Int> witheringPosition = new List<Vector3Int>();
}