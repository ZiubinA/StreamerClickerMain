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
        if (coinManager.playerData.coins >= coinManager.playerData.buttonCost)  // Check if the player has enough coins
        {
            // Deduct the cost and update the click value
            coinManager.playerData.coins -= coinManager.playerData.buttonCost;
            coinManager.playerData.clickValue *= 1.1f;  // Increase the click value by 10%
            coinManager.playerData.buttonCost += 50;   // Increase the button cost by 50 coins

            coinManager.UpdateCoinLabel();  // Update the coin label
            coinManager.SavePlayerData();  // Save the updated player data
        }
        else
        {
            Debug.Log("Not enough coins to purchase upgrade.");
        }
    }
}
