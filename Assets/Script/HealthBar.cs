using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("HP")]
    public int maxHP = 100;
    public int currentHP = 100;

    [Header("UI")]
    public Slider healthSlider;
    public Gradient colorGradient;
    public Image fill;

    void Start()
    {
        currentHP = maxHP;

        healthSlider.maxValue = maxHP;
        healthSlider.value = currentHP;

        fill.color = colorGradient.Evaluate(1f);
    }

    // ลดเลือด
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if(currentHP < 0)
            currentHP = 0;

        UpdateBar();

        if(currentHP <= 0)
        {
            Die();
        }
    }

    // เพิ่มเลือด
    public void Heal(int amount)
    {
        currentHP += amount;

        if(currentHP > maxHP)
            currentHP = maxHP;

        UpdateBar();
    }

    void UpdateBar()
    {
        healthSlider.value = currentHP;

        fill.color = colorGradient.Evaluate(
            healthSlider.normalizedValue
        );
    }

    void Die()
    {
        Debug.Log("Player Dead");

        // GameManager.instance.LoseGame();
    }
}