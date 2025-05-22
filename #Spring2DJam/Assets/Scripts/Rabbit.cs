using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Rabbit : MonoBehaviour
{
    public event EventHandler<State> OnStateChange;
    public event EventHandler<bool> OnIsMove;
    public event EventHandler<bool> OnSpriteFlip;

    [SerializeField] float velocity = 3f;
    [SerializeField] TileBase ruinedTilePrefab;
    public enum State
    {
        Casual,
        Angry,
        Overgrown,
        Fed
    }
    State state = State.Casual;
    Vector3Int targetCarrot = new Vector3Int(6, 1);
    bool isHungry = true, isFoundOG, isDragging, isFindField = false;
    float eatDelay = 1.5f;
    int eatedWither = 0, maxEatedWither = 3;

    void Start()
    {
        FlipSprite(targetCarrot);
        SearchForCarrot();
    }
    void FixedUpdate()
    {
        OnStateChange?.Invoke(this, state);
        switch (state)
        {
            case State.Casual:
                Move();
                break;
            case State.Angry:
                Move();
                //Vector3Int currentPosition = new Vector3Int((int)transform.position.x, (int)transform.position.y);
                //if (!isFindField) CheckForField(currentPosition);
                //else
                //{
                //    Debug.Log("Ruin field at " + currentPosition);
                //    //Ruin your field
                //    GameManager.Instance.interactableTilemap.SetTile(currentPosition, ruinedTilePrefab);
                //    isFindField = false;
                //    state = State.Fed;
                //    //Move();
                //}
                break;
            case State.Overgrown:
                //World position to Cell Position
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
                Vector3 destination = new Vector3(6.3f, 2f, 0f);
                transform.position = Vector3.MoveTowards(transform.position, destination, velocity * Time.deltaTime);
                FlipSprite(destination);
                if (transform.position == destination) Destroy(gameObject);
                break;
        }
    }
    void CheckForField(Vector3Int currentPosition)
    {
        for (int x = currentPosition.x - 1; x <= currentPosition.x + 1; x++)
        {
            for (int y = currentPosition.y - 1; y <= currentPosition.y + 1; y++)
            {
                //Neighbour positions and current (all 9)
                Vector3Int checkPos = new Vector3Int(x, y, 0);
                TileData interactableData = MapManager.Instance.GetTileDataFromMap(GameManager.Instance.interactableTilemap, checkPos);
                if (interactableData != null) return;
                TileData data = MapManager.Instance.GetTileDataFromMap(GameManager.Instance.farmTilemap, checkPos);
                if (data == null || !data.canPlant)
                {
                    transform.position = Vector3.MoveTowards(transform.position, Vector3.left, velocity * Time.deltaTime);
                    isFindField = false;
                    Debug.Log("Can't find field");
                }
                else
                {
                    Vector3 targetPosition = GameManager.Instance.farmTilemap.GetCellCenterWorld(checkPos);
                    if (Vector3.Distance(transform.position, targetPosition) > .1f)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, targetPosition, velocity * Time.deltaTime);
                    }
                    Debug.Log("Field in " + checkPos + " position");
                    if (Vector3.Distance(transform.position, targetPosition) < .3f)
                    {
                        isFindField = true;
                        return;
                    }
                }
            }
        }
    }
    void CheckForWithering(Vector3Int currentPosition)
    {
        if (isDragging) return;
        for (int x = currentPosition.x - 1; x <= currentPosition.x + 1; x++)
        {
            for (int y = currentPosition.y - 1; y <= currentPosition.y + 1; y++)
            {
                //Neighbour positions and current (all 9)
                Vector3Int checkPos = new Vector3Int(x, y, 0);

                if (GameManager.Instance.witheringPositionList.Contains(checkPos))
                {
                    OnIsMove?.Invoke(this, true);
                    //Store it
                    TileBase tileWithWithering = GameManager.Instance.farmTilemap.GetTile(checkPos);
                    //TODO: choose random one to move towards
                    // Move toward it
                    Vector3 targetPosition = GameManager.Instance.farmTilemap.GetCellCenterWorld(checkPos);
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, velocity * Time.deltaTime);
                    FlipSprite(targetPosition);
                    //Eat when close enough
                    if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                    {
                        AudioManager.Instance.Play("EatWither");
                        eatDelay -= Time.deltaTime;
                        if (eatDelay > 0) return;
                        // Eat it
                        eatedWither++;
                        GameManager.Instance.witheringPositionList.Remove(checkPos);
                        GameManager.Instance.vegetationTilemap.SetTile(checkPos, null);
                        eatDelay = 1.5f;
                        // Set farm tile infected
                        if (UnityEngine.Random.Range(0, 100) >= 50) //50% chance
                        {
                            MapManager.Instance.ChangeFarmTile(GameManager.Instance.farmTilemap, checkPos);
                        }
                        //Became fed
                        if (eatedWither >= maxEatedWither)
                        {
                            state = State.Fed;
                            transform.localScale = Vector3.one;
                            return;
                        }
                    }

                    return; // Eat only one per frame
                }
                else
                {
                    //Move random direction
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
        if (GameManager.Instance.witheringPositionList.Contains(targetCarrot)) SearchForCarrot();
        //else if (GameManager.Instance.ogCarrotPositions.Count > 0) SearchForCarrot();
        //else if (GameManager.Instance.carrotPositions.Count > 0) SearchForCarrot();
        Vector3 targetPosition = GameManager.Instance.vegetationTilemap.GetCellCenterWorld(targetCarrot);
        if (isHungry)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, velocity * Time.deltaTime);
            FlipSprite(targetCarrot);
        }
        else
        {
            Vector3 destination = new Vector3(13f, 0f, 0f);
            transform.position = Vector3.MoveTowards(transform.position, destination, velocity * Time.deltaTime);
            FlipSprite(destination);
        }
        //AudioManager.Instance.Play("Jump");
        //Eat when reach destination
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f) Eat();
        if (transform.position == new Vector3(13f, 0f, 0f)) Destroy(gameObject, 1f);
    }
    void SearchForCarrot()
    {
        if (GameManager.Instance.ogCarrotPositionList.Count != 0)
        {
            //There is at least one OG carrot
            int randomIndex = UnityEngine.Random.Range(0, GameManager.Instance.ogCarrotPositionList.Count);
            targetCarrot = GameManager.Instance.ogCarrotPositionList[randomIndex];
            isFoundOG = true;
            state = State.Casual;
        }
        else if (GameManager.Instance.carrotPositionList.Count != 0)
        {
            //There is at least one carrot
            int randomIndex = UnityEngine.Random.Range(0, GameManager.Instance.carrotPositionList.Count);
            targetCarrot = GameManager.Instance.carrotPositionList[randomIndex];
            isFoundOG = false;
            state = State.Casual;
        }
        else
        {
            //There is no carrot
            state = State.Angry;
        }
    }
    void Eat()
    {
        OnIsMove?.Invoke(this, false);
        if (!isHungry) return;
        AudioManager.Instance.Play("EatCarrot");
        eatDelay -= Time.deltaTime;
        if (eatDelay > 0) return;
        isHungry = false;
        if (!isFoundOG)
        {
            GameManager.Instance.carrotPositionList.Remove(targetCarrot);
            GameManager.Instance.vegetationTilemap.SetTile(targetCarrot, null);
            eatDelay = 1.5f;
        }
        else
        {
            state = State.Overgrown;
            //Evolve!!!
            transform.localScale = new Vector3(1.25f, 1.25f);
            GameManager.Instance.ogCarrotPositionList.Remove(targetCarrot);
            eatDelay = 1.5f;
        }
    }
    void FlipSprite(Vector3 destination)
    {
        if (destination.x < transform.position.x) OnSpriteFlip?.Invoke(this, true);
        else if (destination.x > transform.position.x) OnSpriteFlip?.Invoke(this, false);
    }
    Vector2 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}