using System.Collections;
using UnityEngine;

// Blocks the way until Open() is called (the gatekeeper's <<open_gate>> command does it).
// Dissolves away if its Sprite Renderer uses the Sprite Dissolve material.
public class Gate : MonoBehaviour
{
    public float openTime = 1.2f;

    public void Open()
    {
        StartCoroutine(Dissolve());
    }

    IEnumerator Dissolve()
    {
        var sprite = GetComponent<SpriteRenderer>();
        var block = new MaterialPropertyBlock();
        for (float t = 0; t < openTime; t += Time.deltaTime)
        {
            sprite.GetPropertyBlock(block);
            block.SetFloat("_Amount", t / openTime);
            sprite.SetPropertyBlock(block);
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
