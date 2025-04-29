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
    bool isDragging;

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
                //if there is a carrot, move to the carrot
                //if there isn't a carrot, state = State.Angry
                break;
            case State.Angry:
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
    Vector2 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}