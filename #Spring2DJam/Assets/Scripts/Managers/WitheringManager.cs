using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class WitheringManager : MonoBehaviour
{
    //[SerializeField] Tilemap tilemap;
    [SerializeField] TileBase witheringTile;
    [SerializeField] Withering witheringPrefab;
    [Header("For test:")]
    [SerializeField] float witheringDelay;

    List<Vector3Int> hasWithering = new List<Vector3Int>();

    void Update()
    {
        //Test
        if (Input.GetMouseButtonDown(1)) StartWither();
        //TODO: StartWither at random position
        //StartWither(witheringDelay);

        //if (Input.GetMouseButtonDown(0)) CureWither();
    }
    public void FinishedWithering(Vector3Int position)
    {
        GameManager.Instance.vegetationTilemap.SetTile(position, witheringTile);
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
    }
    void StartWither(/*float delay*/)
    {
        //delay -= Time.deltaTime;
        // if (delay > 0) return;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPosition = GameManager.Instance.vegetationTilemap.WorldToCell(mousePosition);
        TileData data = MapManager.Instance.GetTileDataFromMap(GameManager.Instance.farmTilemap, gridPosition);

        SpreadWithering(gridPosition, data);
    }
    void CureWither()
    {
        //if has the power
        //Destroy wither
        //can plant again

        //hasWithering.Remove(position);
    }
    public bool HasWithering(Vector3Int tilePosition) => HasWithering(tilePosition);
}