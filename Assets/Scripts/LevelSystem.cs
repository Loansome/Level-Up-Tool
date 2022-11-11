using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    public Player playerStats;
    public Slider experienceBar;
    public Text experienceText;
    public Text levelText;

    public AnimationCurve experienceLevelCurve;
    public int currentLevel;
    public int currentExperience;
    public int previousLevel;
    public int nextLevel;

}
