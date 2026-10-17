using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    public static CoinUI Instance;

    [Header("Referencias")]
    public TMP_Text coinText;

    private int coins;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateText();
    }

    public void AddCoin()
    {
        coins++;
        UpdateText();
    }

    void UpdateText()
    {
        coinText.text = "Monedas: " + coins;
    }
}