using UnityEngine;

public class CarrotPlanter : MonoBehaviour
{
    [SerializeField] CarrotGrower carrotGrower;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int gridPosition = GameManager.Instance.vegetationTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (GameManager.Instance.witheringPosition.Contains(gridPosition)) return;
            TileData data = MapManager.Instance.GetTileDataFromMap(GameManager.Instance.farmTilemap, gridPosition);
            if (data == null || !data.canPlant) return;
            TileData VegetationData = MapManager.Instance.GetTileDataFromMap(GameManager.Instance.vegetationTilemap, gridPosition);
            if (VegetationData != null) return;
            CarrotGrower plantedCarrot = Instantiate(carrotGrower, gridPosition, Quaternion.identity);
            plantedCarrot.SetCarrotInfected(data.infected);
        }
    }
}