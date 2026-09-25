using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter Instance;
    public int score;

    void Awake() { Instance = this; }

    public void Add(int amount) { score += amount; }

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width - 160, 10, 150, 40), "<size=22><b>Coins: " + score + "</b></size>");
    }
}
