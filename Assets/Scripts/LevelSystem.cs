using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    [Header("References")]
    public Player playerStats;
    public Slider experienceBar;
    public Text experienceText;
    public Text levelText;

    [Header("Level Data")]
    public Vector2Int minMaxLevel = new Vector2Int(0, 10); // X value is minimum level, Y value is maximum level
    public Vector2Int minMaxExperience = new Vector2Int(0, 100); // X value is minimum experience, Y value is maximum experience
    public AnimationCurve experienceCurve;
    public int currentLevel;
    public int currentExperience;
    public int previousLevel;
    public int nextLevel;

    public List<LevelBonus> levelUpBonus = new();

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
