using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSystem : MonoBehaviour
{
    public Button resetButton;  // Button that triggers reset
    public Button yesButton;    // Button that confirms the reset
    public Button noButton;     // Button that cancels the reset
    public GameObject confirmationPanel;  // Panel asking for confirmation
    public GameObject upgradePanel;  // The panel containing the upgrade buttons
    public Button cameraUpgradeButton;
    public Button notebookUpgradeButton;
    public Button micUpgradeButton;
    public Button upgradeFurnitureButton;  // Button for upgrading furniture
    public TextMeshProUGUI cameraCostText;
    public TextMeshProUGUI notebookCostText;
    public TextMeshProUGUI micCostText;
    public Button upgradeLevelButton;  // Button for upgrading level

    private CoinManager coinManager;

    // Default upgrade costs
    private int defaultCameraCost = 200;
    private int defaultUpgradeLevelButton = 250;
    private int defaultNotebookCost = 200;
    private int defaultMicCost = 50;

    // Track the current upgrade costs
    private int cameraCost;
    private int notebookCost;
    private int micCost;

    private float clickMultiplier = 10f;

    private bool isPanelOpen = false;  // Track if the panel is open or closed

    void Start()
    {
        coinManager = FindObjectOfType<CoinManager>();  // Find the CoinManager script
        upgradePanel.SetActive(false);  // Hide the upgrade panel initially
        confirmationPanel.SetActive(false);  // Hide confirmation panel initially

        // Add listeners to buttons
        cameraUpgradeButton.onClick.AddListener(UpgradeCamera);
        notebookUpgradeButton.onClick.AddListener(UpgradeNotebook);
        micUpgradeButton.onClick.AddListener(UpgradeMicrophone);
        upgradeFurnitureButton.onClick.AddListener(ToggleUpgradePanel);

        // Add listener for the reset button to show the confirmation panel
        resetButton.onClick.AddListener(ShowConfirmationPanel);

        // Add listeners for the confirmation buttons
        yesButton.onClick.AddListener(ResetProgress);
        noButton.onClick.AddListener(CloseConfirmationPanel);

        // Load costs and player data at the start
        LoadPlayerData();
        UpdateCostTexts();
    }

    // Load player data (including costs)
    void LoadPlayerData()
    {
        if (coinManager.playerData != null)
        {
            // Load the saved upgrade costs or default to initial values
            cameraCost = coinManager.playerData.cameraCost != 0 ? coinManager.playerData.cameraCost : defaultCameraCost;
            notebookCost = coinManager.playerData.notebookCost != 0 ? coinManager.playerData.notebookCost : defaultNotebookCost;
            micCost = coinManager.playerData.micCost != 0 ? coinManager.playerData.micCost : defaultMicCost;
        }
        else
        {
            // Initialize if player data is not set
            cameraCost = defaultCameraCost;
            notebookCost = defaultNotebookCost;
            micCost = defaultMicCost;
        }
    }

    // Show the confirmation panel
    void ShowConfirmationPanel()
    {
        confirmationPanel.SetActive(true);  // Show the panel
    }

    // Close the confirmation panel without resetting
    void CloseConfirmationPanel()
    {
        confirmationPanel.SetActive(false);  // Hide the confirmation panel
    }

    // Toggle the upgrade panel when the "Upgrade Furniture" button is pressed
    void ToggleUpgradePanel()
    {
        if (isPanelOpen)
        {
            CloseUpgradePanel();
        }
        else
        {
            OpenUpgradePanel();
        }
    }

    // Open the upgrade panel
    void OpenUpgradePanel()
    {
        upgradePanel.SetActive(true);  // Make the panel visible
        isPanelOpen = true;  // Mark the panel as open
    }

    // Reset all progress
    void ResetProgress()
    {
        // Reset player data (coins, upgrades, etc.)
        coinManager.playerData.coins = 0;  // Reset coins
        coinManager.playerData.clickValue = 1f;  // Reset click value
        cameraCost = defaultCameraCost;  // Reset to default costs
        notebookCost = defaultNotebookCost;  // Reset to default costs
        coinManager.playerData.buttonCost = defaultUpgradeLevelButton;
        micCost = defaultMicCost;  // Reset to default costs
        UpdateCostTexts();  // Update UI with reset values

        coinManager.SavePlayerData();  // Save the reset data
        coinManager.UpdateCoinLabel();  // Update the coin label in the UI

        CloseUpgradePanel();  // Close the upgrade panel if it's open
        CloseConfirmationPanel();  // Close the confirmation panel
    }

    // Hide the upgrade panel when the player closes it
    public void CloseUpgradePanel()
    {
        upgradePanel.SetActive(false);
        isPanelOpen = false;
    }

    // Upgrade the camera if the player has enough coins
    void UpgradeCamera()
    {
        if (coinManager.playerData.coins >= cameraCost)
        {
            coinManager.playerData.coins -= cameraCost;
            coinManager.playerData.clickValue *= clickMultiplier;
            cameraCost *= 2;  // Double the cost for the next upgrade
            coinManager.playerData.cameraCost = cameraCost;  // Save the updated cost
            UpdateCostTexts();
            coinManager.SavePlayerData();
            coinManager.UpdateCoinLabel();
        }
    }

    // Upgrade the notebook if the player has enough coins
    void UpgradeNotebook()
    {
        if (coinManager.playerData.coins >= notebookCost)
        {
            coinManager.playerData.coins -= notebookCost;
            coinManager.playerData.clickValue *= clickMultiplier;
            notebookCost *= 2;  // Double the cost for the next upgrade
            coinManager.playerData.notebookCost = notebookCost;  // Save the updated cost
            UpdateCostTexts();
            coinManager.UpdateCoinLabel();
            coinManager.SavePlayerData();
        }
    }

    // Upgrade the microphone if the player has enough coins
    void UpgradeMicrophone()
    {
        if (coinManager.playerData.coins >= micCost)
        {
            coinManager.playerData.coins -= micCost;
            coinManager.playerData.clickValue *= clickMultiplier;
            micCost *= 2;  // Double the cost for the next upgrade
            coinManager.playerData.micCost = micCost;  // Save the updated cost
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
