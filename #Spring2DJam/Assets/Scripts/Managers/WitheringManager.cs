using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class WitheringManager : MonoBehaviour
{
    [SerializeField] TileBase[] witheringTileArray;
    [SerializeField] Withering witheringPrefab;
    [Header("For test:")]
    [SerializeField] float witheringDelay;

    //List<Vector3Int> hasWithering = new List<Vector3Int>();
    Vector3Int startPosition = new Vector3Int(-5, -3);

    void Start()
    {
        //TODO: StartWither at random position
        StartWither();
    }
    public void FinishedWithering(Vector3Int position)
    {
        if (GameManager.Instance.carrotPositions.Contains(position)) GameManager.Instance.carrotPositions.Remove(position);
        if (GameManager.Instance.ogCarrotPositions.Contains(position)) GameManager.Instance.ogCarrotPositions.Remove(position);
        GameManager.Instance.vegetationTilemap.SetTile(position, witheringTileArray[Random.Range(0, witheringTileArray.Length)]);
    }

    public void TryToSpread(Vector3Int position, float spreadChance)
    {
        for (int x = position.x - 1; x < position.x + 2; x++)
        {
            for (int y = position.y - 1; y < position.y + 2; y++)
            {
                TryToWither(new Vector3Int(x, y));
            }
        }
        void TryToWither(Vector3Int tilePosition)
        {
            //if (hasWithering.Contains(tilePosition)) return;
            if (GameManager.Instance.hasWithering.Contains(tilePosition)) return;
            TileData tileData = MapManager.Instance.GetTileDataFromMap(GameManager.Instance.farmTilemap, tilePosition);
            //if (tileData.hasWithering) return;
            if (tileData != null && tileData.canWither)
            {
                if (Random.Range(0f, 100f) <= tileData.spreadChance) SpreadWithering(tilePosition, tileData);
            }
        }
    }
    void SpreadWithering(Vector3Int tilePosition, TileData tileData)
    {
        if (tileData == null) return;
        Withering wither = Instantiate(witheringPrefab);
        wither.transform.position = GameManager.Instance.vegetationTilemap.GetCellCenterWorld(tilePosition);
        wither.StartWithering(tilePosition, tileData, this);
        //tileData.hasWithering = true;
        //hasWithering.Add(tilePosition);
        GameManager.Instance.hasWithering.Add(tilePosition);
    }
    void StartWither()
    {
        TileData data = MapManager.Instance.GetTileDataFromMap(GameManager.Instance.farmTilemap, startPosition);
        SpreadWithering(startPosition, data);
    }
    void CureWither(Vector3Int position)
    {
        //if has the power
        //Destroy wither
        //GameManager.Instance.vegetationTilemap.SetTile(position, null);
        //hasWithering.Remove(position);
        //make OGCarrot
    }
}