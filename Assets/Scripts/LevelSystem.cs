using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player playerStats; // References to the player and UI elements.
    [SerializeField] private Slider experienceBar;
    [SerializeField] private Text experienceText;
    [SerializeField] private Text levelText;
    [SerializeField] private GameObject levelRewardsMenu;
    [SerializeField] private Text levelRewardsText;

    [Header("Level Data")]
    [Tooltip("Sets the lowest (X) and highest (Y) levels the player can reach")]
    [SerializeField] private Vector2Int minMaxLevel = new Vector2Int(0, 10);
    [Tooltip("Set the starting (X) and total (Y) amount of experience the player can obtain over all levels")]
    [SerializeField] private Vector2Int minMaxExperience = new Vector2Int(0, 100);

    [Tooltip("Set the curve for how much experience is gained per level")]
    [SerializeField] private AnimationCurve experienceCurve;
    [SerializeField] private int currentExperience;
    private int currentLevel;
    private int previousLevelExperience;
    private int nextLevelExperience;

    [Tooltip("List of bonuses the player is awarded for each level")]
    [SerializeField] private List<LevelBonus> levelUpBonus = new();

    private bool isMaxLevel = false;
    private Coroutine runningTimer = null; // Used for the timer that deactivates the "Level Up!" menu.



	private void Update()
	{
        previousLevelExperience = (int)experienceCurve.Evaluate(currentLevel); // Get the experience of the previous/next levels.
        nextLevelExperience = (int)experienceCurve.Evaluate(currentLevel + 1);

        if (currentLevel < minMaxLevel.x) currentLevel = minMaxLevel.x; // Keeps levels and experience from going under or over the limits.
        if (currentLevel > minMaxLevel.y) currentLevel = minMaxLevel.y;
        if (currentExperience < minMaxExperience.x) currentExperience = minMaxExperience.x;
        if (currentExperience > minMaxExperience.y) currentExperience = minMaxExperience.y;

        if (currentExperience - previousLevelExperience < 0) LevelDown(); // If experience goes past the level threshold, award or remove bonuses for that level.
        if (nextLevelExperience - currentExperience <= 0) LevelUp();

        LevelBarUpdate();
    }

    public void GainExperience(int experience)
	{
        currentExperience += experience;
	}
    public void LoseExperience(int experience)
    {
        currentExperience -= experience;
    }

    private void LevelUp()
	{
        if (isMaxLevel) return; // Holds the script from errors when max level is reached.

        LevelBonus levelRewards = levelUpBonus[currentLevel];

        playerStats.IncreaseStat(PlayerStatus.Health, levelRewards.healthBonus); // Award the player with the stat bonuses.
        playerStats.IncreaseStat(PlayerStatus.Magic, levelRewards.magicBonus);
        playerStats.IncreaseStat(PlayerStatus.Attack, levelRewards.attackBonus);
        playerStats.IncreaseStat(PlayerStatus.Defense, levelRewards.defenseBonus);
        if (levelRewards.abilityBonus != null) playerStats.AddAbility(levelRewards.abilityBonus);

        LevelRewardsUpdate(levelRewards);

        currentLevel++;

        if (currentLevel == minMaxLevel.y) isMaxLevel = true;
	}
    private void LevelDown()
	{
        currentLevel--;

        LevelBonus levelRewards = levelUpBonus[currentLevel];

        playerStats.DecreaseStat(PlayerStatus.Health, levelRewards.healthBonus); // Decrease the player's stats from that level.
        playerStats.DecreaseStat(PlayerStatus.Magic, levelRewards.magicBonus);
        playerStats.DecreaseStat(PlayerStatus.Attack, levelRewards.attackBonus);
        playerStats.DecreaseStat(PlayerStatus.Defense, levelRewards.defenseBonus);
        if (levelRewards.abilityBonus != null) playerStats.RemoveAbility(levelRewards.abilityBonus);

        isMaxLevel = false;
    }
    private void LevelBarUpdate() // Updates the experience bar and text.
	{
        experienceBar.maxValue = nextLevelExperience;
        experienceBar.minValue = previousLevelExperience;
        experienceBar.value = currentExperience;
        experienceText.text = currentExperience.ToString();
        levelText.text = currentLevel.ToString();
	}

    private void LevelRewardsUpdate(LevelBonus levelRewards) // Enables the "Level Up!" menu, sets the visuals for what was upgraded, then disappears after a few seconds.
    {
        if (runningTimer != null) StopCoroutine(runningTimer); // If the timer is already running, reset it.

        string rewards = "";
        if (levelRewards.healthBonus > 0) rewards += "Health Bonus: " + levelRewards.healthBonus + "\n";
        if (levelRewards.magicBonus > 0) rewards += "Magic Bonus: " + levelRewards.magicBonus + "\n";
        if (levelRewards.attackBonus > 0) rewards += "Attack Bonus: " + levelRewards.attackBonus + "\n";
        if (levelRewards.defenseBonus > 0) rewards += "Defense Bonus: " + levelRewards.defenseBonus + "\n";
        if (levelRewards.abilityBonus != null) rewards += "Ability Unlock! " + levelRewards.abilityBonus.name;

        levelRewardsMenu.SetActive(true);
        levelRewardsText.text = rewards;
        runningTimer = StartCoroutine(Timer());
    }

    IEnumerator Timer() // Timer used for disabling the "Level Up!" menu.
	{
        yield return new WaitForSecondsRealtime(3);
        levelRewardsMenu.SetActive(false);
	}

    public void OnValidate() // OnValidate is used for updating the Unity inspector values for changes.
	{
        if (experienceCurve.length < 2) // If the curve bugs out and has no values, creates temporary values so it can be used and referenced.
		{
            Debug.Log("No values to pull from. Resetting curve.");
            experienceCurve.keys = new Keyframe[2];
            experienceCurve.keys[0] = new Keyframe(0, 0);
            experienceCurve.keys[1] = new Keyframe(10, 100);
            return;
        }

        Keyframe[] curveKeys = experienceCurve.keys; // When minimum/maximum values for levels/experience is changed, update the level curve.
        curveKeys[0].time = minMaxLevel.x;
        curveKeys[0].value = minMaxExperience.x;
        curveKeys[experienceCurve.length - 1].time = minMaxLevel.y;
        curveKeys[experienceCurve.length - 1].value = minMaxExperience.y;
        experienceCurve.keys = curveKeys;

        if (minMaxLevel.y > levelUpBonus.Count) // When max levels is increased, add new bonuses corresponding to each level, with default values.
        {
            for (int i = levelUpBonus.Count; i < minMaxLevel.y; i++)
            {
                levelUpBonus.Add(new LevelBonus());
                levelUpBonus[i] = new("Level " + (i + 1), 10f, 5f, 2f, 1f);
            }
        }
        else if (minMaxLevel.y < levelUpBonus.Count) // When max levels is decreased, remove the bonuses for those levels. **WILL NOT SAVE VALUES!! Undo/Redo can help**
        {
            for (int i = minMaxLevel.y; i < levelUpBonus.Count; i++)
            {
                levelUpBonus.RemoveAt(levelUpBonus.Count - 1);
            }
        }
	}
}
