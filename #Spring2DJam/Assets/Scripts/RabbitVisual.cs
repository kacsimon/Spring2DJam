using UnityEngine;

public class RabbitVisual : MonoBehaviour
{
    const string IS_MOVE = "isMove";
    const string IS_ANGRY = "isAngry";
    const string IS_OG = "isOvergrown";
    const string IS_FED = "isFed";

    Rabbit rabbit;
    Animator animator;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        rabbit = GetComponentInParent<Rabbit>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        rabbit.OnStateChange += Rabbit_OnStateChange;
        rabbit.OnIsMove += Rabbit_OnIsMove;
        rabbit.OnSpriteFlip += Rabbit_OnSpriteFlip;
    }
    void Rabbit_OnIsMove(object sender, bool _bool)
    {
        animator.SetBool(IS_MOVE, _bool);
    }
    void Rabbit_OnStateChange(object sender, Rabbit.State state)
    {
        switch (state)
        {
            case Rabbit.State.Casual:
                DisableAllAnimation();
                animator.SetBool(IS_FED, true);
                break;
            case Rabbit.State.Angry:
                DisableAllAnimation();
                animator.SetBool(IS_ANGRY, true);
                break;
            case Rabbit.State.Overgrown:
                DisableAllAnimation();
                animator.SetBool(IS_OG, true);
                break;
            case Rabbit.State.Fed:
                DisableAllAnimation();
                animator.SetBool(IS_FED, true);
                break;
        }
    }
    void Rabbit_OnSpriteFlip(object sender, bool _bool)
    {
        spriteRenderer.flipX = _bool;
    }
    void DisableAllAnimation()
    {
        animator.SetBool(IS_ANGRY, false);
        animator.SetBool(IS_OG, false);
        animator.SetBool(IS_FED, false);
    }
}