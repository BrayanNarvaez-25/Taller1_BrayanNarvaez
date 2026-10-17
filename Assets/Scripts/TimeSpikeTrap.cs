using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSpikeTrap : MonoBehaviour
{
    [Header("Ajustes del Tiempo")]
    public float waitTime = 2f;
    public float activeTime = 1f;

    [Header("Daño de la trampa")]
    public int damageAmount = 1;

    [Header("Estado Trampa")]
    public bool isDangerous = false;

    private Animator animator;
    private readonly List<PlayerHealth> playersInside = new List<PlayerHealth>();

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(TrapRoutine());
    }

    void Update()
    {
        if (!isDangerous) return;

        foreach (PlayerHealth player in playersInside)
        {
            if (player != null)
                player.RecibirDanio(transform, damageAmount);
        }
    }

    IEnumerator TrapRoutine()
    {
        while (true)
        {
            isDangerous = false;
            yield return new WaitForSeconds(waitTime);

            if (animator != null)
            {
                animator.SetTrigger("Active");
            }

            isDangerous = true;
            yield return new WaitForSeconds(activeTime);

            isDangerous = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth player = other.GetComponentInParent<PlayerHealth>();
        if (player != null && !playersInside.Contains(player))
            playersInside.Add(player);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        PlayerHealth player = other.GetComponentInParent<PlayerHealth>();
        if (player != null)
            playersInside.Remove(player);
    }
}