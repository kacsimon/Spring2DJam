using UnityEngine;
using UnityEngine.Tilemaps;

public class CarrotGrower : MonoBehaviour
{
    [SerializeField] Carrot carrot;
    //[SerializeField] Tilemap tilemap;
    [Header("For test:")]
    [SerializeField] float minGrowTime;
    [SerializeField] float maxGrowTime;

    public enum State
    {
        Seedling,
        Plant,
        Carrot
    }
    public State state = State.Seedling;

    float growTimer;
    Vector3Int growPosition;

    void Start()
    {
        //TileData tileData = MapManager.Instance.GetTileDataFromVegetation(growPosition);
        growPosition = new Vector3Int((int)transform.position.x, (int)transform.position.y);
        GameManager.Instance.vegetationTilemap.SetTile(growPosition, carrot.seedlingPrefab);
        SetGrowTimer();
    }
    void Update()
    {
        GrowCarrot(growPosition);
    }
    void GrowCarrot(Vector3Int position)
    {
        growTimer -= Time.deltaTime;
        if (growTimer <= 0)
        {
            switch (state)
            {
                case State.Seedling:
                    GameManager.Instance.vegetationTilemap.SetTile(position, carrot.plantPrefab);
                    state = State.Plant;
                    SetGrowTimer();
                    break;
                case State.Plant:
                    GameManager.Instance.vegetationTilemap.SetTile(position, carrot.carrotPrefab);
                    state = State.Carrot;
                    SetGrowTimer();
                    break;
                case State.Carrot:
                    //Harvest
                    break;
            }
        }
    }
    void SetGrowTimer()
    {
        growTimer = Random.Range(minGrowTime, maxGrowTime);
    }
}