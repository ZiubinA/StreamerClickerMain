using TMPro;
using UnityEngine;
using System.IO;

public class CoinManager : MonoBehaviour
{
    /* ----------  UI REFERENCES  ---------- */
    public TextMeshProUGUI coinLabel;        // Coins UI label
    public TextMeshProUGUI clickValueLabel;  // Click?value UI label
    public TextMeshProUGUI diamondLabel;     // NEW: Diamonds UI label

    /* ----------  PLAYER DATA  ---------- */
    public PlayerData playerData;            // Holds coins, upgrades, diamonds, etc.

    /* ----------  DIAMOND?REWARD STATE  ---------- */
    private int clicksSinceLastDiamond = 0;  // Counts clicks toward the *current* diamond goal
    private int nextDiamondThreshold = 100; // First goal: 100 clicks
    private int currentDiamondReward = 1;   // First reward: 1 diamond
    private const int diamondRewardIncrement = 2;   // +2 diamonds each stage (1 ? 3 ? 5 …)
    private const int thresholdIncrement = 100; // Goal grows +100 clicks each stage (100 ? 200 ? 300 …)

    /* ----------  SAVE?FILE PATH  ---------- */
    private string filePath;

    /* ----------  UNITY LIFECYCLE  ---------- */
    void Start()
    {
        filePath = Application.persistentDataPath + "/playerData.json";
        LoadPlayerData();   // Load or create PlayerData
        UpdateUI();         // Draw the starting UI
    }

    /* ----------  CLICK HANDLER  ---------- */
    // Called from the main image’s Button / EventTrigger
    public void OnImageClicked()
    {
        AddCoins(playerData.clickValue); // Award coins per click
        HandleDiamondsAfterClick();      // Check if a diamond payout is due
    }

    /* ----------  COIN LOGIC  ---------- */
    public void AddCoins(float amount)
    {
        playerData.coins += (int)amount;
        UpdateCoinLabel();
        SavePlayerData();
    }

    /* ----------  DIAMOND LOGIC  ---------- */
    private void HandleDiamondsAfterClick()
    {
        clicksSinceLastDiamond++;

        if (clicksSinceLastDiamond >= nextDiamondThreshold)
        {
            // 1) Award diamonds
            playerData.diamonds += currentDiamondReward;

            // 2) Prepare for next stage
            clicksSinceLastDiamond = 0;
            currentDiamondReward += diamondRewardIncrement; // 1?3?5…
            nextDiamondThreshold += thresholdIncrement;     // 100?200?300…

            // 3) Persist & update UI
            UpdateDiamondLabel();
            SavePlayerData();
        }
    }

    /* ----------  UI HELPERS  ---------- */
    public void UpdateUI()
    {
        UpdateCoinLabel();
        UpdateClickValueDisplay();
        UpdateDiamondLabel();
    }

    public void UpdateCoinLabel()
    {
        if (coinLabel != null)
            coinLabel.text = "Coins: " + FormatNumber(playerData.coins);
    }

    public void UpdateDiamondLabel()
    {
        if (diamondLabel != null)
            diamondLabel.text = "Diamonds: " + playerData.diamonds; // Diamonds are usually small, no need for K/M/B formatting
    }

    public void UpdateClickValueDisplay()
    {
        if (clickValueLabel != null)
            clickValueLabel.text = "Click Value: " + FormatNumber((int)playerData.clickValue);
    }

    /* ----------  UPGRADE PURCHASES  ---------- */
    public bool PurchaseUpgrade(string upgradeId)
    {
        UpgradeData upgrade = playerData.GetUpgrade(upgradeId);

        if (upgrade != null && playerData.coins >= upgrade.currentCost)
        {
            playerData.coins -= upgrade.currentCost;
            upgrade.LevelUp();
            playerData.CalculateClickValue();

            UpdateUI();
            SavePlayerData();
            return true;
        }
        return false;
    }

    /* ----------  NUMBER FORMATTING  ---------- */
    string FormatNumber(long number)
    {
        if (number >= 1_000_000_000_000_000) return (number / 1_000_000_000_000_000f).ToString("0.0") + "Q";
        else if (number >= 1_000_000_000_000) return (number / 1_000_000_000_000f).ToString("0.0") + "T";
        else if (number >= 1_000_000_000) return (number / 1_000_000_000f).ToString("0.0") + "B";
        else if (number >= 1_000_000) return (number / 1_000_000f).ToString("0.0") + "M";
        else if (number >= 1_000) return (number / 1_000f).ToString("0.0") + "K";
        else return number.ToString();
    }

    /* ----------  SAVE / LOAD  ---------- */
    public void SavePlayerData()
    {
        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(filePath, json);
    }

    public void LoadPlayerData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            playerData = JsonUtility.FromJson<PlayerData>(json);
            playerData.CalculateClickValue();
        }
        else
        {
            playerData = new PlayerData();
            playerData.InitializeDefaultUpgrades();
            playerData.CalculateClickValue();
            SavePlayerData();
        }
    }

    /* ----------  RESET (OPTIONAL)  ---------- */
    public void ResetPlayerData()
    {
        playerData = new PlayerData();
        playerData.InitializeDefaultUpgrades();
        playerData.CalculateClickValue();

        clicksSinceLastDiamond = 0;
        nextDiamondThreshold = 100;
        currentDiamondReward = 1;

        SavePlayerData();
        UpdateUI();
    }
}
