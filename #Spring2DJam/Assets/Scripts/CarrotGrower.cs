using UnityEngine;

public class CarrotGrower : MonoBehaviour
{
    [SerializeField] Carrot carrot;
    [Header("For test:")]
    [SerializeField] float minGrowTime;
    [SerializeField] float maxGrowTime;

    public enum State
    {
        Seedling,
        Plant,
        Carrot,
        OGPlant,
        OGCarrot
    }
    public State state = State.Seedling;

    float growTimer;
    Vector3Int growPosition;
    bool infected, isGrown;

    void Start()
    {
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
        if (growTimer > 0) return;
        switch (state)
        {
            case State.Seedling:
                if (!infected)
                {
                    GameManager.Instance.vegetationTilemap.SetTile(position, carrot.plantPrefab);
                    state = State.Plant;
                }
                else
                {
                    GameManager.Instance.vegetationTilemap.SetTile(position, carrot.ogPlantPrefab);
                    state = State.OGPlant;
                }
                SetGrowTimer();
                break;
            case State.Plant:
                GameManager.Instance.vegetationTilemap.SetTile(position, carrot.carrotPrefab);
                state = State.Carrot;
                SetGrowTimer();
                break;
            case State.Carrot:
                if (!GameManager.Instance.carrotPositions.Contains(position))
                {
                    if (!isGrown)
                    {
                        GameManager.Instance.carrotPositions.Add(position);
                        isGrown = true;
                    }
                    else
                    {
                        Debug.Log("Bunny ate carrot");
                        GameManager.Instance.vegetationTilemap.SetTile(position, null);
                        Destroy(gameObject);
                    }
                }
                break;
            case State.OGPlant:
                GameManager.Instance.vegetationTilemap.SetTile(position, carrot.ogCarrotPrefab);
                state = State.OGCarrot;
                SetGrowTimer();
                break;
            case State.OGCarrot:
                if (!GameManager.Instance.ogCarrotPositions.Contains(position))
                {
                    GameManager.Instance.ogCarrotPositions.Add(position);
                    Debug.Log("Bunny ate carrot");
                    GameManager.Instance.vegetationTilemap.SetTile(position, null);
                    Destroy(gameObject);
                }
                break;
        }
    }
    void SetGrowTimer() => growTimer = Random.Range(minGrowTime, maxGrowTime);
    public void SetCarrotInfected() => infected = true;
}