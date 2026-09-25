using UnityEngine;

// Reads everything from an EnemyData asset. Change the asset, not the script.
public class Enemy : MonoBehaviour
{
    public EnemyData data;
    public float patrolDistance = 2f;

    Vector3 start;

    void Start()
    {
        start = transform.position;
        if (data == null) return;
        GetComponent<SpriteRenderer>().color = data.tint;
        transform.localScale = Vector3.one * data.size;
    }

    void Update()
    {
        if (data == null) return;
        float x = Mathf.PingPong(Time.time * data.speed, patrolDistance * 2) - patrolDistance;
        transform.position = start + new Vector3(x, 0, 0);
    }

    void OnGUI()
    {
        var cam = Camera.main;
        if (cam == null || data == null) return;
        Vector3 p = cam.WorldToScreenPoint(transform.position + Vector3.up * data.size);
        GUI.Label(new Rect(p.x - 60, Screen.height - p.y - 40, 140, 40), data.displayName + "\nHP " + data.health + "  DMG " + data.damage);
    }
}
