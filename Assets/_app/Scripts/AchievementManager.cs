using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AchievementManager : MonoBehaviour
{
    [Header("Achievements")]
    public List<Achievement> achievements = new List<Achievement>();

    [Header("UI References")]
    public GameObject achievementUIPrefab;
    public Transform achievementUIParent;
    public CoinManager coinManager;

    void Start()
    {
        LoadAchievements();

        if (coinManager != null && coinManager.playerData != null)
        {
            int coins = coinManager.playerData.coins;

            var cameraUpgrade = coinManager.playerData.GetUpgrade("camera");
            int cameraLvl = cameraUpgrade != null ? cameraUpgrade.level : 0;

            int loginCount = PlayerPrefs.GetInt("LoginCount", 0);

            UpdateProgress(coins, cameraLvl, loginCount);
        }
        else
        {
            Debug.LogWarning("⚠️ coinManager или playerData не назначены!");
        }

        GenerateUI();
    }



    void LoadAchievements()
    {
        achievements = new List<Achievement>
        {
            new Achievement { id = "coins_1000", title = "Collect 1000 coins", reward = 100 },
            new Achievement { id = "upgrade_camera", title = "Camera 3 lvl.", reward = 500 },
            new Achievement { id = "login_5", title = "Five launches", reward = 250 }
        };
    }

    void GenerateUI()
    {
        Debug.Log("🧩 Генерация UI достижений...");

        foreach (var ach in achievements)
        {
            Debug.Log("Создаю: " + ach.title);
            GameObject item = Instantiate(achievementUIPrefab, achievementUIParent);
            var ui = item.GetComponent<AchievementUI>();
            ui.Setup(ach, this);
        }
    }




    public void TryClaim(Achievement ach)
    {
        if (ach.isCompleted && !ach.isClaimed)
        {
            ach.isClaimed = true;
            coinManager.AddCoins(ach.reward);
        }
    }

    public void UpdateProgress(int coins, int cameraLvl, int loginCount)
    {
        foreach (var a in achievements)
        {
            if (a.id == "coins_1000" && coins >= 1000)
                a.isCompleted = true;
            if (a.id == "upgrade_camera" && cameraLvl >= 3)
                a.isCompleted = true;
            if (a.id == "login_5" && loginCount >= 5)
                a.isCompleted = true;
        }
    }



    void TrackLogin()
    {
        int count = PlayerPrefs.GetInt("LoginCount", 0);
        PlayerPrefs.SetInt("LoginCount", count + 1);
        PlayerPrefs.Save();
    }
}
