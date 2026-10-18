using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    public int maxLives = 3;
    private int currentLives;

    public int CurrentLives => currentLives;
    public event Action<int> OnLivesChanged;

    [Header("Invulnerabilidad")]
    public float invulnerabilityTime = 1f;
    public float blinkInterval = 0.1f;

    [Header("Empujón")]
    public float knockbackForceX = 6f;
    public float knockbackForceY = 4f;
    public float knockbackDuration = 0.25f;

    [Header("Sacudida de cámara")]
    public float shakeDuration = 0.2f;
    public float shakeStrength = 0.15f;

    public bool IsKnockedBack { get; private set; }

    private bool isInvulnerable;
    private bool isDead;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        currentLives = maxLives;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        CheckEnemy(collision);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        CheckEnemy(collision);
    }

    void CheckEnemy(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            RecibirDanio(collision.transform);
        }
    }

    // Método público: lo usan enemigos, trampas, proyectiles, etc.
    public void RecibirDanio(Transform fuente, int cantidad = 1)
    {
        if (isDead || isInvulnerable) return;

        currentLives = Mathf.Max(0, currentLives - cantidad);
        OnLivesChanged?.Invoke(currentLives);
        animator.SetTrigger("Hurt");

        // Sacudida de cámara (se ignora si no hay CameraShake en la escena)
        if (CameraShake.Instance != null)
            CameraShake.Instance.Sacudir(shakeDuration, shakeStrength);

        if (currentLives <= 0)
        {
            Die();
            return;
        }

        float direction = Mathf.Sign(transform.position.x - fuente.position.x);

        StartCoroutine(KnockbackRoutine(direction));
        StartCoroutine(InvulnerabilityRoutine());
    }

    IEnumerator KnockbackRoutine(float direction)
    {
        IsKnockedBack = true;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(direction * knockbackForceX, knockbackForceY), ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        IsKnockedBack = false;
    }

    IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;

        float elapsed = 0f;
        while (elapsed < invulnerabilityTime)
        {
            if (spriteRenderer != null)
                spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

        isInvulnerable = false;
    }

    void Die()
    {
        isDead = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}