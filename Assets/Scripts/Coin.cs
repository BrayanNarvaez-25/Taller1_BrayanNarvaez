using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (CoinUI.Instance != null)
            CoinUI.Instance.AddCoin();

        if (AudioManager.Instance != null)
            AudioManager.Instance.ReproducirSfx(AudioManager.Instance.sonidoMoneda);

        Destroy(gameObject);
    }
}