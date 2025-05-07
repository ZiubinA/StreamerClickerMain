using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    public float floatSpeed = 10f;
    public float fadeDuration = 1f;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private float timer = 0f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Float upward
        rectTransform.anchoredPosition += Vector2.up * floatSpeed * Time.deltaTime;

        // Fade out
        canvasGroup.alpha = 1f - (timer / fadeDuration);

        if (timer >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }
}
