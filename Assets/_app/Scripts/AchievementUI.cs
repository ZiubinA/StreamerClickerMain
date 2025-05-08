using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AchievementUI : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descText;
    public Button claimButton;

    private Achievement achievement;
    private AchievementManager manager;

    public void Setup(Achievement a, AchievementManager m)
    {
        Debug.Log("Setup вызван для: " + a.title);
        achievement = a;
        manager = m;

        titleText.text = a.title;
        descText.text = a.description;
        claimButton.interactable = a.isCompleted && !a.isClaimed;

        claimButton.onClick.AddListener(() =>
        {
            manager.TryClaim(achievement);
            claimButton.interactable = false;
        });
    }
}
