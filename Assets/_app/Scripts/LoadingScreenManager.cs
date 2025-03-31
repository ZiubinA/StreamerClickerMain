using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreenManager : MonoBehaviour
{
    public Slider loadingBar;  // Reference to the loading bar
    public Text loadingText;   // Reference to the loading text
    public string mainSceneName = "Main"; // Name of the main scene to load

    private void Start()
    {
        StartCoroutine(LoadMainScene());  // Start the loading process
    }

    private IEnumerator LoadMainScene()
    {
        // Begin loading the main scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mainSceneName);
        asyncLoad.allowSceneActivation = false;  // Don't activate the scene immediately

        while (!asyncLoad.isDone)
        {
            // Update the loading progress
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            loadingBar.value = progress;  // Update the loading bar
            loadingText.text = "Loading... " + (progress * 100) + "%";  // Update the loading text

            // If the scene is almost loaded (90%), wait for 3 seconds
            if (asyncLoad.progress >= 0.9f)
            {
                loadingText.text = "Please wait...";  // You can change the message if you'd like

                // Add a delay of 3 seconds before allowing scene activation
                yield return new WaitForSeconds(3f);

                asyncLoad.allowSceneActivation = true;  // Activate the scene after the delay
            }

            yield return null;  // Wait until the next frame
        }
    }


}

