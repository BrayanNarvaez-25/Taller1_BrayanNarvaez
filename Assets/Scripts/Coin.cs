using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (CoinUI.Instance != null)
            CoinUI.Instance.AddCoin();

        Destroy(gameObject);
    }
}