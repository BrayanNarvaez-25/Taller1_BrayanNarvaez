using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [Header("Salud")]
    public int maxHealth = 10;
    private int currentHealth;

    [Header("UI")]
    public Slider barraVidaUI;
    public GameObject panelVictoriaUI;

    [Header("Movimiento")]
    public float moveSpeed = 2f;

    [Header("Jugador")]
    public Transform player;
    public SpriteRenderer spriteRenderer;

    // Fase de Furia (no se muestran en el Inspector)
    private const float multiplicadorFuria = 1.8f; // +80%
    private readonly Color colorFuria = Color.red;
    private const float distanciaMinima = 0.1f;

    private bool enFuria;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (player == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null) player = obj.transform;
        }

        currentHealth = maxHealth;

        if (barraVidaUI != null)
        {
            barraVidaUI.maxValue = maxHealth;
            barraVidaUI.value = maxHealth;
        }

        // El panel debe empezar oculto
        if (panelVictoriaUI != null)
            panelVictoriaUI.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        float distanciaX = player.position.x - rb.position.x;
        if (Mathf.Abs(distanciaX) <= distanciaMinima) return;

        float direccion = Mathf.Sign(distanciaX);
        Vector2 destino = new Vector2(rb.position.x + direccion * moveSpeed * Time.deltaTime, rb.position.y);

        rb.MovePosition(destino);
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);

        if (barraVidaUI != null)
            barraVidaUI.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        if (!enFuria && currentHealth <= maxHealth / 2)
            EntrarEnFuria();
    }

    void EntrarEnFuria()
    {
        enFuria = true;
        moveSpeed *= multiplicadorFuria;

        if (spriteRenderer != null)
            spriteRenderer.color = colorFuria;
    }

    void Die()
    {
        if (panelVictoriaUI != null)
            panelVictoriaUI.SetActive(true);

        Time.timeScale = 0f;

        Destroy(gameObject);
    }
}