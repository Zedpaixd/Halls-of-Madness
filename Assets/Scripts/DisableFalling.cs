using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableFalling : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.tag.Equals("Player"))
        {
            other.TryGetComponent<Rigidbody>(out var something);
            if (something)
            {
                something.isKinematic = true;
                something.useGravity = false;
                something.tag = "Untagged";
            }
        }
    }
}
