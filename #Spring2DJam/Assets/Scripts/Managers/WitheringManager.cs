using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WitheringManager : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] TileBase witheringTile;
    [SerializeField] Withering witheringPrefab;

    List<Vector3Int> hasWithering = new List<Vector3Int>();

    void Update()
    {
        //Test
        if (Input.GetMouseButtonDown(0)) StartWither();
        //TODO: StartWither at random position
        
        //if (Input.GetMouseButtonDown(0)) CureWither();
    }
    public void FinishedWithering(Vector3Int position)
    {
        tilemap.SetTile(position, witheringTile);
        hasWithering.Remove(position);
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
            if (hasWithering.Contains(tilePosition)) return;
            TileData tileData = MapManager.Instance.GetTileData(tilePosition);
            if (tileData != null && tileData.canWither)
            {
                if (Random.Range(0f, 100f) <= tileData.spreadChance) SpreadWithering(tilePosition, tileData);
            }
        }
    }
    void SpreadWithering(Vector3Int tilePosition, TileData tileData)
    {
        Withering wither = Instantiate(witheringPrefab);
        wither.transform.position = tilemap.GetCellCenterWorld(tilePosition);
        wither.StartWithering(tilePosition, tileData, this);

        hasWithering.Add(tilePosition);
    }
    void StartWither()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPosition = tilemap.WorldToCell(mousePosition);
        TileData data = MapManager.Instance.GetTileData(gridPosition);

        SpreadWithering(gridPosition, data);
    }
    void CureWither()
    {
        //if has the power
        //Destroy wither
    }
}