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
    public int totalLevels;
    public int totalExperience;
    public int currentLevel;
    public int currentExperience;
    public int previousLevel;
    public int nextLevel;

    public List<LevelBonus> levelUpBonus = new();

	public void OnValidate()
	{
        if (levelUpBonus.Count != totalLevels)
        {
            for (int i = 0; i < totalLevels; i++)
            {
                levelUpBonus.Add(new LevelBonus());
                levelUpBonus[i] = new("Level " + (i + 1), 10f, 5f, 2f, 1f);
            }
        }
        totalExperience = (int)experienceLevelCurve[experienceLevelCurve.length - 1].value;
        int curveSize = (int)experienceLevelCurve[experienceLevelCurve.length - 1].time;

        if (totalLevels < curveSize)
        {
            for (int i = totalLevels; i < curveSize; i++)
            {
                levelUpBonus.Add(new LevelBonus());
                levelUpBonus[i] = new("Level " + (i + 1), 10f, 5f, 2f, 1f);
            }
        }
        else if (totalLevels > curveSize)
        {
            for (int i = curveSize; i < totalLevels; i++)
            {
                levelUpBonus.RemoveAt(levelUpBonus.Count - 1);
            }
        }

        totalLevels = curveSize;
	}
}
