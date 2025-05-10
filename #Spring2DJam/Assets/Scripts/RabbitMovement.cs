using System;
using UnityEngine;

public class RabbitMovement : MonoBehaviour
{
    public event EventHandler<State> OnStateChange;
    public event EventHandler<bool> OnIsMove;
    public event EventHandler<bool> OnSpriteFlip;

    [SerializeField] float velocity = 3f;
    public enum State
    {
        Casual,
        Angry,
        Overgrown,
        Fed
    }
    State state = State.Casual;
    bool isDragging, isFoundOG;

    void FixedUpdate()
    {
        Move();
    }
    void OnMouseDrag()
    {
        if (state != State.Overgrown) return;
        isDragging = true;
        transform.position = (Vector3)GetMouseWorldPosition();
        //Change visual (fire event)
    }
    void OnMouseExit()
    {
        isDragging = false;
    }
    void Move()
    {
        //FlipSprite(destination);

        switch (state)
        {
            case State.Casual:
                //search for carrot
                //if there is a carrot, move to the carrot
                //if there isn't a carrot, state = State.Angry
                break;
            case State.Angry:
                //search for carrot
                //move to a random field
                break;
            case State.Overgrown:
                //move to neighbour tiles where has withering
                //if no withering start to move random direction
                //check for withering again
                break;
            case State.Fed:
                //move back to the spawn point
                break;
        }
        OnStateChange?.Invoke(this, state);
    }
    void Eat()
    {
        //go to carrot or wither
        //play eat sound
        //wait a few second
        //move on
    }
    void FlipSprite(Vector3 destination)
    {
        if (destination.x < transform.position.x) OnSpriteFlip?.Invoke(this, true);
        else if (destination.x > transform.position.x) OnSpriteFlip?.Invoke(this, false);
    }
    void SearchForCarrot()
    {
        if (GameManager.Instance.ogCarrotPositionList.Count != 0)
        {
            //There is at least one OG carrot
            isFoundOG = true;
            int randomIndex = UnityEngine.Random.Range(0, GameManager.Instance.ogCarrotPositionList.Count);
            Vector3Int targetCarrot = GameManager.Instance.ogCarrotPositionList[randomIndex];
            Vector3 targetPosition = GameManager.Instance.vegetationTilemap.GetCellCenterWorld(targetCarrot);
        }
        else if (GameManager.Instance.carrotPositionList.Count != 0)
        {
            //There is at least one carrot
            isFoundOG = false;
            int randomIndex = UnityEngine.Random.Range(0, GameManager.Instance.carrotPositionList.Count);
            Vector3Int targetCarrot = GameManager.Instance.carrotPositionList[randomIndex];
        }
        else
        {
            //There is no carrot
            state = State.Angry;
        }
    }
    Vector2 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}