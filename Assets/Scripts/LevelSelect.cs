using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    static bool loading = false;
    public static void StartLevel(string level, MonoBehaviour instance)
    {
        instance.StartCoroutine(loadLevelAsync(level));
    }
    public void LoadLevel(string level)
    {
        StartCoroutine(loadLevelAsync(level));
    }

    public void Back()
    {
        StartCoroutine(loadLevelAsync(SceneManager.GetActiveScene().buildIndex - 1));
    }

    public static void loadMainMenu(MonoBehaviour instance)
    {
        LevelSelect.StartLevel("Main Menu", instance);
    }

    public static void reload(MonoBehaviour instance)
    {
        LevelSelect.StartLevel(SceneManager.GetActiveScene().name, instance);
    }

    public void loadMainMenu()
    {
        StartLevel("MainMenu", this);
    }

    static IEnumerator loadLevelAsync(int index)
    {
        loading = true;
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(index);
        // Disable auto activation of new scene
        asyncOp.allowSceneActivation = false;

        // Check if done
        while (!asyncOp.isDone)
        {

            // Check progress
            if (asyncOp.progress >= 0.9f)
            {
                asyncOp.allowSceneActivation = true;
                loading = false;

                // Avoid infinite loop
                yield return null;
            }

            // Avoid infinite loop
            yield return null;
        }
    }

    static IEnumerator loadLevelAsync(string sceneName)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName);

        // Disable auto activation of new scene
        asyncOp.allowSceneActivation = false;

        // Check if done
        while (!asyncOp.isDone)
        {
            // Check progress
            if (asyncOp.progress >= 0.9f)
            {
                asyncOp.allowSceneActivation = true;

                // Avoid infinite loop
                yield return null;
            }

            // Avoid infinite loop
            yield return null;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

