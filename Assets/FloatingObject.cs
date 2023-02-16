using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    Rigidbody objectBody;
    void Start()
    {
        objectBody = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FloatingTrigger")){
            objectBody.constraints = RigidbodyConstraints.FreezePosition;
            objectBody.constraints = RigidbodyConstraints.FreezeRotation;
            objectBody.useGravity = false;
            objectBody.drag = 100;

            //Destroy(objectBody);
        }
    }
}
