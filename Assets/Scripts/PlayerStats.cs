using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerStatus { Health, Magic, Attack, Defense } // Determines which stat that should be updated.

public class PlayerStats : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BaseStats baseStats; // References to the base stats of the player and UI elements.
    [SerializeField] private UIManager uiManager;

    [Header("Player Data")]
    [SerializeField] private List<Ability> heldAbilities = new(); // The abilities the player has access to.

    private float _maxHealth; // Max value of each stat.
    private float _maxMagic;
    private float _attackPower; // Currently used for lowering magic cost.
    private float _defensePower; // Currently used for lowering damage the player takes.

    private float _healthModifier; // The extra bonus points added to the player's max stats.
    private float _magicModifier;
    private float _attackModifier;
    private float _defenseModifier;

    private float _currentHealth;
    private float _currentMagic;


    void Awake()
    {
        _maxHealth = baseStats.healthPoints + _healthModifier; // Set max stats based on the BaseStats, plus the starting bonuses.
        _maxMagic = baseStats.magicPoints + _magicModifier;
        _attackPower = baseStats.attackPower + _attackModifier;
        _defensePower = baseStats.defensePower + _defenseModifier;

        _currentHealth = _maxHealth;
        _currentMagic = _maxMagic;

        uiManager.HealthBarUpdate(_currentHealth, _maxHealth);
        uiManager.MagicBarUpdate(_currentMagic, _maxMagic);
        uiManager.AttackBarUpdate(_attackPower);
        uiManager.DefenseBarUpdate(_defensePower);
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
        if (_currentHealth > 0)
        {
            _currentHealth -= amount - (_defensePower / 10); // Takes away health. Defense Power is used to lessen the amount of damage taken.
            if (_currentHealth <= 0) _currentHealth = 0; // Makes sure health doesn't go negative.
            uiManager.HealthBarUpdate(_currentHealth, _maxHealth);
        }
	}

    public void UseAbility(string name)
	{
        if (_currentMagic > 0)
        {
            Ability spell = heldAbilities.Find(t => t.name == name); // Find the ability to use...
            if (spell != null) _currentMagic -= spell.cost - (_attackPower / 10); // ...and cast it by using up magic points.
            if (_currentMagic <= 0) _currentMagic = 0; // Makes sure magic doesn't go negative.
            uiManager.MagicBarUpdate(_currentMagic, _maxMagic);
        }
    }

    public void AddAbility(Ability ability) // Used by the level system to give the player abilites.
	{
        heldAbilities.Add(ability);
	}
    public void RemoveAbility(Ability ability) // Removes ability from the player. Used when the player goes down a level.
	{
        heldAbilities.Remove(ability);
	}

    public void IncreaseStat(PlayerStatus stat, float amount) // Increases the maximum value of the specified stat.
	{
        switch (stat)
		{
            case PlayerStatus.Health:
                _healthModifier += amount;
				_maxHealth = baseStats.healthPoints + _healthModifier;
				_currentHealth += amount; // Gives the player some health when they level up.
                uiManager.HealthBarUpdate(_currentHealth, _maxHealth);
                break;
            case PlayerStatus.Magic:
                _magicModifier += amount;
                _maxMagic = baseStats.magicPoints + _magicModifier;
                _currentMagic += amount;
                uiManager.MagicBarUpdate(_currentMagic, _maxMagic);
                break;
            case PlayerStatus.Attack:
                _attackModifier += amount;
                _attackPower = baseStats.attackPower + _attackModifier;
                uiManager.AttackBarUpdate(_attackPower);
                break;
            case PlayerStatus.Defense:
                _defenseModifier += amount;
                _defensePower = baseStats.defensePower + _defenseModifier;
                uiManager.DefenseBarUpdate(_defensePower);
                break;
        }
	}
    public void DecreaseStat(PlayerStatus stat, float amount) // Decreases maximum value of the stat.
	{
        IncreaseStat(stat, -amount);
	}

}
