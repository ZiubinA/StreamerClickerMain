using UnityEngine;
using TMPro;
using System;

public class DailyRewardManager : MonoBehaviour
{
    public GameObject rewardPanel;
    public TextMeshProUGUI rewardText;
    public CoinManager coinManager; // Привязать в инспекторе
    public int rewardAmount = 100;

    private void Start()
    {
        CheckDailyReward();
    }

    public void CheckDailyReward()
    {
        string lastClaimString = PlayerPrefs.GetString("LastDailyReward", "");
        DateTime lastClaim;

        if (string.IsNullOrEmpty(lastClaimString) || !DateTime.TryParse(lastClaimString, out lastClaim) || lastClaim.Date < DateTime.Now.Date)
        {
            ShowRewardPanel();
        }
        else
        {
            rewardPanel.SetActive(false);
        }
    }

    void ShowRewardPanel()
    {
        rewardText.text = $"Daily Reward: +{rewardAmount} coins!";
        rewardPanel.SetActive(true);
    }

    public void ClaimReward()
    {
        PlayerPrefs.SetString("LastDailyReward", DateTime.Now.ToString());
        PlayerPrefs.Save();

        coinManager.AddCoins(rewardAmount); // Начисляем награду

        rewardPanel.SetActive(false);
    }
}
