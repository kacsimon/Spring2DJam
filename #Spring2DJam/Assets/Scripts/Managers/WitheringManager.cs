using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Collections;

public class WitheringManager : MonoBehaviour
{
    [SerializeField] TileBase[] witheringTileArray;
    [SerializeField] Withering previewPrefab;
    [SerializeField] float witheringAnimationTime, spreadDelay;

    List<Vector3Int> previewPositionList = new List<Vector3Int>();
    Vector3Int startPosition = new Vector3Int(-7, -3);

    void Start()
    {
        StartWither();
    }
    void Update()
    {
        foreach (var withtering in GameManager.Instance.witheringPositionList)
        {
            TileData tileData = MapManager.Instance.GetTileDataFromMap(GameManager.Instance.farmTilemap, withtering);
            //spreadDelay -= Time.deltaTime;
            //if (spreadDelay <= 0)
            {

                //spreadDelay = tileData.spreadDelay;
                //spreadDelay = 20f;
                StartCoroutine(TryToSpreadNeighbours(withtering));
            }
        }
    }
    public void FinishedWithering(Vector3Int position)
    {
        //Play animation
        //StartCoroutine(WitheringAnimation(position));
        GameManager.Instance.vegetationTilemap.SetTile(position, witheringTileArray[Random.Range(1, witheringTileArray.Length)]);
        GameManager.Instance.witheringPositionList.Add(position);
        if (GameManager.Instance.carrotPositionList.Contains(position)) GameManager.Instance.carrotPositionList.Remove(position);
        if (GameManager.Instance.ogCarrotPositionList.Contains(position)) GameManager.Instance.ogCarrotPositionList.Remove(position);
        previewPositionList.Remove(position);
    }
    IEnumerator WitheringAnimation(Vector3Int position)
    {
        GameManager.Instance.vegetationTilemap.SetTile(position, witheringTileArray[0]);
        yield return new WaitForSeconds(witheringAnimationTime);
        GameManager.Instance.vegetationTilemap.SetTile(position, witheringTileArray[Random.Range(1, witheringTileArray.Length)]);
    }
    public IEnumerator TryToSpreadNeighbours(Vector3Int position)
    {
        for (int x = position.x - 1; x < position.x + 2; x++)
        {
            for (int y = position.y - 1; y < position.y + 2; y++)
            {
                yield return new WaitForSeconds(spreadDelay);
                TryToSpread(new Vector3Int(x, y));
            }
        }
        void TryToSpread(Vector3Int tilePosition)
        {
            if (previewPositionList.Contains(tilePosition)) return;
            if (GameManager.Instance.witheringPositionList.Contains(tilePosition)) return;
            TileData tileData = MapManager.Instance.GetTileDataFromMap(GameManager.Instance.farmTilemap, tilePosition);
            if (tileData != null && tileData.canWither)
            {
                if (Random.Range(0f, 100f) <= tileData.spreadChance) SpreadPreview(tilePosition, tileData);
            }
        }
    }
    void SpreadPreview(Vector3Int tilePosition, TileData tileData)
    {
        if (tileData == null) return;
        Withering wither = Instantiate(previewPrefab);
        wither.transform.position = GameManager.Instance.vegetationTilemap.GetCellCenterWorld(tilePosition);
        wither.StartWithering(tilePosition, tileData, this);
        previewPositionList.Add(tilePosition);
    }
    void StartWither()
    {
        TileData data = MapManager.Instance.GetTileDataFromMap(GameManager.Instance.farmTilemap, startPosition);
        SpreadPreview(startPosition, data);
    }
}