using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public BaseStats baseStats;

    public Slider healthBar;
    public Text healthText;

    public float maxHealth;
    public float maxMagic;
    public float attackPower;
    public float defensePower;

    private float _healthModifier;
    private float _magicModifier;
    private float _attackModifier;
    private float _defenseModifier;

    public float currentHealth;
    public float currentMagic;

    void Awake()
    {
        maxHealth = baseStats.healthPoints + _healthModifier;
        maxMagic = baseStats.magicPoints + _magicModifier;
        attackPower = baseStats.attackPower + _attackModifier;
        defensePower = baseStats.defensePower + _defenseModifier;

        currentHealth = maxHealth;
        currentMagic = maxMagic;
        HealthBarUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            TakeDamage(5);
        if (Input.GetKeyDown(KeyCode.R))
            IncreaseMaxHealth(10);
    }

    private void HealthBarUpdate()
	{
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        healthText.text = currentHealth + "/" + maxHealth;
	}

    public void TakeDamage(float amount)
	{
        currentHealth -= amount;
        HealthBarUpdate();
	}
    public void IncreaseMaxHealth(float amount)
	{
        _healthModifier += amount;
        maxHealth = baseStats.healthPoints + _healthModifier;
        HealthBarUpdate();
	}
    public void IncreaseMagic(float amount)
	{
        _magicModifier += amount;
	}
    public void IncreaseDamagePower(float amount)
	{
        _attackModifier += amount;
	}
    public void IncreaseDefensePower(float amount)
	{
        _defenseModifier += amount;
	}
}
