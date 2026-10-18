using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Fuentes de Audio")]
    public AudioSource fuenteMusica;
    public AudioSource fuenteSFX;

    [Header("Clips de Sonido")]
    public AudioClip musicaFondo;
    public AudioClip sonidoSalto;
    public AudioClip sonidoGolpe;
    public AudioClip sonidoMoneda;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (Instance != this) return;

        if (musicaFondo != null && fuenteMusica != null)
        {
            fuenteMusica.clip = musicaFondo;
            fuenteMusica.loop = true;
            fuenteMusica.Play();
        }
    }

    public void ReproducirSfx(AudioClip clip)
    {
        if (clip != null && fuenteSFX != null)
        {
            fuenteSFX.PlayOneShot(clip);
        }
    }
}