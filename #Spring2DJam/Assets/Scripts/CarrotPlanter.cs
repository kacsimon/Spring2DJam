using UnityEngine;

public class CarrotPlanter : MonoBehaviour
{
    [SerializeField] CarrotGrower carrotGrower;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Don't plant if rabbit in front
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null) return;
            Vector3Int gridPosition = GameManager.Instance.vegetationTilemap.WorldToCell(mousePosition);
            if (GameManager.Instance.witheringPositionList.Contains(gridPosition)) return;
            //Check that can plant
            TileData data = MapManager.Instance.GetTileDataFromMap(GameManager.Instance.farmTilemap, gridPosition);
            if (data == null || !data.canPlant) return;
            //Check if it already has something on it
            TileData VegetationData = MapManager.Instance.GetTileDataFromMap(GameManager.Instance.vegetationTilemap, gridPosition);
            if (VegetationData != null) return;
            //Plant the carrot
            CarrotGrower plantedCarrot = Instantiate(carrotGrower, gridPosition, Quaternion.identity);
            //Set infected if the field infected
            plantedCarrot.SetCarrotInfected(data.isInfected);
        }
    }
}