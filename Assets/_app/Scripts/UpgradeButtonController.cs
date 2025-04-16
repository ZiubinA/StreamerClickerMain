using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButtonController : MonoBehaviour
{
    [Header("UI Components")]
    public Button upgradeButton;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI effectText;
    public Image upgradeIcon;
    public Image backgroundImage;

    [Header("Color Settings")]
    public Color additiveColor = new Color(0.2f, 0.8f, 0.3f); // Green for additive
    public Color multiplicativeColor = new Color(0.8f, 0.3f, 0.8f); // Purple for multiplicative
    public Color disabledColor = new Color(0.5f, 0.5f, 0.5f); // Gray for disabled

    private UpgradeData upgradeData;
    private UpgradeManager upgradeManager;
    private string upgradeId;

    // Initialize the button with upgrade data
    public void Initialize(UpgradeData data, UpgradeManager manager)
    {
        upgradeData = data;
        upgradeManager = manager;
        upgradeId = data.id;

        // Set up the button click event
        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        }

        // Update the UI with initial values
        UpdateUI();
    }

    // Update all UI elements based on current upgrade data
    public void UpdateUI()
    {
        if (upgradeData == null) return;

        // Get the current data from the player data
        UpgradeData currentData = upgradeManager.coinManager.playerData.GetUpgrade(upgradeId);
        if (currentData == null) return;

        // Update the data reference
        upgradeData = currentData;

        // Update name
        if (nameText != null)
        {
            nameText.text = upgradeData.name;
        }

        // Update level
        if (levelText != null)
        {
            levelText.text = "Level: " + upgradeData.level;
        }

        // Update cost
        if (costText != null)
        {
            costText.text = "Cost: " + upgradeData.currentCost;
        }

        // Update effect description
        if (effectText != null)
        {
            effectText.text = upgradeData.GetEffectDescription();
        }

        // Update background color based on type
        if (backgroundImage != null)
        {
            backgroundImage.color = upgradeData.isAdditive ? additiveColor : multiplicativeColor;
        }

        // Update button interactability based on affordability
        if (upgradeButton != null)
        {
            bool canAfford = upgradeManager.coinManager.playerData.coins >= upgradeData.currentCost;
            upgradeButton.interactable = canAfford;

            // You could also change the button color based on affordability
            if (backgroundImage != null)
            {
                // Apply a darker shade if can't afford
                if (!canAfford)
                {
                    Color darkenedColor = upgradeData.isAdditive ?
                        Color.Lerp(additiveColor, disabledColor, 0.5f) :
                        Color.Lerp(multiplicativeColor, disabledColor, 0.5f);
                    backgroundImage.color = darkenedColor;
                }
            }
        }
    }

    // Handle button click
    private void OnUpgradeButtonClicked()
    {
        if (upgradeManager != null && !string.IsNullOrEmpty(upgradeId))
        {
            upgradeManager.TryPurchaseUpgrade(upgradeId);
        }
    }
}