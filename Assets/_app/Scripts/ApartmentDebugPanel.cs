using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ApartmentDebugPanel : MonoBehaviour
{
    [Header("References")]
    public CoinManager coinManager;
    public ApartmentManager apartmentManager;

    [Header("UI Elements")]
    public GameObject debugPanelObject;
    public Button addCoinsButton;
    public Button upgradeApartmentButton;
    public Button resetApartmentButton;
    public Button toggleDebugPanelButton;
    public TextMeshProUGUI infoText;

    [Header("Settings")]
    public int coinsToAdd = 50000;

    private bool isPanelVisible = false;

    void Start()
    {
        // Find managers if not assigned
        if (coinManager == null)
            coinManager = FindObjectOfType<CoinManager>();

        if (apartmentManager == null)
            apartmentManager = FindObjectOfType<ApartmentManager>();

        // Setup buttons
        if (addCoinsButton != null)
            addCoinsButton.onClick.AddListener(AddDebugCoins);

        if (upgradeApartmentButton != null)
            upgradeApartmentButton.onClick.AddListener(ForceUpgradeApartment);

        if (resetApartmentButton != null)
            resetApartmentButton.onClick.AddListener(ResetApartment);

        if (toggleDebugPanelButton != null)
            toggleDebugPanelButton.onClick.AddListener(ToggleDebugPanel);

        // Hide debug panel by default in builds
#if !UNITY_EDITOR && !DEVELOPMENT_BUILD
            debugPanelObject.SetActive(false);
            toggleDebugPanelButton.gameObject.SetActive(false);
#else
        UpdateInfoText();
        debugPanelObject.SetActive(isPanelVisible);
#endif
    }

    public void ToggleDebugPanel()
    {
        isPanelVisible = !isPanelVisible;
        debugPanelObject.SetActive(isPanelVisible);

        if (isPanelVisible)
            UpdateInfoText();
    }

    public void AddDebugCoins()
    {
        if (coinManager != null)
        {
            coinManager.AddCoins(coinsToAdd);
            Debug.Log($"Added {coinsToAdd} debug coins");
            UpdateInfoText();
        }
    }

    public void ForceUpgradeApartment()
    {
        if (apartmentManager != null)
        {
            apartmentManager.ForceUpgradeApartment();
            Debug.Log("Forced apartment upgrade");
            UpdateInfoText();
        }
    }

    public void ResetApartment()
    {
        if (apartmentManager != null)
        {
            apartmentManager.ResetApartment();
            Debug.Log("Reset apartment to level 1");
            UpdateInfoText();
        }
    }

    private void UpdateInfoText()
    {
        if (infoText != null && coinManager != null && apartmentManager != null)
        {
            int currentLevel = apartmentManager.GetCurrentApartmentLevel();
            int maxLevel = apartmentManager.GetMaxApartmentLevel();
            int nextThreshold = apartmentManager.GetNextUpgradeThreshold();
            long currentCoins = coinManager.playerData.coins;

            string info = $"Apartment Level: {currentLevel}/{maxLevel}\n";
            info += $"Current Coins: {currentCoins}\n";

            if (currentLevel < maxLevel)
            {
                info += $"Next upgrade at: {nextThreshold} coins\n";
                info += $"Need {Mathf.Max(0, nextThreshold - currentCoins)} more coins";
            }
            else
            {
                info += "Maximum level reached!";
            }

            infoText.text = info;
        }
    }
}