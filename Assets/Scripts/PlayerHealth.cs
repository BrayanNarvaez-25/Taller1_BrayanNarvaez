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

    public bool IsKnockedBack { get; private set; }

    private bool isInvulnerable;
    private bool isDead;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        // En Awake para que la vida ya esté lista cuando HealtUI lea el valor en su Start
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
        TryTakeDamage(collision);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        TryTakeDamage(collision);
    }

    void TryTakeDamage(Collision2D collision)
    {
        if (isDead || isInvulnerable) return;
        if (!collision.gameObject.CompareTag("Enemy")) return;

        currentLives--;
        OnLivesChanged?.Invoke(currentLives);
        animator.SetTrigger("Hurt");

        if (currentLives <= 0)
        {
            Die();
            return;
        }

        float direction = Mathf.Sign(transform.position.x - collision.transform.position.x);
        if (direction == 0) direction = 1;

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