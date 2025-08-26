using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Image sceneHover;

    private void Awake()
    {
        sceneHover.gameObject.SetActive(false);
    }
    public void LoadScene(Action onComplete)
    {
        sceneHover.DOFade(1, 1f).From(0).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            sceneHover.gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }
}
