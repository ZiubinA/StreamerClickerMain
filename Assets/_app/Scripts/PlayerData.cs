using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    // Basic player stats
    public int coins = 0;
    public int diamonds = 0;
    public float baseClickValue = 1f;

    // Dictionary can't be serialized by Unity's JsonUtility
    // So we use these serializable lists instead
    public List<UpgradeData> upgrades = new List<UpgradeData>();

    // Final computed click value 
    public float clickValue = 1f;

    public float passiveIncomePerSec = 1f;   // coins generated each second
    public long lastQuitTicks = 0;    // used for offline payout
    // Calculate the final click value based on all upgrades
    public void CalculateClickValue()
    {
        float additiveSum = baseClickValue;
        float multiplicativeFactor = 1f;

        foreach (var upgrade in upgrades)
        {
            if (upgrade.isAdditive)
            {
                additiveSum += upgrade.currentValue;
            }
            else
            {
                multiplicativeFactor *= (1f + upgrade.currentValue);
            }
        }

        clickValue = additiveSum * multiplicativeFactor;
    }

    // Get an upgrade by ID
    public UpgradeData GetUpgrade(string upgradeId)
    {
        return upgrades.Find(u => u.id == upgradeId);
    }

    public void RecalculatePassiveIncome()
    {
        float additiveSum = 1f;   // base income everyone gets
        float multiplicativeTotal = 1f;   // neutral multiplier

        foreach (var up in upgrades)
        {
            if (up.id == "sponsor" || up.id.StartsWith("passive_"))
            {
                if (up.isAdditive)
                    additiveSum += up.currentValue;          // +X coins/sec
                else
                    multiplicativeTotal *= (1f + up.currentValue); // +Y %
            }
        }

        passiveIncomePerSec = additiveSum * multiplicativeTotal;
    }

    // Add a new upgrade type
    public void AddUpgrade(UpgradeData upgrade)
    {
        if (GetUpgrade(upgrade.id) == null)
        {
            upgrades.Add(upgrade);
        }
    }

    // Initialize default upgrades
    public void InitializeDefaultUpgrades()
    {
        if (upgrades.Count == 0)
        {
            // Basic Click Upgrade (Multiplicative)
            upgrades.Add(new UpgradeData
            {
                id = "click_upgrade",
                name = "Click Upgrade",
                description = "Increases click value by a percentage",
                isAdditive = false,
                baseValue = 0.2f,             // 20% increase per level
                currentValue = 0f,
                level = 0,
                baseCost = 100,
                currentCost = 100,
                costMultiplier = 1.5f
            });

            // Camera Upgrade (Multiplicative)
            upgrades.Add(new UpgradeData
            {
                id = "camera",
                name = "Camera",
                description = "Multiplies your streaming quality and viewer donations",
                isAdditive = false,
                baseValue = 0.5f,             // 50% increase per level
                currentValue = 0f,
                level = 0,
                baseCost = 200,
                currentCost = 200,
                costMultiplier = 1.8f
            });

            // Notebook Upgrade (Additive)
            upgrades.Add(new UpgradeData
            {
                id = "notebook",
                name = "Laptop",
                description = "Adds flat coins per click",
                isAdditive = true,
                baseValue = 2f,               // +2 coins per level
                currentValue = 0f,
                level = 0,
                baseCost = 200,
                currentCost = 200,
                costMultiplier = 1.6f
            });

            // Microphone Upgrade (Additive)
            upgrades.Add(new UpgradeData
            {
                id = "microphone",
                name = "Microphone",
                description = "Improves audio quality and viewer engagement",
                isAdditive = true,
                baseValue = 1f,               // +1 coin per level
                currentValue = 0f,
                level = 0,
                baseCost = 100,
                currentCost = 100,
                costMultiplier = 1.4f
            });

            //Passive income
            upgrades.Add(new UpgradeData
            {
                id = "sponsor",
                name = "Stream Sponsor",
                description = "Generates coins every second",
                isAdditive = true,        // we’ll treat this as +X coins/sec
                baseValue = 2f,          // +2 coins/sec per level
                currentValue = 0f,
                level = 0,
                baseCost = 500,
                currentCost = 500,
                costMultiplier = 1.7f
            });

            // Passive - additive
            upgrades.Add(new UpgradeData
            {
                id = "passive_ad_break",
                name = "Ad Break",
                description = "Generates steady ad revenue",
                isAdditive = true,
                baseValue = 5f,      // +5 coins/sec each level
                /* … costs … */
            });

            // Passive - multiplicative
            upgrades.Add(new UpgradeData
            {
                id = "passive_merch_boost",
                name = "Merch Boost %",
                description = "Boosts all passive income",
                isAdditive = false,   // multiplicative
                baseValue = 0.25f,   // +25 % each level
                /* … costs … */
            });
        }
    }
}

[Serializable]
public class UpgradeData
{
    // Identification
    public string id;               // Unique ID for the upgrade
    public string name;             // Display name
    public string description;      // Tooltip description

    // Upgrade properties
    public bool isAdditive;         // True = add to base, False = multiply total
    public float baseValue;         // Amount added or multiplied per level
    public float currentValue;      // Current effect (baseValue * level)
    public int level;               // Current upgrade level

    // Cost properties
    public int baseCost;            // Initial cost
    public int currentCost;         // Current cost
    public float costMultiplier;    // How much cost increases per level

    // Level up this upgrade
    public void LevelUp()
    {
        level++;

        // Update the current value based on level
        currentValue = baseValue * level;

        // Update the cost for the next level
        currentCost = (int)(currentCost * costMultiplier);
    }

    // Reset this upgrade to level 0
    public void Reset()
    {
        level = 0;
        currentValue = 0f;
        currentCost = baseCost;
    }

    // Get the effect description (for UI)
    public string GetEffectDescription()
    {
        if (isAdditive)
        {
            return $"+{baseValue} coins per click";
        }
        else
        {
            return $"+{baseValue * 100}% per click";
        }
    }

    // Clone this upgrade (for creating new upgrade types)
    public UpgradeData Clone()
    {
        return new UpgradeData
        {
            id = this.id,
            name = this.name,
            description = this.description,
            isAdditive = this.isAdditive,
            baseValue = this.baseValue,
            currentValue = this.currentValue,
            level = this.level,
            baseCost = this.baseCost,
            currentCost = this.currentCost,
            costMultiplier = this.costMultiplier
        };
    }
}