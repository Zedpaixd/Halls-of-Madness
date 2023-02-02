using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldItem : MonoBehaviour
{

    public Collision collidedWith;
    public bool collidedWithPlayer = false;

    void OnCollisionEnter(Collision collision)
    {
        transform.parent.parent.GetComponent<PickUp>().CollisionDetected(this);  //stupid hardcode but w/e
        collidedWith = collision;
        if (collision.transform.tag == "Player") collidedWithPlayer = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        collidedWith = null;
        if (collision.transform.tag == "Player") collidedWithPlayer = false;
    }
}
