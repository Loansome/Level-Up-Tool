using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Player Stat UI")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private Text healthText;
    [SerializeField] private Slider magicBar;
    [SerializeField] private Text magicText;
    [SerializeField] private Text attackText;
    [SerializeField] private Text defenseText;

    [Header("Level System UI")]
    [SerializeField] private Slider experienceBar;
    [SerializeField] private Text experienceText;
    [SerializeField] private Text levelText;
    [SerializeField] private GameObject levelRewardsMenu;
    [SerializeField] private Text levelRewardsText;

    private Coroutine runningTimer = null; // Used to hold the timer that deactivates the "Level Up!" menu.



    // PlayerStats UI
    public void HealthBarUpdate(float current, float max)
    {
        if (current <= 0) current = 0;
        healthBar.maxValue = max;
        healthBar.value = current;
        healthText.text = "Health: " + (int)current + "/" + max;
    }
    public void MagicBarUpdate(float current, float max)
    {
        if (current <= 0) current = 0;
        magicBar.maxValue = max;
        magicBar.value = current;
        magicText.text = "Magic: " + (int)current + "/" + max;
    }
    public void AttackBarUpdate(float attackPower)
    {
        if (attackPower <= 0) attackPower = 0;
        attackText.text = "Attack Power: " + (int)attackPower;
    }
    public void DefenseBarUpdate(float defensePower)
    {
        if (defensePower <= 0) defensePower = 0;
        defenseText.text = "Defense Power: " + (int)defensePower;
    }


    // LevelSystem UI
    public void LevelBarUpdate(int nextEXP, int prevEXP, int currentEXP, int currentLVL) // Updates the experience bar and text.
    {
        experienceBar.maxValue = nextEXP;
        experienceBar.minValue = prevEXP;
        experienceBar.value = currentEXP;
        experienceText.text = currentEXP.ToString();
        levelText.text = currentLVL.ToString();
    }

    public void LevelRewardsMenuUpdate(LevelBonus levelRewards) // Enables the "Level Up!" menu, sets the visuals for what was upgraded, then disappears after a few seconds.
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
}
