using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Zoom : MonoBehaviour
{
    [Min(0)] public float zoomedFOV;
    [Range(0, 0.1f)] public float lerpValue;
    float baseFOV, targetFOV, lerpedFOV;
    bool zoomed, zooming;
    Camera cam;
    void Start()
    {
        cam = GetComponent<Camera>();
        baseFOV = cam.fieldOfView;
        lerpedFOV = baseFOV;
    }
    public void onZoom(InputAction.CallbackContext ctx)
    {
        if (ctx.started) return;
        zoomed = ctx.performed;
        targetFOV = zoomed ? zoomedFOV : baseFOV;
        zooming = true;
    }
    void Update()
    {
        if (!zooming) return;
        lerpedFOV = Mathf.Lerp(lerpedFOV, targetFOV, lerpValue);
        cam.fieldOfView = lerpedFOV;
        if ((zoomed && lerpedFOV < (targetFOV + 0.001f)) || (!zoomed && lerpedFOV > (targetFOV - 0.001f)))
        {
            cam.fieldOfView = targetFOV;
            zooming = false;
        }
    }
}
