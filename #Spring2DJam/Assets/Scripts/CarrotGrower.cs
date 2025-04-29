using UnityEngine;
using UnityEngine.Tilemaps;

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
        //World position to Cell Position
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
        if (GameManager.Instance.witheringPositionList.Contains(position)) return;
        growTimer -= Time.deltaTime;
        if (growTimer > 0) return;
        switch (state)
        {
            case State.Seedling:
                if (!infected)
                {
                    SetCarrotVisual(position, carrot.plantPrefab);
                    state = State.Plant;
                }
                else
                {
                    SetCarrotVisual(position, carrot.ogPlantPrefab);
                    state = State.OGPlant;
                }
                SetGrowTimer();
                break;
            case State.Plant:
                SetCarrotVisual(position, carrot.carrotPrefab);
                state = State.Carrot;
                SetGrowTimer();
                break;
            case State.Carrot:
                if (!GameManager.Instance.carrotPositionList.Contains(position))
                {
                    ///Need to refactor here
                    if (!isGrown)
                    {
                        GameManager.Instance.carrotPositionList.Add(position);
                        isGrown = true;
                    }
                    else
                    {
                        SetCarrotVisual(position, null);
                        Destroy(gameObject);
                    }
                }
                break;
            case State.OGPlant:
                SetCarrotVisual(position, carrot.ogCarrotPrefab);
                state = State.OGCarrot;
                SetGrowTimer();
                break;
            case State.OGCarrot:
                if (!GameManager.Instance.ogCarrotPositionList.Contains(position))
                {
                    ///Need to refactor here
                    if (!isGrown)
                    {
                        GameManager.Instance.ogCarrotPositionList.Add(position);
                        isGrown = true;
                    }
                    else
                    {
                        SetCarrotVisual(position, null);
                        Destroy(gameObject);
                    }
                }
                break;
        }
    }
    void SetCarrotVisual(Vector3Int _position, TileBase _prefab)
    {
        GameManager.Instance.vegetationTilemap.SetTile(_position, _prefab);
    }
    void SetGrowTimer() => growTimer = Random.Range(minGrowTime, maxGrowTime);
    public void SetCarrotInfected(bool _bool) => infected = _bool;
}