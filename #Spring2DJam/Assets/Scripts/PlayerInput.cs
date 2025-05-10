using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    int clickCount = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Don't click if rabbit in front
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null) return;
            Vector3Int gridPosition = GameManager.Instance.vegetationTilemap.WorldToCell(mousePosition);
            //Don't click if withering there
            if (GameManager.Instance.witheringPositionList.Contains(gridPosition)) return;
            //Check if it ruined
            TileData data = MapManager.Instance.GetTileDataFromMap(GameManager.Instance.vegetationTilemap, gridPosition);
            if (data == null || !data.isRuined) return;
            //Click!
            clickCount++;
            GameManager.Instance.vegetationTilemap.SetTile(gridPosition, data.tileArray[clickCount]);

            //RepairField(gridPosition);
        }
    }
    //int GetRepairedTime(Vector3Int _gridPosition, TileData _data)
    //{
    //    _data.tileArray.
    //}
    //void RepairField(Vector3Int _gridPosition, TileData _data, int _amount)
    //{
    //    if (data.tileArray.Length > clickCount)
    //    {
    //        //Change the ruined tile
    //        GameManager.Instance.vegetationTilemap.SetTile(_gridPosition, data.tileArray[clickCount]);
    //    }
    //    else
    //    {
    //        //Outside the bounds of the array
    //        //Field repaired
    //        GameManager.Instance.vegetationTilemap.SetTile(_gridPosition, null);
    //    }
    //}
}