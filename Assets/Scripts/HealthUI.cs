using UnityEngine;
using UnityEngine.UI;

public class HealtUI : MonoBehaviour
{
    [Header("Referencias")]
    public PlayerHealth playerHealth;
    public Image[] hearts; // Ordenados de izquierda a derecha

    void Start()
    {
        playerHealth.OnLivesChanged += UpdateHearts;
        UpdateHearts(playerHealth.CurrentLives);
    }

    void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnLivesChanged -= UpdateHearts;
    }

    void UpdateHearts(int lives)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].gameObject.SetActive(i < lives);
        }
    }
}