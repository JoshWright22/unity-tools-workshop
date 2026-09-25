using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float speed = 90f;  // a knob

    void Update()  // every frame
    {
        float step = speed * Time.deltaTime;
        transform.Rotate(0, 0, step);
    }
}
