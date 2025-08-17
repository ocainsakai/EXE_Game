using UnityEngine;
using UniRx;
using System;
using TMPro;
public class CoinView : MonoBehaviour
{
    [SerializeField] CoinRSO coinRSO;
    [SerializeField] TextMeshProUGUI coinText;
    public void Awake()
    {
        coinRSO.onwnerCoins.Subscribe(coin => UpdateCoinText(coin)).AddTo(this);
    }

    private void UpdateCoinText(int coin)
    {
        coinText.text = $"COIN: {coin}";
    }
}
