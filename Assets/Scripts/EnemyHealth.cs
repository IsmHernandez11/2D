using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Salud del Enemigo")]
    public int MaxHealth = 3;
    public int currentHealth;

    void Start()
    {
        currentHealth = MaxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}