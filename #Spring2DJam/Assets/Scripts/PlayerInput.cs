using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] EarthPile earthPile;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Don't click if rabbit in front
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null) return;
            Vector3Int gridPosition = GameManager.Instance.interactableTilemap.WorldToCell(mousePosition);
            ////Don't click if withering there
            //if (GameManager.Instance.witheringPositionList.Contains(gridPosition)) return;
            //Check if it ruined
            if (GameManager.Instance.interactableTilemap.GetTile(gridPosition) == null) return;
            TileData data = MapManager.Instance.GetTileDataFromMap(GameManager.Instance.interactableTilemap, gridPosition);
            //data.clickCount = 0;
            RepairField(gridPosition, data);
        }
    }
    void RepairField(Vector3Int _gridPosition, TileData _data)
    {
        if (GameManager.Instance.interactableTilemap.GetTile(_gridPosition) == earthPile.EarthPilePrefabArray[_data.clickCount])
        {
            _data.clickCount++;
            if (_data.clickCount < earthPile.EarthPilePrefabArray.Length)
            {
                //Change the ruined tile
                GameManager.Instance.interactableTilemap.SetTile(_gridPosition, earthPile.EarthPilePrefabArray[_data.clickCount]);
            }
            else
            {
                //Outside the bounds of the array
                //Field repaired
                GameManager.Instance.interactableTilemap.SetTile(_gridPosition, null);
                _data.clickCount = 0;

            }
        }
    }
}