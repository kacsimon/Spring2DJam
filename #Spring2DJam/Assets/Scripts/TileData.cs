using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileData")]
public class TileData : ScriptableObject
{
    public TileBase[] tiles;
    public bool canWither;
    public float spreadChance, spreadInterval, witheringTime;
}
