using TMPro;
using UnityEngine;
using System.IO;

public class CoinManager : MonoBehaviour
{
    public TextMeshProUGUI coinLabel;  // Reference to the TextMeshPro component (UI Text)
    public PlayerData playerData;      // Reference to the PlayerData to hold coin information

    private string filePath;

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
        coinLabel.text = "Coins: " + playerData.coins;  // Update the TextMeshPro UI element
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
            playerData = new PlayerData { coins = 0, clickValue = 1f, buttonCost = 100 };
        }
    }
}
