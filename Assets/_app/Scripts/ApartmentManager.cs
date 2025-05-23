using UnityEngine;
using UnityEngine.UI;

public class ApartmentManager : MonoBehaviour
{
    [Header("Apartment Settings")]
    [SerializeField] private ApartmentLevel[] apartmentLevels;
    [SerializeField] private Image backgroundImage; // The main background image of your game

    [Header("Upgrade Thresholds")]
    [SerializeField] private int[] coinThresholds = { 0, 10000, 100000, 1000000, 10000000 };

    [Header("Debug")]
    [SerializeField] private bool enableDebugLogs = true;

    private int currentApartmentIndex = 0;
    private CoinManager coinManager;
    private ToastManager toastManager;

    void Start()
    {
        // Find references
        coinManager = FindObjectOfType<CoinManager>();
        toastManager = FindObjectOfType<ToastManager>();

        // Load saved apartment level
        LoadSavedApartment();

        // Set initial background
        UpdateBackground(false);

        if (enableDebugLogs)
            Debug.Log($"ApartmentManager started. Current level: {currentApartmentIndex + 1}/{apartmentLevels.Length}");
    }

    // Check regularly for upgrades
    void Update()
    {
        // Only check every second or so to avoid constant checks
        if (Time.frameCount % 30 == 0)
        {
            CheckForApartmentUpgrade();
        }
    }

    private void CheckForApartmentUpgrade()
    {
        // Don't check if we're at max level
        if (currentApartmentIndex >= apartmentLevels.Length - 1)
            return;

        // Get current coin amount
        if (coinManager != null && coinManager.playerData != null)
        {
            long playerCoins = coinManager.playerData.coins;
            int nextThreshold = coinThresholds[currentApartmentIndex + 1];

            // Check if player has enough coins for next apartment
            if (playerCoins >= nextThreshold)
            {
                if (enableDebugLogs)
                    Debug.Log($"Auto-upgrading apartment! Coins: {playerCoins}, Threshold: {nextThreshold}");

                currentApartmentIndex++;
                SaveCurrentApartment();
                UpdateBackground(true);
            }
        }
    }

    private void UpdateBackground(bool showCelebration)
    {
        if (currentApartmentIndex < apartmentLevels.Length && backgroundImage != null)
        {
            // Update background image
            backgroundImage.sprite = apartmentLevels[currentApartmentIndex].apartmentSprite;

            // Show celebration if needed
            if (showCelebration)
            {
                ShowUpgradeCelebration();
            }
        }
    }

    private void ShowUpgradeCelebration()
    {
        string message = apartmentLevels[currentApartmentIndex].unlockMessage;

        // Use toast manager if available
        if (toastManager != null)
        {
            toastManager.ShowToast("Apartment Upgraded: " + message);
        }
        else
        {
            Debug.Log("Apartment upgraded: " + message);
        }
    }

    private void SaveCurrentApartment()
    {
        PlayerPrefs.SetInt("CurrentApartmentIndex", currentApartmentIndex);
        PlayerPrefs.Save();
    }

    private void LoadSavedApartment()
    {
        currentApartmentIndex = PlayerPrefs.GetInt("CurrentApartmentIndex", 0);
    }

    // For testing only
    public void ForceUpgradeApartment()
    {
        if (currentApartmentIndex < apartmentLevels.Length - 1)
        {
            currentApartmentIndex++;
            SaveCurrentApartment();
            UpdateBackground(true);

            if (enableDebugLogs)
                Debug.Log($"Force upgraded apartment to level {currentApartmentIndex + 1}");
        }
    }
    public void ResetApartment()
    {
        // Reset to first apartment
        currentApartmentIndex = 0;

        // Update visuals
        UpdateBackground(false);

        // Save the reset state
        SaveCurrentApartment();

        Debug.Log("Apartment reset to level 1");
    }

    // Add these helper methods to ApartmentManager.cs
    public int GetCurrentApartmentLevel()
    {
        return currentApartmentIndex + 1;
    }

    public int GetMaxApartmentLevel()
    {
        return apartmentLevels.Length;
    }

    public int GetNextUpgradeThreshold()
    {
        if (currentApartmentIndex >= coinThresholds.Length - 1)
            return -1;

        return coinThresholds[currentApartmentIndex + 1];
    }
}