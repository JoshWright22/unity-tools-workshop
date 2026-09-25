using UnityEngine;
using UnityEngine.InputSystem;

// A/D or arrows to move, Space to jump.
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviour
{
    public float speed = 6f;
    public float jumpForce = 12f;

    Rigidbody2D body;
    Collider2D col;

    public bool IsGrounded { get; private set; }

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        body.freezeRotation = true;
    }

    void Update()
    {
        IsGrounded = CheckGround();

        var keys = Keyboard.current;
        if (keys == null) return;

        float x = 0;
        if (keys.aKey.isPressed || keys.leftArrowKey.isPressed) x -= 1;
        if (keys.dKey.isPressed || keys.rightArrowKey.isPressed) x += 1;

        var v = body.linearVelocity;
        v.x = x * speed;
        if (keys.spaceKey.wasPressedThisFrame && IsGrounded) v.y = jumpForce;
        body.linearVelocity = v;
    }

    bool CheckGround()
    {
        if (col == null) return true;
        var b = col.bounds;
        var hits = Physics2D.BoxCastAll(b.center, new Vector2(b.size.x * 0.9f, 0.05f), 0, Vector2.down, b.extents.y + 0.05f);
        foreach (var hit in hits)
            if (hit.collider != col && !hit.collider.isTrigger) return true;
        return false;
    }
}
