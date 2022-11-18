using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerStatus { Health, Magic, Attack, Defense }

public class Player : MonoBehaviour
{
    [SerializeField] private BaseStats baseStats;

    [SerializeField] private Slider healthBar;
    [SerializeField] private Text healthText;

    private float maxHealth;
    private float maxMagic;
    private float attackPower;
    private float defensePower;

    private float _healthModifier;
    private float _magicModifier;
    private float _attackModifier;
    private float _defenseModifier;

    private float currentHealth;
    [SerializeField] private float currentMagic;

    [SerializeField] private List<Ability> heldAbilities = new();

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
        if (Input.GetKeyDown(KeyCode.J))
            UseAbility("Blizzard");
        if (Input.GetKeyDown(KeyCode.K))
            UseAbility("Flare");
    }

    public void TakeDamage(float amount)
	{
        if (currentHealth > 0)
        {
            currentHealth -= amount - (defensePower / 10);
            HealthBarUpdate();
        }
        else currentHealth = 0;
	}
    public void UseAbility(string name)
	{
        if (currentMagic > 0)
        {
            Ability spell = heldAbilities.Find(t => t.name == name);
            if (spell != null) DecreaseStat(PlayerStatus.Magic, spell.cost);
        }
        else currentMagic = 0;
    }

    public void IncreaseStat(PlayerStatus stat, float amount)
	{
        switch (stat)
		{
            case PlayerStatus.Health:
                _healthModifier += amount;
				maxHealth = baseStats.healthPoints + _healthModifier;
				currentHealth += amount;
                HealthBarUpdate();
                break;
            case PlayerStatus.Magic:
                _magicModifier += amount;
                maxMagic = baseStats.magicPoints + _magicModifier;
                currentMagic += amount;
                //HealthBarUpdate();
                break;
            case PlayerStatus.Attack:
                _attackModifier += amount;
                attackPower = baseStats.attackPower + _attackModifier;
                //HealthBarUpdate();
                break;
            case PlayerStatus.Defense:
                _defenseModifier += amount;
                defensePower = baseStats.defensePower + _defenseModifier;
                //HealthBarUpdate();
                break;
        }
	}
    public void DecreaseStat(PlayerStatus stat, float amount)
	{
        IncreaseStat(stat, -amount);
	}
    public void AddAbility(Ability ability)
	{
        heldAbilities.Add(ability);
	}
    public void RemoveAbility(Ability ability)
	{
        heldAbilities.Remove(ability);
	}

    private void HealthBarUpdate()
	{
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        healthText.text = (int)currentHealth + "/" + maxHealth;
	}
}
