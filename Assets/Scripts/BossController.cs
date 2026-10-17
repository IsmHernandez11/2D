using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("Vida del Boss")]
    public int maxHealth = 10;
    private int currentHealth;

    [Header("UI")]
    public Slider barraVidaUI;
    public GameObject panelVictoriaUI;

    [Header("Movimiento")]
    public float speed = 2f;
    public Transform player;

    [Header("Ataque")]
    public float attackDistance = 1.5f;
    public int attackDamage = 1;
    public float attackCooldown = 1f;

    [Header("Muerte")]
    public float tiempoAnimacionMuerte = 1.5f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Collider2D bossCollider;

    private bool faseFuria = false;
    private bool facingRight = true;
    private bool estaMuerto = false;

    private float ultimoAtaque = 0f;

    void Start()
    {
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        bossCollider = GetComponent<Collider2D>();

        if (barraVidaUI != null)
        {
            barraVidaUI.maxValue = maxHealth;
            barraVidaUI.value = currentHealth;
        }

        // El panel de victoria empieza oculto
        if (panelVictoriaUI != null)
        {
            panelVictoriaUI.SetActive(false);
        }
    }

    void Update()
    {
        // Si ya murió no hace nada más
        if (estaMuerto)
            return;

        if (player == null)
            return;

        float distance = Mathf.Abs(
            player.position.x - transform.position.x
        );

        // Si está cerca, atacar
        if (distance <= attackDistance)
        {
            rb.linearVelocity = new Vector2(
                0,
                rb.linearVelocity.y
            );

            animator.SetFloat("Speed", 0);

            if (Time.time >= ultimoAtaque + attackCooldown)
            {
                Attack();
                ultimoAtaque = Time.time;
            }

            return;
        }

        // Caminar hacia el jugador
        float direction =
            player.position.x > transform.position.x ? 1f : -1f;

        rb.linearVelocity = new Vector2(
            direction * speed,
            rb.linearVelocity.y
        );

        animator.SetFloat(
            "Speed",
            Mathf.Abs(rb.linearVelocity.x)
        );

        // Girar
        if (direction > 0 && !facingRight)
        {
            Flip();
        }
        else if (direction < 0 && facingRight)
        {
            Flip();
        }

        // Fase de furia
        if (currentHealth <= maxHealth / 2 && !faseFuria)
        {
            faseFuria = true;

            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.red;
            }

            speed = speed * 1.8f;
        }
    }

    public void TakeDamage(int damage)
    {
        // Si ya murió no puede seguir recibiendo daño
        if (estaMuerto)
            return;

        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        if (barraVidaUI != null)
        {
            barraVidaUI.value = currentHealth;
        }

        // Si murió
        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
            return;
        }

        // Animación de golpe
        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }
    }

    void Attack()
    {
        if (estaMuerto)
            return;

        rb.linearVelocity = new Vector2(
            0,
            rb.linearVelocity.y
        );

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        if (Vector2.Distance(
                transform.position,
                player.position
            ) <= attackDistance)
        {
            PlayerHealth playerHealth =
                player.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.RecibirDanio(transform);
            }
        }
    }

    IEnumerator Die()
    {
        estaMuerto = true;

        // Dejar de moverse
        rb.linearVelocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;


        // Animación de muerte
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Esperar a que se vea la animación
        yield return new WaitForSeconds(tiempoAnimacionMuerte);
        rb.bodyType = RigidbodyType2D.Static;

        // Mostrar victoria
        if (panelVictoriaUI != null)
        {
            panelVictoriaUI.SetActive(true);
        }

        // Detener el juego
        Time.timeScale = 0f;
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;

        transform.localScale = scale;
    }
}