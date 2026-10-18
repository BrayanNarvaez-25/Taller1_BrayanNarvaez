using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private Vector3 posicionOriginal;
    private Coroutine sacudidaActual;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Sacudir(float duracion, float fuerza)
    {
        // Si ya hay una sacudida en curso, la corta y devuelve la cámara a su sitio
        if (sacudidaActual != null)
        {
            StopCoroutine(sacudidaActual);
            transform.localPosition = posicionOriginal;
        }

        sacudidaActual = StartCoroutine(RutinaSacudida(duracion, fuerza));
    }

    IEnumerator RutinaSacudida(float duracion, float fuerza)
    {
        posicionOriginal = transform.localPosition;

        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracion)
        {
            float offsetX = Random.Range(-1f, 1f) * fuerza;
            float offsetY = Random.Range(-1f, 1f) * fuerza;

            transform.localPosition = new Vector3(
                posicionOriginal.x + offsetX,
                posicionOriginal.y + offsetY,
                posicionOriginal.z
            );

            tiempoTranscurrido += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = posicionOriginal;
        sacudidaActual = null;
    }
}