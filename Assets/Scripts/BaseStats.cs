using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level System/BaseStats")]
public class BaseStats : ScriptableObject
{
    public float healthPoints; // Data used for the starting stats of the player.
    public float magicPoints;
    public float attackPower;
    public float defensePower;
}
