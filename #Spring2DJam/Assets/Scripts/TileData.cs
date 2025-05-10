using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileData")]
public class TileData : ScriptableObject
{
    public TileBase[] tileArray;
    public bool canWither, canPlant, isInfected, isRuined;
    public float spreadDelay, spreadChance, spreadInterval, witheringTime;
}