using TMPro;
using UnityEngine;
using System.IO;

public class CoinManager : MonoBehaviour
{
    public TextMeshProUGUI coinLabel;  // Reference to the UI Text (coin label)
    public PlayerData playerData;     // Reference to the PlayerData to hold coin information

    private string filePath;          // File path for saving player data

    void Start()
    {
        filePath = Application.persistentDataPath + "/playerData.json";  // File path for saving player data
        LoadPlayerData();  // Load player data on start
        UpdateCoinLabel();  // Update the UI with the current coin count
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // Detect left mouse click
        {
            AddCoins(playerData.clickValue);  // Add coins per click (1 coin per click)
        }
    }

    public void AddCoins(float amount)
    {
        playerData.coins += (int)amount;  // Add coins to the player
        UpdateCoinLabel();  // Update the coin label UI
        SavePlayerData();   // Save data after updating coins
    }

    public void UpdateCoinLabel()
    {
        coinLabel.text = FormatNumber(playerData.coins);  // Update the TextMeshPro UI element
    }

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
            return number.ToString(); // For smaller numbers, just return the number as it is
        }
    }


    public void SavePlayerData()
    {
        string json = JsonUtility.ToJson(playerData);  // Convert PlayerData to JSON
        File.WriteAllText(filePath, json);  // Save the JSON to a file
    }

    public void LoadPlayerData()
    {
        if (File.Exists(filePath))  // If file exists, load the data
        {
            string json = File.ReadAllText(filePath);  // Read the JSON file
            playerData = JsonUtility.FromJson<PlayerData>(json);  // Deserialize JSON to PlayerData object
        }
        else
        {
            // Initialize with default values if no file exists
            playerData = new PlayerData { coins = 0, clickValue = 1f, buttonCost = 100, cameraCost = 200, notebookCost = 200, micCost = 200 };
        }
    }
}
