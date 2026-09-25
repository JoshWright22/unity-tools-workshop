using UnityEngine;
using UnityEngine.InputSystem;

// Shows the lesson steps in the corner of the Game view. Press H to hide it.
public class LessonInfo : MonoBehaviour
{
    public string title = "Lesson";
    [TextArea(5, 20)] public string steps = "";

    bool hidden;
    GUIStyle box;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.hKey.wasPressedThisFrame)
            hidden = !hidden;
    }

    void OnGUI()
    {
        if (hidden) return;
        if (box == null)
        {
            box = new GUIStyle(GUI.skin.box) { alignment = TextAnchor.UpperLeft, wordWrap = true, fontSize = 14, richText = true };
            box.padding = new RectOffset(10, 10, 8, 8);
        }
        int lines = steps.Split('\n').Length;
        GUI.Box(new Rect(10, 10, 380, 50 + 20 * lines), "<b>" + title + "</b>   (H to hide)\n\n" + steps, box);
    }
}
