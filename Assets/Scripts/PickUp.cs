using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Transform hand; // hand pos
    private GameObject heldObject;   // obj ur holding
    private Rigidbody heldObjectRB;  // rigibody of ^

    [Header("Physics stuff")]
    [SerializeField] private float pickupForce = 200f; // force for picking up (can't pick up sth too heavy)
    [SerializeField] private float pickupRange = 5f; // max range for picking up

    [Header("Layer for picking up")]
    [SerializeField] int LayerMask;

    HeldItem childS = null;
    

    private void Update()
    {
       // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward));

        if (Input.GetKeyUp(KeyCode.E))
        {
            if (heldObject == null)   // could maybe be written better
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))  // "out hit" acts like a reference
                {
                    if (hit.transform.tag == "Pickupable" || hit.transform.tag == "SpPickupable")
                        pickUp(hit.transform.gameObject);
                }
                    
                
            }
            else
            {
                dropObject();
            }

        }
        if (heldObject != null)
        {
            move();
        }
    }

    void move()
    {
        if (childS != null && childS.collidedWithPlayer == false)
            if (Vector3.Distance(heldObject.transform.position, hand.position) > 0.1f)  // if object is not in vicinity, move it there
            {
                Vector3 moveDir = (hand.position - heldObject.transform.position);
                heldObjectRB.AddForce(moveDir * pickupForce);
            }
    }

    void pickUp(GameObject pickedObject)             // physics stuff via rigidbody so it stays still
    {
        if (pickedObject.GetComponent<Rigidbody>())
        {
            heldObjectRB = pickedObject.GetComponent<Rigidbody>();
            heldObjectRB.useGravity = false;
            heldObjectRB.drag = 10;
            heldObjectRB.constraints = RigidbodyConstraints.FreezeRotation;

            heldObjectRB.transform.parent = hand;

            if (pickedObject.transform.tag == "SpPickupable")
            {
                pickedObject.AddComponent<HeldItem>();
            }
            heldObject = pickedObject;

        }
    }

    void dropObject()                               // revert to original state, both object and held item
    {
        heldObjectRB.useGravity = true;
        heldObjectRB.drag = 1;
        heldObjectRB.constraints = RigidbodyConstraints.None;

        heldObject.transform.parent = null;
        Destroy(heldObject.GetComponent<HeldItem>());
        heldObject = null;
    }


    public void CollisionDetected(HeldItem childScript)
    {
        childS = childScript;
    }

    public void CollisionUndetected(HeldItem childScript)
    {
        childS = null;
    }

}