using UnityEngine;

public class AchievementsToggle : MonoBehaviour
{
    public GameObject achievementsPanel;

    public void ToggleAchievements()
    {
        bool isActive = achievementsPanel.activeSelf;
        achievementsPanel.SetActive(!isActive);
    }
}
