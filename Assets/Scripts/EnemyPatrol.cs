using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Ajustes de movimiento")]
    public float speed = 3f;
    private bool movingRight = true;

    [Header("Detectores")]
    public Transform groundCheck;
    public float distanceToGround = 1.5f;
    public float distanceToWall = 1.5f;
    public LayerMask groundLayer;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the enemy
        rb.velocity = new Vector2(movingRight ? speed : -speed, rb.velocity.y);

        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        RaycastHit2D isGroundAhead = Physics2D.Raycast(groundCheck.position, Vector2.down, distanceToGround, groundLayer);
        RaycastHit2D isWallAhead = Physics2D.Raycast(transform.position, direction, distanceToWall, groundLayer);

        if (isGroundAhead.collider == null || isWallAhead.collider != null)
        {
            Flip();
        }
    }

    void Flip()
    {
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * distanceToGround);
        }
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)direction * distanceToWall);
    }
}
