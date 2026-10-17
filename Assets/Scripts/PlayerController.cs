using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rd;

    public float speed = 5f;
    public float jumForce = 7f;

    private Animator animator;
    private bool facinRight = true;

    private bool isGrounded;
    private PlayerHealth playerHealth;

    [Header("Estado del jugador")]
    public bool puedeMoverse = true;

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
        float move = Input.GetAxis("Horizontal");

        // Mientras recibe el empujón,
        // las teclas NO controlan el Rigidbody
        if (playerHealth.IsKnockback)
        {
            animator.SetFloat("Speed", 0);
            return;
        }

        // Si no puede moverse, detenemos el movimiento horizontal
        if (!puedeMoverse)
        {
            rd.linearVelocity = new Vector2(0, rd.linearVelocity.y);
            animator.SetFloat("Speed", 0);
            return;
        }

        float speedAnimation = Mathf.Abs(move);
        animator.SetFloat("Speed", speedAnimation);

        rd.linearVelocity = new Vector2(
            move * speed,
            rd.linearVelocity.y
        );

        if (move > 0 && !facinRight)
        {
            Flip();
        }
        else if (move < 0 && facinRight)
        {
            Flip();
        }

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && puedeMoverse)
        {
            rd.AddForce(
                Vector2.up * jumForce,
                ForceMode2D.Impulse
            );

            isGrounded = false;
            animator.SetBool("isJump", true);

            if (polvoSaltoPrefabs != null)
            {
                Instantiate(
                    polvoSaltoPrefabs,
                    transform.position,
                    Quaternion.identity
                );
            }

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.ReproducirSfx(
                    AudioManager.Instance.sonidoSalto
                );
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJump", false);
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
        facinRight = !facinRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;

        transform.localScale = scale;
    }
}