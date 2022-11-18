using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level System/Ability")]
public class Ability : ScriptableObject
{
    public KeyCode keybind;
    public float amount;
}
