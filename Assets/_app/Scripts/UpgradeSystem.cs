using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSystem : MonoBehaviour
{
    // UI Buttons
    public Button resetButton;
    public Button yesButton;
    public Button noButton;
    public Button upgradeFurnitureButton;
    public Button upgradeLevelButton;

    // Specific upgrade buttons with their text components
    public Button cameraUpgradeButton;
    public TextMeshProUGUI cameraButtonText;

    public Button laptopUpgradeButton;
    public TextMeshProUGUI laptopButtonText;

    public Button microphoneUpgradeButton;
    public TextMeshProUGUI microphoneButtonText;

    // UI Panels
    public GameObject confirmationPanel;
    public GameObject upgradePanel;

    // Feedback text for purchases
    public GameObject feedbackTextPrefab;
    public Transform feedbackParent;
    public float feedbackDuration = 1.5f;

    private CoinManager coinManager;
    private bool isPanelOpen = false;

    private ApartmentManager apartmentManager;

    void Start()
    {
        coinManager = FindObjectOfType<CoinManager>();
        apartmentManager = FindObjectOfType<ApartmentManager>();

        // Hide panels initially
        if (confirmationPanel != null)
            confirmationPanel.SetActive(false);
        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        // If text components weren't assigned, try to find them in children
        if (cameraButtonText == null && cameraUpgradeButton != null)
            cameraButtonText = cameraUpgradeButton.GetComponentInChildren<TextMeshProUGUI>();
        if (laptopButtonText == null && laptopUpgradeButton != null)
            laptopButtonText = laptopUpgradeButton.GetComponentInChildren<TextMeshProUGUI>();
        if (microphoneButtonText == null && microphoneUpgradeButton != null)
            microphoneButtonText = microphoneUpgradeButton.GetComponentInChildren<TextMeshProUGUI>();

        // Add button listeners
        if (cameraUpgradeButton != null)
            cameraUpgradeButton.onClick.AddListener(UpgradeCamera);
        if (laptopUpgradeButton != null)
            laptopUpgradeButton.onClick.AddListener(UpgradeLaptop);
        if (microphoneUpgradeButton != null)
            microphoneUpgradeButton.onClick.AddListener(UpgradeMicrophone);

        if (resetButton != null)
            resetButton.onClick.AddListener(ShowConfirmationPanel);
        if (yesButton != null)
            yesButton.onClick.AddListener(ResetProgress);
        if (noButton != null)
            noButton.onClick.AddListener(CloseConfirmationPanel);
        if (upgradeFurnitureButton != null)
            upgradeFurnitureButton.onClick.AddListener(ToggleUpgradePanel);

        // Update the UI with current prices
        UpdateUpgradeButtonTexts();
    }

    // Update button texts to show current prices
    public void UpdateUpgradeButtonTexts()
    {
        UpgradeData cameraUpgrade = coinManager?.playerData.GetUpgrade("camera");
        UpgradeData laptopUpgrade = coinManager?.playerData.GetUpgrade("notebook"); // Note: ID is "notebook" not "laptop"
        UpgradeData microphoneUpgrade = coinManager?.playerData.GetUpgrade("microphone");

        if (cameraUpgrade != null && cameraButtonText != null)
            cameraButtonText.text = $"Camera: {cameraUpgrade.currentCost}";

        if (laptopUpgrade != null && laptopButtonText != null)
            laptopButtonText.text = $"Laptop: {laptopUpgrade.currentCost}";

        if (microphoneUpgrade != null && microphoneButtonText != null)
            microphoneButtonText.text = $"Microphone: {microphoneUpgrade.currentCost}";
    }

    // Camera upgrade function
    void UpgradeCamera()
    {
        PurchaseUpgrade("camera", cameraUpgradeButton.transform.position);
    }

    // Laptop upgrade function
    void UpgradeLaptop()
    {
        PurchaseUpgrade("notebook", laptopUpgradeButton.transform.position); // Note: ID is "notebook"
    }

    // Microphone upgrade function
    void UpgradeMicrophone()
    {
        PurchaseUpgrade("microphone", microphoneUpgradeButton.transform.position);
    }

    // Common purchase logic
    void PurchaseUpgrade(string upgradeId, Vector3 buttonPosition)
    {
        if (coinManager == null)
            return;

        UpgradeData upgrade = coinManager.playerData.GetUpgrade(upgradeId);

        if (upgrade == null)
            return;

        if (coinManager.playerData.coins >= upgrade.currentCost)
        {
            // Successful purchase
            coinManager.playerData.coins -= upgrade.currentCost;
            upgrade.LevelUp();
            coinManager.playerData.CalculateClickValue();

            // Update UI
            UpdateUpgradeButtonTexts();
            coinManager.UpdateUI();
            coinManager.SavePlayerData();

            // Show success feedback
            ShowFeedbackMessage("Upgraded!", Color.green, buttonPosition);
        }
        else
        {
            // Not enough coins
            ShowFeedbackMessage("Not enough coins!", Color.red, buttonPosition);
        }
    }

    // Show feedback message
    void ShowFeedbackMessage(string message, Color color, Vector3 position)
    {
        if (feedbackTextPrefab == null)
            return;

        GameObject feedbackObj = Instantiate(feedbackTextPrefab, position, Quaternion.identity,
                                           feedbackParent != null ? feedbackParent : transform);

        TextMeshProUGUI textComponent = feedbackObj.GetComponent<TextMeshProUGUI>();

        if (textComponent != null)
        {
            textComponent.text = message;
            textComponent.color = color;

            // Animate the feedback text (optional)
            StartCoroutine(AnimateFeedbackText(feedbackObj, textComponent));

            // Destroy after duration
            Destroy(feedbackObj, feedbackDuration);
        }
    }

    // Animation for feedback text (similar to your ClickFeedback)
    System.Collections.IEnumerator AnimateFeedbackText(GameObject feedbackObj, TextMeshProUGUI textComponent)
    {
        float timer = 0f;
        Vector3 startPosition = feedbackObj.transform.position;
        Color originalColor = textComponent.color;

        while (timer < feedbackDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / feedbackDuration;

            // Fade out gradually
            textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b,
                                         1 - progress);

            // Move upward
            feedbackObj.transform.position = startPosition + new Vector3(0, progress * 30f, 0);

            yield return null;
        }
    }

    // Toggle upgrade panel visibility
    void ToggleUpgradePanel()
    {
        if (isPanelOpen)
            CloseUpgradePanel();
        else
            OpenUpgradePanel();
    }

    // Open upgrade panel
    void OpenUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(true);
            isPanelOpen = true;
            UpdateUpgradeButtonTexts(); // Update the prices when opening panel
        }
    }

    // Close upgrade panel
    void CloseUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
            isPanelOpen = false;
        }
    }

    // Show confirmation panel for reset
    void ShowConfirmationPanel()
    {
        if (confirmationPanel != null)
            confirmationPanel.SetActive(true);
    }

    // Close confirmation panel
    void CloseConfirmationPanel()
    {
        if (confirmationPanel != null)
            confirmationPanel.SetActive(false);
    }

    // Reset all progress
    void ResetProgress()
    {
        if (coinManager != null)
        {
            coinManager.ResetPlayerData();
            // ADD THIS:
            if (apartmentManager != null)
            {
                apartmentManager.ResetApartment();
            }
            else
            {
                Debug.LogError("ApartmentManager not found in UpgradeSystem!");
            }
            UpdateUpgradeButtonTexts();
            CloseConfirmationPanel();
            CloseUpgradePanel();
        }
    }
}