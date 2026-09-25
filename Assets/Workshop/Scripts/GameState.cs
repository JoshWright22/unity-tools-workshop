using UnityEngine;

// Call Win() from a UnityEvent to end the game.
public class GameState : MonoBehaviour
{
    bool won;
    float time;

    public bool Won => won;

    void Update()
    {
        if (!won) time += Time.deltaTime;
    }

    public void Win()
    {
        won = true;
    }

    void OnGUI()
    {
        if (!won) return;
        var style = new GUIStyle(GUI.skin.label) { fontSize = 48, alignment = TextAnchor.MiddleCenter, richText = true };
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "<b>You win!</b>\n<size=24>" + time.ToString("0.0") + " seconds</size>", style);
    }
}
