using UnityEngine;

// Adds to the score. Call Collect() from a UnityEvent (like Touched's On Touched).
public class Collectible : MonoBehaviour
{
    public int value = 1;

    public void Collect()
    {
        if (ScoreCounter.Instance != null) ScoreCounter.Instance.Add(value);
    }
}
