using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerStats playerStats; // References to the player and UI elements.
    [SerializeField] private UIManager uiManager;

    [Header("Level Data")]
    [Tooltip("Sets the lowest (X) and highest (Y) levels the player can reach")]
    [SerializeField] private Vector2Int minMaxLevel = new Vector2Int(0, 10);
    [Tooltip("Set the starting (X) and total (Y) amount of experience the player can obtain over all levels")]
    [SerializeField] private Vector2Int minMaxExperience = new Vector2Int(0, 100);
    [Tooltip("Set the curve for how much experience is gained per level")]
    [SerializeField] private AnimationCurve experienceCurve;
    [SerializeField] private int currentExperience;

    private int _currentLevel;
    private int _previousLevelExperience;
    private int _nextLevelExperience;

    [Tooltip("List of bonuses the player is awarded for each level")]
    [SerializeField] private List<LevelBonus> levelUpBonus = new();

    private bool _isMaxLevel = false;


	private void Update()
	{
        _previousLevelExperience = (int)experienceCurve.Evaluate(_currentLevel); // Get the experience of the previous/next levels.
        _nextLevelExperience = (int)experienceCurve.Evaluate(_currentLevel + 1);

        if (_currentLevel < minMaxLevel.x) _currentLevel = minMaxLevel.x; // Keeps levels and experience from going under or over the limits.
        if (_currentLevel > minMaxLevel.y) _currentLevel = minMaxLevel.y;
        if (currentExperience < minMaxExperience.x) currentExperience = minMaxExperience.x;
        if (currentExperience > minMaxExperience.y) currentExperience = minMaxExperience.y;

        if (currentExperience - _previousLevelExperience < 0) LevelDown(); // If experience goes past the level threshold, award or remove bonuses for that level.
        if (_nextLevelExperience - currentExperience <= 0) LevelUp();

        uiManager.LevelBarUpdate(_nextLevelExperience, _previousLevelExperience, currentExperience);
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
        if (_isMaxLevel) return; // Holds the script from errors when max level is reached.

        LevelBonus levelRewards = levelUpBonus[_currentLevel];

        playerStats.IncreaseStat(PlayerStatus.Health, levelRewards.healthBonus); // Award the player with the stat bonuses.
        playerStats.IncreaseStat(PlayerStatus.Magic, levelRewards.magicBonus);
        playerStats.IncreaseStat(PlayerStatus.Attack, levelRewards.attackBonus);
        playerStats.IncreaseStat(PlayerStatus.Defense, levelRewards.defenseBonus);
        if (levelRewards.abilityBonus != null) playerStats.AddAbility(levelRewards.abilityBonus);

        uiManager.LevelRewardsMenuUpdate(levelRewards);

        _currentLevel++;

        if (_currentLevel == minMaxLevel.y) _isMaxLevel = true;
	}
    private void LevelDown()
	{
        _currentLevel--;

        LevelBonus levelRewards = levelUpBonus[_currentLevel];

        playerStats.DecreaseStat(PlayerStatus.Health, levelRewards.healthBonus); // Take away the rewards from that level.
        playerStats.DecreaseStat(PlayerStatus.Magic, levelRewards.magicBonus);
        playerStats.DecreaseStat(PlayerStatus.Attack, levelRewards.attackBonus);
        playerStats.DecreaseStat(PlayerStatus.Defense, levelRewards.defenseBonus);
        if (levelRewards.abilityBonus != null) playerStats.RemoveAbility(levelRewards.abilityBonus);

        _isMaxLevel = false;
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
