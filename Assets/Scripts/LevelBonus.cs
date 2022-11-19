using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LevelBonus
{
    [HideInInspector] public string name; // Bonuses awarded to the player on each level. Name is used for overwriting "Element x" with (currently) "Level x".
    public float healthBonus;
    public float magicBonus;
    public float attackBonus;
    public float defenseBonus;
    public Ability abilityBonus;

	public LevelBonus(string newName, float newHealth, float newMagic, float newAttack, float newDefense) : this() // Override for setting base values.
	{
        name = newName;
        healthBonus = newHealth;
        magicBonus = newMagic;
        attackBonus = newAttack;
        defenseBonus = newDefense;
	}
}
