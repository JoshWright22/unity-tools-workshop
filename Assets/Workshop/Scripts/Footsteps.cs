using UnityEngine;

// Called by an Animation Event on the Walk clip.
public class Footsteps : MonoBehaviour
{
    public AudioClip sound;

    public void Footstep()
    {
        if (sound != null) AudioSource.PlayClipAtPoint(sound, transform.position);
        else Debug.Log("step");
    }
}
