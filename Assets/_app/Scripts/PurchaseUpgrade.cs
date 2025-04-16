using UnityEngine;
using UnityEngine.UI;

public class PurchaseUpgrade : MonoBehaviour
{
    public Button purchaseButton;  // Reference to the purchase button
    private CoinManager coinManager;

    void Start()
    {
        // Find the CoinManager component in the scene
        coinManager = FindObjectOfType<CoinManager>();

        // Add a listener to the button click event
        purchaseButton.onClick.AddListener(BuyUpgrade);
    }

    // Method to handle purchasing the upgrade
    private void BuyUpgrade()
    {
        UpgradeData clickUpgrade = coinManager.playerData.GetUpgrade("click_upgrade");

        if (clickUpgrade != null && coinManager.playerData.coins >= clickUpgrade.currentCost)
        {
            // Deduct the cost
            coinManager.playerData.coins -= clickUpgrade.currentCost;

            // Level up the upgrade
            clickUpgrade.LevelUp();

            // Recalculate click value
            coinManager.playerData.CalculateClickValue();

            coinManager.UpdateCoinLabel();  // Update the coin label
            coinManager.SavePlayerData();  // Save the updated player data
        }
        else
        {
            Debug.Log("Not enough coins to purchase upgrade.");
        }
    }
}