using UnityEngine;

public class Rabbit : MonoBehaviour
{
    [SerializeField] GameObject[] gfxPrefabArray;

    float velocity = 3f;
    Vector3Int targetCarrot = new Vector3Int(6, 2);
    bool isHungry = true, isOG;
    GameObject gfx;
    public enum State
    {
        Casual,
        Angry,
        Overgrown,
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
                Vector3Int currentPosition = new Vector3Int((int)transform.position.x, (int)transform.position.y);
                if (GameManager.Instance.hasWithering.Contains(currentPosition))
                {
                    Debug.Log("OG Rabbit ate withering in " + currentPosition);
                    //Cure withering
                    GameManager.Instance.vegetationTilemap.SetTile(currentPosition, null);
                    GameManager.Instance.hasWithering.Remove(currentPosition);
                    //Field infected
                }
                break;
        }
    }
    //}
    void OnMouseDrag()
    {
        if (state != State.Overgrown) return;
        transform.position = (Vector3)GetMouseWorldPosition();
    }
    void Move()
    {
        if (GameManager.Instance.hasWithering.Contains(targetCarrot)) return;//SearchForCarrot();
        Vector3 targetPosition = GameManager.Instance.vegetationTilemap.GetCellCenterWorld(targetCarrot);
        if (isHungry) transform.position = Vector3.MoveTowards(transform.position, targetPosition, velocity * Time.deltaTime);
        else transform.position = Vector3.MoveTowards(transform.position, new Vector3(13f, 0f, 0f), velocity * Time.deltaTime);
        //Eat when reach destination
        if (targetPosition == transform.position) Eat();
        if (transform.position == new Vector3(13f, 0f, 0f)) Destroy(gameObject, 1f);
    }
    void SearchForCarrot()
    {
        while (targetCarrot == new Vector3Int(6,2))
        {
            StateChange(0);
            if (GetOGCarrotPosition()) return;
            else if (GetCarrotPosition()) return;
            else StateChange(1); 
        }
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
        //if (!isHungry) return;
        isHungry = false;
        if (!isOG)
        {
            GameManager.Instance.carrotPositions.Remove(targetCarrot);
            transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
        }
        else
        {
            state = State.Overgrown;
            StateChange(2);
            //Evolve!!!
            transform.localScale = new Vector3(1.25f, 1.25f);
            GameManager.Instance.ogCarrotPositions.Remove(targetCarrot);
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