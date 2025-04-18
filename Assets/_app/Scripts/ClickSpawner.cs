using UnityEngine;

public class ClickSpawner : MonoBehaviour
{
    public GameObject floatingEffectPrefab;
    public Canvas floatingCanvas;
    public Camera mainCamera;

    public float spawnDistanceFromCamera = 1f; // z-position
    public Vector3 effectWorldScale = new Vector3(0.01f, 0.01f, 0.01f); // small size

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenPos = Input.mousePosition;
            screenPos.z = spawnDistanceFromCamera;

            Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);

            GameObject effect = Instantiate(floatingEffectPrefab, worldPos, Quaternion.identity, floatingCanvas.transform);

            // 🔽 Scale it down for World Space
            effect.transform.localScale = effectWorldScale;
        }
    }
}
