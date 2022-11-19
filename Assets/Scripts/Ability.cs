using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level System/Ability")]
public class Ability : ScriptableObject
{
    public string description; // Mostly temporary values for the ability system.
    public float damage = 10f;
    public float range = 1f;
    public float cost = 5f;
}
