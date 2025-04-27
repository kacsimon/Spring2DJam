using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class WitheringManager : MonoBehaviour
{
    [SerializeField] TileBase[] witheringTileArray;
    [SerializeField] Withering witheringPrefab;

    List<Vector3Int> hasWithering = new List<Vector3Int>();
    Vector3Int startPosition = new Vector3Int(-7, -3);

    void Start()
    {
        //TODO: StartWither at random position
        StartWither();
    }
    public void FinishedWithering(Vector3Int position)
    {
        GameManager.Instance.witheringPosition.Add(position);
        //GameManager.Instance.vegetationTilemap.SetTile(position, null);
        //Play animation
        //GameManager.Instance.vegetationTilemap.SetTile(position, witheringTileArray[0]);
        GameManager.Instance.vegetationTilemap.SetTile(position, witheringTileArray[Random.Range(1, witheringTileArray.Length - 1)]);
        if (GameManager.Instance.carrotPositions.Contains(position)) GameManager.Instance.carrotPositions.Remove(position);
        if (GameManager.Instance.ogCarrotPositions.Contains(position)) GameManager.Instance.ogCarrotPositions.Remove(position);
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
            //if (!GameManager.Instance.witheringPosition.Contains(tilePosition)) hasWithering.Remove(tilePosition);
            if (hasWithering.Contains(tilePosition)) return;
            //if (GameManager.Instance.witheringPosition.Contains(tilePosition)) return;
            TileData tileData = MapManager.Instance.GetTileDataFromMap(GameManager.Instance.farmTilemap, tilePosition);
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
        hasWithering.Add(tilePosition);
        //GameManager.Instance.vegetationTilemap.SetTile(tilePosition, null);
    }
    void StartWither()
    {
        TileData data = MapManager.Instance.GetTileDataFromMap(GameManager.Instance.farmTilemap, startPosition);
        SpreadWithering(startPosition, data);
    }
}