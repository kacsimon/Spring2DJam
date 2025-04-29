using UnityEngine;
using BaseUtilities;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GameManager : SingletonBase<GameManager>
{
    public Tilemap farmTilemap;
    public Tilemap vegetationTilemap;

    [SerializeField] float witheringSpreadIntensity;
    [SerializeField] float bunnySpawnIntensity;

    //[HideInInspector] public List<Vector3Int> reservedCarrots = new List<Vector3Int>();
    [HideInInspector] public List<Vector3Int> carrotPositionList = new List<Vector3Int>();
    [HideInInspector] public List<Vector3Int> ogCarrotPositionList = new List<Vector3Int>();
    [HideInInspector] public List<Vector3Int> witheringPositionList = new List<Vector3Int>();
}