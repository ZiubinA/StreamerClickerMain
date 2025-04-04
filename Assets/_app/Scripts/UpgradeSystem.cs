using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSystem : MonoBehaviour
{
    public Button upgradeButton;  // Button that opens the upgrade panel
    public GameObject upgradePanel;  // The panel containing the upgrade buttons
    public Button cameraUpgradeButton;
    public Button notebookUpgradeButton;
    public Button micUpgradeButton;
    public Button closeButton;
    public Button upgradeFurnitureButton;  // Button for upgrading furniture
    public TextMeshProUGUI cameraCostText;
    public TextMeshProUGUI notebookCostText;
    public TextMeshProUGUI micCostText;

    private CoinManager coinManager;

    private int cameraCost = 200;
    private int notebookCost = 200;
    private int micCost = 200;
    private float clickMultiplier = 2f;

    private bool isPanelOpen = false;  // Track if the panel is open or closed

    void Start()
    {
        coinManager = FindObjectOfType<CoinManager>();  // Find the CoinManager script
        upgradePanel.SetActive(false);  // Hide the upgrade panel initially

        // Add listeners to buttons
        upgradeButton.onClick.AddListener(OpenUpgradePanel);
        cameraUpgradeButton.onClick.AddListener(UpgradeCamera);
        notebookUpgradeButton.onClick.AddListener(UpgradeNotebook);
        micUpgradeButton.onClick.AddListener(UpgradeMicrophone);
        upgradeFurnitureButton.onClick.AddListener(ToggleUpgradePanel);  // Add listener for furniture upgrade button
        closeButton.onClick.AddListener(CloseUpgradePanel);     // Add listener for closing the panel
        // Update the cost text on start
        UpdateCostTexts();
    }

    // Show the upgrade panel when the button is clicked
    void OpenUpgradePanel()
    {
        upgradePanel.SetActive(true);
        isPanelOpen = true;
    }

    // Hide the upgrade panel when the player closes it (can be done by adding a close button)
    public void CloseUpgradePanel()
    {
        Debug.Log("CloseUpgradePanel clicked");
        upgradePanel.SetActive(false);
        isPanelOpen = false;
    }


    // Toggle the upgrade panel when the "Upgrade Furniture" button is pressed
    void ToggleUpgradePanel()
    {
        if (isPanelOpen)
        {
            // Close the panel if it is already open
            CloseUpgradePanel();
        }
        else
        {
            // Open the panel if it is closed
            OpenUpgradePanel();
        }
    }

    // Upgrade the camera if the player has enough coins
    void UpgradeCamera()
    {
        if (coinManager.playerData.coins >= cameraCost)
        {
            coinManager.playerData.coins -= cameraCost;  // Deduct coins
            coinManager.playerData.clickValue *= clickMultiplier;  // Increase coins per click
            cameraCost *= 2;  // Double the cost for the next upgrade
            UpdateCostTexts();  // Update the displayed cost
            coinManager.SavePlayerData();  // Save the updated data
            coinManager.UpdateCoinLabel();  // Update the coin label
        }
    }

    // Upgrade the notebook if the player has enough coins
    void UpgradeNotebook()
    {
        if (coinManager.playerData.coins >= notebookCost)
        {
            coinManager.playerData.coins -= notebookCost;
            coinManager.playerData.clickValue *= clickMultiplier;
            notebookCost *= 2;
            UpdateCostTexts();
            coinManager.SavePlayerData();
            coinManager.UpdateCoinLabel();
        }
    }

    // Upgrade the microphone if the player has enough coins
    void UpgradeMicrophone()
    {
        if (coinManager.playerData.coins >= micCost)
        {
            coinManager.playerData.coins -= micCost;
            coinManager.playerData.clickValue *= clickMultiplier;
            micCost *= 2;
            UpdateCostTexts();
            coinManager.SavePlayerData();
            coinManager.UpdateCoinLabel();
        }
    }

    // Update the cost texts displayed on the buttons
    void UpdateCostTexts()
    {
        cameraCostText.text = "Camera:" + cameraCost;
        notebookCostText.text = "Laptop:" + notebookCost;
        micCostText.text = "Micro:" + micCost;
    }
}