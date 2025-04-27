using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Rabbit : MonoBehaviour
{
    public event EventHandler<State> OnStateChange;
    public event EventHandler<bool> OnIsMove;

    [SerializeField] float velocity = 3f;
    Vector3Int targetCarrot = new Vector3Int(6, 1);
    bool isHungry = true, isOG, isDragging;
    float eatDelay = 1.5f;
    int eatedWither = 0, maxEatedWither = 3;

    public enum State
    {
        Casual,
        Angry,
        Overgrown,
        Fed
    }
    State state = State.Casual;
    void Start()
    {
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
                if (isDragging) return;
                if (eatedWither == maxEatedWither) 
                {
                    state = State.Fed;
                    eatedWither = 0;
                    return; 
                }
                CheckForWithering(currentPosition);
                break;
            case State.Fed:
                OnIsMove?.Invoke(this, true);
                transform.localScale = Vector3.one;
                transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
                Vector3 destination = new Vector3(6.3f, 2f, 0f);
                transform.position = Vector3.MoveTowards(transform.position, destination, velocity * Time.deltaTime);
                if (transform.position == destination) Destroy(gameObject);
                break;
        }
        OnStateChange?.Invoke(this, state);
    }
    void CheckForWithering(Vector3Int currentPosition)
    {
        for (int x = currentPosition.x - 1; x < currentPosition.x + 2; x++)
        {
            for (int y = currentPosition.y - 1; y < currentPosition.y + 2; y++)
            {
                Vector3Int witherPosition = new Vector3Int(x, y);
                Debug.Log(witherPosition);
                if (GameManager.Instance.witheringPosition.Contains(witherPosition))
                {
                    OnIsMove?.Invoke(this, true);
                    transform.position = Vector3.MoveTowards(transform.position, witherPosition, velocity * Time.deltaTime);
                    eatedWither++;
                    //Cure withering
                    GameManager.Instance.vegetationTilemap.SetTile(currentPosition, null);
                    GameManager.Instance.witheringPosition.Remove(currentPosition);
                    //Field infected
                    TileBase farmTile = GameManager.Instance.farmTilemap.GetTile(currentPosition);
                    TileBase toBeTile = farmTile;
                    if (UnityEngine.Random.Range(0, 100) >= 50) toBeTile = MapManager.Instance.GetChangedFarmTile(farmTile);
                    GameManager.Instance.farmTilemap.SetTile(currentPosition, toBeTile);
                }
            }
        }
    }
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
        OnIsMove?.Invoke(this, true);
        if (GameManager.Instance.witheringPosition.Contains(targetCarrot)) SearchForCarrot();
        //else if (GameManager.Instance.ogCarrotPositions.Count > 0) SearchForCarrot();
        //else if (GameManager.Instance.carrotPositions.Count > 0) SearchForCarrot();
        Vector3 targetPosition = GameManager.Instance.vegetationTilemap.GetCellCenterWorld(targetCarrot);
        if (isHungry) transform.position = Vector3.MoveTowards(transform.position, targetPosition, velocity * Time.deltaTime);
        else transform.position = Vector3.MoveTowards(transform.position, new Vector3(13f, 0f, 0f), velocity * Time.deltaTime);
        //Eat when reach destination
        if (targetPosition == transform.position) Eat();
        if (transform.position == new Vector3(13f, 0f, 0f)) Destroy(gameObject, 1f);
    }
    void SearchForCarrot()
    {
        //StateChange(0);
        if (GetOGCarrotPosition()) return;
        else if (GetCarrotPosition()) return;
        else state = State.Angry;
    }
    bool GetCarrotPosition()
    {
        if (GameManager.Instance.carrotPositions.Count == 0) return false;
        int randomIndex = UnityEngine.Random.Range(0, GameManager.Instance.carrotPositions.Count);
        targetCarrot = GameManager.Instance.carrotPositions[randomIndex];
        isOG = false;
        //isHungry = false;
        return true;
    }
    bool GetOGCarrotPosition()
    {
        if (GameManager.Instance.ogCarrotPositions.Count == 0) return false;
        int randomIndex = UnityEngine.Random.Range(0, GameManager.Instance.ogCarrotPositions.Count);
        targetCarrot = GameManager.Instance.ogCarrotPositions[randomIndex];
        isOG = true;
        //isHungry = false;
        return true;
    }
    void Eat()
    {
        OnIsMove?.Invoke(this, false);
        if (!isHungry) return;
        AudioManager.Instance.Play("Eat");
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
            //Evolve!!!
            transform.localScale = new Vector3(1.25f, 1.25f);
            GameManager.Instance.ogCarrotPositions.Remove(targetCarrot);
            eatDelay = 1.5f;
        }
    }
    Vector2 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}