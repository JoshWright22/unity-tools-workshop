using UnityEngine;

// Hurts the player on touch. Needs a trigger collider.
public class Hazard : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        var health = other.GetComponent<PlayerHealth>();
        if (health != null) health.Hit();
    }
}
