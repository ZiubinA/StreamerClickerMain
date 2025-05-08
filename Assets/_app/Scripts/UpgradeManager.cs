using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [Header("Core References")]
    public CoinManager coinManager;

    [Header("UI Panels")]
    public GameObject upgradePanel;
    public GameObject confirmationPanel;

    [Header("UI References")]
    public Button resetButton;
    public Button closeUpgradeButton;
    public Button yesButton;
    public Button noButton;
    public Button openUpgradesButton;
    public TextMeshProUGUI clickValueLabel;

    [Header("Upgrade Prefabs and Container")]
    public GameObject upgradeButtonPrefab;
    public Transform upgradeContainer;

    public ApartmentManager apartmentManager;

    // Dictionary to store upgrade UI references
    private Dictionary<string, UpgradeButtonController> upgradeButtons = new Dictionary<string, UpgradeButtonController>();
    private bool isPanelOpen = false;

    void Start()
    {
        // Find the CoinManager if not assigned
        if (coinManager == null)
            coinManager = FindObjectOfType<CoinManager>();

        // Hide panels initially
        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        if (confirmationPanel != null)
            confirmationPanel.SetActive(false);

        // Set up button event listeners
        if (resetButton != null)
            resetButton.onClick.AddListener(ShowConfirmationPanel);

        if (closeUpgradeButton != null)
            closeUpgradeButton.onClick.AddListener(CloseUpgradePanel);

        if (yesButton != null)
            yesButton.onClick.AddListener(ConfirmReset);

        if (noButton != null)
            noButton.onClick.AddListener(CloseConfirmationPanel);

        if (openUpgradesButton != null)
            openUpgradesButton.onClick.AddListener(ToggleUpgradePanel);

        // Find the ApartmentManager if not assigned
        if (apartmentManager == null)
            apartmentManager = FindObjectOfType<ApartmentManager>();


        // Initialize upgrade buttons
        InitializeUpgradeButtons();

        // Update UI with current values
        UpdateClickValueDisplay();
    }

    void Update()
    {
        // You could add a check here to update UI elements every second for efficiency
    }

    // Initialize UI for each upgrade
    private void InitializeUpgradeButtons()
    {
        // Clear any existing upgrade buttons
        foreach (Transform child in upgradeContainer)
        {
            Destroy(child.gameObject);
        }
        upgradeButtons.Clear();

        // Create buttons for each upgrade
        foreach (var upgrade in coinManager.playerData.upgrades)
        {
            CreateUpgradeButton(upgrade);
        }
    }

    // Create a button for an upgrade
    private void CreateUpgradeButton(UpgradeData upgrade)
    {
        // Instantiate the button prefab
        GameObject buttonObj = Instantiate(upgradeButtonPrefab, upgradeContainer);

        // Get the button controller component
        UpgradeButtonController buttonController = buttonObj.GetComponent<UpgradeButtonController>();

        if (buttonController != null)
        {
            // Initialize the button with upgrade data
            buttonController.Initialize(upgrade, this);

            // Store reference in dictionary
            upgradeButtons[upgrade.id] = buttonController;
        }
    }

    // Called when a purchase is attempted
    public void TryPurchaseUpgrade(string upgradeId)
    {
        bool purchased = coinManager.PurchaseUpgrade(upgradeId);

        if (purchased)
        {
            // Update the UI for this specific upgrade
            if (upgradeButtons.ContainsKey(upgradeId))
            {
                upgradeButtons[upgradeId].UpdateUI();
            }

            // Update the click value display
            UpdateClickValueDisplay();
        }
    }

    // Update the click value display
    public void UpdateClickValueDisplay()
    {
        if (clickValueLabel != null)
        {
            clickValueLabel.text = "Click Value: " + Mathf.Round(coinManager.playerData.clickValue * 10f) / 10f;
        }
    }

    // Toggle the upgrade panel
    public void ToggleUpgradePanel()
    {
        if (isPanelOpen)
            CloseUpgradePanel();
        else
            OpenUpgradePanel();
    }

    // Open the upgrade panel
    public void OpenUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(true);
            isPanelOpen = true;

            // Update all upgrade buttons
            foreach (var buttonPair in upgradeButtons)
            {
                buttonPair.Value.UpdateUI();
            }
        }
    }

    // Close the upgrade panel
    public void CloseUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
            isPanelOpen = false;
        }
    }

    // Show the reset confirmation panel
    public void ShowConfirmationPanel()
    {
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(true);
        }
    }

    // Close the confirmation panel
    public void CloseConfirmationPanel()
    {
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(false);
        }
    }

    // Reset all progress
    public void ConfirmReset()
    {
        // Reset player data
        coinManager.ResetPlayerData();

        apartmentManager.ResetApartment();

        // Reinitialize all upgrade buttons
        InitializeUpgradeButtons();

        // Update the UI
        UpdateClickValueDisplay();

        // Close panels
        CloseConfirmationPanel();
        CloseUpgradePanel();
    }

    // Refresh all UI elements
    public void RefreshAllUI()
    {
        // Update all upgrade buttons
        foreach (var buttonPair in upgradeButtons)
        {
            buttonPair.Value.UpdateUI();
        }

        // Update click value display
        UpdateClickValueDisplay();
    }
}