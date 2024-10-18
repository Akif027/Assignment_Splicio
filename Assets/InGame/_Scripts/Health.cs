using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;                 // Maximum health
    [SerializeField] float currentHealth;                     // Current health
    public Image healthBar;                         // Reference to the health bar image
    public float healthIncreaseDuration = 0.5f;    // Duration for health increase
    private static Health _instance;
    public static Health Instance { get { return _instance; } }

    void Awake()
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
        // Initialize current health
        currentHealth = 0;
        UpdateHealthBar();
    }

    // Method to increase health
    public void IncreaseHealth(float amount)
    {

        // Increase health, ensuring it doesn't exceed maxHealth
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        // Update the health bar visually
        UpdateHealthBar();


    }

    // Method to update the health bar UI
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            // Calculate health percentage and update the fill amount
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }

    // Method to animate health increase
    private void AnimateHealthIncrease(float amount)
    {
        // Optionally use DOTween to animate the health bar fill
        healthBar.fillAmount = currentHealth / maxHealth; // Update to the current health
        healthBar.DOFillAmount(currentHealth / maxHealth, healthIncreaseDuration).SetEase(Ease.OutSine);
    }
}
