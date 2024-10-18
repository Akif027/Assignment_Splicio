using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;    // Maximum health
    [SerializeField] private float currentHealth = 0f;  // Current health (initialized at 0)

    [Header("UI References")]
    [SerializeField] private Image healthBar;           // Reference to the health bar image


    private static Health _instance;
    public static Health Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateHealthBar(); // Initialize health bar at the start
    }

    // Public method to increase health
    public void IncreaseHealth(float amount)
    {
        // Increase health and clamp it to maxHealth
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);


        // Reset health if it reaches max
        if (currentHealth >= maxHealth)
        {
            currentHealth = 0;
            UpdateHealthBar();
        }
    }

    // Method to update the health bar fill amount
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }


}
