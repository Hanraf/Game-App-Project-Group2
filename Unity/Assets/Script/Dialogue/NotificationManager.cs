using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public GameObject notificationPanel;

    public void ShowNotification()
    {
        notificationPanel.SetActive(true);
    }

    public void HideNotification()
    {
        notificationPanel.SetActive(false);
    }
}
