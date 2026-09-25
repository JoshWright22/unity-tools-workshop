using UnityEngine;

// Feeds the Animator its parameters. The Animator decides what to do with them.
public class PlayerAnimator : MonoBehaviour
{
    Animator animator;
    Rigidbody2D body;
    PlayerMover mover;
    SpriteRenderer sprite;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        body = GetComponent<Rigidbody2D>();
        mover = GetComponent<PlayerMover>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        float vx = body.linearVelocity.x;
        animator.SetFloat("Speed", Mathf.Abs(vx));
        animator.SetBool("Grounded", mover == null || mover.IsGrounded);
        if (vx > 0.1f) sprite.flipX = false;
        else if (vx < -0.1f) sprite.flipX = true;
    }
}
