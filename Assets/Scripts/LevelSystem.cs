using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player playerStats;
    [SerializeField] private Slider experienceBar;
    [SerializeField] private Text experienceText;
    [SerializeField] private Text levelText;

    [Header("Level Data")]
    [SerializeField] private Vector2Int minMaxLevel = new Vector2Int(0, 10); // X value is minimum level, Y value is maximum level
    [SerializeField] private Vector2Int minMaxExperience = new Vector2Int(0, 100); // X value is minimum experience, Y value is maximum experience
    [SerializeField] private AnimationCurve experienceCurve;
    [SerializeField] private int currentLevel;
    [SerializeField] private int currentExperience;
    private int previousLevelExperience;
    private int nextLevelExperience;

    [SerializeField] private List<LevelBonus> levelUpBonus = new();

    private bool isMaxLevel = false;

	private void Update()
	{
        previousLevelExperience = (int)experienceCurve.Evaluate(currentLevel);
        nextLevelExperience = (int)experienceCurve.Evaluate(currentLevel + 1);

        if (currentLevel < minMaxLevel.x) currentLevel = minMaxLevel.x;
        if (currentLevel > minMaxLevel.y) currentLevel = minMaxLevel.y;
        if (currentExperience < minMaxExperience.x) currentExperience = minMaxExperience.x;
        if (currentExperience > minMaxExperience.y) currentExperience = minMaxExperience.y;

        if (currentExperience - previousLevelExperience < 0) LevelDown();
        if (nextLevelExperience - currentExperience <= 0) LevelUp();

        LevelBarUpdate();
    }

    private void LevelUp()
	{
        if (isMaxLevel) return;
        playerStats.IncreaseStat(PlayerStatus.Health, levelUpBonus[currentLevel].healthBonus);
        playerStats.IncreaseStat(PlayerStatus.Magic, levelUpBonus[currentLevel].magicBonus);
        playerStats.IncreaseStat(PlayerStatus.Attack, levelUpBonus[currentLevel].attackBonus);
        playerStats.IncreaseStat(PlayerStatus.Defense, levelUpBonus[currentLevel].defenseBonus);
        if (levelUpBonus[currentLevel].abilityBonus != null) playerStats.AddAbility(levelUpBonus[currentLevel].abilityBonus);
        currentLevel++;
        if (currentLevel == minMaxLevel.y) isMaxLevel = true;
	}
    private void LevelDown()
	{
        currentLevel--;
        playerStats.DecreaseStat(PlayerStatus.Health, levelUpBonus[currentLevel].healthBonus);
        playerStats.DecreaseStat(PlayerStatus.Magic, levelUpBonus[currentLevel].magicBonus);
        playerStats.DecreaseStat(PlayerStatus.Attack, levelUpBonus[currentLevel].attackBonus);
        playerStats.DecreaseStat(PlayerStatus.Defense, levelUpBonus[currentLevel].defenseBonus);
        if (levelUpBonus[currentLevel].abilityBonus != null) playerStats.RemoveAbility(levelUpBonus[currentLevel].abilityBonus);
        isMaxLevel = false;
    }
    private void LevelBarUpdate()
	{
        experienceBar.maxValue = nextLevelExperience;
        experienceBar.minValue = previousLevelExperience;
        experienceBar.value = currentExperience;
        experienceText.text = currentExperience.ToString();
        levelText.text = currentLevel.ToString();
	}

	public void OnValidate()
	{
        if (experienceCurve.length < 2)
		{
            Debug.Log("No values to pull from. Resetting curve.");
            experienceCurve.keys = new Keyframe[2];
            experienceCurve.keys[0] = new Keyframe(0, 0);
            experienceCurve.keys[1] = new Keyframe(10, 100);
            return;
        }

        Keyframe[] curveKeys = experienceCurve.keys;
        curveKeys[0].time = minMaxLevel.x;
        curveKeys[0].value = minMaxExperience.x;
        curveKeys[experienceCurve.length - 1].time = minMaxLevel.y;
        curveKeys[experienceCurve.length - 1].value = minMaxExperience.y;
        experienceCurve.keys = curveKeys;

        if (minMaxLevel.y > levelUpBonus.Count)
        {
            for (int i = levelUpBonus.Count; i < minMaxLevel.y; i++)
            {
                levelUpBonus.Add(new LevelBonus());
                levelUpBonus[i] = new("Level " + (i + 1), 10f, 5f, 2f, 1f);
            }
        }
        else if (minMaxLevel.y < levelUpBonus.Count)
        {
            for (int i = minMaxLevel.y; i < levelUpBonus.Count; i++)
            {
                levelUpBonus.RemoveAt(levelUpBonus.Count - 1);
            }
        }
	}
}
