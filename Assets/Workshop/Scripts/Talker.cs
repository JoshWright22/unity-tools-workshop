using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

// Walk up to this and press E to start a Yarn node.
public class Talker : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public string startNode = "Start";

    bool playerNear;

    void Update()
    {
        if (!playerNear || dialogueRunner == null || dialogueRunner.IsDialogueRunning) return;
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            dialogueRunner.StartDialogue(startNode);
    }

    void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Player")) playerNear = true; }
    void OnTriggerExit2D(Collider2D other) { if (other.CompareTag("Player")) playerNear = false; }

    void OnGUI()
    {
        if (!playerNear || dialogueRunner == null || dialogueRunner.IsDialogueRunning) return;
        var p = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.2f);
        GUI.Label(new Rect(p.x - 40, Screen.height - p.y - 20, 120, 30), "<b>Press E</b>");
    }
}
