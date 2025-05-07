using UnityEngine;
using Unity.Notifications.Android;

public class NotificationManager : MonoBehaviour
{
    void Awake()
    {
        CreateNotificationChannel();
    }

    void Start()
    {
        ScheduleTestNotification();
    }

    void CreateNotificationChannel()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "reminder_channel",
            Name = "Daily Reminders",
            Description = "Notifications to remind you to collect daily reward!",
            Importance = Importance.Default,
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
        Debug.Log("Android notification channel registered");
    }

    // NEW: schedule a notification
    public void ScheduleTestNotification()
    {
        // Create the notification
        var notification = new AndroidNotification()
        {
            Title = "Collect Reward!",
            Text = "Your clicks are waiting",
            FireTime = System.DateTime.Now.AddSeconds(10)  // 10-sec delay for testing
        };

        // Send it to our channel
        AndroidNotificationCenter.SendNotification(notification, "reminder_channel");
        Debug.Log("Scheduled test notification for 10 seconds from now");
    }
}
