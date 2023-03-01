using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTarget : MonoBehaviour
{

    public bool change = false;
    public Transform target;
    public FollowPlayer follower;

    private void Update()
    {
        if (change == true)
        {
            //follower.ChangeTarget(target);
            follower.target = target;
        }
    }
}
