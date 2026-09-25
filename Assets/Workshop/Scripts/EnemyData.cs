using UnityEngine;

// Right-click in the Project window > Create > Workshop > Enemy Data
[CreateAssetMenu(menuName = "Workshop/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string displayName = "Slime";
    public int health = 20;
    public float speed = 1.5f;
    public int damage = 5;
    public Color tint = Color.white;
    public float size = 1f;
}
