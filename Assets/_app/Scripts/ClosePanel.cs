using UnityEngine;

public class ClosePanel : MonoBehaviour
{
    public GameObject panelToClose;

    public void Close()
    {
        panelToClose.SetActive(false);
    }
}
