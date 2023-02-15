using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public static bool paused;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnPause()
    {
        paused = !paused;
        Time.timeScale = paused ? 0f : 1f;
    }
}
