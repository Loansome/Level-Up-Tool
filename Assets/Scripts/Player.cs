using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerStatus { Health, Magic, Attack, Defense } // Determines which stat that should be updated.

public class Player : MonoBehaviour
{
    [SerializeField] private BaseStats baseStats; // References to the base stats of the player and UI elements.

    [SerializeField] private Slider healthBar;
    [SerializeField] private Text healthText;
    [SerializeField] private Slider magicBar;
    [SerializeField] private Text magicText;
    [SerializeField] private Text attackText;
    [SerializeField] private Text defenseText;

    private float maxHealth; // Max value of each stat.
    private float maxMagic;
    private float attackPower; // Currently used for lowering magic cost.
    private float defensePower; // Currently used for lowering damage the player takes.

    private float _healthModifier; // The extra bonus points added to the player's max stats.
    private float _magicModifier;
    private float _attackModifier;
    private float _defenseModifier;

    private float currentHealth;
    private float currentMagic;

    [SerializeField] private List<Ability> heldAbilities = new();

    void Awake()
    {
        maxHealth = baseStats.healthPoints + _healthModifier; // Set max stats based on the BaseStats, plus the starting bonuses.
        maxMagic = baseStats.magicPoints + _magicModifier;
        attackPower = baseStats.attackPower + _attackModifier;
        defensePower = baseStats.defensePower + _defenseModifier;

        currentHealth = maxHealth;
        currentMagic = maxMagic;

        HealthBarUpdate();
        MagicBarUpdate();
        AttackBarUpdate();
        DefenseBarUpdate();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Tests damage of the player.
            TakeDamage(5);
        if (Input.GetKeyDown(KeyCode.J)) // Tests ability and magic system.
            UseAbility("Blizzard");
        if (Input.GetKeyDown(KeyCode.K))
            UseAbility("Flare");
    }

    public void TakeDamage(float amount)
	{
        if (currentHealth > 0)
        {
            currentHealth -= amount - (defensePower / 10); // Takes away health. Defense Power is used to lessen the amount of damage taken.
            if (currentHealth <= 0) currentHealth = 0; // Makes sure health doesn't go negative.
            HealthBarUpdate();
        }
	}
    public void UseAbility(string name)
	{
        if (currentMagic > 0)
        {
            Ability spell = heldAbilities.Find(t => t.name == name); // Find the ability to use...
            if (spell != null) currentMagic -= spell.cost - (attackPower / 10); // ...and cast it by using up magic points.
            if (currentMagic <= 0) currentMagic = 0; // Makes sure magic doesn't go negative.
            MagicBarUpdate();
        }
    }

    public void IncreaseStat(PlayerStatus stat, float amount) // Increases the maximum value of the stat.
	{
        switch (stat)
		{
            case PlayerStatus.Health:
                _healthModifier += amount;
				maxHealth = baseStats.healthPoints + _healthModifier;
				currentHealth += amount; // Gives the player some health when they level up.
                HealthBarUpdate();
                break;
            case PlayerStatus.Magic:
                _magicModifier += amount;
                maxMagic = baseStats.magicPoints + _magicModifier;
                currentMagic += amount;
                MagicBarUpdate();
                break;
            case PlayerStatus.Attack:
                _attackModifier += amount;
                attackPower = baseStats.attackPower + _attackModifier;
                AttackBarUpdate();
                break;
            case PlayerStatus.Defense:
                _defenseModifier += amount;
                defensePower = baseStats.defensePower + _defenseModifier;
                DefenseBarUpdate();
                break;
        }
	}
    public void DecreaseStat(PlayerStatus stat, float amount) // Decreases maximum value of the stat.
	{
        IncreaseStat(stat, -amount);
	}
    public void AddAbility(Ability ability) // Used by the level system to give the player abilites.
	{
        heldAbilities.Add(ability);
	}
    public void RemoveAbility(Ability ability) // Removes ability from the player. Used when the player goes down a level.
	{
        heldAbilities.Remove(ability);
	}

    // Updates the UI elements, called after calculations.
    private void HealthBarUpdate()
    {
        if (currentHealth <= 0) currentHealth = 0;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        healthText.text = "Health: " + (int)currentHealth + "/" + maxHealth;
	}
    private void MagicBarUpdate()
    {
        if (currentMagic <= 0) currentMagic = 0;
        magicBar.maxValue = maxMagic;
        magicBar.value = currentMagic;
        magicText.text = "Magic: " + (int)currentMagic + "/" + maxMagic;
    }
    private void AttackBarUpdate()
    {
        if (attackPower <= 0) attackPower = 0;
        attackText.text = "Attack Power: " + (int)attackPower;
    }
    private void DefenseBarUpdate()
    {
        if (defensePower <= 0) defensePower = 0;
        defenseText.text = "Defense Power: " + (int)defensePower;
    }
}
