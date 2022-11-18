using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LevelBonus
{
    [HideInInspector] public string name;
    public float healthBonus;
    public float magicBonus;
    public float attackBonus;
    public float defenseBonus;
    public Ability abilityBonus;

	public LevelBonus(string newName, float newHealth, float newMagic, float newAttack, float newDefense) : this()
	{
        name = newName;
        healthBonus = newHealth;
        magicBonus = newMagic;
        attackBonus = newAttack;
        defenseBonus = newDefense;
	}
}
