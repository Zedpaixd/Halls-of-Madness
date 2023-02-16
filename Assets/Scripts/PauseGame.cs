using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseGame : MonoBehaviour
{
    public static bool paused;
    Movement movement;
    static GameObject pauseScreen;
    void Start()
    {
        movement = GameObject.FindWithTag("Player").GetComponent<Movement>();
        pauseScreen = transform.GetChild(1).gameObject;
    }

    void Update()
    {
        
    }

    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }
        paused = !paused;
        if (paused)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            pauseScreen.SetActive(true);
        } else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            pauseScreen.SetActive(false);
        }
    }

    public static void Resume()
    {
        paused = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        pauseScreen.SetActive(false);
    }
}
