using UnityEngine;
using System.Collections;

public class TimedSpikeTrap : MonoBehaviour
{
    [Header("Ajustes del Tiempo")]
    public float waitTime = 2f;
    public float activeTime = 1f;

    [Header("Daño de la trampa")]
    public int damageAmount = 1;

    [Header("Estado Trampa")]
    public bool isDangerous = false;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(TrapRoutine());
    }

    IEnumerator TrapRoutine()
    {
        while (true)
        {
            isDangerous = false;

            yield return new WaitForSeconds(waitTime);

            if (animator != null)
            {
                animator.SetTrigger("Active");
            }

            isDangerous = true;

            yield return new WaitForSeconds(activeTime);

            isDangerous = false;
        }
    }

    void ProcesarDanio(Collider2D other)
    {
        if (isDangerous)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.RecibirDanio(transform);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ProcesarDanio(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        ProcesarDanio(other);
    }

    void Update()
    {

    }
}