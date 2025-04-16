using System.Collections;
using UnityEngine;
using TMPro;

public class ClickFeedback : MonoBehaviour
{
    [Header("Click Feedback Settings")]
    public GameObject clickTextPrefab;
    public Transform spawnParent;
    public float lifetime = 1.0f;
    public float moveSpeed = 100.0f;
    public float fadeSpeed = 2.0f;
    public Vector2 randomOffset = new Vector2(50f, 50f);

    [Header("Animation Settings")]
    public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 1.5f, 1, 0.8f);
    public AnimationCurve alphaCurve = AnimationCurve.EaseInOut(0, 1f, 1, 0f);

    private CoinManager coinManager;

    void Start()
    {
        coinManager = FindObjectOfType<CoinManager>();

        // If no spawn parent is assigned, use this transform
        if (spawnParent == null)
            spawnParent = transform;
    }

    void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Create click feedback at mouse position
            CreateClickFeedback(Input.mousePosition);
        }
    }

    // Create visual feedback showing how many coins were earned from the click
    void CreateClickFeedback(Vector3 position)
    {
        if (clickTextPrefab != null && coinManager != null)
        {
            // Add random offset to position to avoid stacking
            Vector3 offsetPosition = position + new Vector3(
                Random.Range(-randomOffset.x, randomOffset.x),
                Random.Range(-randomOffset.y, randomOffset.y),
                0
            );

            // Instantiate feedback text at the click position
            GameObject feedbackObj = Instantiate(clickTextPrefab, offsetPosition, Quaternion.identity, spawnParent);

            // Get the TextMeshPro component
            TextMeshProUGUI textComponent = feedbackObj.GetComponent<TextMeshProUGUI>();

            if (textComponent != null)
            {
                // Format click value (round to 1 decimal if needed)
                string valueText = coinManager.playerData.clickValue >= 10 ?
                    Mathf.RoundToInt(coinManager.playerData.clickValue).ToString() :
                    (Mathf.Round(coinManager.playerData.clickValue * 10) / 10).ToString();

                // Set the text to show the current click value
                textComponent.text = "+" + valueText;

                // Start the animation coroutine
                StartCoroutine(AnimateFeedbackText(feedbackObj, textComponent));
            }
        }
    }

    // Animate the feedback text (move up, scale, and fade out)
    IEnumerator AnimateFeedbackText(GameObject feedbackObj, TextMeshProUGUI textComponent)
    {
        float timer = 0f;
        Vector3 startPosition = feedbackObj.transform.position;
        Vector3 startScale = feedbackObj.transform.localScale;
        Color originalColor = textComponent.color;

        while (timer < lifetime)
        {
            timer += Time.deltaTime;
            float progress = timer / lifetime;

            // Calculate alpha based on animation curve
            float alpha = alphaCurve.Evaluate(progress);

            // Calculate scale based on animation curve
            float scale = scaleCurve.Evaluate(progress);

            // Update color with new alpha
            textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            // Update scale
            feedbackObj.transform.localScale = startScale * scale;

            // Move the text upward
            feedbackObj.transform.position = startPosition + new Vector3(0f, timer * moveSpeed, 0f);

            yield return null;
        }

        // Destroy the feedback object when animation is complete
        Destroy(feedbackObj);
    }
}