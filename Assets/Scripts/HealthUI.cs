using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [Header("Referencias")]
    public PlayerHealth playerHealth;

    [Header("Corazones")]
    public GameObject[] hearts;

    void Update()
    {
        if (playerHealth == null)
            return;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < playerHealth.lives)
            {
                hearts[i].SetActive(true);
            }
            else
            {
                hearts[i].SetActive(false);
            }
        }
    }
}