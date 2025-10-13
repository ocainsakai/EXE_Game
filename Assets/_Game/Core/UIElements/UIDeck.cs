using UnityEngine;
using UnityEngine.UI;

public class UIDeck : MonoBehaviour
{
    [SerializeField] DeckManager manager;
    [SerializeField] Image Art;
    private void OnEnable()
    {
        Art.sprite = manager.DeckCover;
    }
}
