using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rd;
    public float speed = 5f;
    public float jumpForce = 7f;
    public bool isGrounded;

    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        rd.velocity = new Vector2(move * speed, rd.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rd.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}