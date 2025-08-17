using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;

    private void Awake()
    {
        Buuble.Break += BreakBubble;
    }
    [ContextMenu("play")]
    public void BreakBubble()
    {
        audioSource.Play();
    }
}
