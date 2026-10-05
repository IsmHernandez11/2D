using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rd;
    public float speed = 5f; // es 5 float, lo convierte a decimal
    public float jumForce = 7f;
    private Animator animator;
    private bool facinRight = true;

    private bool isGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        
        float speedAnimation = Mathf.Abs(move);
        animator.SetFloat("Speed", speedAnimation);

        rd.velocity = new Vector2(move * speed, rd.velocity.y);

       
        if(move > 0 && !facinRight)
        {
            Flip();
        }else if(move < 0 && facinRight)
        {
            Flip();
        }

        //salto

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rd.AddForce(Vector2.up * jumForce, ForceMode2D.Impulse);
            isGrounded = false;
            animator.SetBool("isJump",true);
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
