using UnityEngine;
using BaseUtilities;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GameManager : SingletonBase<GameManager>
{
    public Tilemap farmTilemap;
    public Tilemap vegetationTilemap;

    public List<Vector3Int> carrotPositions = new List<Vector3Int>();
    public List<Vector3Int> ogCarrotPositions = new List<Vector3Int>();
}