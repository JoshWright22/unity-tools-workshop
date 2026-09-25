using UnityEngine;

// Simple camera follow. Lesson 7 swaps this for a Cinemachine Camera.
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smooth = 5f;

    void LateUpdate()
    {
        if (target == null) return;
        var goal = new Vector3(target.position.x, target.position.y + 1f, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, goal, smooth * Time.deltaTime);
    }
}
