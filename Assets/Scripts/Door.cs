using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    public float openTime;
    Transform hinge;
    float origRot;
    bool triggered, turning, open;
    float timer;
    void Start()
    {
        hinge = transform.GetChild(0);
        origRot = hinge.eulerAngles.y;

    }

    void Update()
    {
        OpenClose();
    }

    void OpenClose()
    {
        if (!turning) { return; }
        timer += Time.deltaTime;
        if (timer > openTime)
        {
            turning = false;
            open = !open;
            return;
        }
        print(timer / openTime);
        hinge.rotation = Quaternion.Euler(0, 
            open ? Mathf.SmoothStep(origRot - 90f, origRot, timer / openTime) : 
            Mathf.SmoothStep(origRot, origRot - 90f, timer / openTime), 0);
    }

    public void OnInteract()
    {
        if (!triggered || turning) { return; }
        turning = true;
        timer = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggered = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggered = false;
        }
    }
}
