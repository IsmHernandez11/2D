using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vidas")]
    public int lives = 3;

    [Header("Daño")]
    public float invulnerabilityTime = 1f;

    [Header("Empujón")]
    public float knockbackForce = 7f;
    public float knockbackUpForce = 4f;
    public float knockbackTime = 0.25f;

    private bool isInvulnerable = false;

    public bool IsKnockback { get; private set; }

    private Rigidbody2D rd;
    private Animator animator;

    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isInvulnerable)
        {
            RecibirDanio(collision.transform);
        }
    }

    public void RecibirDanio(Transform enemy)
    {
        if (isInvulnerable) return;

        lives--;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ReproducirSfx(
                AudioManager.Instance.sonidoHurt
            );
        }

        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.Sacudir(0.2f, 0.2f);
        }

        Debug.Log("Vidas restantes: " + lives);

        // Animación de daño
        if (animator != null)
        {
            animator.SetTrigger("Hurt1");
        }

        if (lives <= 0)
        {
            RestartLevel();
            return;
        }

        // Dirección contraria al enemigo o trampa
        float direction;

        if (transform.position.x < enemy.position.x)
        {
            direction = -1f;
        }
        else
        {
            direction = 1f;
        }

        StartCoroutine(DamageRoutine(direction));
    }

    IEnumerator DamageRoutine(float direction)
    {
        isInvulnerable = true;
        IsKnockback = true;

        // Quitamos la velocidad anterior
        rd.linearVelocity = Vector2.zero;

        // Aplicamos empujón
        rd.AddForce(
            new Vector2(
                direction * knockbackForce,
                knockbackUpForce
            ),
            ForceMode2D.Impulse
        );

        // Durante este tiempo el jugador NO controla el movimiento
        yield return new WaitForSeconds(knockbackTime);

        IsKnockback = false;

        // Sigue siendo invulnerable hasta completar el tiempo
        yield return new WaitForSeconds(
            invulnerabilityTime - knockbackTime
        );

        isInvulnerable = false;
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}