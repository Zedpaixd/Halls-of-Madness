using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSceneSwap : MonoBehaviour
{

    [SerializeField] private AsyncLoading AL;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.layer.Equals(6))
            AL.startLoad = 1;
    }

    private void OnTriggerExit(Collider other)
    {
        AL.teleport = true;
    }
}
