using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    public static CoinUI Instance;

    public TMP_Text coinText;

    private int coins = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateCoinText();
    }

    public void AddCoin()
    {
        coins++;
        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        coinText.text = "MONEDAS: " + coins;
    }
}