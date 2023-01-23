using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRoom : MonoBehaviour
{
    public GameObject original;
    public GameObject other;
    public bool onOriginal = true;

    // <summary> 
    // Alternates the camera between 2 different predefined spots that ideally should be positioned in different spots but
    // similar looking spots such that the illusion will be created. The second spot (other) will be given the movement script
    // so it moves together with the original player object
    // </summary>

    void applyFilters(bool enabled)
    {
        if (enabled)
        {
            Debug.Log("Apply filters");
        }
        else
        {
            Debug.Log("Remove filters");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            this.transform.position = !onOriginal ? original.transform.position : other.transform.position;
            applyFilters(onOriginal);
            onOriginal = !onOriginal;
        }
    }
}
