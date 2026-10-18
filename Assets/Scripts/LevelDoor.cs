using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDoor : MonoBehaviour
{
    [Header("Cambio de nivel")]
    public string nombreSiguienteEscena = "Nivel2";

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.gameObject.name == "Player")
        {
            SceneManager.LoadScene(nombreSiguienteEscena);
        }
    }
}