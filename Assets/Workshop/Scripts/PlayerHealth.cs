using System.Collections;
using UnityEngine;

// Getting hit flashes the sprite and sends you back to the start.
// The flash only shows if the Sprite Renderer uses the Sprite Flash material.
public class PlayerHealth : MonoBehaviour
{
    public float flashTime = 0.25f;

    Vector3 spawn;
    SpriteRenderer sprite;
    Rigidbody2D body;
    MaterialPropertyBlock block;

    void Start()
    {
        spawn = transform.position;
        sprite = GetComponentInChildren<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        block = new MaterialPropertyBlock();
        SetFlash(0);
    }

    public void Hit()
    {
        StopAllCoroutines();
        StartCoroutine(Flash());
        transform.position = spawn;
        if (body != null) body.linearVelocity = Vector2.zero;
    }

    IEnumerator Flash()
    {
        for (float t = 0; t < flashTime; t += Time.deltaTime)
        {
            SetFlash(1 - t / flashTime);
            yield return null;
        }
        SetFlash(0);
    }

    void SetFlash(float amount)
    {
        sprite.GetPropertyBlock(block);
        block.SetFloat("_FlashAmount", amount);
        sprite.SetPropertyBlock(block);
    }
}
