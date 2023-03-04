using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInput : MonoBehaviour
{
    Transform canvas, settings;
    GameObject settingPanel;
    
    private void Awake()
    {
        canvas = GameObject.Find("Canvas").transform;
        settings = canvas.GetChild(1);
        settingPanel = settings.GetChild(0).gameObject;
    }
    public void OnPause()
    {
        for (int i = 0; i < settings.childCount; i++)
        {
            var child = settings.GetChild(i).gameObject;
            if (child.activeSelf)
            {
                if (i == 0)
                {
                    settings.gameObject.SetActive(false);
                }
                else
                {
                    settingPanel.SetActive(true);
                    child.SetActive(false);
                }
                break;
            }
        }
    }
}
