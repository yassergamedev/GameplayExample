using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("HealthBar")]
    public Slider healthSlider;      
    public Slider backHealthSlider;  

    public float health;
    private float lerpTimer;
    public float maxHealth = 100f;

    private float durationTimer; 

    [Header("Death Settings")]
    public Animator playerAnimator;  




    void Start()
    {
        playerAnimator.enabled = false;
        health = maxHealth;

    }

    void Update()
    {

        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();

    }

    public void UpdateHealthUI()
    {
        float fillA = healthSlider.value;
        float fillB = backHealthSlider.value;
        float hFraction = health / maxHealth;

        if (fillB > hFraction)
        {
            healthSlider.value = hFraction;
            lerpTimer += Time.deltaTime;
        }
        else if (fillB < hFraction)
        {
            healthSlider.value = hFraction;
            lerpTimer += Time.deltaTime;
        }
    }

    public void TakeDamage(float damage)
    {

        health -= damage;
        lerpTimer = 0f;
        durationTimer = 0f;
        
    }

 

    public void RestoreHealth(float healAmount)
    {

        health += healAmount;
        health = Mathf.Clamp(health, 0, maxHealth);
        lerpTimer = 0f;
    }

  

}
