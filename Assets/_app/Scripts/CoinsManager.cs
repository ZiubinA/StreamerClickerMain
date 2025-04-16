using TMPro;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CoinManager : MonoBehaviour
{
    public TextMeshProUGUI coinLabel;     // Reference to the UI Text (coin label)
    public TextMeshProUGUI clickValueLabel; // Reference to show current click value (optional)
    public PlayerData playerData;         // Reference to the PlayerData to hold coin information
                                         

    private string filePath;              // File path for saving player data


    void Start()
    {
        filePath = Application.persistentDataPath + "/playerData.json";  // File path for saving player data
        LoadPlayerData();  // Load player data on start
        UpdateUI();        // Update the UI with the current values

    }

    // Add this method to your CoinManager class
    public void OnImageClicked()
    {
        // Add coins when the main image is clicked
        AddCoins(playerData.clickValue);
    }


    public void AddCoins(float amount)
    {
        playerData.coins += (int)amount;  // Add coins to the player
        UpdateCoinLabel();  // Update the coin label UI
        SavePlayerData();   // Save data after updating coins
    }

    public void UpdateUI()
    {
        UpdateCoinLabel();    // Update the coin display
        UpdateClickValueDisplay(); // Update the click value display
    }

    public void UpdateCoinLabel()
    {
        if (coinLabel != null)
        {
            coinLabel.text = "Coins: " + FormatNumber(playerData.coins);  // Update the TextMeshPro UI element
        }
    }

    public void UpdateClickValueDisplay()
    {
        // Update click value display if the reference exists
        if (clickValueLabel != null)
        {
            clickValueLabel.text = "Click Value: " + FormatNumber((int)playerData.clickValue);
        }
    }

    // Apply an upgrade - called when player purchases an upgrade
    public bool PurchaseUpgrade(string upgradeId)
    {
        // Find the upgrade by ID
        UpgradeData upgrade = playerData.GetUpgrade(upgradeId);

        // Check if upgrade exists and player has enough coins
        if (upgrade != null && playerData.coins >= upgrade.currentCost)
        {
            // Deduct cost
            playerData.coins -= upgrade.currentCost;

            // Level up the upgrade
            upgrade.LevelUp();

            // Recalculate click value
            playerData.CalculateClickValue();

            // Update UI and save
            UpdateUI();
            SavePlayerData();

            return true; // Purchase successful
        }

        return false; // Purchase failed
    }

    // Format large numbers with K, M, B, etc. suffixes
    string FormatNumber(long number)
    {
        if (number >= 1_000_000_000_000_000) // 1 Quadrillion
        {
            return (number / 1_000_000_000_000_000f).ToString("0.0") + "Q"; // "1.5Q"
        }
        else if (number >= 1_000_000_000_000) // 1 Trillion
        {
            return (number / 1_000_000_000_000f).ToString("0.0") + "T"; // "1.5T"
        }
        else if (number >= 1_000_000_000) // 1 Billion
        {
            return (number / 1_000_000_000f).ToString("0.0") + "B"; // "1.5B"
        }
        else if (number >= 1_000_000) // 1 Million
        {
            return (number / 1_000_000f).ToString("0.0") + "M"; // "1.5M"
        }
        else if (number >= 1_000) // 1 Thousand
        {
            return (number / 1_000f).ToString("0.0") + "K"; // "1.5K"
        }
        else
        {
            return number.ToString(); // For smaller numbers, just return the number as is
        }
    }

    // Save player data to file
    public void SavePlayerData()
    {
        string json = JsonUtility.ToJson(playerData, true);  // Convert PlayerData to JSON (pretty print)
        File.WriteAllText(filePath, json);  // Save the JSON to a file
    }

    // Load player data from file
    public void LoadPlayerData()
    {
        if (File.Exists(filePath))  // If file exists, load the data
        {
            string json = File.ReadAllText(filePath);  // Read the JSON file
            playerData = JsonUtility.FromJson<PlayerData>(json);  // Deserialize JSON to PlayerData object

            // Calculate click value after loading
            playerData.CalculateClickValue();
        }
        else
        {
            // Initialize with default values if no file exists
            playerData = new PlayerData();
            playerData.InitializeDefaultUpgrades();
            playerData.CalculateClickValue();
            SavePlayerData(); // Save the default data
        }
    }

    // Reset all player data
    public void ResetPlayerData()
    {
        playerData = new PlayerData();
        playerData.InitializeDefaultUpgrades();
        playerData.CalculateClickValue();
        SavePlayerData();
        UpdateUI();
    }
}