using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Collections;

public class WitheringManager : MonoBehaviour
{
    [SerializeField] TileBase[] witheringTileArray;
    [SerializeField] Withering witheringPrefab;
    [SerializeField] float witheringAnimationTime;

    List<Vector3Int> hasWitheringList = new List<Vector3Int>();
    Vector3Int startPosition = new Vector3Int(-7, -3);

    void Start()
    {
        //TODO: StartWither at random position
        StartWither();
    }
    public void FinishedWithering(Vector3Int position)
    {
        //Play animation
        StartCoroutine(Withering(position));
        GameManager.Instance.witheringPositionList.Add(position);
        if (GameManager.Instance.carrotPositionList.Contains(position)) GameManager.Instance.carrotPositionList.Remove(position);
        if (GameManager.Instance.ogCarrotPositionList.Contains(position)) GameManager.Instance.ogCarrotPositionList.Remove(position); 
    }
    IEnumerator Withering(Vector3Int position)
    {
        GameManager.Instance.vegetationTilemap.SetTile(position, witheringTileArray[0]);
        yield return new WaitForSeconds(witheringAnimationTime);
        GameManager.Instance.vegetationTilemap.SetTile(position, witheringTileArray[Random.Range(1, witheringTileArray.Length)]);
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
            if (hasWitheringList.Contains(tilePosition)) return;
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
        hasWitheringList.Add(tilePosition);
        //GameManager.Instance.vegetationTilemap.SetTile(tilePosition, null);
    }
    void StartWither()
    {
        TileData data = MapManager.Instance.GetTileDataFromMap(GameManager.Instance.farmTilemap, startPosition);
        SpreadWithering(startPosition, data);
    }
}