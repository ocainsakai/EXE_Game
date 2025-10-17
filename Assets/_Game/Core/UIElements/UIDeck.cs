using _Game.Addons.Deck.Scripts.CardCollection;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIDeck : MonoBehaviour
{
    [SerializeField] DeckManager manager;
    [FormerlySerializedAs("Art")] [SerializeField] Image art;
    private void OnEnable()
    {
        art.sprite = manager.DeckCover;
    }
}
