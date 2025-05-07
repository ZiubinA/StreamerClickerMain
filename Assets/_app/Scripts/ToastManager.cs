using System.Collections;
using UnityEngine;
using TMPro;            // remove if you’re using UnityEngine.UI.Text
using UnityEngine.UI;   // remove if you’re using TMP

public class ToastManager : MonoBehaviour
{
    [Header("Drag your panel here")]
    public CanvasGroup toastGroup;

    [Header("Drag your TextMeshProUGUI or Text here")]
    public TMP_Text toastText;    // or use `public Text toastText;`

    [Tooltip("How long the toast stays fully visible")]
    public float displayTime = 2f;

    [Tooltip("Fade duration in seconds")]
    public float fadeDuration = 0.3f;

    Coroutine current;

    /// <summary>
    /// Call this to show a message on screen.
    /// </summary>
    public void ShowToast(string message)
    {
        if (current != null) StopCoroutine(current);
        current = StartCoroutine(DoToast(message));
    }

    IEnumerator DoToast(string message)
    {
        toastText.text = message;
        // fade in
        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            toastGroup.alpha = t / fadeDuration;
            yield return null;
        }
        toastGroup.alpha = 1f;

        // wait
        yield return new WaitForSecondsRealtime(displayTime);

        // fade out
        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            toastGroup.alpha = 1f - (t / fadeDuration);
            yield return null;
        }
        toastGroup.alpha = 0f;
        current = null;
    }
}
