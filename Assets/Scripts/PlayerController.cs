using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rd;
    public float speed = 5f;
    public float jumpForce = 7f;
    private Animator animator;
    private PlayerHealth playerHealth;
    private bool isGrounded;
    private bool facingRight = true;

    [Header("Efectos visuales")]
    public GameObject polvoSaltoPrefabs;

    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        // Durante el empujón no se procesa el movimiento ni el salto
        if (playerHealth.IsKnockedBack) return;

        float move = Input.GetAxis("Horizontal");
        float speedAnimation = Mathf.Abs(move);
        animator.SetFloat("Speed", speedAnimation);
        rd.linearVelocity = new Vector2(move * speed, rd.linearVelocity.y);

        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rd.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            animator.SetBool("isJumping", true);

            if (AudioManager.Instance != null)
                AudioManager.Instance.ReproducirSfx(AudioManager.Instance.sonidoSalto);

            if (polvoSaltoPrefabs != null)
            {
                Instantiate(polvoSaltoPrefabs, transform.position, Quaternion.identity);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}