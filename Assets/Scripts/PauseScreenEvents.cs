using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreenEvents : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public static void Resume()
    {
        PauseGame.Resume();
    }
    public static void Options()
    {
        print("You have no choices");
    }
    public static void ExitToDesktop()
    {
        Application.Quit();
    }
}
