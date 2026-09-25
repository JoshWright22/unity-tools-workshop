using UnityEngine;
using UnityEngine.Events;

// Put this on anything with a trigger collider. Wire up what happens in the Inspector.
public class Touched : MonoBehaviour
{
    public UnityEvent onTouched;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            onTouched.Invoke();
    }
}
