using BulletHellTemplate;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public AudioClip backgroundSceneMusic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        AudioManager.Singleton.PlayAmbientAudio(backgroundSceneMusic, "master");
    }

    
}
