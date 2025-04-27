using UnityEngine;
using UnityEngine.Tilemaps;

public class Rabbit : MonoBehaviour
{
    [SerializeField] GameObject[] gfxPrefabArray;
    //for test
    [SerializeField] float velocity = 3f;
    Vector3Int targetCarrot = new Vector3Int(6, 2);
    bool isHungry = true, isOG, isDragging;
    GameObject gfx;
    float eatDelay = 1.5f;
    int eatedCarrot = 0, maxEatedCarrot = 3;

    public enum State
    {
        Casual,
        Angry,
        Overgrown,
        Fed
    }
    State state;
    void Start()
    {
        gfx = Instantiate(gfxPrefabArray[0], transform);
        transform.localRotation = new Quaternion(0f, 180f, 0f, 0f);
        SearchForCarrot();
    }
    void FixedUpdate()
    {
        switch (state)
        {
            case State.Casual:
                Move();
                break;
            case State.Angry:
                Move();
                break;
            case State.Overgrown:
                if (isDragging) return;
                if (eatedCarrot == maxEatedCarrot) state = State.Fed;
                    Vector3Int currentPosition = new Vector3Int((int)transform.position.x, (int)transform.position.y);
                if (GameManager.Instance.witheringPosition.Contains(currentPosition))
                {
                    Debug.Log("OG Rabbit ate withering in " + currentPosition);
                    eatedCarrot++;
                    //Cure withering
                    GameManager.Instance.vegetationTilemap.SetTile(currentPosition, null);
                    GameManager.Instance.witheringPosition.Remove(currentPosition);
                    //Field infected
                    TileData tileData = MapManager.Instance.GetTileDataFromMap(GameManager.Instance.farmTilemap, currentPosition);
                    TileBase farmTile = GameManager.Instance.farmTilemap.GetTile(currentPosition);
                    Debug.Log(farmTile);
                }
                break;
            case State.Fed:
                transform.localScale = Vector3.one;
                transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
                StateChange(0);
                Vector3 destination = new Vector3(6.3f, 2f, 0f);
                transform.position = Vector3.MoveTowards(transform.position, destination, velocity * Time.deltaTime);
                if (transform.position == destination) Destroy(gameObject);
                break;
        }
    }
    //}
    void OnMouseDrag()
    {
        if (state != State.Overgrown) return;
        isDragging = true;
        transform.position = (Vector3)GetMouseWorldPosition();
    }
    void OnMouseExit()
    {
        isDragging = false;
    }
    void Move()
    {
        if (GameManager.Instance.witheringPosition.Contains(targetCarrot)) SearchForCarrot();
        Vector3 targetPosition = GameManager.Instance.vegetationTilemap.GetCellCenterWorld(targetCarrot);
        if (isHungry) transform.position = Vector3.MoveTowards(transform.position, targetPosition, velocity * Time.deltaTime);
        else transform.position = Vector3.MoveTowards(transform.position, new Vector3(13f, 0f, 0f), velocity * Time.deltaTime);
        //Eat when reach destination
        if (targetPosition == transform.position) Eat();
        if (transform.position == new Vector3(13f, 0f, 0f)) Destroy(gameObject, 1f);
    }
    void SearchForCarrot()
    {
        StateChange(0);
        if (GetOGCarrotPosition()) return;
        else if (GetCarrotPosition()) return;
        else StateChange(1);
    }
    bool GetCarrotPosition()
    {
        if (GameManager.Instance.carrotPositions.Count == 0) return false;
        int randomIndex = Random.Range(0, GameManager.Instance.carrotPositions.Count);
        targetCarrot = GameManager.Instance.carrotPositions[randomIndex];
        isOG = false;
        //isHungry = false;
        return true;
    }
    bool GetOGCarrotPosition()
    {
        if (GameManager.Instance.ogCarrotPositions.Count == 0) return false;
        int randomIndex = Random.Range(0, GameManager.Instance.ogCarrotPositions.Count);
        targetCarrot = GameManager.Instance.ogCarrotPositions[randomIndex];
        isOG = true;
        //isHungry = false;
        return true;
    }
    void Eat()
    {
        if (!isHungry) return;
        eatDelay -= Time.deltaTime;
        if (eatDelay > 0) return;
        isHungry = false;
        if (!isOG)
        {
            GameManager.Instance.carrotPositions.Remove(targetCarrot);
            GameManager.Instance.vegetationTilemap.SetTile(targetCarrot, null);
            transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
            eatDelay = 1.5f;
        }
        else
        {
            state = State.Overgrown;
            StateChange(2);
            //Evolve!!!
            transform.localScale = new Vector3(1.25f, 1.25f);
            GameManager.Instance.ogCarrotPositions.Remove(targetCarrot);
            eatDelay = 1.5f;
        }
    }
    void StateChange(int stateID)
    {
        Destroy(gfx);
        gfx = Instantiate(gfxPrefabArray[stateID], transform);
    }
    Vector2 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}